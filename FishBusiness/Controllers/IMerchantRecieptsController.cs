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
using System.Net.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace FishBusiness.Controllers
{
    [Authorize]
    public class IMerchantRecieptsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public IMerchantRecieptsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
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
        // GET: IMerchantReciepts
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.IMerchantReciept.Include(i => i.Merchant).Where(c=>c.Date.Date == TimeNow().Date);
            return View(await applicationDbContext.ToListAsync());
        } 
        
        public IActionResult GetIMerchantRecHistory(DateTime date)
        {
            //System.Threading.Thread.Sleep(3000);
            var applicationDbContext = _context.IMerchantReciept.Where(c => c.Date.Date == date.Date).Include(i => i.Merchant).ToList();
            return PartialView( applicationDbContext);
        }

        // GET: IMerchantReciepts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var iMerchantReciept = await _context.IMerchantReciept
                .Include(i => i.Merchant)
                .FirstOrDefaultAsync(m => m.IMerchantRecieptID == id);


            if (iMerchantReciept == null)
            {
                return NotFound();
            }

            ImerchRecDetailsVm model = new ImerchRecDetailsVm();
            model.ImerchantReciept = iMerchantReciept;

            model.NormalIMerchantItems = _context.IMerchantRecieptItem.Include(c => c.Fish).Include(c => c.ProductionType).Where(c => c.IMerchantRecieptID == iMerchantReciept.IMerchantRecieptID && c.AmountId == null).ToList();
            model.AmountIMerchantItems = _context.IMerchantRecieptItem.Include(c => c.Fish).Include(c => c.ProductionType).Where(c => c.IMerchantRecieptID == iMerchantReciept.IMerchantRecieptID && c.AmountId != null).ToList();


            var results = from p in model.AmountIMerchantItems
                          group p.IMerchantRecieptItemID by p.AmountId into g
                          select new AmountVm { AmountId = g.Key, items = g };

            model.Amounts = results;


            return View(model);
        }

        // GET: IMerchantReciepts/Create
        public IActionResult Create()
        {
            ViewData["MerchantID"] = new SelectList(_context.Merchants.Where(c => c.IsOwner == false && c.IsFromOutsideCity == false), "MerchantID", "MerchantName");
            ViewData["FishID"] = new SelectList(_context.Fishes, "FishID", "FishName");


            ViewData["ProductionTypeID"] = new SelectList(_context.ProductionTypes, "ProductionTypeID", "ProductionName");
            IMerchantRecVM model = new IMerchantRecVM();



            return View(model);
        }

        [HttpPost]

        public IActionResult Create(DataVM data)
        {
            if (data.MerchantID != 0)
            {
                //IMerchantReciept ImerchantReciept = new IMerchantReciept() { Date = data.Date, MerchantID = data.MerchantID, TotalOfReciept = data.TotalOfReciept };
                //_context.Add(ImerchantReciept);
                //PaidForMerchant p = new PaidForMerchant() { MerchantID = data.MerchantID, Date = data.Date, Payment = data.payment, PreviousDebtsForMerchant = data.CurrentDebt, IsCash = !data.IsCash , IsPaidForUs=false , PersonID=3 };
                //_context.Add(p);

                //var person = _context.People.Find(3);
                //person.credit -= data.payment;
                Merchant merchant = _context.Merchants.Find(data.MerchantID);
                merchant.PreviousDebtsForMerchant = data.CurrentDebt;
                _context.SaveChanges();


                var FishesCookie = data.FishNames.TrimEnd(data.FishNames[data.FishNames.Length - 1]);
                var ProductionTypesCookie = data.ProductionTypes.TrimEnd(data.ProductionTypes[data.ProductionTypes.Length - 1]);
                var qtysCookie = data.qtys.TrimEnd(data.qtys[data.qtys.Length - 1]);
                var unitpricesCookie = data.unitprices.TrimEnd(data.unitprices[data.unitprices.Length - 1]);

                string[] Fishes = FishesCookie.Split(",").Select(c => Convert.ToString(c)).ToArray();
                string[] Productions = ProductionTypesCookie.Split(",").Select(c => Convert.ToString(c)).ToArray();

                string[] qtys = qtysCookie.Split(",").Select(c => Convert.ToString(c)).ToArray();
                decimal[] unitPrices = unitpricesCookie.Split(",").Select(c => Convert.ToDecimal(c)).ToArray();

                IMerchantReciept imerchantReciept;
                Merchant m;
                if (data.RecID == 0)
                {
                    imerchantReciept = new IMerchantReciept() { Date = data.Date, MerchantID = data.MerchantID, TotalOfReciept = data.TotalOfReciept };
                    _context.Add(imerchantReciept);
                    _context.SaveChanges();
                }
                else
                {
                    imerchantReciept = _context.IMerchantReciept.Find(data.RecID);
                    m = _context.Merchants.Find(data.MerchantID);
                    imerchantReciept.TotalOfReciept += data.TotalOfReciept;

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



                            IMerchantRecieptItem NewIMerchantRecieptItems = new IMerchantRecieptItem()
                            {
                                IMerchantRecieptID = imerchantReciept.IMerchantRecieptID,
                                FishID = fish.FishID,
                                ProductionTypeID = Produc.ProductionTypeID,
                                Qty = splitItemQty[j],
                                UnitPrice = unitPrices[i],
                                AmountId = amountID

                            };
                            _context.IMerchantRecieptItem.Add(NewIMerchantRecieptItems);

                            _context.SaveChanges();

                            var s = _context.Stocks.Where(c => c.FishID == fish.FishID).FirstOrDefault();
                            if (s != null)
                            {
                                if (s.ProductionTypeID == Produc.ProductionTypeID)
                                {

                                    s.Qty += splitItemQty[j];


                                }
                                else
                                {
                                    var ss = _context.Stocks.Where(c => c.FishID == fish.FishID && c.ProductionTypeID == Produc.ProductionTypeID).FirstOrDefault();
                                    if (ss != null)
                                    {
                                        ss.Qty += double.Parse(qtys[i]);

                                    }
                                    else
                                    {

                                        Stock stoc = new Stock()
                                        {
                                            FishID = fish.FishID,
                                            ProductionTypeID = Produc.ProductionTypeID,
                                            Qty = splitItemQty[j],
                                            Date = imerchantReciept.Date
                                        };
                                        _context.Stocks.Add(stoc);
                                    }

                                    //Stock stoc = new Stock()
                                    //{
                                    //    FishID = fish.FishID,
                                    //    ProductionTypeID = Produc.ProductionTypeID,
                                    //    Qty = splitItemQty[j],
                                    //    Date = imerchantReciept.Date
                                    //};
                                    //_context.Stocks.Add(stoc);
                                }
                            }
                            else
                            {

                                Stock stock = new Stock()
                                {
                                    FishID = fish.FishID,
                                    ProductionTypeID = Produc.ProductionTypeID,
                                    Qty = splitItemQty[j],
                                };
                                _context.Stocks.Add(stock);
                            }

                            var stockHistory = _context.StockHistories.ToList().Where(c => c.Date.ToShortDateString() == TimeNow().ToShortDateString() && c.FishID == fish.FishID && c.ProductionTypeID == Produc.ProductionTypeID).FirstOrDefault();
                            if (stockHistory != null)
                            {
                                stockHistory.Total += splitItemQty[j];
                            }
                            else
                            {
                                StockHistory history = new StockHistory() { FishID = fish.FishID, ProductionTypeID = Produc.ProductionTypeID, Total = splitItemQty[j], Date = TimeNow() };
                                _context.StockHistories.Add(history);
                            }

                        }
                    }
                    else
                    {



                        var fish = _context.Fishes.Single(x => x.FishName == Fishes[i]);
                        var Produc = _context.ProductionTypes.Single(x => x.ProductionName == Productions[i]);

                        var TodaysIMerchantRecItems = _context.IMerchantRecieptItem.Include(c => c.IMerchantReciept).ToList().Where(c => c.IMerchantReciept.Date.ToShortDateString() == TimeNow().ToShortDateString() && c.IMerchantRecieptID == imerchantReciept.IMerchantRecieptID).ToList();
                        var IMerchantRecieptItems = TodaysIMerchantRecItems.Where(c => c.FishID == fish.FishID && c.UnitPrice == unitPrices[i]).FirstOrDefault();
                        if (IMerchantRecieptItems != null)
                        {
                            if (IMerchantRecieptItems.ProductionTypeID == Produc.ProductionTypeID)
                            {
                                IMerchantRecieptItems.Qty += double.Parse(qtys[i]);
                            }
                            else
                            {
                                IMerchantRecieptItem NewIMerchantRecieptItems = new IMerchantRecieptItem()
                                {
                                    IMerchantRecieptID = imerchantReciept.IMerchantRecieptID,
                                    FishID = fish.FishID,
                                    ProductionTypeID = Produc.ProductionTypeID,
                                    Qty = double.Parse(qtys[i]),
                                    UnitPrice = unitPrices[i],

                                }; _context.IMerchantRecieptItem.Add(NewIMerchantRecieptItems);
                            }

                        }
                        else
                        {
                            IMerchantRecieptItem NewIMerchantRecieptItems = new IMerchantRecieptItem()
                            {
                                IMerchantRecieptID = imerchantReciept.IMerchantRecieptID,
                                FishID = fish.FishID,
                                ProductionTypeID = Produc.ProductionTypeID,
                                Qty = double.Parse(qtys[i]),
                                UnitPrice = unitPrices[i],

                            }; _context.IMerchantRecieptItem.Add(NewIMerchantRecieptItems);
                        }
                        _context.SaveChanges();




                        var s = _context.Stocks.Where(c => c.FishID == fish.FishID).FirstOrDefault();
                        if (s != null)
                        {
                            if (s.ProductionTypeID == Produc.ProductionTypeID)
                            {
                                s.Qty += double.Parse(qtys[i]);

                            }
                            else
                            {
                                var ss = _context.Stocks.Where(c => c.FishID == fish.FishID && c.ProductionTypeID == Produc.ProductionTypeID).FirstOrDefault();
                                if (ss != null)
                                {
                                    ss.Qty += double.Parse(qtys[i]);

                                }
                                else
                                {

                                    Stock stoc = new Stock()
                                    {
                                        FishID = fish.FishID,
                                        ProductionTypeID = Produc.ProductionTypeID,
                                        Qty = double.Parse(qtys[i]),
                                        Date = imerchantReciept.Date
                                    };
                                    _context.Stocks.Add(stoc);
                                }
                            }
                        }
                        else
                        {

                            Stock stock = new Stock()
                            {
                                FishID = fish.FishID,
                                ProductionTypeID = Produc.ProductionTypeID,
                                Qty = double.Parse(qtys[i])
                            };
                            _context.Stocks.Add(stock);
                        }


                        var stockHistory = _context.StockHistories.ToList().Where(c => c.Date.ToShortDateString() == TimeNow().ToShortDateString() && c.FishID == fish.FishID && c.ProductionTypeID == Produc.ProductionTypeID).FirstOrDefault();
                        if (stockHistory != null)
                        {
                            stockHistory.Total += double.Parse(qtys[i]);
                        }
                        else
                        {
                            StockHistory history = new StockHistory() { FishID = fish.FishID, ProductionTypeID = Produc.ProductionTypeID, Total = double.Parse(qtys[i]), Date = TimeNow() };
                            _context.StockHistories.Add(history);
                        }

                    }
                    //_context.IMerchantRecieptItem.Add(IMerchantRecieptItems);
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
                return Json(new { message = "success", id = imerchantReciept.IMerchantRecieptID });
                //return RedirectToAction(nameof(Details),new { id= ImerchantReciept.IMerchantRecieptID });
            }
            //  ViewData["MerchantID"] = new SelectList(_context.Merchants, "MerchantID", "MerchantName", model.MerchantID);
            //return View(model);
            return Json(new { message = "fail" });
        }
        public IActionResult GetMerchant(int? id)
        {
            Merchant m = _context.Merchants.Find(id);
            if (m != null)
            {
                return Json(new { debts = m.PreviousDebtsForMerchant });
            }
            return Json(new { debts = 0 });

        }

        public IActionResult GetMerchantt(int? id, DateTime date)
        {
            Merchant m = _context.Merchants.Find(id);

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
                    return Json(new { RecID = recID, debts = m.PreviousDebtsForMerchant });
                }
                return Json(new { RecID = 0, debts = m.PreviousDebtsForMerchant });
            }

            return Json(new { RecID = 0, debts = m.PreviousDebtsForMerchant });

        }

        public IActionResult SaveItems(IMerchantRecieptItem item)
        {



            // items.Add(item);
            Fish fish = _context.Fishes.Find(item.FishID);

            ProductionType production = _context.ProductionTypes.Find(item.ProductionTypeID);
            var res = new { productionName = production.ProductionName, fishName = fish.FishName, qty = item.Qty, unitPrice = item.UnitPrice, total = (decimal)item.Qty * item.UnitPrice };

            return Json(res);

        }


        // GET: IMerchantReciepts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var iMerchantReciept = await _context.IMerchantReciept.FindAsync(id);
            if (iMerchantReciept == null)
            {
                return NotFound();
            }
            ViewData["MerchantID"] = new SelectList(_context.Merchants, "MerchantID", "MerchantName", iMerchantReciept.MerchantID);
            return View(iMerchantReciept);
        }

        // POST: IMerchantReciepts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IMerchantRecieptID,Date,MerchantID,TotalOfReciept")] IMerchantReciept iMerchantReciept)
        {
            if (id != iMerchantReciept.IMerchantRecieptID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(iMerchantReciept);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IMerchantRecieptExists(iMerchantReciept.IMerchantRecieptID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MerchantID"] = new SelectList(_context.Merchants, "MerchantID", "MerchantName", iMerchantReciept.MerchantID);
            return View(iMerchantReciept);
        }

        // GET: IMerchantReciepts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {



            if (id == null)
            {
                return NotFound();
            }


            var user = await _userManager.GetUserAsync(User);
            var roles = await _userManager.GetRolesAsync(user);
            int PID = 1;
            if (roles.Contains("partner"))
            {
                PID = 2;

            }


            var IMerchantReciept = await _context.IMerchantReciept.Include(c => c.Merchant).FirstOrDefaultAsync(ww => ww.IMerchantRecieptID == id);
            if (IMerchantReciept == null)
            {
                return NotFound();
            }
            var IMerchantRecieptItems = _context.IMerchantRecieptItem.Where(x => x.IMerchantRecieptID == id).ToList();
            if (IMerchantRecieptItems.Any())
            {
                foreach (var item in IMerchantRecieptItems)
                {
                    var stockitem = _context.Stocks.FirstOrDefault(c => c.FishID == item.FishID && c.ProductionTypeID == item.ProductionTypeID);
                    if (stockitem != null)
                    {
                        if (item.ProductionTypeID == 2)//طاوله
                        {
                            stockitem.Qty -= item.Qty;
                        }
                        else
                        {
                            stockitem.TotalWeight -= item.Qty;
                            stockitem.Qty -= item.Qty;
                        }
                    }


                    var stockHistory = _context.StockHistories.ToList().Where(c => c.Date.ToShortDateString() == IMerchantReciept.Date.ToShortDateString() && c.FishID == item.FishID && c.ProductionTypeID == item.ProductionTypeID).FirstOrDefault();
                    if (stockHistory != null)
                    {
                        stockHistory.Total -= item.Qty;
                    }

                }



                _context.SaveChanges();

            }

            var person = _context.People.Find(PID);
            var mer = _context.Merchants.Find(IMerchantReciept.MerchantID);
            mer.PreviousDebtsForMerchant -= IMerchantReciept.TotalOfReciept;

            _context.Remove(IMerchantReciept);

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }

        // POST: IMerchantReciepts/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var iMerchantReciept = await _context.IMerchantReciept.FindAsync(id);
        //    _context.IMerchantReciept.Remove(iMerchantReciept);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool IMerchantRecieptExists(int id)
        {
            return _context.IMerchantReciept.Any(e => e.IMerchantRecieptID == id);
        }
    }
}
