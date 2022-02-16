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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace FishBusiness.Controllers
{
    [Authorize]
    public class SarhasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public SarhasController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
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
        // GET: Sarhas
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Sarhas.Include(s => s.Boat);
            return View(await applicationDbContext.Where(c=>c.IsFinished==true).ToListAsync());
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
            ViewBag.depts = _context.Debts_Sarhas.Include(x => x.Person).Where(x => x.SarhaID == id && (x.PersonID==3|| x.PersonID == 4)).Include(d => d.Debt);
            ViewBag.Total = _context.Debts_Sarhas.Where(x => x.SarhaID == id && (x.PersonID == 3 || x.PersonID == 4)).Sum(x => x.Price);

            ViewBag.deptsOfHalaka = _context.Debts_Sarhas.Include(x => x.Person).Where(x => x.SarhaID == id && (x.PersonID == 1 || x.PersonID == 2)).Include(d => d.Debt);
            ViewBag.TotalOfHalaka = _context.Debts_Sarhas.Where(x => x.SarhaID == id && (x.PersonID == 1 || x.PersonID == 2)).Sum(x => x.Price);
            return View(sarha);
        }

        // GET: Sarhas/Create
        public IActionResult Create()
        {
            ViewBag.Boats = new SelectList(_context.Boats.Where(c => c.IsActive == true), "BoatID", "BoatName");
            //ViewBag.People = new SelectList(_context.People.OrderByDescending(x => x.PersonID).Take(2).ToList(), "PersonID", "Name");
            ViewBag.People = new SelectList(_context.People.ToList(), "PersonID", "Name");
            //ViewBag.halek = _context.Debts.ToList();
            SarhaViewModel sarhaViewModel = new SarhaViewModel();
            sarhaViewModel.Debts = _context.Debts.ToList();

            return View(sarhaViewModel);
        }

        // POST: Sarhas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        // [ValidateAntiForgeryToken]
        public IActionResult Create([FromBody()] SarhaVM sVM)
        {
            if (ModelState.IsValid)
            {
                Sarha s = new Sarha()
                {
                    BoatID = sVM.BoatID,
                    NumberOfFishermen = sVM.NoFisherMen,
                    NumberOfBoxes = sVM.NoBoxes,
                   
                    DateOfSarha = TimeNow()
                };
                _context.Sarhas.Add(s);
                _context.SaveChanges();
                var latestSarha = _context.Sarhas.Max(x => x.SarhaID);
                var pricesCookie = Request.Cookies["MyItems"];
                if (pricesCookie != null)
                {


                    decimal[] result = pricesCookie.Split(",".ToCharArray()).Select(c => Convert.ToDecimal(c)).ToArray();
                    for (int i = 0; i < _context.Debts.ToList().Count(); i++)
                    {
                        if (result[i] != 0)
                        {
                            Debts_Sarha d_s = new Debts_Sarha()
                            {
                                SarhaID = s.SarhaID,
                                DebtID = _context.Debts.ToList().ElementAt(i).DebtID,
                                Price = result[i],
                                PersonID=1,
                                Date = TimeNow()

                            };
                           


                            _context.Debts_Sarhas.Add(d_s);
                            Person pp = _context.People.Find(1);
                            pp.credit -= result[i];
                            _context.SaveChanges();
                        }
                    }
                    var boat = _context.Boats.Find(sVM.BoatID);
                    boat.DebtsOfHalek += result.Sum();
                    _context.SaveChanges();
                    Response.Cookies.Delete("MyItems");

                }
                return Json(new { message = "success", id = sVM.BoatID });
            }
            ViewBag.Boats = new SelectList(_context.Boats, "BoatID", "BoatName", sVM.BoatID);
            SarhaViewModel sarhaViewModel = new SarhaViewModel();
            sarhaViewModel.Sarha = new Sarha()
            {
                BoatID = sVM.BoatID,
                NumberOfFishermen = sVM.NoFisherMen,
                NumberOfBoxes = sVM.NoBoxes,
                DateOfSarha = sVM.DateOfSarha
            };
            return View(sarhaViewModel);
        }

        // GET: Sarhas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {

            var user = await _userManager.GetUserAsync(User);
            var roles = await _userManager.GetRolesAsync(user);
            int PID = 1;
            if (roles.Contains("partner"))
            {
                PID = 2;
               
            }
           
            if (id == null)
            {
                return NotFound();
            }

            var sarha = _context.Sarhas.Find(id);
            var dept_sarha = _context.Debts_Sarhas.Where(x => x.SarhaID == id&&x.PersonID==PID).Include(x => x.Debt).Include(x => x.Person);

            var ds = dept_sarha.Select(c => c.DebtID);
            // ViewBag.OtherDebts = _context.Debts.Where(d => !ds.Contains(d.DebtID)).ToList();

            if (sarha == null)
            {
                return NotFound();
            }
            SarhaViewModel sarhaViewModel = new SarhaViewModel()
            {
                Sarha = sarha,
                Debts_Sarhas = dept_sarha,
                OtherDebts = _context.Debts.Where(d => !ds.Contains(d.DebtID)).ToList(),
                Debts = _context.Debts.ToList()

            };
           
            ViewBag.Boats = new SelectList(_context.Boats, "BoatID", "BoatName", sarha.BoatID);
            var boat = _context.Boats.Find(sarha.BoatID);
            var Halek = _context.Debts_Sarhas.Where(x => x.SarhaID == id).Sum(x => x.Price);
            boat.DebtsOfHalek -= Halek;
            _context.SaveChanges();
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
                        var oldPrice = item.Price;
                        Person p = _context.People.Find(item.PersonID);
                        item.Price = result[i];
                        if (oldPrice > result[i])
                            p.credit += oldPrice - result[i];
                        else
                            p.credit -= result[i] - oldPrice;
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



        [HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Editing(EditSarhaVm editSarhaVm)int id , int BoatID , int NoFisherMen , int NoBoxes ,DateTime DateOfSarha
        public async Task<IActionResult> Editing(EditSarhaVm editSarhaVm)
        {
          
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);
                    var roles = await _userManager.GetRolesAsync(user);
                    int PID = 1;
                    if (roles.Contains("partner"))
                        PID = 2;
                    var sar = _context.Sarhas.Find(editSarhaVm.id);
                    sar.BoatID = editSarhaVm.BoatID;
                    //sar.DateOfSarha = editSarhaVm.DateOfSarha;
                    sar.DateOfSarha = sar.DateOfSarha;
                    sar.NumberOfFishermen = editSarhaVm.NoFisherMen;
                    sar.NumberOfBoxes = editSarhaVm.NoBoxes;
                    _context.SaveChanges();
                    var boatt = _context.Boats.Find(editSarhaVm.BoatID);

                    if (editSarhaVm.OldHalekPrices != null)
                    {
                        string ss = editSarhaVm.OldHalekPrices.TrimEnd(editSarhaVm.OldHalekPrices[editSarhaVm.OldHalekPrices.Length - 1]);
                        decimal[] oldPrices = ss.Split(",".ToCharArray()).Select(c => Convert.ToDecimal(c.Trim())).ToArray();


                        var dept_sarha = _context.Debts_Sarhas.Where(x => x.SarhaID == editSarhaVm.id);

                        for (int i = 0; i < dept_sarha.Count(); i++)
                        {
                            var element = dept_sarha.ToList().ElementAt(i);
                            var oldPrice = element.Price;
                            element.Date = element.Date;
                            Person p = _context.People.Find(PID);
                            element.PersonID = PID;
                            element.Price = oldPrices[i];
                            if (oldPrice > oldPrices[i])
                            {
                                decimal diff= oldPrice - oldPrices[i];
                                //p.credit += diff;
                                HalekDifference h = new HalekDifference()
                                {
                                    BoatID = sar.BoatID,
                                    PersonID = PID,
                                    Date = TimeNow(),
                                    ReturnedValue = diff
                                };
                                _context.HalekDifferences.Add(h);
                                _context.SaveChanges();
                            }
                            else
                            {
                                decimal diff = oldPrices[i] - oldPrice;
                                //p.credit -= diff ;
                               
                            }
                        }
                        boatt.DebtsOfHalek += oldPrices.Sum();
                    }

                    if (editSarhaVm.NHalekPrices != null)
                    {

                        string cc = editSarhaVm.NHalekPrices.TrimEnd(editSarhaVm.NHalekPrices[editSarhaVm.NHalekPrices.Length - 1]);
                        decimal[] newPrices = cc.Split(",".ToCharArray()).Select(c => Convert.ToDecimal(c.Trim())).ToArray();

                        var dept_sarhaa = _context.Debts_Sarhas.Where(x => x.SarhaID == editSarhaVm.id).Include(x => x.Debt).Include(x => x.Person);

                        var ds = dept_sarhaa.Select(c => c.DebtID);
                        var lst = _context.Debts.Where(d => !ds.Contains(d.DebtID)).ToList();

                        for (int i = 0; i < lst.Count(); i++)
                        {
                            if (newPrices[i] != 0)
                            {


                                Debts_Sarha d_s = new Debts_Sarha()
                                {
                                    SarhaID = editSarhaVm.id,
                                    DebtID = lst.ElementAt(i).DebtID,
                                    Price = newPrices[i],
                                    PersonID = PID,
                                    Date = TimeNow()

                                };
                                _context.Debts_Sarhas.Add(d_s);
                                //Person pp = _context.People.Find(PID);
                                //pp.credit -= newPrices[i];
                                _context.SaveChanges();
                            }
                        }
                      
                        boatt.DebtsOfHalek += newPrices.Sum();
                        //boatt.DebtsOfHalek += oldPrices.Sum();
                        _context.SaveChanges();
                        Response.Cookies.Delete("MyItems");
                        Response.Cookies.Delete("NMyItems");
                      

                    }

                    _context.SaveChanges();
                    return Json(new { message = "success", id = sar.BoatID });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SarhaExists(editSarhaVm.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

            }
            ViewData["BoatID"] = new SelectList(_context.Boats, "BoatID", "BoatLeader", editSarhaVm.BoatID);
            return Json(new { message = "fail" });
        }
        // GET: Sarhas/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sarha = _context.Sarhas
                .Include(s => s.Boat)
                .FirstOrDefault(m => m.SarhaID == id);
            if (sarha == null)
            {
                return NotFound();
            }
            _context.Sarhas.Remove(sarha);
            var boat = _context.Boats.Find(sarha.BoatID);
            var Halek = _context.Debts_Sarhas.Where(x => x.SarhaID == id).Sum(x => x.Price);
            boat.DebtsOfHalek -= Halek;
            Person p = _context.People.Find(1);
            p.credit += Halek;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public IActionResult UpdateFishermen(int id, int Fishermen)
        {
            var s = _context.Sarhas.Find(id);
            s.NumberOfFishermen = Fishermen;
            _context.SaveChanges();
            return Json(new { message = "success", boatId = s.BoatID });
        }
        private bool SarhaExists(int id)
        {
            return _context.Sarhas.Any(e => e.SarhaID == id);
        }
    }
}
