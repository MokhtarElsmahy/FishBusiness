using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using FishBusiness.ViewModels;
using FishBusiness.Models;
using Microsoft.AspNetCore.Authorization;

namespace FishBusiness.Controllers
{
    [Authorize]
    public class HalakaSellRecieptsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;



        public HalakaSellRecieptsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
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
        //HalakaSellReciepts
        public IActionResult Index()
        {
            var model = _context.HalakSellReciepts.Where(c=>c.Date.Date== TimeNow().Date).ToList();
            return View(model);
        }


        public IActionResult HalakaSellRecieptsHistory(DateTime date)
        {
            var model = _context.HalakSellReciepts.Where(c => c.Date.Date == date.Date).ToList();
            return PartialView(model);
        }


        public async Task<IActionResult> Collect(int id)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var roles = await _userManager.GetRolesAsync(user);
                int PID = 1;
                if (roles.Contains("partner"))
                {
                    PID = 2;

                }
                var HalakSellReciept = await _context.HalakSellReciepts.FirstOrDefaultAsync(ww => ww.HalakSellRecieptID == id);
                HalakSellReciept.IsCash = true;
                var person = _context.People.Find(PID);
                person.credit += HalakSellReciept.TotalOfPrices;
                _context.SaveChanges();
                
                return Json(new { message = "success" });
            }
            catch (Exception)
            {

                return Json(new { message = "fail" });
            }
           
        }



        public async Task<IActionResult> Create()
        {

            ViewData["ProductionTypeID"] = new SelectList(_context.ProductionTypes, "ProductionTypeID", "ProductionName");

            HalakaSellRecVm model = new HalakaSellRecVm();
            model.Stocks = await _context.Stocks.Include(r => r.Fish).ToListAsync();


            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(string buyerName, bool IsCash, DateTime Date, decimal TotalOfPrices, string FishNames, string ProductionTypes, string qtyss, string NOfBoxess, string UnitePrices)
        {
            if (ModelState.IsValid)
            {

                var user = await _userManager.GetUserAsync(User);
                var roles = await _userManager.GetRolesAsync(user);
                int PID = 1;
                if (roles.Contains("partner"))
                {
                    PID = 2;

                }

                HalakSellReciept halakSellReciept = new HalakSellReciept { buyerName = buyerName, IsCash = IsCash, Date = Date, PersonID = PID, TotalOfPrices = TotalOfPrices };
                _context.HalakSellReciepts.Add(halakSellReciept);

                var person = _context.People.Find(PID);
                if (IsCash == true)
                {

                    person.credit += TotalOfPrices;
                }
                _context.SaveChanges();


                var FishesCookie = FishNames.TrimEnd(FishNames[FishNames.Length - 1]);
                var ProductionTypesCookie = ProductionTypes.TrimEnd(ProductionTypes[ProductionTypes.Length - 1]);
                var qtysCookie = qtyss.TrimEnd(qtyss[qtyss.Length - 1]);
                var NOfBoxesCookie = NOfBoxess.TrimEnd(NOfBoxess[NOfBoxess.Length - 1]);
                var UnitPricesCookie = NOfBoxess.TrimEnd(NOfBoxess[NOfBoxess.Length - 1]);

                string[] Fishes = FishesCookie.Split(",").Select(c => Convert.ToString(c)).ToArray();
                string[] Productions = ProductionTypesCookie.Split(",").Select(c => Convert.ToString(c)).ToArray();
                double[] qtys = qtysCookie.Split(",").Select(c => Convert.ToDouble(c)).ToArray();
                //decimal[] unitPrices = unitpricesCookie.Split(",").Select(c => Convert.ToDecimal(c)).ToArray();
                int[] NOfBoxes = NOfBoxesCookie.Split(",").Select(c => Convert.ToInt32(c)).ToArray();
                decimal[] Prices = UnitPricesCookie.Split(",").Select(c => Convert.ToDecimal(c)).ToArray();

                for (int i = 0; i < Fishes.Length; i++)
                {
                    var fish = _context.Fishes.Single(x => x.FishName == Fishes[i]);
                    var Produc = _context.ProductionTypes.Single(x => x.ProductionName == Productions[i]);

                    HalakSellRecieptItem halakSellRecieptItem = new HalakSellRecieptItem()
                    {
                        FishID = fish.FishID,
                        BoxQty = NOfBoxes[i],
                        ProductionTypeID = Produc.ProductionTypeID,
                        Qty = qtys[i],
                        UnitPrice = Prices[i],
                        HalakSellRecieptID = halakSellReciept.HalakSellRecieptID
                    };
                    _context.HalakSellRecieptItems.Add(halakSellRecieptItem);

                    var stock = _context.Stocks.Where(i => i.FishID == fish.FishID && i.ProductionTypeID == Produc.ProductionTypeID).FirstOrDefault();

                    if (stock.ProductionTypeID == 3)
                    {
                        stock.Qty -= qtys[i] * Convert.ToDouble(NOfBoxes[i]);
                        stock.TotalWeight -= qtys[i] * Convert.ToDouble(NOfBoxes[i]);
                    }
                    else
                    {
                        stock.Qty -= qtys[i];

                    }

                    _context.SaveChanges();
                    stock = _context.Stocks.Where(i => i.FishID == fish.FishID && i.ProductionTypeID == Produc.ProductionTypeID).FirstOrDefault();
                    if (stock.Qty == 0)
                    {
                        _context.Stocks.Remove(stock);

                    }
                }
                _context.SaveChanges();
                return Json(new { message = "success" });
            }
            else
            {
                return Json(new { message = "fail" });
            }


        }



        public async Task<IActionResult> Details(int? id)
        {
            // System.Threading.Thread.Sleep(5000);
            if (id == null)
            {
                return Json(new { message = "fail" });
            }
            var items = _context.HalakSellRecieptItems.Include(c => c.Fish).Include(c => c.ProductionType).Where(c => c.HalakSellRecieptID == id).ToList();
            return PartialView(items);
        }



        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var user = await _userManager.GetUserAsync(User);
            var roles = await _userManager.GetRolesAsync(user);
            int PID = 1;
            if (roles.Contains("partner"))
            {
                PID = 2;

            }


            var HalakSellReciept = await _context.HalakSellReciepts.FirstOrDefaultAsync(ww => ww.HalakSellRecieptID == id);
            if (HalakSellReciept == null)
            {
                return NotFound();
            }
            var HalakSellRecieptItems = _context.HalakSellRecieptItems.Where(x => x.HalakSellRecieptID == id).ToList();
            if (HalakSellRecieptItems.Any())
            {
                foreach (var item in HalakSellRecieptItems)
                {
                    var stockitem = _context.Stocks.FirstOrDefault(c => c.FishID == item.FishID && c.ProductionTypeID == item.ProductionTypeID);
                    if (stockitem != null)
                    {
                        if (item.ProductionTypeID == 2)//طاوله
                        {
                            stockitem.Qty += item.Qty;
                        }
                        else
                        {
                            stockitem.TotalWeight += item.Qty;
                            stockitem.Qty += item.Qty;
                        }
                    }
                    else
                    {
                        Stock stock = new Stock();
                        stock.FishID = item.FishID;
                        stock.Date = DateTime.Now;
                        stock.ProductionTypeID = item.ProductionTypeID;
                        if (item.ProductionTypeID == 2)
                        {
                            stock.Qty = item.Qty;
                        }
                        else
                        {
                            stock.Qty = item.Qty;
                            stock.TotalWeight = item.Qty;
                        }
                        _context.Stocks.Add(stock);
                    }
                }
                _context.SaveChanges();
                _context.HalakSellRecieptItems.RemoveRange(HalakSellRecieptItems);
            }

            var person = _context.People.Find(PID);
            if (HalakSellReciept.IsCash == true)
            {
                person.credit -= HalakSellReciept.TotalOfPrices;

            }

            _context.Remove(HalakSellReciept);

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }
}
