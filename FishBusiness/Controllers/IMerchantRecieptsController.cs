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

namespace FishBusiness.Controllers
{
    public class IMerchantRecieptsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public IMerchantRecieptsController(ApplicationDbContext context)
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
        // GET: IMerchantReciepts
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.IMerchantReciept.Include(i => i.Merchant);
            return View(await applicationDbContext.ToListAsync());
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


            var items = _context.IMerchantRecieptItem.Include(i=>i.Fish).Include(i=>i.ProductionType).Where(i => i.IMerchantRecieptID == iMerchantReciept.IMerchantRecieptID).ToList();

            ImerchRecDetailsVm model = new ImerchRecDetailsVm();
            model.ImerchantReciept = iMerchantReciept;
            model.ImerchantRecieptItems = items;
            if (iMerchantReciept == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // GET: IMerchantReciepts/Create
        public IActionResult Create()
        {
            ViewData["MerchantID"] = new SelectList(_context.Merchants.Where(c=>c.IsOwner==false && c.IsFromOutsideCity==false), "MerchantID", "MerchantName");
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

                int[] qtys = qtysCookie.Split(",").Select(c => Convert.ToInt32(c)).ToArray();
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
                    var fish = _context.Fishes.Single(x => x.FishName == Fishes[i]);
                    var Produc = _context.ProductionTypes.Single(x => x.ProductionName == Productions[i]);

                    var TodaysIMerchantRecItems = _context.IMerchantRecieptItem.Include(c => c.IMerchantReciept).ToList().Where(c => c.IMerchantReciept.Date.ToShortDateString() == TimeNow().ToShortDateString() && c.IMerchantRecieptID == imerchantReciept.IMerchantRecieptID).ToList();
                    var IMerchantRecieptItems = TodaysIMerchantRecItems.Where(c => c.FishID == fish.FishID && c.UnitPrice == unitPrices[i]).FirstOrDefault();
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
                                IMerchantRecieptID = imerchantReciept.IMerchantRecieptID,
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
                            IMerchantRecieptID = imerchantReciept.IMerchantRecieptID,
                            FishID = fish.FishID,
                            ProductionTypeID = Produc.ProductionTypeID,
                            Qty = qtys[i],
                            UnitPrice = unitPrices[i],

                        }; _context.IMerchantRecieptItem.Add(NewIMerchantRecieptItems);
                    }
                    _context.SaveChanges();
                   
                    var s = _context.Stocks.Where(c => c.FishID == fish.FishID).FirstOrDefault();
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
                                Date = imerchantReciept.Date
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
            var res = new { productionName = production.ProductionName, fishName = fish.FishName, qty = item.Qty, unitPrice = item.UnitPrice, total = item.Qty * item.UnitPrice };

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

            var iMerchantReciept = await _context.IMerchantReciept
                .Include(i => i.Merchant)
                .FirstOrDefaultAsync(m => m.IMerchantRecieptID == id);
            if (iMerchantReciept == null)
            {
                return NotFound();
            }

            return View(iMerchantReciept);
        }

        // POST: IMerchantReciepts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var iMerchantReciept = await _context.IMerchantReciept.FindAsync(id);
            _context.IMerchantReciept.Remove(iMerchantReciept);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IMerchantRecieptExists(int id)
        {
            return _context.IMerchantReciept.Any(e => e.IMerchantRecieptID == id);
        }
    }
}
