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
    public class ExpensesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExpensesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Expenses
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Expenses.Include(e => e.Boat).OrderBy(x=>x.Boat.BoatName);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Expenses/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var expense = await _context.Expenses
        //        .Include(e => e.Boat)
        //        .FirstOrDefaultAsync(m => m.ExpenseID == id);
        //    if (expense == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(expense);
        //}

        // GET: Expenses/Create
        public IActionResult Create()
        {
            ViewData["BoatID"] = new SelectList(_context.Boats, "BoatID", "BoatName");
            return View();
        }

        // POST: Expenses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ExpenseID,BoatID,Price,Cause,Date")] Expense expense)
        {
            if (ModelState.IsValid)
            {
                _context.Add(expense);
                Person p = _context.People.Find(1);
                p.credit -= expense.Price;
                Boat boat = await _context.Boats.FindAsync(expense.BoatID);
                boat.TotalOfExpenses += expense.Price;

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BoatID"] = new SelectList(_context.Boats, "BoatID", "BoatName", expense.BoatID);
            return View(expense);
        }

        // GET: Expenses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null)
            {
                return NotFound();
            }
            ViewData["BoatID"] = new SelectList(_context.Boats, "BoatID", "BoatName", expense.BoatID);
            
            return View(expense);
        }

        // POST: Expenses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ExpenseID,BoatID,Price,Cause,Date")] Expense expense)
        {
            if (id != expense.ExpenseID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    var Oldexpense = await _context.Expenses.FindAsync(expense.ExpenseID);
                    if (Oldexpense != null)
                    {
                        _context.Entry(Oldexpense).State = EntityState.Detached;
                    }

                    Boat boat = await _context.Boats.FindAsync(expense.BoatID);

                    boat.TotalOfExpenses -= Oldexpense.Price;

                    boat.TotalOfExpenses += expense.Price;
                    await _context.SaveChangesAsync();

                    _context.Update(expense);
                    if(expense.Price != Oldexpense.Price)
                    {
                        Person p = _context.People.Find(1);
                        if(expense.Price > Oldexpense.Price)
                        {
                            p.credit -= expense.Price - Oldexpense.Price;
                        }
                        else
                            p.credit += Oldexpense.Price - expense.Price ;
                    }
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExpenseExists(expense.ExpenseID))
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
            ViewData["BoatID"] = new SelectList(_context.Boats, "BoatID", "BoatName", expense.BoatID);
            return View(expense);
        }

        // GET: Expenses/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var expense = await _context.Expenses
        //        .Include(e => e.Boat)
        //        .FirstOrDefaultAsync(m => m.ExpenseID == id);
        //    if (expense == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(expense);
        //}

        // POST: Expenses/Delete/5
        
     
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null)
            {
                return NotFound();
            }
            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExpenseExists(int id)
        {
            return _context.Expenses.Any(e => e.ExpenseID == id);
        }
    }
}
