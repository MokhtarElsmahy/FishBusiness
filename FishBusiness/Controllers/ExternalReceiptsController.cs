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
    public class ExternalReceiptsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExternalReceiptsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ExternalReceipts
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ExternalReceipts.Include(e => e.Boat).Include(e => e.Sarha);
            return View(await applicationDbContext.ToListAsync());
        }
        public DateTime TimeNow()
        {
            TimeZone localZone = TimeZone.CurrentTimeZone;
            DateTime currentDate = DateTime.Now;
            DateTime currentUTC =
           localZone.ToUniversalTime(currentDate);
            return currentUTC.AddHours(2);
        }
        // GET: ExternalReceipts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var externalReceipt = await _context.ExternalReceipts
                .Include(e => e.Boat)
                .Include(e => e.Sarha)
                .FirstOrDefaultAsync(m => m.ExternalReceiptID == id);
            if (externalReceipt == null)
            {
                return NotFound();
            }

            return View(externalReceipt);
        }

        // GET: ExternalReceipts/Create
        public IActionResult Create()
        {
            ViewData["BoatID"] = new SelectList(_context.Boats, "BoatID", "BoatName");
            ViewData["SarhaID"] = new SelectList(_context.Sarhas, "SarhaID", "SarhaID");
            return View();
        }

        // POST: ExternalReceipts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ExternalReceiptID,BoatID,SarhaID,TotalBeforePaying,Date,Commission,PaidFromDebts,TotalAfterPaying,FinalIncome")] ExternalReceipt externalReceipt)
        {
            //if (ModelState.IsValid)
            //{
            //    // Subtracting Paid From Halek
            var boat = _context.Boats.Find(externalReceipt.BoatID);
            boat.DebtsOfHalek -= Convert.ToDecimal(externalReceipt.PaidFromDebts);
            var p = _context.People.Find(1);
            p.credit += Convert.ToDecimal(externalReceipt.PaidFromDebts);
            var sarhaId = _context.Sarhas.Where(x => x.BoatID == externalReceipt.BoatID).Max(x => x.SarhaID);
            var TotalAfterPaying = externalReceipt.TotalBeforePaying - externalReceipt.Commission - externalReceipt.PaidFromDebts;
            // Salary for Each One
            var sarha = _context.Sarhas.Find(sarhaId);
            var IndividualSalary = (Convert.ToDecimal(TotalAfterPaying) / 2) / sarha.NumberOfFishermen;
            // Calculating Final Income 
            // for shared boats
            decimal FinalIncome;
            // 5 -> Shared Boat ... We will change it later
            if (boat.TypeID == 2)
            {
                FinalIncome = (Convert.ToDecimal(TotalAfterPaying) / 2) - IndividualSalary;
                boat.IncomeOfSharedBoat += FinalIncome;
                IncomesOfSharedBoat i = new IncomesOfSharedBoat()
                {
                    BoatID = boat.BoatID,
                    Date = TimeNow(),
                    Income = FinalIncome
                };
                _context.IncomesOfSharedBoats.Add(i);
                p.credit += FinalIncome;
            }
            // for ordinary boats
            else
                FinalIncome = Convert.ToDecimal(TotalAfterPaying);
            externalReceipt.SarhaID = sarhaId;
            externalReceipt.TotalAfterPaying = TotalAfterPaying;
            externalReceipt.FinalIncome = FinalIncome;
            _context.Add(externalReceipt);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = externalReceipt.ExternalReceiptID });
            //}
            //ViewData["BoatID"] = new SelectList(_context.Boats, "BoatID", "BoatName", externalReceipt.BoatID);
            //ViewData["SarhaID"] = new SelectList(_context.Sarhas, "SarhaID", "SarhaID", externalReceipt.SarhaID);
            //return View(externalReceipt);
        }


        // GET: ExternalReceipts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var externalReceipt = await _context.ExternalReceipts
                           .Include(e => e.Boat)
                           .Include(e => e.Sarha)
                           .FirstOrDefaultAsync(m => m.ExternalReceiptID == id);
            if (externalReceipt == null)
            {
                return NotFound();
            }
            // Increase Halek Again
            var boat = _context.Boats.Find(externalReceipt.BoatID);
            boat.DebtsOfHalek += Convert.ToDecimal(externalReceipt.PaidFromDebts);
            var p = _context.People.Find(1);
            p.credit -= Convert.ToDecimal(externalReceipt.PaidFromDebts);
            // Decrease Shared Boat Income
            if (boat.TypeID == 2)
            {
                boat.IncomeOfSharedBoat -= externalReceipt.FinalIncome;
                p.credit -= externalReceipt.FinalIncome;
            }

            _context.ExternalReceipts.Remove(externalReceipt);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


        }



        private bool ExternalReceiptExists(int id)
        {
            return _context.ExternalReceipts.Any(e => e.ExternalReceiptID == id);
        }
    }
}
