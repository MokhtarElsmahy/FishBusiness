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
    public class MerchantRecieptsController : Controller
    {
        private readonly ApplicationDbContext _context;
        //private int Id;
        public MerchantRecieptsController(ApplicationDbContext context)
        {
            _context = context;

        }

        #region merchant
        // GET: MerchantReciepts
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.MerchantReciepts.Include(m => m.Merchant);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: MerchantReciepts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var merchantReciept = await _context.MerchantReciepts
                .Include(m => m.Merchant)
                .FirstOrDefaultAsync(m => m.MerchantRecieptID == id);
            if (merchantReciept == null)
            {
                return NotFound();
            }
            ViewBag.Items = _context.MerchantRecieptItems.Where(i => i.MerchantRecieptID == id).Include(x => x.Fish).Include(x => x.ProductionType).Include(x => x.Boat);

            MerchantRecDetailsVm model = new MerchantRecDetailsVm();
            model.MerchantReciept = merchantReciept;
            model.NormalMerchantItems = _context.MerchantRecieptItems.Include(c => c.Fish).Include(c => c.Boat).Include(c => c.ProductionType).Where(c => c.MerchantRecieptID == merchantReciept.MerchantRecieptID && c.AmountId == null).ToList();
            model.AmountMerchantItems = _context.MerchantRecieptItems.Include(c => c.Fish).Include(c => c.Boat).Include(c => c.ProductionType).Where(c => c.MerchantRecieptID == merchantReciept.MerchantRecieptID && c.AmountId != null).ToList();

            var results = from p in model.AmountMerchantItems
                          group p.MerchantRecieptItemID by p.AmountId into g
                          select new AmountVm { AmountId = g.Key, items = g };

            model.Amounts = results;



            return View(model);


        }

        public DateTime TimeNow()
        {
            TimeZone localZone = TimeZone.CurrentTimeZone;
            DateTime currentDate = DateTime.Now;
            DateTime currentUTC =
           localZone.ToUniversalTime(currentDate);
            return currentUTC.AddHours(2);
        }

        //public IActionResult GetMerchant(int? id, DateTime date)
        //{
        //    Merchant m = _context.Merchants.Find(id);
        //    if (m != null)
        //    {
        //        return Json(new { debts = m.PreviousDebts });
        //    }
        //    return Json(new {debts = 0 });

        //}

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
                        return Json(new { rec = recID, debts = m.PreviousDebts, owner = "no" });
                    }
                    return Json(new { rec = 0, debts = m.PreviousDebts, owner = "no" });
                }

                return Json(new { rec = 0, debts = m.PreviousDebts, owner = "no" });

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
        public IActionResult GetFishPrice(int fishId, int boatId)
        {

            var BoatrecId = _context.BoatOwnerReciepts.Where(i => i.BoatID == boatId).Max(i=>i.BoatOwnerRecieptID);
            var fishPrice = _context.BoatOwnerItems.SingleOrDefault(t => t.BoatOwnerRecieptID == BoatrecId && t.FishID == fishId);

            var res = new { unitPrice = fishPrice.UnitPrice };

            return Json(res);

        }
        public IActionResult GetBoatItems(int? id)
        {
            var LastRecieptOfBoat = _context.BoatOwnerReciepts.Where(r => r.BoatID == id).Max(rs => rs.BoatOwnerRecieptID);
            var itemsOfLastReciept = _context.BoatOwnerItems.Where(i => i.BoatOwnerRecieptID == LastRecieptOfBoat).Include(i => i.Fish);
            var res = itemsOfLastReciept.Select(r => new { fishId = r.Fish.FishID, fishName = r.Fish.FishName });
            return Json(res);

        }


        public IActionResult SaveItems(MerchantRecieptItem item)
        {



            // items.Add(item);
            Fish fish = _context.Fishes.Find(item.FishID);
            Boat boat = _context.Boats.Find(item.BoatID);
            ProductionType production = _context.ProductionTypes.Find(item.ProductionTypeID);
            var res = new { boatName = boat.BoatName, productionName = production.ProductionName, fishName = fish.FishName, qty = item.Qty, unitPrice = item.UnitPrice, total = (decimal) item.Qty * item.UnitPrice };

            return Json(res);

        }



        // GET: MerchantReciepts/Create
        public IActionResult Create()
        {
            ViewData["MerchantID"] = new SelectList(_context.Merchants.Where(m => m.IsFromOutsideCity == false), "MerchantID", "MerchantName");
            ViewData["Boats"] = new SelectList(_context.Boats.Where(b => b.IsActive == true &&b.BoatLicenseNumber!="0").ToList(), "BoatID", "BoatName");
            ViewData["ProductionTypeID"] = new SelectList(_context.ProductionTypes, "ProductionTypeID", "ProductionName");

            MRecVM vM = new MRecVM();
            return View(vM);
        }
        [HttpPost]
        public IActionResult Create(MerRecCreateVm model)
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
                double[] qtys = qtysCookie.Split(",").Select(c => Convert.ToDouble(c)).ToArray();
                decimal[] unitPrices = unitpricesCookie.Split(",").Select(c => Convert.ToDecimal(c)).ToArray();


                Merchant m = _context.Merchants.Find(model.MerchantID);
            
                 _context.SaveChanges();

                if (m.IsOwner==false)
                {
                    MerchantReciept merchantReciept;
                  
                    if (model.RecID == 0)
                    {
                        merchantReciept = new MerchantReciept() { Date = model.Date, payment = model.payment, TotalOfReciept = model.TotalOfReciept, MerchantID = model.MerchantID, CurrentDebt = model.CurrentDebt };
                        _context.Add(merchantReciept);
                         _context.SaveChanges();
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

                     _context.SaveChanges();
                    return Json(new { message = "success", id = merchantReciept.MerchantRecieptID });
                }
                else
                {
                    IMerchantReciept ImerchantReciept;
                    if (model.RecID == 0)
                    {
                        ImerchantReciept = new IMerchantReciept() { Date = model.Date, MerchantID = model.MerchantID, TotalOfReciept = model.TotalOfReciept };
                        _context.Add(ImerchantReciept);
                         _context.SaveChanges();
                    }
                    else
                    {
                        ImerchantReciept = _context.IMerchantReciept.Find(model.RecID);
                        ImerchantReciept.TotalOfReciept += model.TotalOfReciept;

                    }
                    for (int i = 0; i < Fishes.Length; i++)
                    {
                        var fish = _context.Fishes.Single(x => x.FishName == Fishes[i]);
                        var Produc = _context.ProductionTypes.Single(x => x.ProductionName == Productions[i]);


                        var TodaysIMerchantRecItems = _context.IMerchantRecieptItem.Include(c => c.IMerchantReciept).ToList().Where(c => c.IMerchantReciept.Date.ToShortDateString() == TimeNow().ToShortDateString() && c.IMerchantRecieptID == ImerchantReciept.IMerchantRecieptID).ToList();
                        var IMerchantRecieptItems = TodaysIMerchantRecItems.Where(c => c.FishID == fish.FishID && c.UnitPrice == unitPrices[i]).FirstOrDefault();
                        // IMerchantRecieptItem IMerchantRecieptItems;
                        if (IMerchantRecieptItems != null)
                        {
                            if (IMerchantRecieptItems.ProductionTypeID == Produc.ProductionTypeID)
                            {
                                IMerchantRecieptItems.Qty += qtys[i];
                            }
                            else
                            {
                                IMerchantRecieptItem NewIMerchantRecieptItems = new IMerchantRecieptItem()
                                {
                                    IMerchantRecieptID = ImerchantReciept.IMerchantRecieptID,
                                    FishID = fish.FishID,
                                    ProductionTypeID = Produc.ProductionTypeID,
                                    Qty = qtys[i],
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
                                Qty = qtys[i],
                                UnitPrice = unitPrices[i],

                            }; _context.IMerchantRecieptItem.Add(NewIMerchantRecieptItems);
                        }
                        _context.SaveChanges();
                        var s = _context.Stocks.ToList().Where(c => c.FishID == fish.FishID).FirstOrDefault();
                        if (s != null)
                        {
                            if (s.ProductionTypeID == Produc.ProductionTypeID)
                            {
                                s.Qty += qtys[i];

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
                                Qty = qtys[i]
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
                }

            }
            ViewData["MerchantID"] = new SelectList(_context.Merchants, "MerchantID", "MerchantName", model.MerchantID);
            //return View(model);
            return Json(new { message = "fail" });
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var merchantReciept = await _context.MerchantReciepts.Include(ww => ww.Merchant).FirstOrDefaultAsync(ww => ww.MerchantRecieptID == id);
            if (merchantReciept == null)
            {
                return NotFound();
            }
            var merchantRecieptItems = _context.MerchantRecieptItems.Where(x => x.MerchantRecieptID == id).ToList();
            _context.MerchantRecieptItems.RemoveRange(merchantRecieptItems);

            var merchant = _context.Merchants.Find(merchantReciept.MerchantID);
            merchant.PreviousDebts -= merchantReciept.TotalOfReciept;
            _context.MerchantReciepts.Remove(merchantReciept);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool MerchantRecieptExists(int id)
        {
            return _context.MerchantReciepts.Any(e => e.MerchantRecieptID == id);
        }

        #endregion



        #region Person
        [HttpGet]
        public IActionResult PersonRecieptCreate()
        {
            var boatids = _context.BoatOwnerReciepts.ToList().Where(c => c.Date.ToShortDateString() == TimeNow().ToShortDateString()).Select(c => c.BoatID);
            ViewData["Boats"] = new SelectList(_context.Boats.Where(b => b.IsActive == true && boatids.Contains(b.BoatID)).ToList(), "BoatID", "BoatName");
            ViewData["ProductionTypeID"] = new SelectList(_context.ProductionTypes, "ProductionTypeID", "ProductionName");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> PersonRecieptCreate(PersonReciept model)
        {
            if (ModelState.IsValid)
            {
                PersonReciept PersonReciept = new PersonReciept() { Date = model.Date, PersonName = model.PersonName, TotalPrice = model.TotalPrice };
                _context.PersonReciepts.Add(PersonReciept);
                await _context.SaveChangesAsync();

                #region Cookies

                var FishesCookie = Request.Cookies["FishNames"];
                var ProductionTypesCookie = Request.Cookies["ProductionTypes"];
                var qtysCookie = Request.Cookies["qtys"];
                var unitpricesCookie = Request.Cookies["unitprices"];
                var boatsCookie = Request.Cookies["boats"];

                string[] Fishes = FishesCookie.Split(",").Select(c => Convert.ToString(c)).ToArray();
                string[] Productions = ProductionTypesCookie.Split(",").Select(c => Convert.ToString(c)).ToArray();
                string[] boats = boatsCookie.Split(",").Select(c => Convert.ToString(c)).ToArray();
                double[] qtys = qtysCookie.Split(",").Select(c => Convert.ToDouble(c)).ToArray();
                decimal[] unitPrices = unitpricesCookie.Split(",").Select(c => Convert.ToDecimal(c)).ToArray();
                #endregion

                for (int i = 0; i < Fishes.Length; i++)
                {
                    var fish = _context.Fishes.Single(x => x.FishName == Fishes[i]);
                    var Produc = _context.ProductionTypes.Single(x => x.ProductionName == Productions[i]);
                    var boat = _context.Boats.Single(x => x.BoatName == boats[i]);
                    PersonRecieptItem PersonRecieptItem = new PersonRecieptItem()
                    {
                        PersonRecieptID = PersonReciept.PersonRecieptID,
                        FishID = fish.FishID,
                        ProductionTypeID = Produc.ProductionTypeID,
                        Qty = qtys[i],
                        UnitPrice = unitPrices[i],
                        BoatID = boat.BoatID
                    };

                    _context.PersonRecieptItems.Add(PersonRecieptItem);
                    await _context.SaveChangesAsync();
                }


                //فى الوقت الحالى يتم دفع الثمن الى المحصل 
                Person p = _context.People.Find(3);
                p.credit += PersonReciept.TotalPrice;

                await _context.SaveChangesAsync();


                return Json(new { message = "success" });
                //return RedirectToAction(nameof(Index));
            }

            //return View(model);
            return Json(new { message = "fail" });
        }


        public IActionResult SavePersonItems(PersonRecieptItem item)
        {



            // items.Add(item);
            Fish fish = _context.Fishes.Find(item.FishID);
            Boat boat = _context.Boats.Find(item.BoatID);
            ProductionType production = _context.ProductionTypes.Find(item.ProductionTypeID);
            var res = new { boatName = boat.BoatName, productionName = production.ProductionName, fishName = fish.FishName, qty = item.Qty, unitPrice = item.UnitPrice, total = (decimal)item.Qty * item.UnitPrice };

            return Json(res);

        }

        public async Task<IActionResult> PersonRecieptIndex()
        {
            var applicationDbContext = _context.PersonReciepts;
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> DetailsPersonReciept(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var PersonReciept = await _context.PersonReciepts

                .FirstOrDefaultAsync(m => m.PersonRecieptID == id);
            if (PersonReciept == null)
            {
                return NotFound();
            }
            ViewBag.Items = _context.PersonRecieptItems.Where(i => i.PersonRecieptID == id).Include(x => x.Fish).Include(x => x.ProductionType).Include(x => x.Boat);
            return View(PersonReciept);
        }


        public async Task<IActionResult> DeletePersonReciept(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var PersonReciept = await _context.PersonReciepts.FirstOrDefaultAsync(ww => ww.PersonRecieptID == id);
            if (PersonReciept == null)
            {
                return NotFound();
            }
            var PersonRecieptItems = _context.PersonRecieptItems.Where(x => x.PersonRecieptID == id).ToList();
            _context.PersonRecieptItems.RemoveRange(PersonRecieptItems);
            _context.Remove(PersonReciept);

            //يتم خصم ثمن الفاتوره من الحج مجدى واعطائها للمشترى
            Person p = _context.People.Find(1);
            p.credit -= PersonReciept.TotalPrice;

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        } 
        #endregion



    }
}
