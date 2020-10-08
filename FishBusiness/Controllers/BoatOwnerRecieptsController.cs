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
    public class BoatOwnerRecieptsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BoatOwnerRecieptsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BoatOwnerReciepts
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.BoatOwnerReciepts.Include(b => b.Boat).Include(b => b.Sarha);
            return View(await applicationDbContext.ToListAsync());
        }
        public DateTime TimeNow()
        {
            TimeZone localZone = TimeZone.CurrentTimeZone;
            DateTime currentDate = DateTime.Now;
            DateTime currentUTC =
           localZone.ToUniversalTime(currentDate);
            return currentUTC.AddHours(2);
        }
        // GET: BoatOwnerReciepts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boatOwnerReciept = await _context.BoatOwnerReciepts
                .Include(b => b.Boat)
                .Include(b => b.Sarha)
                .FirstOrDefaultAsync(m => m.BoatOwnerRecieptID == id);
            if (boatOwnerReciept == null)
            {
                return NotFound();
            }
            ViewBag.Items = _context.BoatOwnerItems.Where(i => i.BoatOwnerRecieptID == id).Include(x => x.Fish).Include(x => x.ProductionType);
            return View(boatOwnerReciept);
        }

        // GET: BoatOwnerReciepts/Create
        public IActionResult Create()
        {
            ViewData["BoatID"] = new SelectList(_context.Boats.Where(b => b.IsActive == true), "BoatID", "BoatName");
            ViewData["ProductionTypeID"] = new SelectList(_context.ProductionTypes, "ProductionTypeID", "ProductionName");

            ViewData["FishID"] = new SelectList(_context.Fishes, "FishID", "FishName");

            //
            ViewData["MerchantID"] = new SelectList(_context.Merchants.Where(m => m.IsFromOutsideCity == false), "MerchantID", "MerchantName");
            // commission
            // ViewBag.Commission = _context.Cofigs.Find(1);
            ViewBag.Commission = _context.Cofigs.Find(2);
            return View();
        }
        public IActionResult GetBoatItems(int? id)
        {
            var LastRecieptOfBoat = _context.BoatOwnerReciepts.Where(r => r.BoatID == id).Max(rs => rs.BoatOwnerRecieptID);
            var itemsOfLastReciept = _context.BoatOwnerItems.Where(i => i.BoatOwnerRecieptID == LastRecieptOfBoat).Include(i => i.Fish);
            var res = itemsOfLastReciept.Select(r => new { fishId = r.Fish.FishID, fishName = r.Fish.FishName });
            return Json(res);

        }
        public IActionResult GetMerchant(int? id, DateTime date)
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
                        return Json(new { RecID = recID, debts = m.PreviousDebts, owner = "no" });
                    }
                    return Json(new { RecID = 0, debts = m.PreviousDebts, owner = "no" });
                }

                return Json(new { RecID = 0, debts = m.PreviousDebts, owner = "no" });

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
                        return Json(new { RecID = recID, debts = 0, owner = "owner" });
                    }
                    return Json(new { RecID = 0, debts = 0, owner = "owner" });
                }

                return Json(new { RecID = 0, debts = 0, owner = "owner" });
            }

        }
        public IActionResult SaveItems(MerchantRecieptItemVm item)
        {


            // items.Add(item);
            Fish fish = _context.Fishes.Find(item.FishID);
            Boat boat = _context.Boats.Find(item.BoatID);
            ProductionType production = _context.ProductionTypes.Find(item.ProductionTypeID);
            var BoatOwnerRecItem = _context.BoatOwnerItems.Include(i => i.Fish).Where(i => i.BoatOwnerRecieptID == item.BoatOwnerRecID && i.Fish.FishName == fish.FishName && i.BoatOwnerReciept.BoatID == boat.BoatID && i.ProductionTypeID == production.ProductionTypeID).FirstOrDefault();
            if (BoatOwnerRecItem.Qty >= item.Qty)
            {
                var res = new { boatName = boat.BoatName, productionName = production.ProductionName, fishName = fish.FishName, qty = item.Qty, unitPrice = item.UnitPrice, total = item.Qty * item.UnitPrice, original = BoatOwnerRecItem.Qty };
                return Json(res);

            }
            else
            {

                return Json(new { message = "moreqty" });
            }



        }

        public IActionResult GetFishPrice(int fishId, int RecieptID)
        {

            var item = _context.BoatOwnerItems.SingleOrDefault(i => i.BoatOwnerRecieptID == RecieptID && i.FishID == fishId);
            var res = new { unitPrice = item.UnitPrice };

            return Json(res);

        }

        [HttpPost]
        public async Task<IActionResult> MCreate(MerRecCreateVm model)
        {
            if (model.MerchantID != 0)
            {


                var FishesCookie = model.FishNames.TrimEnd(model.FishNames[model.FishNames.Length - 1]);
                var ProductionTypesCookie = model.ProductionTypes.TrimEnd(model.ProductionTypes[model.ProductionTypes.Length - 1]);
                var qtysCookie = model.qtys.TrimEnd(model.qtys[model.qtys.Length - 1]);
                var unitpricesCookie = model.unitprices.TrimEnd(model.unitprices[model.unitprices.Length - 1]);
                var boatsCookie = model.boats.TrimEnd(model.boats[model.boats.Length - 1]);

                string[] Fishes = FishesCookie.Split(",").Select(c => Convert.ToString(c)).ToArray();
                string[] Productions = ProductionTypesCookie.Split(",").Select(c => Convert.ToString(c)).ToArray();
                string[] boats = boatsCookie.Split(",").Select(c => Convert.ToString(c)).ToArray();
                int[] qtys = qtysCookie.Split(",").Select(c => Convert.ToInt32(c)).ToArray();
                decimal[] unitPrices = unitpricesCookie.Split(",").Select(c => Convert.ToDecimal(c)).ToArray();

                var merchantt = _context.Merchants.Find(model.MerchantID);


                if (merchantt.IsOwner == false)
                {


                    MerchantReciept merchantReciept;
                    Merchant m;
                    if (model.RecID == 0)
                    {
                        merchantReciept = new MerchantReciept() { Date = model.Date, payment = model.payment, TotalOfReciept = model.TotalOfReciept, MerchantID = model.MerchantID, CurrentDebt = model.CurrentDebt };
                        _context.Add(merchantReciept);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        merchantReciept = _context.MerchantReciepts.Find(model.RecID);
                        m = _context.Merchants.Find(model.MerchantID);
                        merchantReciept.TotalOfReciept += model.TotalOfReciept;
                        merchantReciept.payment += model.payment;
                    }


                    for (int i = 0; i < Fishes.Length; i++)
                    {
                        var fish = _context.Fishes.Single(x => x.FishName == Fishes[i]);
                        var Produc = _context.ProductionTypes.Single(x => x.ProductionName == Productions[i]);
                        var boat = _context.Boats.Single(x => x.BoatName == boats[i]);

                        var TodaysMerchantRecItems = _context.MerchantRecieptItems.Include(c => c.MerchantReciept).ToList().Where(c => c.MerchantReciept.Date.ToShortDateString() == TimeNow().ToShortDateString() && c.MerchantRecieptID == merchantReciept.MerchantRecieptID).ToList();
                        var existingFish = TodaysMerchantRecItems.Where(c => c.FishID == fish.FishID && c.BoatID == boat.BoatID).FirstOrDefault();
                        if (existingFish != null)
                        {
                            if (existingFish.ProductionTypeID == Produc.ProductionTypeID)
                            {
                                existingFish.Qty += qtys[i];
                            }
                            else
                            {
                                MerchantRecieptItem MerchantRecieptItems = new MerchantRecieptItem()
                                {
                                    MerchantRecieptID = merchantReciept.MerchantRecieptID,
                                    FishID = fish.FishID,
                                    ProductionTypeID = Produc.ProductionTypeID,
                                    Qty = qtys[i],
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
                                FishID = fish.FishID,
                                ProductionTypeID = Produc.ProductionTypeID,
                                Qty = qtys[i],
                                UnitPrice = unitPrices[i],
                                BoatID = boat.BoatID
                            };
                            _context.MerchantRecieptItems.Add(MerchantRecieptItems);
                        }
                        #region trying
                        //if (TodaysMerchantRecItems != null && TodaysMerchantRecItems.Count > 0)
                        //{
                        //    for (int j = 0; j < TodaysMerchantRecItems.Count; j++)
                        //    {
                        //        if (TodaysMerchantRecItems.ElementAt(j).FishID == fish.FishID && TodaysMerchantRecItems.ElementAt(j).ProductionTypeID == Produc.ProductionTypeID && TodaysMerchantRecItems.ElementAt(j).BoatID == boat.BoatID)
                        //        {
                        //            TodaysMerchantRecItems.ElementAt(j).Qty += qtys[i];
                        //        }
                        //        else
                        //        {
                        //            MerchantRecieptItem MerchantRecieptItems = new MerchantRecieptItem()
                        //            {
                        //                MerchantRecieptID = merchantReciept.MerchantRecieptID,
                        //                FishID = fish.FishID,
                        //                ProductionTypeID = Produc.ProductionTypeID,
                        //                Qty = qtys[i],
                        //                UnitPrice = unitPrices[i],
                        //                BoatID = boat.BoatID
                        //            };
                        //            _context.MerchantRecieptItems.Add(MerchantRecieptItems);
                        //        }


                        //    }


                        //}
                        //else
                        //{
                        //    MerchantRecieptItem MerchantRecieptItems = new MerchantRecieptItem()
                        //    {
                        //        MerchantRecieptID = merchantReciept.MerchantRecieptID,
                        //        FishID = fish.FishID,
                        //        ProductionTypeID = Produc.ProductionTypeID,
                        //        Qty = qtys[i],
                        //        UnitPrice = unitPrices[i],
                        //        BoatID = boat.BoatID
                        //    };
                        //    _context.MerchantRecieptItems.Add(MerchantRecieptItems);

                        //} 
                        #endregion
                        _context.SaveChanges();

                    }
                    m = _context.Merchants.Find(model.MerchantID);
                    m.PreviousDebts = model.CurrentDebt + model.TotalOfReciept;
                    merchantReciept.CurrentDebt = model.CurrentDebt;

                    await _context.SaveChangesAsync();

                    return Json(new { message = "success", id = merchantReciept.MerchantRecieptID });
                }
                else
                {

                    #region MyRegion
                    //IMerchantReciept ImerchantReciept = new IMerchantReciept() { Date = model.Date, MerchantID = model.MerchantID, TotalOfReciept = model.TotalOfReciept };
                    //_context.Add(ImerchantReciept);
                    ////PaidForMerchant p = new PaidForMerchant() { MerchantID = model.MerchantID, Date = model.Date, Payment = model.payment, PreviousDebtsForMerchant = model.CurrentDebt, IsCash = !data.IsCash, IsPaidForUs = false, PersonID = 3 };
                    ////_context.Add(p);

                    ////var person = _context.People.Find(3);
                    ////person.credit -= model.payment;
                    //Merchant merchant = _context.Merchants.Find(model.MerchantID);
                    //merchant.PreviousDebtsForMerchant = model.CurrentDebt;
                    //_context.SaveChanges(); 
                    #endregion

                    //-----------------------------------------------------
                    IMerchantReciept ImerchantReciept;
                    if (model.RecID == 0)
                    {
                        ImerchantReciept = new IMerchantReciept() { Date = model.Date, MerchantID = model.MerchantID, TotalOfReciept = model.TotalOfReciept };
                        _context.Add(ImerchantReciept);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        ImerchantReciept = _context.IMerchantReciept.Find(model.RecID);
                        ImerchantReciept.TotalOfReciept += model.TotalOfReciept;

                    }
                    //--------------------------------------------------------



                    var FishesCookiee = model.FishNames.TrimEnd(model.FishNames[model.FishNames.Length - 1]);
                    var ProductionTypesCookiee = model.ProductionTypes.TrimEnd(model.ProductionTypes[model.ProductionTypes.Length - 1]);
                    var qtysCookiee = model.qtys.TrimEnd(model.qtys[model.qtys.Length - 1]);
                    var unitpricesCookiee = model.unitprices.TrimEnd(model.unitprices[model.unitprices.Length - 1]);
                    var boatsCookiee = model.boats.TrimEnd(model.boats[model.boats.Length - 1]);

                    string[] Fishess = FishesCookiee.Split(",").Select(c => Convert.ToString(c)).ToArray();
                    string[] Productionss = ProductionTypesCookiee.Split(",").Select(c => Convert.ToString(c)).ToArray();
                    int[] qtyss = qtysCookiee.Split(",").Select(c => Convert.ToInt32(c)).ToArray();
                    decimal[] unitPricess = unitpricesCookiee.Split(",").Select(c => Convert.ToDecimal(c)).ToArray();
                    string[] boatss = boatsCookiee.Split(",").Select(c => Convert.ToString(c)).ToArray();

                    for (int i = 0; i < Fishess.Length; i++)
                    {
                        var fish = _context.Fishes.Single(x => x.FishName == Fishess[i]);
                        var Produc = _context.ProductionTypes.Single(x => x.ProductionName == Productionss[i]);


                        var TodaysIMerchantRecItems = _context.IMerchantRecieptItem.Include(c => c.IMerchantReciept).ToList().Where(c => c.IMerchantReciept.Date.ToShortDateString() == TimeNow().ToShortDateString() && c.IMerchantRecieptID == ImerchantReciept.IMerchantRecieptID).ToList();
                        var IMerchantRecieptItems = TodaysIMerchantRecItems.Where(c => c.FishID == fish.FishID && c.UnitPrice == unitPricess[i]).FirstOrDefault();
                        // IMerchantRecieptItem IMerchantRecieptItems;
                        if (IMerchantRecieptItems != null)
                        {
                            if (IMerchantRecieptItems.ProductionTypeID == Produc.ProductionTypeID)
                            {
                                IMerchantRecieptItems.Qty += qtyss[i];
                            }
                            else
                            {
                                IMerchantRecieptItem NewIMerchantRecieptItems = new IMerchantRecieptItem()
                                {
                                    IMerchantRecieptID = ImerchantReciept.IMerchantRecieptID,
                                    FishID = fish.FishID,
                                    ProductionTypeID = Produc.ProductionTypeID,
                                    Qty = qtyss[i],
                                    UnitPrice = unitPricess[i],

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
                                Qty = qtyss[i],
                                UnitPrice = unitPricess[i],

                            }; _context.IMerchantRecieptItem.Add(NewIMerchantRecieptItems);
                        }
                        _context.SaveChanges();
                        var s = _context.Stocks.ToList().Where(c => c.FishID == fish.FishID).FirstOrDefault();
                        if (s != null)
                        {
                            if (s.ProductionTypeID == Produc.ProductionTypeID)
                            {
                                s.Qty += qtyss[i];

                            }
                            else
                            {
                                Stock stoc = new Stock()
                                {
                                    FishID = fish.FishID,
                                    ProductionTypeID = Produc.ProductionTypeID,
                                    Qty = qtys[i],
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
                                Qty = qtyss[i]
                            };
                            _context.Stocks.Add(stock);
                        }



                        _context.SaveChanges();


                    }

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
                    return Json(new { message = "success", id = ImerchantReciept.IMerchantRecieptID });
                    //return RedirectToAction(nameof(Details),new { id= ImerchantReciept.IMerchantRecieptID });

                }
            }
            ViewData["MerchantID"] = new SelectList(_context.Merchants, "MerchantID", "MerchantName", model.MerchantID);
            //return View(model);
            return Json(new { message = "fail" });
        }
        // POST: BoatOwnerReciepts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BoatOwnerReciept boatOwnerReciept)
        {


            var TotalBeforePaymentCookie = boatOwnerReciept.TotalBeforePaying;
            var commisionCookie = boatOwnerReciept.Commission;
            var PercentageCommissionCookie = boatOwnerReciept.PercentageCommission;
            var PaidFromDebtsCookie = boatOwnerReciept.PaidFromDebts;
            var TotalProductionCookie = boatOwnerReciept.TotalAfterPaying;
            //find latest sarha related to selected boat
            var sarhaId = _context.Sarhas.Where(x => x.BoatID == boatOwnerReciept.BoatID && x.IsFinished == false).Max(x => x.SarhaID);
            boatOwnerReciept.SarhaID = sarhaId;
            boatOwnerReciept.TotalBeforePaying = Convert.ToDecimal(TotalBeforePaymentCookie);
            boatOwnerReciept.Commission = Convert.ToDecimal(commisionCookie);
            boatOwnerReciept.PercentageCommission = Convert.ToInt32(PercentageCommissionCookie);
            boatOwnerReciept.TotalAfterPaying = Convert.ToDecimal(TotalProductionCookie);
            boatOwnerReciept.IsCalculated = false;
            boatOwnerReciept.IsCollected = false;
            boatOwnerReciept.PaidFromDebts = Convert.ToDecimal(PaidFromDebtsCookie);
            Person p = _context.People.Find(1);
            p.credit += Convert.ToDecimal(commisionCookie); ;
            // Subtracting Paid From Halek
            var boat = _context.Boats.Find(boatOwnerReciept.BoatID);
            boat.DebtsOfHalek -= Convert.ToDecimal(PaidFromDebtsCookie);
            // Salary for Each One
            var sarha = _context.Sarhas.Find(sarhaId);
            var IndividualSalary = (Convert.ToDecimal(TotalProductionCookie) / 2) / sarha.NumberOfFishermen;
            // Calculating Final Income 
            // for shared boats
            decimal FinalIncome = Convert.ToDecimal(TotalProductionCookie);
            // 5 -> Shared Boat ... We will change it later
            //if (boat.TypeID == 2)
            //{
            //    FinalIncome = (Convert.ToDecimal(TotalProductionCookie) / 2) - IndividualSalary;
            //    boat.IncomeOfSharedBoat += FinalIncome;
            //    IncomesOfSharedBoat i = new IncomesOfSharedBoat()
            //    {
            //        BoatID = boat.BoatID,
            //        Date = TimeNow(),
            //        Income = FinalIncome
            //    };
            //    _context.IncomesOfSharedBoats.Add(i);
            //    p.credit += FinalIncome;
            //}
            //// for ordinary boats
            //else
            //FinalIncome = Convert.ToDecimal(TotalProductionCookie);
            boatOwnerReciept.FinalIncome = FinalIncome;
            _context.Add(boatOwnerReciept);
            await _context.SaveChangesAsync();
            // Cookies Of Receipt Items
            var FishesCookie = Request.Cookies["FishNames"];
            //بورى,,طوبار,شركوس/سوبيد

            var ProductionTypesCookie = Request.Cookies["ProductionTypes"];
            var qtysCookie = Request.Cookies["qtys"];
            var unitpricesCookie = Request.Cookies["unitprices"];
            var pricesCookie = Request.Cookies["prices"];
            string[] Fishes = FishesCookie.Split(",").Select(c => Convert.ToString(c)).ToArray();


            string[] Productions = ProductionTypesCookie.Split(",").Select(c => Convert.ToString(c)).ToArray();
            string[] qtys = qtysCookie.Split(",").Select(c => Convert.ToString(c)).ToArray();
            decimal[] unitPrices = unitpricesCookie.Split(",").Select(c => Convert.ToDecimal(c)).ToArray();
            decimal[] prices = pricesCookie.Split(",").Select(c => Convert.ToDecimal(c)).ToArray();



            var latestReceipt = _context.BoatOwnerReciepts.Max(x => x.BoatOwnerRecieptID);
            for (int i = 0; i < Fishes.Length; i++)
            {

                string[] splitItem = Fishes[i].Split("/").Select(c => Convert.ToString(c)).ToArray();
                if (splitItem.Length > 1)
                {
                    Guid amountID = Guid.NewGuid();
                    int[] splitItemQty = qtys[i].Split("/").Select(c => Convert.ToInt32(c)).ToArray();
                    for (int j = 0; j < splitItem.Length; j++)
                    {

                       
                        var fish = _context.Fishes.Single(x => x.FishName == splitItem[j]);
                        var Produc = _context.ProductionTypes.Single(x => x.ProductionName == Productions[i]);

                        BoatOwnerItem boatOwnerItem = new BoatOwnerItem()
                        {
                            BoatOwnerRecieptID = latestReceipt,
                            FishID = fish.FishID,
                            ProductionTypeID = Produc.ProductionTypeID,
                            Qty = splitItemQty[j],
                            UnitPrice = unitPrices[i],
                            AmountId = amountID
                        };
                        _context.BoatOwnerItems.Add(boatOwnerItem);
                        _context.SaveChanges();
                    }
                }
                else
                {
                    var fish = _context.Fishes.Single(x => x.FishName == Fishes[i]);
                    var Produc = _context.ProductionTypes.Single(x => x.ProductionName == Productions[i]);

                    BoatOwnerItem boatOwnerItem = new BoatOwnerItem()
                    {
                        BoatOwnerRecieptID = latestReceipt,
                        FishID = fish.FishID,
                        ProductionTypeID = Produc.ProductionTypeID,
                        Qty = int.Parse(qtys[i]),
                        UnitPrice = unitPrices[i]
                      
                    };
                    _context.BoatOwnerItems.Add(boatOwnerItem);
                    _context.SaveChanges();
                }
            }
            var sarhaa = _context.Sarhas.Find(sarhaId);
            sarhaa.IsFinished = true;
            sarha.DateOfSarha = boatOwnerReciept.Date;

            //-----------------------------------------------------------------------------------------------------------------------------------------
            //اضافة مبدئيه لمع حدوث اكسبشن مع احمدفتح الله ويجب على المالك تعديل بيانات السرحه قبل حساب فاتورة المركب
            //وسيتم تعديل التاريخ بعد عمل فاتوره المركب ليصبح بنفس تاريخ عمل الفاتوره
            Sarha s = new Sarha() { BoatID = boat.BoatID, IsFinished = false, NumberOfBoxes = 0, NumberOfFishermen = 6, DateOfSarha = TimeNow() };
            _context.Sarhas.Add(s);
            //-------------------------------------------------------------------------------------------------------------------------------------------
            _context.SaveChanges();
            //return RedirectToAction(nameof(Index));
            //return RedirectToAction("Details",new { id= latestReceipt });
            return Json(new { message = "success", id = boatOwnerReciept.BoatID, reciept = boatOwnerReciept.BoatOwnerRecieptID });

        }
        public static Guid ToGuid(int value)
        {
            byte[] bytes = new byte[16];
            BitConverter.GetBytes(value).CopyTo(bytes, 0);
            return new Guid(bytes);
        }

        public IActionResult Distribute(int id)
        {
            var rec = _context.BoatOwnerReciepts.Include(c=>c.Boat).Where(c=>c.BoatOwnerRecieptID == id).FirstOrDefault();
            ViewBag.Merchants = new SelectList( _context.Merchants.Where(c => c.IsFromOutsideCity == false).ToList(),"MerchantID","MerchantName");

            DistributionVm model = new DistributionVm();
            model.BoatOwnerReciept = rec;
            model.NormalboatOwnerItems = _context.BoatOwnerItems.Include(c=>c.Fish).Include(c=>c.ProductionType).Where(c => c.BoatOwnerRecieptID == rec.BoatOwnerRecieptID && c.AmountId == null).ToList();
            model.AmountboatOwnerItems = _context.BoatOwnerItems.Include(c=>c.Fish).Include(c=>c.ProductionType).Where(c => c.BoatOwnerRecieptID == rec.BoatOwnerRecieptID && c.AmountId != null).ToList();

            var results = from p in model.AmountboatOwnerItems
                          group p.BoatOwnerItemID by p.AmountId into g
                          select new AmountViewModel { AmountId = g.Key, items = g };

            model.Amounts = results;

            return View(model);
        }
        // GET: BoatOwnerReciepts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var boatOwnerReciept = await _context.BoatOwnerReciepts.FindAsync(id);
            if (boatOwnerReciept == null)
            {
                return NotFound();
            }
            // Increase Halek Again
            var boat = _context.Boats.Find(boatOwnerReciept.BoatID);
            boat.DebtsOfHalek += Convert.ToDecimal(boatOwnerReciept.PaidFromDebts);

            // Decrease Shared Boat Income
            if (boat.TypeID == 2)
            {
                boat.IncomeOfSharedBoat -= boatOwnerReciept.FinalIncome;
            }

            _context.BoatOwnerReciepts.Remove(boatOwnerReciept);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool BoatOwnerRecieptExists(int id)
        {
            return _context.BoatOwnerReciepts.Any(e => e.BoatOwnerRecieptID == id);
        }
    }
}
