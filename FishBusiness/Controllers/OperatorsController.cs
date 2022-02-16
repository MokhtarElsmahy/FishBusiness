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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace FishBusiness.Controllers
{
    [Authorize]
    public class OperatorsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public OperatorsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public DateTime TimeNow()
        {
            TimeZone localZone = TimeZone.CurrentTimeZone;
            DateTime currentDate = DateTime.Now;
            DateTime currentUTC =
           localZone.ToUniversalTime(currentDate);
            return currentUTC.AddHours(2);
        }
        // GET: Operators
        public async Task<IActionResult> Index()
        {
            return View(await _context.Operators.ToListAsync());
        }

        #region CRUD
        // GET: Operators/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @operator = await _context.Operators
                .FirstOrDefaultAsync(m => m.OperatorID == id);
            if (@operator == null)
            {
                return NotFound();
            }

            return View(@operator);
        }

        // GET: Operators/Create
        public IActionResult Create()
        {
            return PartialView();
        }

     
        [HttpPost]
        public async Task<IActionResult> CreateC([Bind("OperatorID,OperatorName,Credit,Phone,Address,JobDesc")] Operator @operator)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@operator);
                await _context.SaveChangesAsync();
               return Json(new { message = "success" });
            }
            return Json(new { message = "fail" });
        }

        // GET: Operators/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @operator = await _context.Operators.FindAsync(id);
            if (@operator == null)
            {
                return NotFound();
            }
            return PartialView(@operator);
        }

        // POST: Operators/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
       
        public async Task<IActionResult> EditC([Bind("OperatorID,OperatorName,Credit,Phone,Address,JobDesc")] Operator @operator)
        {
            
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@operator);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OperatorExists(@operator.OperatorID))
                    {
                        return Json(new { message = "fail" });
                    }
                    else
                    {
                        return Json(new { message = "fail" });
                    }
                }
                return Json(new { message = "success" });
            }
           return Json(new { message = "fail" });
        }

    

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var merchant = await _context.Operators
                .FirstOrDefaultAsync(m => m.OperatorID == id);
            if (merchant == null)
            {
                return NotFound();
            }

            _context.Operators.Remove(merchant);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    

        private bool OperatorExists(int id)
        {
            return _context.Operators.Any(e => e.OperatorID == id);
        }
        #endregion

        public IActionResult Profile(int id)
        {
            OperatorProfileVm model = new OperatorProfileVm();
            var @operator = _context.Operators.Find(id);
            if (@operator == null)
            {
                return NotFound();
            }
            model.Operator = @operator;
            model.PaidsForOperator = _context.PaidForOperators.Include(c=>c.Person).Where(c => c.OperatorID == id).ToList();
            model.OperatorDeals = _context.OperatorDeals.Include(c => c.Person).Where(c => c.OperatorID == id).ToList();
            return View(model);
        }

   

        public async Task<IActionResult> NewDeal(int OperatorID, decimal PaidValue)
        {
            OperatorProfileVm model = new OperatorProfileVm();

            var user = await _userManager.GetUserAsync(User);

            var ID = _userManager.GetUserId(User);
            var userDetails = _context.Users.Find(ID);

            var op = _context.Operators.Find(OperatorID);
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains("admin"))
            {
                var person = _context.People.Find(1);
                op.Credit += PaidValue;
             
                OperatorDeal d = new OperatorDeal() { Date = TimeNow(), OperatorID = op.OperatorID, PersonID = person.PersonID, Price = PaidValue , DebtsAfterDeal = op.Credit };
                _context.OperatorDeals.Add(d);



            }
            else if (roles.Contains("partner"))
            {
                var person = _context.People.Find(2);
                op.Credit += PaidValue;
                OperatorDeal d = new OperatorDeal() { Date = TimeNow(), OperatorID = op.OperatorID, PersonID = person.PersonID, Price = PaidValue , DebtsAfterDeal =op.Credit};
                _context.OperatorDeals.Add(d);
              
            }
            _context.SaveChanges();
            return Json(new { message = "success", operatorCredit = op.Credit });


        }
    }
}
