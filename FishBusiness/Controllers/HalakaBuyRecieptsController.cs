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
    public class HalakaBuyRecieptsController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public DateTime TimeNow()
        {
            TimeZone localZone = TimeZone.CurrentTimeZone;
            DateTime currentDate = DateTime.Now;
            DateTime currentUTC =
           localZone.ToUniversalTime(currentDate);
            return currentUTC.AddHours(2);
        }

        public HalakaBuyRecieptsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;

            _userManager = userManager;

        }

       

       

        public IActionResult Index()
        {
            var lst = _context.HalakaBuyReciepts.Where(c=>c.Date.Date==TimeNow().Date).ToList();
            return View(lst);
        }
        public IActionResult HalakaBuyRecieptsHistory(DateTime date)
        {
            var lst = _context.HalakaBuyReciepts.Where(c => c.Date.Date == date.Date).ToList();
            return PartialView(lst);
        }

        public  IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var HalakaBuyReciept =  _context.HalakaBuyReciepts
                
                .FirstOrDefault(m => m.HalakaBuyRecieptID == id);


            if (HalakaBuyReciept == null)
            {
                return NotFound();
            }

            HalakaBuyRecDetailsVm model = new HalakaBuyRecDetailsVm();
            model.halakaBuyReciept = HalakaBuyReciept;

            model.NormalIMerchantItems = _context.HalakaBuyRecieptItems.Include(c => c.Fish).Include(c => c.ProductionType).Where(c => c.HalakaBuyRecieptID == HalakaBuyReciept.HalakaBuyRecieptID && c.AmountId == null).ToList();
            model.AmountIMerchantItems = _context.HalakaBuyRecieptItems.Include(c => c.Fish).Include(c => c.ProductionType).Where(c => c.HalakaBuyRecieptID == HalakaBuyReciept.HalakaBuyRecieptID && c.AmountId != null).ToList();


            var results = from p in model.AmountIMerchantItems
                          group p.HalakaBuyRecieptItemID by p.AmountId into g
                          select new AmountVm { AmountId = g.Key, items = g };

            model.Amounts = results;


            return PartialView(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["FishID"] = new SelectList(_context.Fishes, "FishID", "FishName");


            ViewData["ProductionTypeID"] = new SelectList(_context.ProductionTypes, "ProductionTypeID", "ProductionName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(HalakaBuyRecVm data)
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

                var FishesCookie = data.FishNames.TrimEnd(data.FishNames[data.FishNames.Length - 1]);
                var ProductionTypesCookie = data.ProductionTypes.TrimEnd(data.ProductionTypes[data.ProductionTypes.Length - 1]);
                var qtysCookie = data.qtys.TrimEnd(data.qtys[data.qtys.Length - 1]);
                var unitpricesCookie = data.unitprices.TrimEnd(data.unitprices[data.unitprices.Length - 1]);

                string[] Fishes = FishesCookie.Split(",").Select(c => Convert.ToString(c)).ToArray();
                string[] Productions = ProductionTypesCookie.Split(",").Select(c => Convert.ToString(c)).ToArray();

                string[] qtys = qtysCookie.Split(",").Select(c => Convert.ToString(c)).ToArray();
                decimal[] unitPrices = unitpricesCookie.Split(",").Select(c => Convert.ToDecimal(c)).ToArray();

                HalakaBuyReciept halakaBuyReciept = new HalakaBuyReciept() { Date = data.Date, PersonID = PID, SellerName = data.SellerName, TotalOfPrices = data.TotalOfReciept };
                _context.HalakaBuyReciepts.Add(halakaBuyReciept);
                var pp = _context.People.Find(PID);
                pp.credit -= data.TotalOfReciept;
                _context.SaveChanges();

                for (int i = 0; i < Fishes.Length; i++)
                {
                    string[] splitItem = Fishes[i].Split("/").Select(c => Convert.ToString(c)).ToArray();

                    if (splitItem.Length > 1)
                    {
                        Guid amountID = Guid.NewGuid();
                        double[] splitItemQty = qtys[i].Split("/").Select(c => Convert.ToDouble(c)).ToArray();
                        for (int j = 0; j < splitItem.Length; j++)
                        {


                            var fish = _context.Fishes.Single(x => x.FishName == splitItem[j]);
                            var Produc = _context.ProductionTypes.Single(x => x.ProductionName == Productions[i]);



                            HalakaBuyRecieptItem HalakaBuyRecieptItem = new HalakaBuyRecieptItem()
                            {
                                HalakaBuyRecieptID = halakaBuyReciept.HalakaBuyRecieptID,
                                FishID = fish.FishID,
                                ProductionTypeID = Produc.ProductionTypeID,
                                Qty = splitItemQty[j],
                                UnitPrice = unitPrices[i],
                                AmountId = amountID

                            };
                            _context.HalakaBuyRecieptItems.Add(HalakaBuyRecieptItem);

                            _context.SaveChanges();

                            var s = _context.Stocks.Where(c => c.FishID == fish.FishID).FirstOrDefault();
                            if (s != null)
                            {
                                if (s.ProductionTypeID == Produc.ProductionTypeID)
                                {
                                    if (s.ProductionTypeID == 3)
                                    {
                                        s.Qty += splitItemQty[j];
                                        s.TotalWeight += splitItemQty[j];

                                    }
                                    else
                                    {
                                        s.Qty += splitItemQty[j];
                                    }

                                }
                                else
                                {

                                    Stock stoc = new Stock()
                                    {
                                        FishID = fish.FishID,
                                        ProductionTypeID = Produc.ProductionTypeID,
                                        Qty = splitItemQty[j],
                                        Date = data.Date
                                    };
                                    _context.Stocks.Add(stoc);
                                }
                            }
                            else
                            {

                                Stock stock = new Stock()
                                {
                                    FishID = fish.FishID,
                                    ProductionTypeID = Produc.ProductionTypeID,
                                    Qty = splitItemQty[j],
                                };
                                _context.Stocks.Add(stock);
                            }



                            var stockHistory = _context.StockHistories.ToList().Where(c => c.Date.ToShortDateString() == TimeNow().ToShortDateString() && c.FishID == fish.FishID && c.ProductionTypeID == Produc.ProductionTypeID).FirstOrDefault();
                            if (stockHistory != null)
                            {
                                stockHistory.Total += splitItemQty[j];
                            }
                            else
                            {
                                StockHistory history = new StockHistory() { FishID = fish.FishID, ProductionTypeID = Produc.ProductionTypeID, Total = splitItemQty[j], Date = TimeNow() };
                                _context.StockHistories.Add(history);
                            }



                        }
                    }
                    else
                    {



                        var fish = _context.Fishes.Single(x => x.FishName == Fishes[i]);
                        var Produc = _context.ProductionTypes.Single(x => x.ProductionName == Productions[i]);


                        HalakaBuyRecieptItem NewIMerchantRecieptItems = new HalakaBuyRecieptItem()
                        {
                            HalakaBuyRecieptID = halakaBuyReciept.HalakaBuyRecieptID,
                            FishID = fish.FishID,
                            ProductionTypeID = Produc.ProductionTypeID,
                            Qty = double.Parse(qtys[i]),
                            UnitPrice = unitPrices[i],

                        };
                        _context.HalakaBuyRecieptItems.Add(NewIMerchantRecieptItems);

                        _context.SaveChanges();

                        var s = _context.Stocks.Where(c => c.FishID == fish.FishID).FirstOrDefault();
                        if (s != null)
                        {
                            if (s.ProductionTypeID == Produc.ProductionTypeID)
                            {
                                s.Qty += double.Parse(qtys[i]);
                                _context.SaveChanges();
                            }
                            else
                            {

                                Stock stoc = new Stock()
                                {
                                    FishID = fish.FishID,
                                    ProductionTypeID = Produc.ProductionTypeID,
                                    Qty = double.Parse(qtys[i]),
                                    Date = data.Date
                                };
                                _context.Stocks.Add(stoc);
                                _context.SaveChanges();
                            }
                        }
                        else
                        {

                            Stock stock = new Stock()
                            {
                                FishID = fish.FishID,
                                ProductionTypeID = Produc.ProductionTypeID,
                                Qty = double.Parse(qtys[i])
                            };
                            _context.Stocks.Add(stock);
                            _context.SaveChanges();
                        }



                        var stockHistory = _context.StockHistories.ToList().Where(c => c.Date.ToShortDateString() == TimeNow().ToShortDateString() && c.FishID == fish.FishID && c.ProductionTypeID == Produc.ProductionTypeID).FirstOrDefault();
                        if (stockHistory != null)
                        {
                            stockHistory.Total += double.Parse(qtys[i]);
                        }
                        else
                        {
                            StockHistory history = new StockHistory() { FishID = fish.FishID, ProductionTypeID = Produc.ProductionTypeID, Total = double.Parse(qtys[i]), Date = TimeNow() };
                            _context.StockHistories.Add(history);
                        }

                    }
                    //_context.IMerchantRecieptItem.Add(IMerchantRecieptItems);
                  


                }

                var stockrows = _context.Stocks.ToList();
                foreach (var item in stockrows)
                {
                    if (item.ProductionTypeID == 3)//ميزان
                    {
                        item.TotalWeight = item.Qty;
                    }
                    //else
                    //{
                    //    //الطوايل هتتم عن طريق التصنيف 
                    //}
                }

                _context.SaveChanges();
                return Json(new { message = "success" });
                //return RedirectToAction(nameof(Details),new { id= ImerchantReciept.IMerchantRecieptID });
            }
            else
            {
                return Json(new { message = "fail" });

            }
            //   return View();
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


            var HalakaBuyReciept = await _context.HalakaBuyReciepts.FirstOrDefaultAsync(ww => ww.HalakaBuyRecieptID == id);
            if (HalakaBuyReciept == null)
            {
                return NotFound();
            }
            var HalakaBuyRecieptItems = _context.HalakaBuyRecieptItems.Where(x => x.HalakaBuyRecieptID == id).ToList();
            if (HalakaBuyRecieptItems.Any())
            {
                foreach (var item in HalakaBuyRecieptItems)
                {
                    var stockitem = _context.Stocks.FirstOrDefault(c => c.FishID == item.FishID && c.ProductionTypeID == item.ProductionTypeID);
                    if (stockitem != null)
                    {
                        if (item.ProductionTypeID == 2)//طاوله
                        {
                            stockitem.Qty -= item.Qty;
                        }
                        else
                        {
                            stockitem.TotalWeight -= item.Qty;
                            stockitem.Qty -= item.Qty;
                        }
                    }

                    var stockHistory = _context.StockHistories.ToList().Where(c => c.Date.ToShortDateString() == HalakaBuyReciept.Date.ToShortDateString() && c.FishID == item.FishID && c.ProductionTypeID == item.ProductionTypeID).FirstOrDefault();
                    if (stockHistory != null)
                    {
                        stockHistory.Total -= item.Qty;
                    }

                }
                _context.SaveChanges();
               
            }

            var person = _context.People.Find(PID);
            person.credit += HalakaBuyReciept.TotalOfPrices;

            _context.Remove(HalakaBuyReciept);

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
