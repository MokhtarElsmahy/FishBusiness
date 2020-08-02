using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FishBusiness.Models;
using FishBusiness.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FishBusiness.Controllers
{
    public class BoatsController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly IHostingEnvironment _hosting;
        public BoatsController(ApplicationDbContext _db, IHostingEnvironment hosting)
        {
            db = _db;
            _hosting = hosting;
        }
        public async Task<IActionResult> Index()
        {
            return View(await db.Boats.ToListAsync());
        }
        public async Task<IActionResult> ActiveBoats()
        {
            return View(await db.Boats.Where(x=>x.IsActive==true).Include(x => x.BoatType).ToListAsync());
        }
        public async Task<IActionResult> InActiveBoats()
        {
            return View(await db.Boats.Where(x => x.IsActive == false).Include(x=>x.BoatType).ToListAsync());
        }
        public async Task<IActionResult> SharedBoats()
        {
            // Find its id in your db
            return View(await db.Boats.Where(x => x.BoatType.TypeID == 2).ToListAsync());
        }
        public async Task<IActionResult> BasicBoats()
        {
            // Find its id in your db
            return View(await db.Boats.Where(x => x.BoatType.TypeID == 1).ToListAsync());
        }
        [HttpGet]
        public IActionResult Create()
        {
            //BoatVM model = new BoatVM();
            ViewBag.Types = new SelectList(db.BoatTypes.ToList(), "TypeID", "TypeName");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async  Task<IActionResult> Create(BoatVM model)
        {
         
            if (ModelState.IsValid)
            {
                model.BoatImage = "default.png";
                if (model.File != null)
                {
                    string uploads = Path.Combine(_hosting.WebRootPath, "img");
                    string fullPath = Path.Combine(uploads, model.File.FileName);
                    model.File.CopyTo(new FileStream(fullPath, FileMode.Create));
                    model.BoatImage = model.File.FileName;
                }
                Boat boat = new Boat()
                {
                    BoatName = model.BoatName,
                    TypeID = model.TypeID,
                    BoatImage = model.BoatImage,
                    BoatLeader = model.BoatLeader,
                    BoatLicenseNumber = model.BoatLicenseNumber,
                    DebtsOfHalek = model.DebtsOfHalek,
                   // DebtsOfMulfunction = model.DebtsOfMulfunction,
                    BoatNumber = model.BoatNumber,
                    DebtsOfStartingWork = model.DebtsOfStartingWork
                    ,IsActive = true
                };
                db.Boats.Add(boat);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Types = new SelectList(await db.BoatTypes.ToListAsync(), "TypeID", "TypeName",model.TypeID);
            return View(model);
        }

       
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boat = await db.Boats
                .FirstOrDefaultAsync(m => m.BoatID == id);
            if (boat == null)
            {
                return NotFound();
            }


            boat.IsActive = false;
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Restore(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boat = await db.Boats
                .FirstOrDefaultAsync(m => m.BoatID == id);
            if (boat == null)
            {
                return NotFound();
            }


            boat.IsActive = true;
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Details(int? id)
        {
            Boat boat;
            if (id==0 || id==null)
            {
                boat = new Boat();
            }
            boat = db.Boats.Include(b => b.BoatType).FirstOrDefault(c => c.BoatID == id);
            ViewBag.expenses = db.Expenses.Where(e => e.BoatID == id).Sum(r => r.Price);
            return PartialView(boat);
        }


        [HttpGet]
        public IActionResult Edit(int? id)
        {
            Boat model = db.Boats.Find(id);

            BoatVM boat = new BoatVM() {
                BoatID=model.BoatID,
                BoatName = model.BoatName,
                TypeID = model.TypeID,
                BoatImage = model.BoatImage,
                BoatLeader = model.BoatLeader,
                BoatLicenseNumber = model.BoatLicenseNumber,
                DebtsOfHalek = model.DebtsOfHalek,
               // DebtsOfMulfunction = model.DebtsOfMulfunction,
                BoatNumber = model.BoatNumber,
                DebtsOfStartingWork = model.DebtsOfStartingWork,
                
            };
            ViewBag.Types = new SelectList(db.BoatTypes.ToList(), "TypeID", "TypeName",model.TypeID);
            return View(boat);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BoatVM model)
        {

            if (ModelState.IsValid)
            {

               
                if (model.File != null)
                {
                    string uploads = Path.Combine(_hosting.WebRootPath, "img");
                    string fullPath = Path.Combine(uploads, model.File.FileName);
                    model.File.CopyTo(new FileStream(fullPath, FileMode.Create));
                    model.BoatImage = model.File.FileName;
                }
                
                Boat boat = new Boat()
                {
                    BoatID=model.BoatID,
                    BoatName = model.BoatName,
                    TypeID = model.TypeID,
                    BoatImage = model.BoatImage,
                    BoatLeader = model.BoatLeader,
                    BoatLicenseNumber = model.BoatLicenseNumber,
                    DebtsOfHalek = model.DebtsOfHalek,
                   // DebtsOfMulfunction = model.DebtsOfMulfunction,
                    BoatNumber = model.BoatNumber,
                    DebtsOfStartingWork = model.DebtsOfStartingWork
                };
                db.Entry(boat).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Types = new SelectList(await db.BoatTypes.ToListAsync(), "TypeID", "TypeName", model.TypeID);
            return View(model);
        }


        [HttpGet]
        public IActionResult Profile(int? id)
        {
           
            Boat model = db.Boats.Include(b => b.BoatType).FirstOrDefault(b => b.BoatID == id);
            ProfileVM profileVM = new ProfileVM();
            BoatInfoVM boat = new BoatInfoVM()
            {
                BoatID = model.BoatID,
                BoatName = model.BoatName,
                Type = model.BoatType.TypeName,
                TypeID =model.TypeID,
                BoatImage = model.BoatImage,
                BoatLeader = model.BoatLeader,
                BoatLicenseNumber = model.BoatLicenseNumber,
                DebtsOfHalek = model.DebtsOfHalek,
                // DebtsOfMulfunction = model.DebtsOfMulfunction,
                BoatNumber = model.BoatNumber,
                DebtsOfStartingWork = model.DebtsOfStartingWork,
                IncomeOfSharedBoat = model.IncomeOfSharedBoat,
                TotalOfExpenses=model.TotalOfExpenses

            };
            var recs = db.BoatOwnerReciepts.Where(r => r.BoatID == model.BoatID).ToList();
            var expenses = db.Expenses.Where(b => b.BoatID == model.BoatID).ToList();
           

            profileVM.BoatInfo = boat;
            profileVM.BoatRecs = recs;
            profileVM.BoatExpenses = expenses;
           

            return View(profileVM);
        }

        [HttpPost]
        public async Task<IActionResult> CalcDebts(decimal PaidValue, int BoatID)
        {
            if (BoatID == null)
            {
                return NotFound();
            }

            var boat = await db.Boats.FindAsync(BoatID);
            if (boat == null)
            {
                return NotFound();
            }
            boat.DebtsOfHalek -= PaidValue;
            await db.SaveChangesAsync();
            return Json(new { debts = boat.DebtsOfHalek });
        }

        [HttpPost]
        public async Task<IActionResult> CalcExpenses(decimal PaidValue, int BoatID)
        {
            if (BoatID == null)
            {
                return NotFound();
            }

            var boat = await db.Boats.FindAsync(BoatID);
            if (boat == null)
            {
                return NotFound();
            }
            boat.TotalOfExpenses -= PaidValue;
            await db.SaveChangesAsync();
            return Json(new { expenses = boat.TotalOfExpenses });
        }
    }
}
