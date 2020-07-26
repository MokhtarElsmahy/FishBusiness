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
    public class MerchantRecieptsController : Controller
    {
        private readonly ApplicationDbContext _context;
        //private int Id;
        public MerchantRecieptsController(ApplicationDbContext context)
        {
            _context = context;
           
        }

        // GET: MerchantReciepts
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.MerchantReciepts.Include(m => m.Merchant);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: MerchantReciepts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var merchantReciept = await _context.MerchantReciepts
                .Include(m => m.Merchant)
                .FirstOrDefaultAsync(m => m.MerchantRecieptID == id);
            if (merchantReciept == null)
            {
                return NotFound();
            }
            ViewBag.Items = _context.MerchantRecieptItems.Where(i => i.MerchantRecieptID == id).Include(x => x.Fish).Include(x => x.ProductionType).Include(x=>x.Boat);
            return View(merchantReciept);
        }

        // GET: MerchantReciepts/Create
        public IActionResult Create()
        {
            ViewData["MerchantID"] = new SelectList(_context.Merchants, "MerchantID", "MerchantName");
            ViewData["Boats"] = new SelectList(_context.Boats.ToList(), "BoatID", "BoatName");
            ViewData["ProductionTypeID"] = new SelectList(_context.ProductionTypes, "ProductionTypeID", "ProductionName");

            MRecVM vM = new MRecVM();
            return View(vM);
        }

        public IActionResult GetMerchant(int? id)
        {
            Merchant m = _context.Merchants.Find(id);
            return Json(new { debts = m.PreviousDebts });

        }

        public IActionResult GetBoatItems(int? id)
        {
            var LastRecieptOfBoat = _context.BoatOwnerReciepts.Where(r => r.BoatID == id).Max(rs => rs.BoatOwnerRecieptID);
            var itemsOfLastReciept = _context.BoatOwnerItems.Where(i => i.BoatOwnerRecieptID == LastRecieptOfBoat).Include(i => i.Fish);
            var res = itemsOfLastReciept.Select(r => new { fishId = r.Fish.FishID, fishName = r.Fish.FishName });
            return Json(res);

        }

       
        public IActionResult SaveItems(MerchantRecieptItem item)
        {
           
           

           // items.Add(item);
            Fish fish = _context.Fishes.Find(item.FishID);
            Boat boat = _context.Boats.Find(item.BoatID);
            ProductionType production = _context.ProductionTypes.Find(item.ProductionTypeID);
            var res = new { boatName=boat.BoatName,productionName=production.ProductionName ,fishName = fish.FishName, qty = item.Qty, unitPrice = item.UnitPrice, total = item.Qty * item.UnitPrice };

            return Json(res);

        }

        [HttpPost]
        public async Task<IActionResult> Create(MerRecCreateVm model)
        {
            if (ModelState.IsValid)
            {
                MerchantReciept merchantReciept = new MerchantReciept() { Date = model.Date, payment = model.payment, TotalOfReciept = model.TotalOfReciept, MerchantID = model.MerchantID,CurrentDebt=model.CurrentDebt };
                _context.Add(merchantReciept);
                await _context.SaveChangesAsync();


                var FishesCookie = Request.Cookies["FishNames"];
                var ProductionTypesCookie = Request.Cookies["ProductionTypes"];
                var qtysCookie = Request.Cookies["qtys"];
                var unitpricesCookie = Request.Cookies["unitprices"];
                var boatsCookie = Request.Cookies["boats"];
                string[] Fishes = FishesCookie.Split(",").Select(c => Convert.ToString(c)).ToArray();
                string[] Productions = ProductionTypesCookie.Split(",").Select(c => Convert.ToString(c)).ToArray();
                string[] boats = boatsCookie.Split(",").Select(c => Convert.ToString(c)).ToArray();
                int[] qtys = qtysCookie.Split(",").Select(c => Convert.ToInt32(c)).ToArray();
                decimal[] unitPrices = unitpricesCookie.Split(",").Select(c => Convert.ToDecimal(c)).ToArray();
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
            
                Merchant m = _context.Merchants.Find(model.MerchantID);
                m.PreviousDebts = model.CurrentDebt;

                await _context.SaveChangesAsync();
                return Json(new { message = "success" });
                //return RedirectToAction(nameof(Index));
            }
            ViewData["MerchantID"] = new SelectList(_context.Merchants, "MerchantID", "MerchantName", model.MerchantID);
            //return View(model);
            return Json(new { message = "fail" });
        }

       

        // GET: MerchantReciepts/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var merchantReciept = await _context.MerchantReciepts
        //        .Include(m => m.Merchant)
        //        .FirstOrDefaultAsync(m => m.MerchantRecieptID == id);
        //    if (merchantReciept == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(merchantReciept);
        //}

        // POST: MerchantReciepts/Delete/5
       
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var merchantReciept = await _context.MerchantReciepts.Include(ww=>ww.Merchant).FirstOrDefaultAsync(ww=>ww.MerchantRecieptID==id);
            if (merchantReciept == null)
            {
                return NotFound();
            }
            var merchantRecieptItems = _context.MerchantRecieptItems.Where(x=>x.MerchantRecieptID==id).ToList();
            _context.MerchantRecieptItems.RemoveRange(merchantRecieptItems);

            var merchant =  _context.Merchants.Find(merchantReciept.MerchantID);
            merchant.PreviousDebts -= merchantReciept.TotalOfReciept;
            _context.MerchantReciepts.Remove(merchantReciept);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool MerchantRecieptExists(int id)
        {
            return _context.MerchantReciepts.Any(e => e.MerchantRecieptID == id);
        }
    }
}
