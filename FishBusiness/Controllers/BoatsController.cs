﻿using System;
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
            return View(await db.Boats.Where(x => x.IsActive == true).Include(x => x.BoatType).ToListAsync());
        }
        public async Task<IActionResult> InActiveBoats()
        {
            return View(await db.Boats.Where(x => x.IsActive == false).Include(x => x.BoatType).ToListAsync());
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
        public async Task<IActionResult> Create(BoatVM model)
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
                    ,
                    IsActive = true
                };
                db.Boats.Add(boat);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Types = new SelectList(await db.BoatTypes.ToListAsync(), "TypeID", "TypeName", model.TypeID);
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
            if (id == 0 || id == null)
            {
                boat = new Boat();
            }
            boat = db.Boats.Include(b => b.BoatType).FirstOrDefault(c => c.BoatID == id);
            //ViewBag.expenses = db.Expenses.Where(e => e.BoatID == id).Sum(r => r.Price);
            ViewBag.expenses = db.Boats.Where(e => e.BoatID == id).FirstOrDefault().TotalOfExpenses;
            return PartialView(boat);
        }


        [HttpGet]
        public IActionResult Edit(int? id)
        {
            Boat model = db.Boats.Find(id);

            BoatVM boat = new BoatVM()
            {
                BoatID = model.BoatID,
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
            ViewBag.Types = new SelectList(db.BoatTypes.ToList(), "TypeID", "TypeName", model.TypeID);
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
                    BoatID = model.BoatID,
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
                TypeID = model.TypeID,
                BoatImage = model.BoatImage,
                BoatLeader = model.BoatLeader,
                BoatLicenseNumber = model.BoatLicenseNumber,
                DebtsOfHalek = model.DebtsOfHalek,
                // DebtsOfMulfunction = model.DebtsOfMulfunction,
                BoatNumber = model.BoatNumber,
                DebtsOfStartingWork = model.DebtsOfStartingWork,
                IncomeOfSharedBoat = model.IncomeOfSharedBoat,
                TotalOfExpenses = model.TotalOfExpenses

            };
            var recs = db.BoatOwnerReciepts.Where(r => r.BoatID == model.BoatID && r.IsCalculated == true && r.IsCollected == true).ToList();
            var ExternalRecs = db.ExternalReceipts.Where(r => r.BoatID == model.BoatID).ToList();
            var expenses = db.Expenses.Where(b => b.BoatID == model.BoatID).ToList();
            var NotCalculatedRec = db.BoatOwnerReciepts.Where(r => r.BoatID == model.BoatID && r.IsCalculated == false).OrderBy(r => r.BoatOwnerRecieptID).ToList();

            profileVM.BoatInfo = boat;

            profileVM.BoatRecs = recs;
            profileVM.ExternalRecs = ExternalRecs;
            profileVM.NotCalculatedRec = NotCalculatedRec;

            profileVM.BoatExpenses = expenses;


            return View(profileVM);
        }


        public async Task<IActionResult> CalculateShow(int? id)
        {


            var rec = await db.BoatOwnerReciepts.FindAsync(id);
            bool flag = false;
            ViewBag.sameNum = false;
            if (db.BoatOwnerReciepts.Where(c => c.BoatID == rec.BoatID && c.IsCollected == false && c.IsCalculated == false).Count() > 0)
            {
                var recs = db.BoatOwnerReciepts.Include(c => c.Sarha).Where(c => c.BoatID == rec.BoatID && c.IsCollected == false && c.IsCalculated == false).ToList();
                List<BoatOwnerReciept> diff = new List<BoatOwnerReciept>();

                for (int i = 0; i < recs.Count; i++)
                {
                    var kar = recs.ElementAt(i);
                    for (int j = 0; j < recs.Count; j++)
                    {
                        if (kar.Sarha.NumberOfFishermen.Equals(recs.ElementAt(j).Sarha.NumberOfFishermen))
                        {
                            if (recs.ElementAt(i).Sarha.NumberOfFishermen.Equals(recs.ElementAt(j).Sarha.NumberOfFishermen))
                            {
                                continue;

                            }


                        }
                        else if (!diff.Contains(kar))
                        {

                            diff.Add(kar);
                        }
                    }
                }
                if (diff.Count() > 0)
                {
                    //means recs not equal in #Fishermen
                    flag = true;
                }

            }

            var boat = db.Boats.Find(rec.BoatID);
            ViewBag.boatType = boat.TypeID;
            ViewBag.flag = false;
            if (db.BoatOwnerReciepts.Where(c => c.BoatID == rec.BoatID && c.IsCollected == false && c.IsCalculated == false).Count() == 1)
            {
                ViewBag.flag = true;
                return PartialView(rec);
            }

            if (flag == false)
            {
                //num of fishermen is the same
                ViewBag.sameNum = true;
                ViewBag.TotalOfRecs = db.BoatOwnerReciepts.Where(c => c.BoatID == rec.BoatID && c.IsCollected == false && c.IsCalculated == false).Sum(c => c.TotalAfterPaying);
                return PartialView(rec);
            }
            return PartialView(rec);
        }
        [HttpGet]
        public IActionResult CalcDebts(int? id, decimal PaidFromDebts, decimal total)
        {
            if (id == null)
            {
                return NotFound();
            }
            var rec = db.BoatOwnerReciepts.Find(id);
            var totalAfterDebt = 0.0m;
            if (rec == null)
            {
                return NotFound();
            }
            if (total != 0.0m)
            {
                totalAfterDebt = total - PaidFromDebts;
            }
            else
            {
                totalAfterDebt = rec.TotalAfterPaying - PaidFromDebts;
            }
            var share = totalAfterDebt / 2;
            var sarhaID = rec.SarhaID;
            var sarha = db.Sarhas.Find(sarhaID);
            var IndividulaSalary = share / sarha.NumberOfFishermen;




            return Json(new { message = "success", salary = IndividulaSalary });
        }

        [HttpGet]
        public IActionResult ChangeSalary(int? id, decimal newSalary, decimal Alltotal)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rec = db.BoatOwnerReciepts.Find(id);
            if (rec == null)
            {
                return NotFound();
            }

            var sarhaID = rec.SarhaID;
            var sarha = db.Sarhas.Find(sarhaID);
            var total = newSalary * 2 * sarha.NumberOfFishermen;
            var halek = 0.0m;
            if (Alltotal != 0.0m)
            {

                halek = Alltotal - total;
            }
            else
            {
                halek = rec.TotalAfterPaying - total;
            }



            return Json(new { message = "success", halek = halek, total = total });
        }

        [HttpPost]
        public IActionResult SaveRec(int? id, decimal individualSalary, decimal halek, decimal total, decimal expense, string flag, int NumberOfFisherMen)
        {

            var rec = db.BoatOwnerReciepts.Find(id);


            var boat = db.Boats.Find(rec.BoatID);
            boat.DebtsOfHalek -= halek;

            if (flag == "True") // fishermen number is the same 
            {


                var recs = db.BoatOwnerReciepts.Where(c => c.BoatID == boat.BoatID && c.IsCollected == false && c.IsCalculated == false).ToList();
                var sumOfTotalAfterPayment = total;
                var finalIncome = 0.0m;
                if (boat.TypeID == 2) //shared boat
                {

                    finalIncome = sumOfTotalAfterPayment / 2;
                    var LeaderSalary = 0.0m;
                    if (NumberOfFisherMen != 0)
                    {
                        LeaderSalary = finalIncome / NumberOfFisherMen;
                    }
                    else
                    {

                         LeaderSalary = finalIncome / 6;
                    }
                    boat.IncomeOfSharedBoat += finalIncome - LeaderSalary;
                    IncomesOfSharedBoat inc = new IncomesOfSharedBoat() { BoatID = boat.BoatID, Date = DateTime.Now, Income = finalIncome - LeaderSalary };
                    db.IncomesOfSharedBoats.Add(inc);

                }
                else
                {
                    if (expense != 0.0m)
                    {

                        boat.TotalOfExpenses -= expense;
                    }
                }

                foreach (var item in recs)
                {
                    item.IsCollected = true;
                    item.IsCalculated = true;
                }


            }
            else
            {
                rec.PaidFromDebts = halek;
                rec.TotalAfterPaying = total;
                rec.IsCalculated = true;
                db.SaveChanges();
                if (db.BoatOwnerReciepts.Where(c => c.BoatID == boat.BoatID && c.IsCollected == false && c.IsCalculated == false).Count() == 0)//check if last rec 
                {
                    var recs = db.BoatOwnerReciepts.Where(c => c.BoatID == boat.BoatID && c.IsCollected == false && c.IsCalculated == true).ToList();
                    var sumOfTotalAfterPayment = recs.Sum(c => c.TotalAfterPaying);
                    var finalIncome = 0.0m;
                    if (boat.TypeID == 2)
                    {

                        finalIncome = sumOfTotalAfterPayment / 2;
                        var LeaderSalary = 0.0m;
                        if (NumberOfFisherMen != 0)
                        {
                            LeaderSalary = finalIncome / NumberOfFisherMen;
                        }
                        else
                        {

                            LeaderSalary = finalIncome / 6;
                        }
                        boat.IncomeOfSharedBoat += finalIncome - LeaderSalary;
                        IncomesOfSharedBoat inc = new IncomesOfSharedBoat() { BoatID = boat.BoatID, Date = DateTime.Now, Income = finalIncome - LeaderSalary };
                        db.IncomesOfSharedBoats.Add(inc);

                    }
                    else
                    {
                        if (expense != 0.0m)
                        {

                            boat.TotalOfExpenses -= expense;
                        }
                    }

                    foreach (var item in recs)
                    {
                        item.IsCollected = true;
                    }
                    db.SaveChanges();
                }
            }


            db.SaveChanges();





            return Json(new { message = "success", current = boat.DebtsOfHalek, income = boat.IncomeOfSharedBoat, cexpense = boat.TotalOfExpenses });
        }



    }
}
