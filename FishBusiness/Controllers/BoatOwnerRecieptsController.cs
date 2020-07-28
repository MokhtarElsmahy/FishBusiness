using System;
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
            ViewBag.Items = _context.BoatOwnerItems.Where(i => i.BoatOwnerRecieptID == id).Include(x=>x.Fish).Include(x=>x.ProductionType);
            return View(boatOwnerReciept);
        }

        // GET: BoatOwnerReciepts/Create
        public IActionResult Create()
        {
            ViewData["BoatID"] = new SelectList(_context.Boats, "BoatID", "BoatName");
            ViewData["ProductionTypeID"] = new SelectList(_context.ProductionTypes, "ProductionTypeID", "ProductionName");
            ViewData["FishID"] = new SelectList(_context.Fishes, "FishID", "FishName");
            // commission
            ViewBag.Commission = _context.Cofigs.Find(1);
            //ViewBag.Commission = _context.Cofigs.Find(2);
            return View();
        }

        // POST: BoatOwnerReciepts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BoatOwnerReciept boatOwnerReciept)
        {
           // Cookies Of Boat Owner Receipt
            var TotalBeforePaymentCookie = Request.Cookies["TotalBeforePayment"];
            var commisionCookie = Request.Cookies["commision"];
            var PaidFromDebtsCookie = Request.Cookies["PaidFromDebts"];
            var TotalProductionCookie = Request.Cookies["TotalProduction"];
            //find latest sarha related to selected boat
            var sarhaId = _context.Sarhas.Where(x => x.BoatID == boatOwnerReciept.BoatID).Max(x=>x.SarhaID);
            boatOwnerReciept.SarhaID = sarhaId;
            boatOwnerReciept.TotalBeforePaying = Convert.ToDecimal(TotalBeforePaymentCookie);
            boatOwnerReciept.Commission = Convert.ToDecimal(commisionCookie);
            boatOwnerReciept.PaidFromDebts = Convert.ToDecimal(PaidFromDebtsCookie);
            boatOwnerReciept.TotalAfterPaying = Convert.ToDecimal(TotalProductionCookie);
            // Subtracting Paid From Halek
            var boat = _context.Boats.Find(boatOwnerReciept.BoatID);
            boat.DebtsOfHalek -= Convert.ToDecimal(PaidFromDebtsCookie);
            // Salary for Each One
            var sarha = _context.Sarhas.Find(sarhaId);
            var IndividualSalary = (Convert.ToDecimal(TotalProductionCookie) / 2) / sarha.NumberOfFishermen;
            // Calculating Final Income 
            // for shared boats
            decimal FinalIncome;
            // 5 -> Shared Boat ... We will change it later
            if (boat.TypeID == 5)
            {
                FinalIncome = (Convert.ToDecimal(TotalProductionCookie) / 2) - IndividualSalary;
                boat.IncomeOfSharedBoat += FinalIncome;
            }
            // for ordinary boats
            else
                FinalIncome = Convert.ToDecimal(TotalProductionCookie);
            boatOwnerReciept.FinalIncome = FinalIncome;
            _context.Add(boatOwnerReciept);
            await _context.SaveChangesAsync();
            // Cookies Of Receipt Items
            var FishesCookie = Request.Cookies["FishNames"];
            var ProductionTypesCookie = Request.Cookies["ProductionTypes"];
            var qtysCookie = Request.Cookies["qtys"];
            var unitpricesCookie = Request.Cookies["unitprices"];
            var pricesCookie = Request.Cookies["prices"];
            string[] Fishes = FishesCookie.Split(",").Select(c => Convert.ToString(c)).ToArray();
            string[] Productions = ProductionTypesCookie.Split(",").Select(c => Convert.ToString(c)).ToArray();
            int[] qtys = qtysCookie.Split(",").Select(c => Convert.ToInt32(c)).ToArray();
            decimal[] unitPrices = unitpricesCookie.Split(",").Select(c => Convert.ToDecimal(c)).ToArray();
            decimal[] prices = pricesCookie.Split(",").Select(c => Convert.ToDecimal(c)).ToArray();
            var latestReceipt = _context.BoatOwnerReciepts.Max(x => x.BoatOwnerRecieptID);
            for (int i = 0; i < Fishes.Length; i++)
            {
                var fish = _context.Fishes.Single(x => x.FishName == Fishes[i]);
                var Produc = _context.ProductionTypes.Single(x => x.ProductionName == Productions[i]);
                BoatOwnerItem boatOwnerItem = new BoatOwnerItem()
                {
                    BoatOwnerRecieptID = latestReceipt,
                    FishID = fish.FishID,
                    ProductionTypeID = Produc.ProductionTypeID,
                    Qty = qtys[i],
                    UnitPrice = unitPrices[i],
                };
                _context.BoatOwnerItems.Add(boatOwnerItem);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
           
        }
      
        // GET: BoatOwnerReciepts/Delete/5
        public async Task<IActionResult> Delete(int? id)
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
            // Increase Halek Again
            var boat = _context.Boats.Find(boatOwnerReciept.BoatID);
            boat.DebtsOfHalek += Convert.ToDecimal(boatOwnerReciept.PaidFromDebts);

            // Decrease Shared Boat Income
            if (boat.TypeID == 5)
            {
                boat.IncomeOfSharedBoat -= boatOwnerReciept.FinalIncome;
            }

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
