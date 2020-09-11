﻿using System;
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
    public class ISellerRecieptsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ISellerRecieptsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ISellerReciepts
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ISellerReciepts.Include(i => i.Merchant);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ISellerReciepts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var iSellerReciept = await _context.ISellerReciepts
                .Include(i => i.Merchant)
                .FirstOrDefaultAsync(m => m.ISellerRecieptID == id);

            if (iSellerReciept == null)
            {
                return NotFound();
            }
            var items = _context.ISellerRecieptItems.Include(i => i.Fish).Include(i => i.ProductionType).Where(i => i.ISellerRecieptID == iSellerReciept.ISellerRecieptID).ToList();
            ISellerRecieptDetailsVm model = new ISellerRecieptDetailsVm();
            model.ISellerReciept = iSellerReciept;
            model.ISellerRecieptItems = items;
            ViewBag.MerchantDebts = _context.Merchants.Where(c => c.MerchantID == iSellerReciept.MerchantID).FirstOrDefault().PreviousDebts;
            return View(model);
        }

        public async Task<IActionResult> Moneytization(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var iSellerReciept = await _context.ISellerReciepts
                .Include(i => i.Merchant)
                .FirstOrDefaultAsync(m => m.ISellerRecieptID == id);

            if (iSellerReciept == null)
            {
                return NotFound();
            }
            var items = _context.ISellerRecieptItems.Include(i => i.Fish).Include(i => i.ProductionType).Where(i => i.ISellerRecieptID == iSellerReciept.ISellerRecieptID).ToList();
            ISellerRecieptDetailsVm model = new ISellerRecieptDetailsVm();
            model.ISellerReciept = iSellerReciept;
            model.ISellerRecieptItems = items;
            ViewBag.PreviousDebts = _context.Merchants.FirstOrDefault(m => m.MerchantID == iSellerReciept.MerchantID).PreviousDebts;
            return View(model);
        }

        [HttpPost]
        public  IActionResult MoneytizationSave(int IsellerRecieptID, double TotalOfPrices, double Commision, double TotalOfPricesAfterCommision, double PaidFromDebt, double DebtsAfterCommisionAndPayment,string Pricescookie)
        {
            
            
            
            decimal[] prices = Pricescookie.Split(",").Select(c => Convert.ToDecimal(c)).ToArray();
          

            var iSellerReciept =  _context.ISellerReciepts
                .Include(i => i.Merchant)
                .FirstOrDefault(m => m.ISellerRecieptID == IsellerRecieptID);
            iSellerReciept.Commision = Commision;
            iSellerReciept.PaidFromDebt = PaidFromDebt;
            iSellerReciept.TotalOfPrices = TotalOfPrices;
            iSellerReciept.DateOfMoneytization = DateTime.Now;
            PaidForMerchant p = new PaidForMerchant() { Date = DateTime.Now, IsCash = true, MerchantID = iSellerReciept.MerchantID, Payment = (decimal)PaidFromDebt, IsPaidForUs = true , PreviousDebtsForMerchant = (decimal)(DebtsAfterCommisionAndPayment - PaidFromDebt) };
            _context.PaidForMerchant.Add(p);
            
            if (iSellerReciept == null)
            {
                return NotFound();
            }
            var items = _context.ISellerRecieptItems.Include(i => i.Fish).Include(i => i.ProductionType).Where(i => i.ISellerRecieptID == iSellerReciept.ISellerRecieptID).ToList();
            
            for (int i =0; i<items.Count();i++)
            {
                items[i].UnitPrice = prices[i];
            }
            _context.SaveChanges();

            var debts = DebtsAfterCommisionAndPayment - PaidFromDebt;
            var merchant = _context.Merchants.FirstOrDefault(m => m.MerchantID == iSellerReciept.MerchantID);
            merchant.PreviousDebts = (decimal)debts;
            _context.SaveChanges();

            return Json(new { message = "success", totalDebts = debts });
           
        }

        public async Task<IActionResult> GetStockInfo(int StockID,string FishName)
        {

            var stock = await _context.Stocks.Include(s=>s.Fish).Include(s=>s.ProductionType).Where(s => s.StockID == StockID && s.Fish.FishName == FishName).FirstOrDefaultAsync();
            
            if (stock !=null)
            {
                return Json(new { message = "success", totalWeight = stock.TotalWeight , productionTypeId = stock.ProductionTypeID });
            }
            else
            {
                return Json(new { message = "fail" });
            }
           
        }
        // GET: ISellerReciepts/Create
        public async Task<IActionResult> Create()
        {
            ViewData["MerchantID"] = new SelectList(_context.Merchants.Where(m=>m.IsFromOutsideCity==true), "MerchantID", "MerchantName");
            ViewData["ProductionTypeID"] = new SelectList(_context.ProductionTypes, "ProductionTypeID", "ProductionName");
            
            IsellerRecVm model = new IsellerRecVm();
            model.Stocks = await _context.Stocks.Include(r=>r.Fish).ToListAsync();

            return View(model);
        }

        // POST: ISellerReciepts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Create(int MerchantID,DateTime Date, double CarPrice)
        {
            if (ModelState.IsValid)
            {
                ISellerReciept sellerReciept = new ISellerReciept();
                sellerReciept.MerchantID = MerchantID;
                sellerReciept.Date = Date;
                sellerReciept.CarPrice = CarPrice;
                sellerReciept.CarDistination = _context.Merchants.Find(MerchantID).Address;
                _context.Add(sellerReciept);
                 _context.SaveChanges();


                var FishesCookie = Request.Cookies["FishNames"];
                var ProductionTypesCookie = Request.Cookies["ProductionTypes"];
                var qtysCookie = Request.Cookies["qtys"];
                //var unitpricesCookie = Request.Cookies["unitprices"];
                var NOfBoxesCookie = Request.Cookies["NOfBoxes"];
                string[] Fishes = FishesCookie.Split(",").Select(c => Convert.ToString(c)).ToArray();
                string[] Productions = ProductionTypesCookie.Split(",").Select(c => Convert.ToString(c)).ToArray();
                int[] qtys = qtysCookie.Split(",").Select(c => Convert.ToInt32(c)).ToArray();
                //decimal[] unitPrices = unitpricesCookie.Split(",").Select(c => Convert.ToDecimal(c)).ToArray();
                int[] NOfBoxes = NOfBoxesCookie.Split(",").Select(c => Convert.ToInt32(c)).ToArray();

                var latestReceipt = _context.ISellerReciepts.Max(x => x.ISellerRecieptID);
                for (int i = 0; i < Fishes.Length; i++)
                {
                    var fish = _context.Fishes.Single(x => x.FishName == Fishes[i]);
                    var Produc = _context.ProductionTypes.Single(x => x.ProductionName == Productions[i]);

                    ISellerRecieptItem ISellerRecieptItem = new ISellerRecieptItem()
                    {
                        ISellerRecieptID = latestReceipt,
                        FishID = fish.FishID,
                        ProductionTypeID = Produc.ProductionTypeID,
                        Qty = qtys[i],
                        BoxQty = NOfBoxes[i],
                    };
                    var stock =  _context.Stocks.Where(i => i.FishID == fish.FishID && i.ProductionTypeID == Produc.ProductionTypeID).FirstOrDefault();
                    stock.Qty -= qtys[i];
                    if (stock.ProductionTypeID == 3)
                    {
                        stock.TotalWeight -= qtys[i];
                    }
                    _context.SaveChanges();
                    stock = _context.Stocks.Where(i => i.FishID == fish.FishID && i.ProductionTypeID == Produc.ProductionTypeID).FirstOrDefault();
                    if (stock.Qty == 0)
                    {
                        _context.Stocks.Remove(stock);
                        
                    }
                    _context.ISellerRecieptItems.Add(ISellerRecieptItem);
                    _context.SaveChanges();
                }
            
                return Json(new { message = "success" , id= latestReceipt});
              
            }

            return Json(new { message = "fail" });
          
        }

        // GET: ISellerReciepts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var iSellerReciept = await _context.ISellerReciepts.FindAsync(id);
            if (iSellerReciept == null)
            {
                return NotFound();
            }
            ViewData["MerchantID"] = new SelectList(_context.Merchants, "MerchantID", "MerchantName", iSellerReciept.MerchantID);
            return View(iSellerReciept);
        }

        // POST: ISellerReciepts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ISellerRecieptID,Date,MerchantID")] ISellerReciept iSellerReciept)
        {
            if (id != iSellerReciept.ISellerRecieptID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(iSellerReciept);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ISellerRecieptExists(iSellerReciept.ISellerRecieptID))
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
            ViewData["MerchantID"] = new SelectList(_context.Merchants, "MerchantID", "MerchantName", iSellerReciept.MerchantID);
            return View(iSellerReciept);
        }

        // GET: ISellerReciepts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var iSellerReciept = await _context.ISellerReciepts
                .Include(i => i.Merchant)
                .FirstOrDefaultAsync(m => m.ISellerRecieptID == id);
            if (iSellerReciept == null)
            {
                return NotFound();
            }

            return View(iSellerReciept);
        }

        // POST: ISellerReciepts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var iSellerReciept = await _context.ISellerReciepts.FindAsync(id);
            _context.ISellerReciepts.Remove(iSellerReciept);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ISellerRecieptExists(int id)
        {
            return _context.ISellerReciepts.Any(e => e.ISellerRecieptID == id);
        }
    }
}
