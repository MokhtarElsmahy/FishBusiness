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
using Microsoft.AspNetCore.Authorization;

namespace FishBusiness.Controllers
{
    [Authorize]
    public class TotalOfProfitsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TotalOfProfitsController(ApplicationDbContext context)
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
        // GET: TotalOfProfits
        public async Task<IActionResult> Index()
        {

            return View(await _context.TotalOfProfits.ToListAsync());
        }

        // GET: TotalOfProfits/Details/5
       


        public  IActionResult Summation(DateTime Date)
        {

            SummationVm model = new SummationVm();
            var Datee = Date.Date;

            var IMerchantReciepts = _context.IMerchantReciept.Include(m => m.Merchant).ToList();
            model.IMerchantReciepts = IMerchantReciepts.Where(m => m.Date.Date == Datee).ToList();

            model.HalakaBuyReciepts= _context.HalakaBuyReciepts.Where(x => x.Date.Date == Datee).ToList(); //مشترى الحلقه من افراد عاديين غير تجار 

            var ISellerReciepts = _context.ISellerReciepts.Include(m => m.Merchant).ToList();
            model.ISellerReciepts = ISellerReciepts.Where(m => m.Date.Date == Datee).ToList();
            // labour
            var additionalLabour = _context.AdditionalPayments.Where(x => x.Date.Date == Datee && x.Name == "عمال").Sum(x => x.Value);
            //var debtSarhaLabour = _context.Debts_Sarhas.Include(x=>x.Debt).ToList().Where(x => x.Date.ToShortDateString() == Date.ToShortDateString() && x.Debt.DebtName == "عمال").Sum(x => x.Price);
            var HalakaHalekLabour = _context.HalakaHaleks.Include(x=>x.Debt).Where(x => x.Date.Date == Datee && x.Debt.DebtName == "عمال").Sum(x => x.Price);
            model.Labour = additionalLabour + HalakaHalekLabour;
            // ice
            var additionalIce = _context.AdditionalPayments.Where(x => x.Date.Date == Datee && x.Name == "ثلج").Sum(x => x.Value);
            //var debtSarhaIce = _context.Debts_Sarhas.Include(x => x.Debt).ToList().Where(x => x.Date.ToShortDateString() == Date.ToShortDateString() && x.Debt.DebtName == "ثلج").Sum(x => x.Price);
            var HalakaHalekIce = _context.HalakaHaleks.Include(x => x.Debt).Where(x => x.Date.Date == Datee && x.Debt.DebtName == "ثلج").Sum(x => x.Price);
            model.Ice = additionalIce + HalakaHalekIce;
            var TodayProfit = _context.TotalOfProfits.Where(x => x.Date.Date == Datee);
            if (TodayProfit.Count() > 0)
            {
                ViewBag.Profit = TodayProfit.FirstOrDefault().Profit;
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult CalcProfits(double ice , double labour , double totalOfPurchases, double totalOfSales, double totalOfCars)
        {
            var profit = totalOfSales - (totalOfPurchases+totalOfCars+ice+labour);
            TotalOfProfit t = new TotalOfProfit() { Date = TimeNow(), Ice = ice, Labour = labour, TotalOfPurchases = totalOfPurchases, TotalOfSales = totalOfSales, Profit = profit };
            _context.TotalOfProfits.Add(t);
            //Person p = _context.People.Find(1);
            //p.credit -= (decimal)(ice + labour);
            _context.SaveChanges();
         
            return Json(new {message="success" , profits = profit });
        }

        [HttpGet]
        public IActionResult SummationOfAday()
        {

            SummationVm model = new SummationVm();
            DateTime Datee = TimeNow().AddDays(10).Date;

            var IMerchantReciepts = _context.IMerchantReciept.Include(m => m.Merchant);
            model.IMerchantReciepts = IMerchantReciepts.Where(m => m.Date.Date == Datee).ToList();

            var ISellerReciepts = _context.ISellerReciepts.Include(m => m.Merchant);
            model.ISellerReciepts = ISellerReciepts.Where(m => m.Date.Date == Datee).ToList();
            return View(model);
        }
       
        public IActionResult SummationOfAdayDate(DateTime Date)
        {

          //  System.Threading.Thread.Sleep(2000);
            DateTime Datee = Date.Date;
           
            var IMerchantReciepts = _context.IMerchantReciept.Include(m => m.Merchant).ToList();
           var  IMerchantRecieptss = IMerchantReciepts.Where(m => m.Date.Date == Datee).Select(m=>new { merchantName = m.Merchant.MerchantName, totalOfReciept=m.TotalOfReciept });
            var TotalOfPurchases = IMerchantRecieptss.Select(c => c.totalOfReciept).Sum();

            var ISellerReciepts = _context.ISellerReciepts.Include(m => m.Merchant).ToList();
            var ISellerRecieptss = ISellerReciepts.Where(m => m.Date.Date == Datee).Select(m=>new { merchantName=m.Merchant.MerchantName , salesValue =(m.TotalOfPrices-m.Commision) ,commision = m.Commision , totalOfPrices=m.TotalOfPrices , carDistination = m.CarDistination ,carPrice=m.CarPrice});
            var TotalOfSales = ISellerRecieptss.Select(c => (c.totalOfPrices - c.commision)).Sum();

            var HalakaBuyReciepts = _context.HalakaBuyReciepts.Where(x => x.Date.Date == Datee).Select(m => new { merchantName = m.SellerName, totalOfReciept = m.TotalOfPrices }).ToList(); //مشترى الحلقه من افراد عاديين غير تجار
            var HalakaBuyRecieptsTotal = HalakaBuyReciepts.Sum(c => c.totalOfReciept);

            var ice_labour_profit = _context.TotalOfProfits.ToList().Where(c=>c.Date.Date==Datee).FirstOrDefault();
            double ice = 0.0;
            double labour = 0.0;
            double profit = 0.0;
            if (ice_labour_profit != null)
            {
                ice = ice_labour_profit.Ice;
                labour = ice_labour_profit.Labour;
                profit = ice_labour_profit.Profit;
            }

            return Json(new { halakaBuyReciepts= HalakaBuyReciepts, halakaBuyRecieptsTotal= HalakaBuyRecieptsTotal, purchases = IMerchantRecieptss , totalOfReciepts= TotalOfPurchases , sales = ISellerRecieptss , totalOfSales= TotalOfSales ,ice= ice, labour= labour ,profit= profit });
        }

    }
}
