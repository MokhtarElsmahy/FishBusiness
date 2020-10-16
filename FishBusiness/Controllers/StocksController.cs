using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FishBusiness;
using FishBusiness.Models;
using FishBusiness.ViewModels;

namespace FishBusiness.Controllers
{
    public class StocksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StocksController(ApplicationDbContext context)
        {
            _context = context;
        }
        public DateTime TimeNow()
        {
            TimeZone localZone = TimeZone.CurrentTimeZone;
            DateTime currentDate = DateTime.Now;
            DateTime currentUTC =
           localZone.ToUniversalTime(currentDate);
            return currentUTC.AddHours(2);
        }
      
        #region stock
        // GET: Stocks
        public IActionResult Index()
        {
            var applicationDbContext = _context.Stocks.Include(s => s.Fish).Include(s => s.ProductionType).ToList();
            //return View( applicationDbContext.Where(s=>s.Date.ToShortDateString()==TimeNow().ToShortDateString()).ToList());
            return View(applicationDbContext.ToList());
        }

        // GET: Stocks/Details/5
        public async Task<IActionResult> Calssify(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stock = await _context.Stocks
                .Include(s => s.Fish)
                .Include(s => s.ProductionType)
                .FirstOrDefaultAsync(m => m.StockID == id);
            if (stock == null)
            {
                return NotFound();
            }
            ViewData["FishID"] = new SelectList(_context.Fishes, "FishID", "FishName", stock.FishID);
            ViewBag.FirstTimeFlag = stock.FirstTimeFlag;

            ViewData["ProductionTypeID"] = new SelectList(_context.ProductionTypes, "ProductionTypeID", "ProductionName", stock.ProductionTypeID);
            return PartialView(stock);
        }

        [HttpPost]

        public IActionResult UpdateStock(StockVM stock)
        {
            var s = _context.Stocks.Where(c => c.FishID == stock.FishID && c.ProductionTypeID == stock.ProductionTypeID).FirstOrDefault();//نقط
            var oldstock = _context.Stocks.Find(stock.StockID);//جمبرى جامبو



            if (s != null) // نوع السمك موجود 
            {

                if (stock.ProductionTypeID == stock.OldProductionType && stock.OldProductionType == 3)//كميزان
                {

                    s.Qty += stock.Qty;
                    oldstock.Qty -= stock.Qty;

                    if (oldstock.Qty == 0)
                    {
                        _context.Stocks.Remove(oldstock);
                    }

                }
                else if (stock.ProductionTypeID == 3 && stock.OldProductionType == 2)
                {
                    s.Qty += stock.Qty;
                    _context.Stocks.Remove(oldstock);
                }



                #region MyRegion
                //if (stock.FirstTimeFlag == false)
                //{

                //    s.Qty = stock.Qty;
                //    s.ProductionTypeID = stock.ProductionTypeID;
                //    s.FirstTimeFlag = true;
                //}
                //else
                //{
                //    s.Qty += stock.Qty;
                //    s.ProductionTypeID = stock.ProductionTypeID;
                //    s.FirstTimeFlag = false;
                //}
                //if (stock.ProductionTypeID != stock.OldProductionType)
                //{
                //    var ss= _context.Stocks.Where(c => c.FishID == stock.FishID && c.ProductionTypeID == stock.OldProductionType).FirstOrDefault();
                //    _context.Stocks.Remove(ss);
                //} 
                #endregion

            }
            else //نوع السمك مش موجود 
            {
                if (stock.ProductionTypeID == stock.OldProductionType && stock.OldProductionType == 3)//كميزان
                {
                    Stock stockkk = new Stock()
                    {
                        FishID = stock.FishID,
                        ProductionTypeID = stock.ProductionTypeID,
                        Qty = stock.Qty,
                        TotalWeight = stock.Qty
                    };
                    oldstock.Qty -= stock.Qty;
                    _context.Stocks.Add(stockkk);

                    if (oldstock.Qty == 0)
                    {
                        _context.Stocks.Remove(oldstock);
                    }

                }
                else if (stock.ProductionTypeID == 3 && stock.OldProductionType == 2)
                {
                    Stock stockkk = new Stock()
                    {
                        FishID = stock.FishID,
                        ProductionTypeID = stock.ProductionTypeID,
                        Qty = stock.Qty,
                        TotalWeight = stock.Qty
                    };
                    _context.Stocks.Add(stockkk);
                    _context.Stocks.Remove(oldstock);
                }
                #region MyRegion
                //var ss = _context.Stocks.Where(c => c.FishID == stock.FishID && c.ProductionTypeID == stock.OldProductionType).FirstOrDefault();
                //_context.Stocks.Remove(ss);
                //Stock stockk = new Stock()
                //{
                //    FishID = stock.FishID,
                //    ProductionTypeID = stock.ProductionTypeID,
                //    Qty = stock.Qty
                //};
                //_context.Stocks.Add(stockk); 
                #endregion
            }
            #region MyRegion
            //else
            //{
            //    Stock stockk = new Stock()
            //    {
            //        FishID = stock.FishID,
            //        ProductionTypeID = stock.ProductionTypeID,
            //        Qty = stock.Qty
            //    };
            //    _context.Stocks.Add(stockk);
            //} 
            #endregion

            _context.SaveChanges();

            var stockrows = _context.Stocks.ToList();
            foreach (var item in stockrows)
            {
                if (item.ProductionTypeID == 3)//ميزان
                {
                    item.TotalWeight = item.Qty;
                }
                //else
                //{
                //    //الطوايل هتتم عن طريق التصنيف 
                //}
            }


            _context.SaveChanges();
            return Json(new { message = "success", newTotalWeight = stock.OldtotalWeight - stock.Qty });
        }




        public IActionResult CreateReport()
        {

            var applicationDbContext = _context.Stocks.Include(s => s.Fish).Include(s => s.ProductionType).ToList();
            //return View(applicationDbContext.Where(s => s.Date.ToShortDateString() == TimeNow().ToShortDateString()).ToList());
            return View(applicationDbContext.ToList());
            // return v()
        }

        public IActionResult Details(DateTime date)
        {


            var iMerchantReciept = _context.IMerchantReciept.Include(i => i.Merchant).Include(i => i.IMerchantRecieptItems).ToList();

            var mr = iMerchantReciept.Where(m => m.Date.ToLongDateString() == date.ToLongDateString()).ToList();

            ViewBag.Date = date.ToLongDateString();

            var items = _context.IMerchantRecieptItem.Include(i => i.Fish).Include(i => i.ProductionType).ToList();


            return View(mr);
        }

        #endregion

        public IActionResult SendBackToSeaPort()
        {
            var fishIdsInStock = _context.Stocks.Select(c => c.FishID);
            var fishesInStock = _context.Fishes.Where(f => fishIdsInStock.Contains(f.FishID)).ToList();
            ViewData["StockFishID"] = new SelectList(fishesInStock, "FishID", "FishName");
            ViewData["ProductionTypeID"] = new SelectList(_context.ProductionTypes, "ProductionTypeID", "ProductionName");
            ViewBag.Commission = _context.Cofigs.Find(2);
            return View();
        }

        public IActionResult GetStockItemType(int fishId)
        {
            var StockItem = _context.Stocks.FirstOrDefault(c => c.FishID == fishId);
            if (StockItem != null)
            {
                return Json(new { message = "success", typeId = StockItem.ProductionTypeID, qantityInStock = StockItem.Qty });
            }
            else
            {
                return Json(new { message = "fail", typeId = 2 }); // remeber to change with Ehab
            }
        }


        public IActionResult CreateStockRec(DateTime Date, decimal TotalBeforePaying, string FishNames, string ProductionTypes, string Unitprices, string Prices, string Qtys, string FishNamesRest, string ProductionTypesRest, string QtysRest)
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


                //--------------Rest In Store ----------------------------
                var FishesCookieRest = FishNamesRest.TrimEnd(FishNamesRest[FishNamesRest.Length - 1]);
                var ProductionTypesCookieRest = ProductionTypesRest.TrimEnd(ProductionTypesRest[ProductionTypesRest.Length - 1]);
                var qtysCookieRest = QtysRest.TrimEnd(QtysRest[QtysRest.Length - 1]);

                string[] FishesRest = FishesCookieRest.Split(",");
                string[] ProductionsRest = ProductionTypesCookieRest.Split(",");
                double[] qtysRest = qtysCookieRest.Split(",").Select(c => Convert.ToDouble(c)).ToArray();



                StockRec stockRec = new StockRec();
                stockRec.Date = Date;
                stockRec.TotalOfRec = TotalBeforePaying;
                try
                {
                    _context.StockRecs.Add(stockRec);
                    _context.SaveChanges();
                }
                catch (Exception)
                {

                    return Json(new { message = "fail" });
                }


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

                            StockRecItem StockRecItem = new StockRecItem()
                            {
                                StockRecID = stockRec.StockRecID,
                                FishID = fish.FishID,
                                ProductionTypeID = Produc.ProductionTypeID,
                                Qty = splitItemQty[j],
                                UnitPrice = unitPrices[i],
                                AmountId = amountID
                            };
                            _context.StockRecItems.Add(StockRecItem);
                            _context.SaveChanges();
                        }
                    }
                    else
                    {
                        var fish = _context.Fishes.Single(x => x.FishName == Fishes[i]);
                        var Produc = _context.ProductionTypes.Single(x => x.ProductionName == Productions[i]);

                        StockRecItem StockRecItem = new StockRecItem()
                        {
                            StockRecID = stockRec.StockRecID,
                            FishID = fish.FishID,
                            ProductionTypeID = Produc.ProductionTypeID,
                            Qty = qtysRest[i],
                            UnitPrice = unitPrices[i],

                        };
                        _context.StockRecItems.Add(StockRecItem);
                        _context.SaveChanges();
                    }
                }

                // المفروض ان
                //totalBeforePaying 
                //يتم اضافته على رصيد علاء او مجدى
                //اسال علاء فيها


                //التعديل على القيم الموجوده بالمخزن
                for (int i = 0; i < FishesRest.Length; i++)
                {
                    var stockItem = _context.Stocks.Include(c => c.Fish).Where(c => c.Fish.FishName == FishesRest[i] && c.ProductionType.ProductionName == ProductionsRest[i]).FirstOrDefault();
                    if (stockItem != null)
                    {
                        if (qtysRest[i] == 0)
                        {
                            _context.Stocks.Remove(stockItem);
                        }
                        else
                        {
                            stockItem.Qty = qtysRest[i];
                        }
                        _context.SaveChanges();
                    }
                }

                return Json(new { message = "success", id = stockRec.StockRecID });
            }
            catch (Exception)
            {
                var stockrecId = _context.StockRecs.Max(c => c.StockRecID);
                if (stockrecId >= 1)
                {
                    var stockre = _context.StockRecs.Find(stockrecId);
                    _context.StockRecs.Remove(stockre);
                    _context.SaveChanges();
                }
                return Json(new { message = "fail to save" });
            }


        }

        public IActionResult DistributeStock(int id)
        {
            var rec = _context.StockRecs.Where(c => c.StockRecID == id).FirstOrDefault();
            ViewBag.Merchants = new SelectList(_context.Merchants.Where(c => c.IsFromOutsideCity == false).ToList(), "MerchantID", "MerchantName");

            StockDistributionVm model = new StockDistributionVm();
            model.StockRec = rec;
            model.NormalStockRecItems = _context.StockRecItems.Include(c => c.Fish).Include(c => c.ProductionType).Where(c => c.StockRecID == rec.StockRecID && c.AmountId == null).ToList();
            model.AmountStockRecItems = _context.StockRecItems.Include(c => c.Fish).Include(c => c.ProductionType).Where(c => c.StockRecID == rec.StockRecID && c.AmountId != null).ToList();


            var results = from p in model.AmountStockRecItems
                          group p.StockRecItemID by p.AmountId into g
                          select new AmountVm { AmountId = g.Key, items = g };

            model.Amounts = results;

            return View(model);
        }


        public async Task<IActionResult> MCreate(MerRecCreateVm model)
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
                            if (GetMerchant(merchantt.MerchantID, TimeNow()) == 0)
                            {
                                merchantReciept = new MerchantReciept()
                                { Date = TimeNow(), payment = 0, TotalOfReciept = AddTo_TotalOfReciept, MerchantID = merchantt.MerchantID, CurrentDebt = merchantt.PreviousDebts + AddTo_PreviousDebts };
                                _context.Add(merchantReciept);
                                await _context.SaveChangesAsync();
                            }
                            else
                            {
                                merchantReciept = _context.MerchantReciepts.Find(GetMerchant(merchantt.MerchantID, TimeNow()));
                                m = _context.Merchants.Find(merchantt.MerchantID);

                                merchantReciept.TotalOfReciept += AddTo_TotalOfReciept;
                                merchantReciept.CurrentDebt += AddTo_CurrentDebt;
                            }
                            merchantt.PreviousDebts += AddTo_PreviousDebts;
                            var boat = _context.Boats.Where(c => c.BoatName == "المخزن" && c.BoatLicenseNumber == "0").FirstOrDefault();
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
                                        BoatID = boat.BoatID,
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
                                var existingFish = TodaysMerchantRecItems.Where(c => c.FishID == Individualfish.FishID && c.BoatID == boat.BoatID).FirstOrDefault();
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
                                            BoatID = boat.BoatID
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
                                        BoatID = boat.BoatID
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

                            if (GetMerchant(merchantt.MerchantID, TimeNow()) == 0)
                            {
                                ImerchantReciept = new IMerchantReciept() { Date = TimeNow(), MerchantID = merchantt.MerchantID, TotalOfReciept = AddTo_TotalOfReciept };
                                _context.Add(ImerchantReciept);
                                await _context.SaveChangesAsync();
                            }
                            else
                            {
                                ImerchantReciept = _context.IMerchantReciept.Find(GetMerchant(merchantt.MerchantID, TimeNow()));
                                ImerchantReciept.TotalOfReciept += AddTo_TotalOfReciept;

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

                                   

                                    IMerchantRecieptItem NewIMerchantRecieptItems = new IMerchantRecieptItem()
                                    {
                                        IMerchantRecieptID = ImerchantReciept.IMerchantRecieptID,
                                        FishID = fish.FishID,
                                        ProductionTypeID = Produc.ProductionTypeID,
                                        Qty = splitItemQty[z],
                                        UnitPrice = unitPrices[i],
                                        AmountId = amountId

                                    };
                                    _context.IMerchantRecieptItem.Add(NewIMerchantRecieptItems);


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
                                }
                            }
                            else
                            {

                                var fish = _context.Fishes.Single(x => x.FishName == Fishes[i]);
                                var Produc = _context.ProductionTypes.Single(x => x.ProductionName == Productions[i]);
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
                                    IMerchantRecieptItem NewIMerchantRecieptItems = new IMerchantRecieptItem()
                                    {
                                        IMerchantRecieptID = ImerchantReciept.IMerchantRecieptID,
                                        FishID = fish.FishID,
                                        ProductionTypeID = Produc.ProductionTypeID,
                                        Qty = Convert.ToDouble(qtys[i]),
                                        UnitPrice = unitPrices[i],

                                    }; _context.IMerchantRecieptItem.Add(NewIMerchantRecieptItems);
                                }
                                _context.SaveChanges();


                                var s = _context.Stocks.ToList().Where(c => c.FishID == fish.FishID).FirstOrDefault();
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
                                            FishID = fish.FishID,
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
                                        FishID = fish.FishID,
                                        ProductionTypeID = Produc.ProductionTypeID,
                                        Qty = Convert.ToDouble(qtys[i])
                                    };
                                    _context.Stocks.Add(stock);
                                }

                            }
                            _context.SaveChanges();



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
                        var boat = _context.Boats.Where(c => c.BoatName == "المخزن" && c.BoatLicenseNumber == "0").FirstOrDefault();
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


                return Json(new { message = "success" });

            }
            //return View(model);
            return Json(new { message = "fail" });
        }


        public int GetMerchant(int? id, DateTime date)
        {
            Merchant m = _context.Merchants.Find(id);
            if (m.IsOwner == false)
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
    }

}