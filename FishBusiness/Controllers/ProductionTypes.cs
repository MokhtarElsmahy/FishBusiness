using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FishBusiness.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace FishBusiness.Controllers
{
    [Authorize]
    public class ProductionTypes : Controller
    {
        private readonly ApplicationDbContext db;

        public ProductionTypes(ApplicationDbContext _db)
        {
            db = _db;
        }
        // GET: ProductionType
        public async Task<IActionResult> Index()
        {
            return View(await db.ProductionTypes.ToListAsync());
        }

        // GET: ProductionType/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProductionType/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProductionType/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductionType model)
        {
           
                if (ModelState.IsValid)
                {
                     db.ProductionTypes.Add(model);
                     await db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return View(model);
                }
          
        }

        // GET: ProductionType/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var pt = await db.ProductionTypes.FindAsync(id);
            if (pt==null)
            {
                return NotFound();
            }
            return View(pt);
        }

        // POST: ProductionType/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductionType model)
        {
            if (ModelState.IsValid)
            {
                // db.ProductionTypes.Update(model);
                db.Entry(model).State = EntityState.Modified;
               await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }
        }

        // GET: ProductionType/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id==null)
            {
                return NotFound();
            }
            var pt = await db.ProductionTypes.FindAsync(id);
            if (pt==null)
            {
                return NotFound();
            }
             db.ProductionTypes.Remove(pt);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
           
        }

        // POST: ProductionType/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
