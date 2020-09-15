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
            ViewBag.Items = _context.BoatOwnerItems.Where(i => i.BoatOwnerRecieptID == id).Include(x => x.Fish).Include(x => x.ProductionType);
            return View(boatOwnerReciept);
        }

        // GET: BoatOwnerReciepts/Create
        public IActionResult Create()
        {
            ViewData["BoatID"] = new SelectList(_context.Boats.Where(b=>b.IsActive==true), "BoatID", "BoatName");
            ViewData["ProductionTypeID"] = new SelectList(_context.ProductionTypes, "ProductionTypeID", "ProductionName");

            ViewData["FishID"] = new SelectList(_context.Fishes, "FishID", "FishName");

            //
            ViewData["MerchantID"] = new SelectList(_context.Merchants.Where(m=>m.IsFromOutsideCity==false), "MerchantID", "MerchantName");
            // commission
           ViewBag.Commission = _context.Cofigs.Find(1);
            //ViewBag.Commission = _context.Cofigs.Find(2);
            return View();
        }
        public IActionResult GetBoatItems(int? id)
        {
            var LastRecieptOfBoat = _context.BoatOwnerReciepts.Where(r => r.BoatID == id).Max(rs => rs.BoatOwnerRecieptID);
            var itemsOfLastReciept = _context.BoatOwnerItems.Where(i => i.BoatOwnerRecieptID == LastRecieptOfBoat).Include(i => i.Fish);
            var res = itemsOfLastReciept.Select(r => new { fishId = r.Fish.FishID, fishName = r.Fish.FishName });
            return Json(res);

        }
        public IActionResult GetMerchant(int? id, DateTime date)
        {
            Merchant m = _context.Merchants.Find(id);
            var rowsOfRec = _context.MerchantReciepts.Where(i => i.MerchantID == id);
            int recID = 0;
            if (rowsOfRec.Any())
            {
                recID = rowsOfRec.Max(i => i.MerchantRecieptID);
            }
            var rec = _context.MerchantReciepts.Find(recID);
            if (rec != null)
            {
                if (rec.Date.ToShortDateString() == date.ToShortDateString())
                {
                    return Json(new { RecID = recID, debts = m.PreviousDebts });
                }
                return Json(new { RecID = 0, debts = m.PreviousDebts });
            }

            return Json(new { RecID = 0, debts = m.PreviousDebts });

        }
        public IActionResult SaveItems(MerchantRecieptItem item)
        {



            // items.Add(item);
            Fish fish = _context.Fishes.Find(item.FishID);
            Boat boat = _context.Boats.Find(item.BoatID);
            ProductionType production = _context.ProductionTypes.Find(item.ProductionTypeID);
            var res = new { boatName = boat.BoatName, productionName = production.ProductionName, fishName = fish.FishName, qty = item.Qty, unitPrice = item.UnitPrice, total = item.Qty * item.UnitPrice };

            return Json(res);

        }

        public IActionResult GetFishPrice(int fishId,int RecieptID)
        {

            var item = _context.BoatOwnerItems.SingleOrDefault(i => i.BoatOwnerRecieptID == RecieptID && i.FishID == fishId);
            var res = new { unitPrice = item.UnitPrice };

            return Json(res);

        }

        [HttpPost]
        public async Task<IActionResult> MCreate(MerRecCreateVm model)
        {
            if (ModelState.IsValid)
            {

                var FishesCookie = Request.Cookies["MFishNames"];
                var ProductionTypesCookie = Request.Cookies["MProductionTypes"];
                var qtysCookie = Request.Cookies["Mqtys"];
                var unitpricesCookie = Request.Cookies["Munitprices"];
                var boatsCookie = Request.Cookies["Mboats"];
                string[] Fishes = FishesCookie.Split(",").Select(c => Convert.ToString(c)).ToArray();
                string[] Productions = ProductionTypesCookie.Split(",").Select(c => Convert.ToString(c)).ToArray();
                string[] boats = boatsCookie.Split(",").Select(c => Convert.ToString(c)).ToArray();
                int[] qtys = qtysCookie.Split(",").Select(c => Convert.ToInt32(c)).ToArray();
                decimal[] unitPrices = unitpricesCookie.Split(",").Select(c => Convert.ToDecimal(c)).ToArray();


                MerchantReciept merchantReciept;
                Merchant m;
                if (model.RecID == 0)
                {
                    merchantReciept = new MerchantReciept() { Date = model.Date, payment = model.payment, TotalOfReciept = model.TotalOfReciept, MerchantID = model.MerchantID, CurrentDebt = model.CurrentDebt };
                    _context.Add(merchantReciept);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    merchantReciept = _context.MerchantReciepts.Find(model.RecID);
                    m = _context.Merchants.Find(model.MerchantID);
                    merchantReciept.TotalOfReciept += model.TotalOfReciept;
                    merchantReciept.payment += model.payment;
                }




                for (int i = 0; i < Fishes.Length; i++)
                {
                    var fish = _context.Fishes.Single(x => x.FishName == Fishes[i]);
                    var Produc = _context.ProductionTypes.Single(x => x.ProductionName == Productions[i]);
                    var boat = _context.Boats.Single(x => x.BoatName == boats[i]);
                    MerchantRecieptItem MerchantRecieptItems = new MerchantRecieptItem()
                    {
                        MerchantRecieptID = merchantReciept.MerchantRecieptID,
                        FishID = fish.FishID,
                        ProductionTypeID = Produc.ProductionTypeID,
                        Qty = qtys[i],
                        UnitPrice = unitPrices[i],
                        BoatID = boat.BoatID
                    };

                    _context.MerchantRecieptItems.Add(MerchantRecieptItems);
                    await _context.SaveChangesAsync();
                }

                m = _context.Merchants.Find(model.MerchantID);
                m.PreviousDebts = model.CurrentDebt + model.TotalOfReciept;
                merchantReciept.CurrentDebt = model.CurrentDebt;


                await _context.SaveChangesAsync();
                return Json(new { message = "success", id = merchantReciept.MerchantRecieptID });
                //return RedirectToAction(nameof(Index));
            }
            ViewData["MerchantID"] = new SelectList(_context.Merchants, "MerchantID", "MerchantName", model.MerchantID);
            //return View(model);
            return Json(new { message = "fail" });
        }
        // POST: BoatOwnerReciepts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BoatOwnerReciept boatOwnerReciept)
        {


            var TotalBeforePaymentCookie = boatOwnerReciept.TotalBeforePaying;
            var commisionCookie = boatOwnerReciept.Commission;
            var PercentageCommissionCookie = boatOwnerReciept.PercentageCommission;
            var PaidFromDebtsCookie = boatOwnerReciept.PaidFromDebts;
            var TotalProductionCookie = boatOwnerReciept.TotalAfterPaying;
            //find latest sarha related to selected boat
            var sarhaId = _context.Sarhas.Where(x => x.BoatID == boatOwnerReciept.BoatID && x.IsFinished == false).Max(x => x.SarhaID);
            boatOwnerReciept.SarhaID = sarhaId;
            boatOwnerReciept.TotalBeforePaying = Convert.ToDecimal(TotalBeforePaymentCookie);
            boatOwnerReciept.Commission = Convert.ToDecimal(commisionCookie);
            boatOwnerReciept.PercentageCommission = Convert.ToInt32(PercentageCommissionCookie);
            boatOwnerReciept.TotalAfterPaying = Convert.ToDecimal(TotalProductionCookie);
            boatOwnerReciept.IsCalculated = false;
            boatOwnerReciept.IsCollected = false;
            boatOwnerReciept.PaidFromDebts = Convert.ToDecimal(PaidFromDebtsCookie);
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
            if (boat.TypeID == 2)
            {
                FinalIncome = (Convert.ToDecimal(TotalProductionCookie) / 2) - IndividualSalary;
                boat.IncomeOfSharedBoat += FinalIncome;
                IncomesOfSharedBoat i = new IncomesOfSharedBoat()
                {
                    BoatID = boat.BoatID,
                    Date = DateTime.Now,
                    Income = FinalIncome
                };
                _context.IncomesOfSharedBoats.Add(i);
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
            var sarhaa = _context.Sarhas.Find(sarhaId);
            sarhaa.IsFinished = true;
            _context.SaveChanges();
            //return RedirectToAction(nameof(Index));
            //return RedirectToAction("Details",new { id= latestReceipt });
            return Json(new { message = "success", id = boatOwnerReciept.BoatID, reciept = boatOwnerReciept.BoatOwnerRecieptID });

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
            if (boat.TypeID == 2)
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
