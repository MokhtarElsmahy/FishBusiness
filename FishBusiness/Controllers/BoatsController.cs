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
                    DebtsOfMulfunction = model.DebtsOfMulfunction,
                    BoatNumber = model.BoatNumber,
                    DebtsOfStartingWork = model.DebtsOfStartingWork
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

            db.Boats.Remove(boat);
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
                DebtsOfMulfunction = model.DebtsOfMulfunction,
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
                    DebtsOfMulfunction = model.DebtsOfMulfunction,
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

    }
}
