using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FishBusiness;
using FishBusiness.Models;

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

            return View(boatOwnerReciept);
        }

        // GET: BoatOwnerReciepts/Create
        public IActionResult Create()
        {
            ViewData["BoatID"] = new SelectList(_context.Boats, "BoatID", "BoatLeader");
            ViewData["SarhaID"] = new SelectList(_context.Sarhas, "SarhaID", "SarhaID");
            return View();
        }

        // POST: BoatOwnerReciepts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BoatOwnerRecieptID,BoatID,SarhaID,TotalBeforePaying,Date,Commission,PaidFromDebts,TotalAfterPaying")] BoatOwnerReciept boatOwnerReciept)
        {
            if (ModelState.IsValid)
            {
                _context.Add(boatOwnerReciept);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BoatID"] = new SelectList(_context.Boats, "BoatID", "BoatLeader", boatOwnerReciept.BoatID);
            ViewData["SarhaID"] = new SelectList(_context.Sarhas, "SarhaID", "SarhaID", boatOwnerReciept.SarhaID);
            return View(boatOwnerReciept);
        }

        // GET: BoatOwnerReciepts/Edit/5
        public async Task<IActionResult> Edit(int? id)
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
            ViewData["BoatID"] = new SelectList(_context.Boats, "BoatID", "BoatLeader", boatOwnerReciept.BoatID);
            ViewData["SarhaID"] = new SelectList(_context.Sarhas, "SarhaID", "SarhaID", boatOwnerReciept.SarhaID);
            return View(boatOwnerReciept);
        }

        // POST: BoatOwnerReciepts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BoatOwnerRecieptID,BoatID,SarhaID,TotalBeforePaying,Date,Commission,PaidFromDebts,TotalAfterPaying")] BoatOwnerReciept boatOwnerReciept)
        {
            if (id != boatOwnerReciept.BoatOwnerRecieptID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(boatOwnerReciept);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BoatOwnerRecieptExists(boatOwnerReciept.BoatOwnerRecieptID))
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
            ViewData["BoatID"] = new SelectList(_context.Boats, "BoatID", "BoatLeader", boatOwnerReciept.BoatID);
            ViewData["SarhaID"] = new SelectList(_context.Sarhas, "SarhaID", "SarhaID", boatOwnerReciept.SarhaID);
            return View(boatOwnerReciept);
        }

        // GET: BoatOwnerReciepts/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

            return View(boatOwnerReciept);
        }

        // POST: BoatOwnerReciepts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var boatOwnerReciept = await _context.BoatOwnerReciepts.FindAsync(id);
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
