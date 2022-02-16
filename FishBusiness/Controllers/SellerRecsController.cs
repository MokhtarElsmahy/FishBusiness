using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using FishBusiness.Data.Migrations;
using FishBusiness.Models;
using FishBusiness.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FishBusiness.Controllers
{
    [Authorize]
    public class SellerRecsController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public SellerRecsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public DateTime TimeNow()
        {
            TimeZone localZone = TimeZone.CurrentTimeZone;
            DateTime currentDate = DateTime.Now;
            DateTime currentUTC =
           localZone.ToUniversalTime(currentDate);
            return currentUTC.AddHours(2);
        }
        public IActionResult Index()
        {
            var lst = _context.SellerRecs.Where(c=>c.Date==TimeNow().Date).Include(c => c.Merchant).ToList();
            return View(lst);
        }

        public IActionResult SellerRecsHistory(DateTime date)
        {
            var lst = _context.SellerRecs.Where(c => c.Date.Date == date.Date).Include(c => c.Merchant).ToList();
            return PartialView(lst);
        }

        public IActionResult Create()
        {

            ViewData["ProductionTypeID"] = new SelectList(_context.ProductionTypes, "ProductionTypeID", "ProductionName");

            ViewData["FishID"] = new SelectList(_context.Fishes, "FishID", "FishName");

            //
            ViewData["MerchantID"] = new SelectList(_context.Merchants.Where(m => m.IsFromOutsideCity == false && m.IsOwner == false), "MerchantID", "MerchantName");
            // commission
            //ViewBag.Commission = _context.Cofigs.Find(1);
            ViewBag.Commission = _context.Cofigs.Find(2);
            return View();
        }

        public IActionResult Distribute(int id)
        {
            var rec = _context.SellerRecs.Include(c => c.Merchant).Where(c => c.SellerRecID == id).FirstOrDefault();
            ViewBag.Merchants = new SelectList(_context.Merchants.Where(c => c.IsFromOutsideCity == false).ToList(), "MerchantID", "MerchantName");

            DistributeSellerRec model = new DistributeSellerRec();
            model.SellerRec = rec;
            model.NormalSellerRecItems = _context.SellerRecItems.Include(c => c.Fish).Include(c => c.ProductionType).Where(c => c.SellerRectID == rec.SellerRecID && c.AmountId == null).ToList();
            model.AmountSellerRecItems = _context.SellerRecItems.Include(c => c.Fish).Include(c => c.ProductionType).Where(c => c.SellerRectID == rec.SellerRecID && c.AmountId != null).ToList();

            var results = from p in model.AmountSellerRecItems
                          group p.SellerRecItemID by p.AmountId into g
                          select new AmountVm { AmountId = g.Key, items = g };

            model.Amounts = results;

            return View(model);
        }



        public async Task<IActionResult> CreateSellerRec(int MerchantID, decimal TotalAfterPaying, int PercentageCommission, decimal Commission, DateTime Date, decimal TotalBeforePaying, string FishNames,
            string ProductionTypes, string Unitprices, string Prices, string Qtys)
        {
            try
            {
                var FishesCookie = FishNames.TrimEnd(FishNames[FishNames.Length - 1]);

                var ProductionTypesCookie = ProductionTypes.TrimEnd(ProductionTypes[ProductionTypes.Length - 1]);
                var qtysCookie = Qtys.TrimEnd(Qtys[Qtys.Length - 1]);
                var unitpricesCookie = Unitprices.TrimEnd(Unitprices[Unitprices.Length - 1]);
                var pricesCookie = Prices.TrimEnd(Prices[Prices.Length - 1]);



                string[] Fishes = FishesCookie.Split(",");//.Select(c => Convert.ToString(c)).ToArray();
                string[] Productions = ProductionTypesCookie.Split(",");//.Select(c => Convert.ToString(c)).ToArray();
                string[] qtys = qtysCookie.Split(",");//.Select(c => Convert.ToString(c)).ToArray();
                decimal[] unitPrices = unitpricesCookie.Split(",").Select(c => Convert.ToDecimal(c)).ToArray();
                decimal[] prices = pricesCookie.Split(",").Select(c => Convert.ToDecimal(c)).ToArray();


                var user = await _userManager.GetUserAsync(User);
                var roles = await _userManager.GetRolesAsync(user);
                int PID = 1;
                if (roles.Contains("partner"))
                {
                    PID = 2;
                }
               


                SellerRec stockRec = new SellerRec();
                stockRec.Date = Date;
                stockRec.TotalBeforePaying = TotalBeforePaying;
                stockRec.PercentageCommission = PercentageCommission;
                stockRec.Commission = Commission;
                stockRec.FinalIncome = TotalAfterPaying;
                stockRec.MerchantID = MerchantID;

                Person p = _context.People.Find(PID);
                p.credit += Commission; //عمولة بيعة لتاجر يتم اضافتها على رصيد اليوزر المستخد للسيستم الان


                _context.SellerRecs.Add(stockRec);
                _context.SaveChanges();




                for (int i = 0; i < Fishes.Length; i++)
                {

                    string[] splitItem = Fishes[i].Split("/").Select(c => Convert.ToString(c)).ToArray();
                    if (splitItem.Length > 1)
                    {
                        Guid amountID = Guid.NewGuid();
                        double[] splitItemQty = qtys[i].Split("/").Select(c => Convert.ToDouble(c)).ToArray();
                        for (int j = 0; j < splitItem.Length; j++)
                        {


                            var fish = _context.Fishes.Single(x => x.FishName == splitItem[j]);
                            var Produc = _context.ProductionTypes.Single(x => x.ProductionName == Productions[i]);

                            SellerRecItem StockRecItem = new SellerRecItem()
                            {
                                SellerRectID = stockRec.SellerRecID,
                                FishID = fish.FishID,
                                ProductionTypeID = Produc.ProductionTypeID,
                                Qty = splitItemQty[j],
                                UnitPrice = unitPrices[i],
                                AmountId = amountID
                            };
                            _context.SellerRecItems.Add(StockRecItem);
                            _context.SaveChanges();
                        }
                    }
                    else
                    {
                        var fish = _context.Fishes.Single(x => x.FishName == Fishes[i]);
                        var Produc = _context.ProductionTypes.Single(x => x.ProductionName == Productions[i]);

                        SellerRecItem StockRecItem = new SellerRecItem()
                        {
                            SellerRectID = stockRec.SellerRecID,
                            FishID = fish.FishID,
                            ProductionTypeID = Produc.ProductionTypeID,
                            Qty = Convert.ToDouble(qtys[i]),
                            UnitPrice = unitPrices[i]

                        };
                        _context.SellerRecItems.Add(StockRecItem);
                        _context.SaveChanges();
                    }
                }
                //المفروض نعمل نسبة العموله 
                //decimal
                return Json(new { message = "success", id = stockRec.SellerRecID });
            }
            catch (Exception)
            {
                var stockrecId = _context.SellerRecs.Max(c => c.SellerRecID);
                if (stockrecId >= 1)
                {
                    var stockre = _context.SellerRecs.Find(stockrecId);
                    _context.SellerRecs.Remove(stockre);
                    _context.SaveChanges();
                }
                return Json(new { message = "fail to save" });
            }


        }


        public async Task<IActionResult> MCreate(MerRecCreateVm model, decimal paidForSeller)
        {
            if (model.RecID != 0)
            {
                var FishesCookie = model.FishNames.TrimEnd(model.FishNames[model.FishNames.Length - 1]);
                var MerchantsCookie = model.MerchantNames.TrimEnd(model.MerchantNames[model.MerchantNames.Length - 1]);
                var ProductionTypesCookie = model.ProductionTypes.TrimEnd(model.ProductionTypes[model.ProductionTypes.Length - 1]);
                var qtysCookie = model.qtys.TrimEnd(model.qtys[model.qtys.Length - 1]);
                var unitpricesCookie = model.unitprices.TrimEnd(model.unitprices[model.unitprices.Length - 1]);

                string[] Fishes = FishesCookie.Split(",");//.Select(c => Convert.ToString(c)).ToArray();
                string[] Productions = ProductionTypesCookie.Split(",");//.Select(c => Convert.ToString(c)).ToArray();
                string[] Merchants = MerchantsCookie.Split(",");//.Select(c => Convert.ToString(c)).ToArray();
                string[] qtys = qtysCookie.Split(",");//.Select(c => Convert.ToString(c)).ToArray();
                decimal[] unitPrices = unitpricesCookie.Split(",").Select(c => Convert.ToDecimal(c)).ToArray();

                for (int i = 0; i < Merchants.Length; i++)
                {
                    var merchantt = _context.Merchants.Where(c => c.MerchantName == Merchants.ElementAt(i)).FirstOrDefault();

                    //  var merchantt = _context.Merchants.Where(c => c.MerchantID == model.MerchantID).FirstOrDefault();
                    if (merchantt != null)
                    {
                        if (merchantt.IsOwner == false)
                        {
                            MerchantReciept merchantReciept;
                            Merchant m;

                            var ee = Fishes[i].TrimEnd(Fishes[i][Fishes[i].Length - 1]);
                            string[] splitItemm = ee.Split("/");
                            //string[] splitItemm = Fishes[i].Split("/").Select(c => Convert.ToString(c)).ToArray();
                            decimal AddTo_TotalOfReciept;
                            decimal AddTo_CurrentDebt;
                            decimal AddTo_PreviousDebts;
                            if (splitItemm.Length > 1)
                            {
                                AddTo_TotalOfReciept = unitPrices[i];
                                AddTo_CurrentDebt = unitPrices[i];
                                AddTo_PreviousDebts = unitPrices[i];
                            }
                            else
                            {
                                AddTo_TotalOfReciept = unitPrices.ElementAt(i) * Convert.ToDecimal(qtys[i]);
                                AddTo_CurrentDebt = unitPrices.ElementAt(i) * Convert.ToDecimal(qtys[i]);
                                AddTo_PreviousDebts = unitPrices.ElementAt(i) * Convert.ToDecimal(qtys[i]);
                            }
                            if (GetMerchant(merchantt.MerchantID, TimeNow(),false) == 0)
                            {
                                merchantReciept = new MerchantReciept()
                                { Date = TimeNow(), payment = 0, TotalOfReciept = AddTo_TotalOfReciept, MerchantID = model.MerchantID, FromMerchant = model.MerchantID, CurrentDebt = merchantt.PreviousDebts + AddTo_PreviousDebts };
                                _context.Add(merchantReciept);
                                await _context.SaveChangesAsync();
                            }
                            else
                            {
                                merchantReciept = _context.MerchantReciepts.Find(GetMerchant(merchantt.MerchantID, TimeNow(),false));
                                m = _context.Merchants.Find(merchantt.MerchantID);

                                merchantReciept.TotalOfReciept += AddTo_TotalOfReciept;
                                merchantReciept.CurrentDebt += AddTo_CurrentDebt;
                            }
                            merchantt.PreviousDebts += AddTo_PreviousDebts;

                            var eeee = Fishes[i].TrimEnd(Fishes[i][Fishes[i].Length - 1]);
                            string[] splitItem = eeee.Split("/");
                            if (splitItem.Length > 1)
                            {
                                var cc = qtys[i].TrimEnd(qtys[i][qtys[i].Length - 1]);
                                double[] splitItemQty = cc.Split("/").Select(c => Convert.ToDouble(c)).ToArray();
                                var amountId = Guid.NewGuid();
                                for (int xx = 0; xx < splitItem.Length; xx++)
                                {
                                    var fishh = _context.Fishes.Single(x => x.FishName == splitItem.ElementAt(xx));
                                    //var Producc = _context.ProductionTypes.Single(x => x.ProductionName == Productions[j]);
                                    var Producc = _context.ProductionTypes.Single(x => x.ProductionName == Productions[i]);
                                    MerchantRecieptItem MerchantRecieptItems = new MerchantRecieptItem()
                                    {
                                        MerchantRecieptID = merchantReciept.MerchantRecieptID,
                                        FishID = fishh.FishID,
                                        ProductionTypeID = Producc.ProductionTypeID,
                                        //Qty = Convert.ToInt32(qtys[i]),
                                        Qty = splitItemQty[xx],
                                        UnitPrice = unitPrices[i],
                                        AmountId = amountId
                                    };
                                    _context.MerchantRecieptItems.Add(MerchantRecieptItems);
                                    _context.SaveChanges();

                                }
                            }
                            else
                            {
                                var Individualfish = _context.Fishes.Single(x => x.FishName == Fishes[i]);
                                var IndividualProduc = _context.ProductionTypes.Single(x => x.ProductionName == Productions[i]);

                                var TodaysMerchantRecItems = _context.MerchantRecieptItems.Include(c => c.MerchantReciept).ToList()
                                    .Where(c => c.MerchantReciept.Date.ToShortDateString() == TimeNow().ToShortDateString() && c.MerchantRecieptID == merchantReciept.MerchantRecieptID).ToList();
                                //لو حصل خطأ هنا هيكون السبب ان مفيش 
                                //BoatID
                                var existingFish = TodaysMerchantRecItems.Where(c => c.FishID == Individualfish.FishID /*&& c.BoatID == boat.BoatID*/).FirstOrDefault();
                                if (existingFish != null)
                                {
                                    if (existingFish.ProductionTypeID == IndividualProduc.ProductionTypeID && existingFish.UnitPrice == unitPrices[i])
                                    {
                                        existingFish.Qty += Convert.ToDouble(qtys[i]);
                                    }
                                    else
                                    {
                                        MerchantRecieptItem MerchantRecieptItems = new MerchantRecieptItem()
                                        {
                                            MerchantRecieptID = merchantReciept.MerchantRecieptID,
                                            FishID = Individualfish.FishID,
                                            ProductionTypeID = IndividualProduc.ProductionTypeID,
                                            Qty = Convert.ToDouble(qtys[i]),
                                            UnitPrice = unitPrices[i],

                                        };
                                        _context.MerchantRecieptItems.Add(MerchantRecieptItems);
                                    }
                                }
                                else
                                {
                                    MerchantRecieptItem MerchantRecieptItems = new MerchantRecieptItem()
                                    {
                                        MerchantRecieptID = merchantReciept.MerchantRecieptID,
                                        FishID = Individualfish.FishID,
                                        ProductionTypeID = IndividualProduc.ProductionTypeID,
                                        Qty = Convert.ToDouble(qtys[i]),
                                        UnitPrice = unitPrices[i],

                                    };
                                    _context.MerchantRecieptItems.Add(MerchantRecieptItems);
                                }
                                _context.SaveChanges();
                            }
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            //-----------------------------------------------------
                            IMerchantReciept ImerchantReciept;
                            var ee = Fishes[i].TrimEnd(Fishes[i][Fishes[i].Length - 1]);
                            string[] splitItemm = ee.Split("/");
                            //string[] splitItemm = Fishes[i].Split("/").Select(c => Convert.ToString(c)).ToArray();
                            decimal AddTo_TotalOfReciept;

                            if (splitItemm.Length > 1)
                            {
                                AddTo_TotalOfReciept = unitPrices[i];

                            }
                            else
                            {
                                AddTo_TotalOfReciept = unitPrices.ElementAt(i) * Convert.ToDecimal(qtys[i]);

                            }
                            var recid = GetMerchant(model.MerchantID, TimeNow(),true);
                            if (recid == 0)
                            {
                                ImerchantReciept = new IMerchantReciept() { Date = TimeNow(), MerchantID = model.MerchantID, TotalOfReciept = AddTo_TotalOfReciept };
                                _context.Add(ImerchantReciept);
                                await _context.SaveChangesAsync();
                            }
                            else
                            {
                                ImerchantReciept = _context.IMerchantReciept.Find(recid);
                                if (ImerchantReciept == null)
                                {
                                    ImerchantReciept = new IMerchantReciept() { Date = TimeNow(), MerchantID = model.MerchantID, TotalOfReciept = AddTo_TotalOfReciept };
                                    _context.Add(ImerchantReciept);
                                    await _context.SaveChangesAsync();
                                  

                                }
                                else
                                {
                                    ImerchantReciept.TotalOfReciept += AddTo_TotalOfReciept;
                                    await _context.SaveChangesAsync();
                                }

                            }
                            //--------------------------------------------------------

                            if (splitItemm.Length > 1)
                            {

                                var amountId = Guid.NewGuid();
                                var cc = qtys[i].TrimEnd(qtys[i][qtys[i].Length - 1]);
                                for (int z = 0; z < splitItemm.Length; z++)
                                {

                                    double[] splitItemQty = cc.Split("/").Select(c => Convert.ToDouble(c)).ToArray();
                                    var fish = _context.Fishes.SingleOrDefault(x => x.FishName == splitItemm[z]);

                                    var Produc = _context.ProductionTypes.SingleOrDefault(x => x.ProductionName == Productions[i]);
                                    //**********************************************************************************
                                    string f = Fishes[i].Trim();
                                    var fishes = _context.Fishes.ToList();
                                 
                                    string FishName = "";
                                    int FishID = 0;
                                    foreach (var item in fishes)
                                    {
                                        if (item.FishName.Contains(f))
                                        {
                                            FishName = item.FishName;
                                            FishID = item.FishID;
                                        }
                                    }
                                    var TodaysIMerchantRecItems = _context.IMerchantRecieptItem.Include(c => c.IMerchantReciept).ToList().Where(c => c.IMerchantReciept.Date.ToShortDateString() == TimeNow().ToShortDateString() && c.IMerchantRecieptID == ImerchantReciept.IMerchantRecieptID).ToList();
                                    var IMerchantRecieptItems = TodaysIMerchantRecItems.Where(c => c.FishID == fish.FishID && c.UnitPrice == unitPrices[i]).FirstOrDefault();
                                    // IMerchantRecieptItem IMerchantRecieptItems;
                                    if (IMerchantRecieptItems != null)
                                    {
                                        if (IMerchantRecieptItems.ProductionTypeID == Produc.ProductionTypeID && IMerchantRecieptItems.UnitPrice == unitPrices[i])
                                        {
                                            IMerchantRecieptItems.Qty += Convert.ToDouble(qtys[i]);
                                        }
                                        else
                                        {
                                            IMerchantRecieptItem NewIMerchantRecieptItems = new IMerchantRecieptItem()
                                            {
                                                IMerchantRecieptID = ImerchantReciept.IMerchantRecieptID,
                                                FishID = fish.FishID,
                                                ProductionTypeID = Produc.ProductionTypeID,
                                                Qty = Convert.ToDouble(qtys[i]),
                                                UnitPrice = unitPrices[i],


                                            }; _context.IMerchantRecieptItem.Add(NewIMerchantRecieptItems);
                                        }

                                    }
                                    else
                                    {
                                        try
                                        {
                                            IMerchantRecieptItem NewIMerchantRecieptItems = new IMerchantRecieptItem()
                                            {
                                                IMerchantRecieptID = ImerchantReciept.IMerchantRecieptID,//*
                                                FishID = fish.FishID,
                                                ProductionTypeID = Produc.ProductionTypeID,
                                                Qty = Convert.ToDouble(splitItemQty[z]),
                                                UnitPrice = unitPrices[i],
                                                AmountId = amountId,


                                            }; _context.IMerchantRecieptItem.Add(NewIMerchantRecieptItems);
                                        }
                                        catch (Exception ex)
                                        {

                                            throw;
                                        }
                                       
                                    }
                                    try
                                    {
                                        _context.SaveChanges();
                                    }
                                    catch (Exception er)
                                    {

                                        throw;
                                    }
                                    //***********************************************************


                                    //IMerchantRecieptItem NewIMerchantRecieptItems = new IMerchantRecieptItem()
                                    //{
                                    //    IMerchantRecieptID = ImerchantReciept.IMerchantRecieptID,
                                    //    FishID = fish.FishID,
                                    //    ProductionTypeID = Produc.ProductionTypeID,
                                    //    Qty = splitItemQty[z],
                                    //    UnitPrice = unitPrices[i],
                                    //    AmountId = amountId,


                                    //};
                                    //_context.IMerchantRecieptItem.Add(NewIMerchantRecieptItems);


                                    _context.SaveChanges();
                                    var s = _context.Stocks.ToList().Where(c => c.FishID == fish.FishID).FirstOrDefault();
                                    if (s != null)
                                    {
                                        if (s.ProductionTypeID == Produc.ProductionTypeID)
                                        {
                                            s.Qty += splitItemQty[z];

                                        }
                                        else
                                        {
                                            Stock stoc = new Stock()
                                            {
                                                FishID = fish.FishID,
                                                ProductionTypeID = Produc.ProductionTypeID,
                                                Qty = splitItemQty[z],
                                                Date = ImerchantReciept.Date
                                            };
                                            _context.Stocks.Add(stoc);
                                        }
                                    }
                                    else
                                    {
                                        Stock stock = new Stock()
                                        {
                                            FishID = fish.FishID,
                                            ProductionTypeID = Produc.ProductionTypeID,
                                            Qty = splitItemQty[z]
                                        };
                                        _context.Stocks.Add(stock);

                                    }

                                    var stockHistory = _context.StockHistories.ToList().Where(c => c.Date.ToShortDateString() == TimeNow().ToShortDateString() && c.FishID == fish.FishID && c.ProductionTypeID == Produc.ProductionTypeID).FirstOrDefault();
                                    if (stockHistory != null)
                                    {
                                        stockHistory.Total += splitItemQty[z];
                                    }
                                    else
                                    {
                                        StockHistory history = new StockHistory() { FishID = fish.FishID, ProductionTypeID = Produc.ProductionTypeID, Total = splitItemQty[z], Date = TimeNow() };
                                        _context.StockHistories.Add(history);
                                    }
                                }
                            }
                            else
                            {

                                string f = Fishes[i].Trim();
                                var fishes = _context.Fishes.ToList();
                                Fish fish = new Fish();
                                string FishName = "";
                                int FishID = 0;
                                foreach (var item in fishes)
                                {
                                    if (item.FishName.Contains(f))
                                    {
                                        FishName = item.FishName;
                                        FishID = item.FishID;
                                    }
                                }
                                // var fish = fishes.Where(x => x.FishName==f).FirstOrDefault();
                                var Produc = _context.ProductionTypes.Single(x => x.ProductionName == Productions[i]);
                                var TodaysIMerchantRecItems = _context.IMerchantRecieptItem.Include(c => c.IMerchantReciept).ToList().Where(c => c.IMerchantReciept.Date.ToShortDateString() == TimeNow().ToShortDateString() && c.IMerchantRecieptID == ImerchantReciept.IMerchantRecieptID).ToList();
                                var IMerchantRecieptItems = TodaysIMerchantRecItems.Where(c => c.FishID == FishID && c.UnitPrice == unitPrices[i]).FirstOrDefault();
                                // IMerchantRecieptItem IMerchantRecieptItems;
                                if (IMerchantRecieptItems != null)
                                {
                                    if (IMerchantRecieptItems.ProductionTypeID == Produc.ProductionTypeID && IMerchantRecieptItems.UnitPrice == unitPrices[i])
                                    {
                                        IMerchantRecieptItems.Qty += Convert.ToDouble(qtys[i]);
                                    }
                                    else
                                    {
                                        IMerchantRecieptItem NewIMerchantRecieptItems = new IMerchantRecieptItem()
                                        {
                                            IMerchantRecieptID = ImerchantReciept.IMerchantRecieptID,
                                            FishID = FishID,
                                            ProductionTypeID = Produc.ProductionTypeID,
                                            Qty = Convert.ToDouble(qtys[i]),
                                            UnitPrice = unitPrices[i],


                                        }; _context.IMerchantRecieptItem.Add(NewIMerchantRecieptItems);
                                    }

                                }
                                else
                                {
                                    IMerchantRecieptItem NewIMerchantRecieptItems = new IMerchantRecieptItem()
                                    {
                                        IMerchantRecieptID = ImerchantReciept.IMerchantRecieptID,//*
                                        FishID = FishID,
                                        ProductionTypeID = Produc.ProductionTypeID,
                                        Qty = Convert.ToDouble(qtys[i]),
                                        UnitPrice = unitPrices[i],
                                        AmountId = null,


                                    }; _context.IMerchantRecieptItem.Add(NewIMerchantRecieptItems);
                                }
                                try
                                {
                                    _context.SaveChanges();
                                }
                                catch (Exception er)
                                {

                                    throw;
                                }



                                var s = _context.Stocks.ToList().Where(c => c.FishID == FishID).FirstOrDefault();
                                if (s != null)
                                {
                                    if (s.ProductionTypeID == Produc.ProductionTypeID)
                                    {
                                        s.Qty += Convert.ToDouble(qtys[i]);

                                    }
                                    else
                                    {
                                        Stock stoc = new Stock()
                                        {
                                            FishID = FishID,
                                            ProductionTypeID = Produc.ProductionTypeID,
                                            Qty = Convert.ToDouble(qtys[i]),
                                            Date = ImerchantReciept.Date
                                        };
                                        _context.Stocks.Add(stoc);
                                    }
                                }
                                else
                                {
                                    Stock stock = new Stock()
                                    {
                                        FishID = FishID,
                                        ProductionTypeID = Produc.ProductionTypeID,
                                        Qty = Convert.ToDouble(qtys[i])
                                    };
                                    _context.Stocks.Add(stock);
                                }
                                var stockHistory = _context.StockHistories.ToList().Where(c => c.Date.ToShortDateString() == TimeNow().ToShortDateString() && c.FishID == FishID && c.ProductionTypeID == Produc.ProductionTypeID).FirstOrDefault();
                                if (stockHistory != null)
                                {
                                    stockHistory.Total += Convert.ToDouble(qtys[i]);
                                }
                                else
                                {
                                    StockHistory history = new StockHistory() { FishID = FishID, ProductionTypeID = Produc.ProductionTypeID, Total = Convert.ToDouble(qtys[i]), Date = TimeNow() };
                                    _context.StockHistories.Add(history);
                                }

                            }
                            try
                            {
                                _context.SaveChanges();

                            }
                            catch (Exception ex)
                            {

                                throw;
                            }



                            var stockrows = _context.Stocks.ToList();
                            foreach (var item in stockrows)
                            {
                                if (item.ProductionTypeID == 3)//ميزان
                                {
                                    item.TotalWeight = item.Qty;
                                }

                            }

                            _context.SaveChanges();


                        }
                    }
                    else
                    {
                        var ee = Fishes[i].TrimEnd(Fishes[i][Fishes[i].Length - 1]);
                        string[] splitItemm = ee.Split("/");
                        decimal AddTo_TotalOfReciept;
                        decimal AddTo_CurrentDebt;
                        decimal AddTo_PreviousDebts;
                        if (splitItemm.Length > 1)
                        {
                            AddTo_TotalOfReciept = unitPrices[i];
                            AddTo_CurrentDebt = unitPrices[i];
                            AddTo_PreviousDebts = unitPrices[i];
                        }
                        else
                        {
                            AddTo_TotalOfReciept = unitPrices.ElementAt(i) * Convert.ToDecimal(qtys[i]);
                            AddTo_CurrentDebt = unitPrices.ElementAt(i) * Convert.ToDecimal(qtys[i]);
                            AddTo_PreviousDebts = unitPrices.ElementAt(i) * Convert.ToDecimal(qtys[i]);
                        }
                        var rec = _context.PersonReciepts.ToList().Where(x => x.Date.ToShortDateString() == TimeNow().ToShortDateString()).FirstOrDefault();
                        if (rec != null)
                        {
                            rec.TotalPrice += AddTo_TotalOfReciept;
                        }
                        else
                        {
                            PersonReciept personReciept = new PersonReciept() { Date = TimeNow(), PersonName = Merchants.ElementAt(i), TotalPrice = AddTo_TotalOfReciept };
                            _context.PersonReciepts.Add(personReciept);
                        }
                        _context.SaveChanges();
                        var boat = _context.Boats.Where(c => c.BoatName == "معاملة" && c.BoatLicenseNumber == "0").FirstOrDefault();
                        if (splitItemm.Length > 1)  // بيعة
                        {
                            var cc = qtys[i].TrimEnd(qtys[i][qtys[i].Length - 1]);
                            double[] splitItemQty = cc.Split("/").Select(c => Convert.ToDouble(c)).ToArray();
                            var amountId = Guid.NewGuid();
                            var latestrec = _context.PersonReciepts.ToList().Where(x => x.Date.ToShortDateString() == TimeNow().ToShortDateString()).FirstOrDefault();
                            for (int xx = 0; xx < splitItemm.Length; xx++)
                            {
                                var fishh = _context.Fishes.Single(x => x.FishName == splitItemm.ElementAt(xx));
                                var Producc = _context.ProductionTypes.Single(x => x.ProductionName == Productions[i]);
                                PersonRecieptItem personRecieptItem = new PersonRecieptItem()
                                {
                                    FishID = fishh.FishID,
                                    ProductionTypeID = Producc.ProductionTypeID,
                                    Qty = splitItemQty[xx],
                                    UnitPrice = unitPrices[i],
                                    BoatID = boat.BoatID,
                                    AmountId = amountId,
                                    PersonRecieptID = latestrec.PersonRecieptID
                                };
                                _context.PersonRecieptItems.Add(personRecieptItem);
                                _context.SaveChanges();

                            }
                        }
                        else
                        {
                            var Individualfish = _context.Fishes.Single(x => x.FishName == Fishes[i]);
                            var IndividualProduc = _context.ProductionTypes.Single(x => x.ProductionName == Productions[i]);
                            var latestrec = _context.PersonReciepts.ToList().Where(x => x.Date.ToShortDateString() == TimeNow().ToShortDateString()).FirstOrDefault();
                            var fishh = _context.Fishes.Single(x => x.FishName == Individualfish.FishName);
                            var Producc = _context.ProductionTypes.Single(x => x.ProductionName == IndividualProduc.ProductionName);
                            PersonRecieptItem personRecieptItem = new PersonRecieptItem()
                            {
                                FishID = fishh.FishID,
                                ProductionTypeID = Producc.ProductionTypeID,
                                Qty = Convert.ToDouble(qtys[i]),
                                UnitPrice = unitPrices[i],
                                BoatID = boat.BoatID,
                                PersonRecieptID = latestrec.PersonRecieptID
                            };
                            _context.PersonRecieptItems.Add(personRecieptItem);
                            _context.SaveChanges();
                        }
                    }
                }

                var recc = _context.SellerRecs.Find(model.RecID);
                var merr = _context.Merchants.Find(model.MerchantID);
                merr.PreviousDebtsForMerchant += recc.FinalIncome;
                _context.SaveChanges();

                if (paidForSeller > 0.0m)
                {
                    var user = await _userManager.GetUserAsync(User);
                    var roles = await _userManager.GetRolesAsync(user);
                    if (roles.Contains("admin"))
                    {

                        var mer = _context.Merchants.Find(model.MerchantID);
                        var person = _context.People.Find(1);
                        PaidForSeller p = new PaidForSeller() { Date = TimeNow(), PersonID = person.PersonID, Payment = paidForSeller, PreviousDebtsForSeller = mer.PreviousDebtsForMerchant - paidForSeller, MerchantID = mer.MerchantID };
                        _context.PaidForSellers.Add(p);
                        mer.PreviousDebtsForMerchant -= paidForSeller;
                        //person.credit -= paidForSeller;



                    }
                    else if (roles.Contains("partner"))
                    {
                        var mer = _context.Merchants.Find(model.MerchantID);
                        var person = _context.People.Find(2);
                        PaidForSeller p = new PaidForSeller() { Date = TimeNow(), PersonID = person.PersonID, Payment = paidForSeller, PreviousDebtsForSeller = mer.PreviousDebtsForMerchant - paidForSeller, MerchantID = mer.MerchantID };
                        _context.PaidForSellers.Add(p);
                        mer.PreviousDebtsForMerchant -= paidForSeller;
                        //person.credit -= paidForSeller;

                    }
                    _context.SaveChanges();
                }

                return Json(new { message = "success" });

            }
            //return View(model);
            return Json(new { message = "fail" });
        }


        public int GetMerchant(int? id, DateTime date ,bool IsOwner)
        {
            Merchant m = _context.Merchants.Find(id);
            if (IsOwner == false)
            {
                var rowsOfRec = _context.MerchantReciepts.Where(i => i.MerchantID == id);
                int recID = 0;
                if (rowsOfRec.Any())
                {
                    recID = rowsOfRec.Max(i => i.MerchantRecieptID);
                }
                var rec = _context.MerchantReciepts.Find(recID);
                if (rec != null)
                {
                    if (rec.Date.ToShortDateString() == date.ToShortDateString())
                    {
                        return recID;
                    }
                    return 0;
                }

                return 0;

            }
            else
            {
                var rowsOfRec = _context.IMerchantReciept.Where(i => i.MerchantID == id);
                int recID = 0;
                if (rowsOfRec.Any())
                {
                    recID = rowsOfRec.Max(i => i.IMerchantRecieptID);
                }
                var rec = _context.IMerchantReciept.Find(recID);
                if (rec != null)
                {
                    if (rec.Date.ToShortDateString() == date.ToShortDateString())
                    {
                        return recID;
                    }
                    return 0;
                }

                return 0;
            }

        }

        public IActionResult Detials(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var SellerRec = _context.SellerRecs.Include(m => m.Merchant)
                .FirstOrDefault(m => m.SellerRecID == id);
            if (SellerRec == null)
            {
                return NotFound();
            }

            SellerRecVm model = new SellerRecVm();
            model.SellerRec = SellerRec;
            model.NormalboatOwnerItems = _context.SellerRecItems.Include(c => c.Fish).Include(c => c.ProductionType).Where(c => c.SellerRectID == SellerRec.SellerRecID && c.AmountId == null).ToList();
            model.AmountboatOwnerItems = _context.SellerRecItems.Include(c => c.Fish).Include(c => c.ProductionType).Where(c => c.SellerRectID == SellerRec.SellerRecID && c.AmountId != null).ToList();

            var results = from p in model.AmountboatOwnerItems
                          group p.SellerRecItemID by p.AmountId into g
                          select new AmountVm { AmountId = g.Key, items = g };

            model.Amounts = results.ToList();
            return View(model);
        }









    }
}
