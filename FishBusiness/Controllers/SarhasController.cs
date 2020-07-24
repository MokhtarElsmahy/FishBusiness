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
    public class SarhasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SarhasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Sarhas
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Sarhas.Include(s => s.Boat);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Sarhas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sarha = await _context.Sarhas
                .Include(s => s.Boat)
                .FirstOrDefaultAsync(m => m.SarhaID == id);
            if (sarha == null)
            {
                return NotFound();
            }
            ViewBag.depts = _context.Debts_Sarhas.Where(x => x.SarhaID == id).Include(d => d.Debt);
            ViewBag.Total = _context.Debts_Sarhas.Where(x => x.SarhaID == id).Sum(x => x.Price);
            return View(sarha);
        }

        // GET: Sarhas/Create
        public IActionResult Create()
        {
            ViewBag.Boats = new SelectList(_context.Boats, "BoatID", "BoatName");
            //ViewBag.halek = _context.Debts.ToList();
            SarhaViewModel sarhaViewModel = new SarhaViewModel();
            sarhaViewModel.Debts = _context.Debts.ToList();
            return View(sarhaViewModel);
        }

        // POST: Sarhas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SarhaViewModel sarhaViewModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sarhaViewModel.Sarha);
                await _context.SaveChangesAsync();
                var latestSarha = _context.Sarhas.Max(x => x.SarhaID);
                var deptPriceCookie = Request.Cookies["MyItems"];
                decimal[] result = deptPriceCookie.Split(",".ToCharArray()).Select(c => Convert.ToDecimal(c)).ToArray();
                int i = 0;
                foreach (var item in _context.Debts.ToList())
                {
                    Debts_Sarha d_s = new Debts_Sarha()
                    {
                        SarhaID = latestSarha,
                        DebtID = item.DebtID,
                        Price = result[i]
                    };
                    _context.Debts_Sarhas.Add(d_s);
                    await _context.SaveChangesAsync();
                    i++;
                }
                var boat = _context.Boats.Find(sarhaViewModel.Sarha.BoatID);
                boat.DebtsOfHalek += result.Sum();
                await _context.SaveChangesAsync();
                Response.Cookies.Delete("MyItems");
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Boats = new SelectList(_context.Boats, "BoatID", "BoatName", sarhaViewModel.Sarha.BoatID);
            return View(sarhaViewModel.Sarha);
        }

        // GET: Sarhas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sarha = await _context.Sarhas.FindAsync(id);
            var dept_sarha = _context.Debts_Sarhas.Where(x => x.SarhaID == id).Include(x => x.Debt);
            if (sarha == null)
            {
                return NotFound();
            }
            SarhaViewModel sarhaViewModel = new SarhaViewModel()
            {
                Sarha = sarha,
                Debts_Sarha = dept_sarha
            };
            ViewBag.Boats = new SelectList(_context.Boats, "BoatID", "BoatName", sarha.BoatID);
            var boat = _context.Boats.Find(sarha.BoatID);
            var Halek = _context.Debts_Sarhas.Where(x => x.SarhaID == id).Sum(x => x.Price);
            boat.DebtsOfHalek -= Halek;
            await _context.SaveChangesAsync();
            return View(sarhaViewModel);
        }

        // POST: Sarhas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SarhaViewModel sarhaViewModel)
        {
            if (id != sarhaViewModel.Sarha.SarhaID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sarhaViewModel.Sarha);
                    var deptPriceCookie = Request.Cookies["MyItems"];
                    decimal[] result = deptPriceCookie.Split(",".ToCharArray()).Select(c => Convert.ToDecimal(c)).ToArray();
                    var dept_sarha = _context.Debts_Sarhas.Where(x => x.SarhaID == id);
                    int i = 0;
                    foreach (var item in dept_sarha)
                    {
                        item.Price = result[i];
                        i++;
                    }
                    Response.Cookies.Delete("MyItems");
                    var boat = _context.Boats.Find(sarhaViewModel.Sarha.BoatID);
                    boat.DebtsOfHalek += result.Sum();
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SarhaExists(sarhaViewModel.Sarha.SarhaID))
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
            ViewData["BoatID"] = new SelectList(_context.Boats, "BoatID", "BoatLeader", sarhaViewModel.Sarha.BoatID);
            return View(sarhaViewModel);
        }

        // GET: Sarhas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sarha = await _context.Sarhas
                .Include(s => s.Boat)
                .FirstOrDefaultAsync(m => m.SarhaID == id);
            if (sarha == null)
            {
                return NotFound();
            }
            _context.Sarhas.Remove(sarha);
            var boat = _context.Boats.Find(sarha.BoatID);
            var Halek = _context.Debts_Sarhas.Where(x => x.SarhaID == id).Sum(x => x.Price);
            boat.DebtsOfHalek -= Halek;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SarhaExists(int id)
        {
            return _context.Sarhas.Any(e => e.SarhaID == id);
        }
    }
}
