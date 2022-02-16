using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FishBusiness;
using FishBusiness.Models;
using Microsoft.AspNetCore.Authorization;

namespace FishBusiness.Controllers
{
    [Authorize]
    public class CofigsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CofigsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Cofigs
        public async Task<IActionResult> Index()
        {
            return View(await _context.Cofigs.ToListAsync());
        }

        // GET: Cofigs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cofig = await _context.Cofigs
                .FirstOrDefaultAsync(m => m.ID == id);
            if (cofig == null)
            {
                return NotFound();
            }

            return View(cofig);
        }

        // GET: Cofigs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cofigs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Value")] Cofig cofig)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cofig);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cofig);
        }

        // GET: Cofigs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cofig = await _context.Cofigs.FindAsync(id);
            if (cofig == null)
            {
                return NotFound();
            }
            return View(cofig);
        }

        // POST: Cofigs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Value")] Cofig cofig)
        {
            if (id != cofig.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cofig);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CofigExists(cofig.ID))
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
            return View(cofig);
        }

        // GET: Cofigs/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var cofig = await _context.Cofigs
        //        .FirstOrDefaultAsync(m => m.ID == id);
        //    if (cofig == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(cofig);
        //}

        // POST: Cofigs/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var cofig = await _context.Cofigs.FindAsync(id);
            if (cofig == null)
            {
                return NotFound();
            }
            _context.Cofigs.Remove(cofig);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CofigExists(int id)
        {
            return _context.Cofigs.Any(e => e.ID == id);
        }
    }
}
