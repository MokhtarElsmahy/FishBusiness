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
    public class StocksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StocksController(ApplicationDbContext context)
        {
            _context = context;
        }
        public DateTime TimeNow()
        {
            TimeZone localZone = TimeZone.CurrentTimeZone;
            DateTime currentDate = DateTime.Now;
            DateTime currentUTC =
           localZone.ToUniversalTime(currentDate);
            return currentUTC.AddHours(2);
        }
        // GET: Stocks
        public  IActionResult Index()
        {
            var applicationDbContext =  _context.Stocks.Include(s => s.Fish).Include(s => s.ProductionType).ToList();
            //return View( applicationDbContext.Where(s=>s.Date.ToShortDateString()==TimeNow().ToShortDateString()).ToList());
            return View( applicationDbContext.ToList());
        }

        // GET: Stocks/Details/5
        public async Task<IActionResult> Calssify(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stock = await _context.Stocks
                .Include(s => s.Fish)
                .Include(s => s.ProductionType)
                .FirstOrDefaultAsync(m => m.StockID == id);
            if (stock == null)
            {
                return NotFound();
            }
            ViewData["FishID"] = new SelectList(_context.Fishes, "FishID", "FishName",stock.FishID);
            ViewBag.FirstTimeFlag = stock.FirstTimeFlag;

            ViewData["ProductionTypeID"] = new SelectList(_context.ProductionTypes, "ProductionTypeID", "ProductionName",stock.ProductionTypeID);
            return PartialView(stock);
        }

        [HttpPost]
      
        public IActionResult UpdateStock(StockVM stock)
        {
            var s = _context.Stocks.Where(c => c.FishID ==stock.FishID && c.ProductionTypeID==stock.ProductionTypeID).FirstOrDefault();//نقط
            var oldstock = _context.Stocks.Find(stock.StockID);//جمبرى جامبو
         


            if (s != null) // نوع السمك موجود 
            {

                if (stock.ProductionTypeID == stock.OldProductionType && stock.OldProductionType ==3)//كميزان
                {
                   
                    s.Qty += stock.Qty;
                    oldstock.Qty -= stock.Qty;
                  
                    if (oldstock.Qty == 0)
                    {
                        _context.Stocks.Remove(oldstock);
                    }
                    
                }
                else if(stock.ProductionTypeID == 3 && stock.OldProductionType == 2)
                {
                    s.Qty += stock.Qty;
                    _context.Stocks.Remove(oldstock);
                }



                #region MyRegion
                //if (stock.FirstTimeFlag == false)
                //{

                //    s.Qty = stock.Qty;
                //    s.ProductionTypeID = stock.ProductionTypeID;
                //    s.FirstTimeFlag = true;
                //}
                //else
                //{
                //    s.Qty += stock.Qty;
                //    s.ProductionTypeID = stock.ProductionTypeID;
                //    s.FirstTimeFlag = false;
                //}
                //if (stock.ProductionTypeID != stock.OldProductionType)
                //{
                //    var ss= _context.Stocks.Where(c => c.FishID == stock.FishID && c.ProductionTypeID == stock.OldProductionType).FirstOrDefault();
                //    _context.Stocks.Remove(ss);
                //} 
                #endregion

            }
            else //نوع السمك مش موجود 
            {
                if (stock.ProductionTypeID == stock.OldProductionType && stock.OldProductionType == 3)//كميزان
                {
                    Stock stockkk = new Stock()
                    {
                        FishID = stock.FishID,
                        ProductionTypeID = stock.ProductionTypeID,
                        Qty = stock.Qty,
                        TotalWeight =stock.Qty
                    };
                    oldstock.Qty -= stock.Qty;
                    _context.Stocks.Add(stockkk);

                    if (oldstock.Qty == 0)
                    {
                        _context.Stocks.Remove(oldstock);
                    }

                }
                else if (stock.ProductionTypeID == 3 && stock.OldProductionType == 2)
                {
                    Stock stockkk = new Stock()
                    {
                        FishID = stock.FishID,
                        ProductionTypeID = stock.ProductionTypeID,
                        Qty = stock.Qty,
                        TotalWeight = stock.Qty
                    };
                    _context.Stocks.Add(stockkk);
                    _context.Stocks.Remove(oldstock);
                }
                #region MyRegion
                //var ss = _context.Stocks.Where(c => c.FishID == stock.FishID && c.ProductionTypeID == stock.OldProductionType).FirstOrDefault();
                //_context.Stocks.Remove(ss);
                //Stock stockk = new Stock()
                //{
                //    FishID = stock.FishID,
                //    ProductionTypeID = stock.ProductionTypeID,
                //    Qty = stock.Qty
                //};
                //_context.Stocks.Add(stockk); 
                #endregion
            }
            #region MyRegion
            //else
            //{
            //    Stock stockk = new Stock()
            //    {
            //        FishID = stock.FishID,
            //        ProductionTypeID = stock.ProductionTypeID,
            //        Qty = stock.Qty
            //    };
            //    _context.Stocks.Add(stockk);
            //} 
            #endregion

            _context.SaveChanges();

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
            return Json(new { message ="success" , newTotalWeight=stock.OldtotalWeight-stock.Qty});
        }



     
        public  IActionResult CreateReport()
        {
            
            var applicationDbContext = _context.Stocks.Include(s => s.Fish).Include(s => s.ProductionType).ToList();
            //return View(applicationDbContext.Where(s => s.Date.ToShortDateString() == TimeNow().ToShortDateString()).ToList());
            return View(applicationDbContext.ToList());
           // return v()
        }

        public IActionResult Details(DateTime date)
        {


            var iMerchantReciept =  _context.IMerchantReciept.Include(i => i.Merchant).Include(i=>i.IMerchantRecieptItems).ToList();

            var mr= iMerchantReciept.Where(m => m.Date.ToLongDateString() == date.ToLongDateString()).ToList();

            ViewBag.Date = date.ToLongDateString();
            
            var items = _context.IMerchantRecieptItem.Include(i => i.Fish).Include(i => i.ProductionType).ToList();


            return View(mr);
        }

    }
}
