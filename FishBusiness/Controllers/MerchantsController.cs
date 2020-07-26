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
    public class MerchantsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MerchantsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Merchants
        public async Task<IActionResult> Index()
        {
            return View(await _context.Merchants.ToListAsync());
        }

        // GET: Merchants/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            MerchantDetailsVm model = new MerchantDetailsVm();
            var merchant = await _context.Merchants
                .FirstOrDefaultAsync(m => m.MerchantID == id);
            if (merchant == null)
            {
                return NotFound();
            }
            model.Merchant = merchant;
            model.MerchantReciepts =await _context.MerchantReciepts.Include(x=>x.Merchant).Where(x => x.MerchantID == id).ToListAsync();
           
            return View(model);
        }

        public IActionResult TableItems()
        {
            return PartialView(_context.Merchants.ToList());
        }


        // GET: Merchants/Create
        public IActionResult Create()
        {
            return PartialView();
        }

        // POST: Merchants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateC([Bind("MerchantID,MerchantName,PreviousDebts,Phone,Address")] Merchant merchant)
        {
            if (ModelState.IsValid)
            {
                _context.Add(merchant);
                await _context.SaveChangesAsync();
                return Json(new {message="success" });
            }
            return View(merchant);
        }

        // GET: Merchants/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var merchant = await _context.Merchants.FindAsync(id);
            if (merchant == null)
            {
                return NotFound();
            }
            return PartialView(merchant);
        }

        // POST: Merchants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> cEdit([Bind("MerchantID,MerchantName,PreviousDebts,Phone,Address")] Merchant merchant)
        {
           

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(merchant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MerchantExists(merchant.MerchantID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                //return RedirectToAction(nameof(Index));
                return Json(new { message="success"});
            }
            return View(merchant);
        }

        // GET: Merchants/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var merchant = await _context.Merchants
                .FirstOrDefaultAsync(m => m.MerchantID == id);
            if (merchant == null)
            {
                return NotFound();
            }

            _context.Merchants.Remove(merchant);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Merchants/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var merchant = await _context.Merchants.FindAsync(id);
        //    _context.Merchants.Remove(merchant);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool MerchantExists(int id)
        {
            return _context.Merchants.Any(e => e.MerchantID == id);
        }
    }
}