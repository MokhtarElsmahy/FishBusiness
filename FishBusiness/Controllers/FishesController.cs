using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FishBusiness.Models;
using Microsoft.AspNetCore.Authorization;

namespace FishBusiness.Controllers
{
    [Authorize]
    public class FishesController : Controller
    {
        private readonly ApplicationDbContext db;

        public FishesController(ApplicationDbContext _db)
        {
            db = _db;
        }
        public async Task<IActionResult> Index()
        {
            return View(await db.Fishes.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Fish model)
        {
            if (ModelState.IsValid)
            {
                db.Fishes.Add(model);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var d = await db.Fishes.FindAsync(id);
            if (d == null)
            {
                return NotFound();
            }
            return View(d);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Fish model)
        {
            if (ModelState.IsValid)
            {
                db.Entry(model).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var debt = await db.Fishes
                .FirstOrDefaultAsync(m => m.FishID == id);
            if (debt == null)
            {
                return NotFound();
            }

            db.Fishes.Remove(debt);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
