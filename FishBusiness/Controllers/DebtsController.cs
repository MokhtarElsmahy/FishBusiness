using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FishBusiness.Models;

namespace FishBusiness.Controllers
{
    public class DebtsController : Controller
    {
        private readonly ApplicationDbContext db;

        public DebtsController(ApplicationDbContext _db)
        {
            db = _db;
        }
        public async Task<IActionResult> Index()
        {
            return View(await db.Debts.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Debt model)
        {
            if (ModelState.IsValid)
            {
                db.Debts.Add(model);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id==null)
            {
                return NotFound();
            }
            var d = await db.Debts.FindAsync(id);
            if (d==null)
            {
                return NotFound();
            }
            return View(d);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Debt model)
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

            var debt = await db.Debts
                .FirstOrDefaultAsync(m => m.DebtID == id);
            if (debt == null)
            {
                return NotFound();
            }

            db.Debts.Remove(debt);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
