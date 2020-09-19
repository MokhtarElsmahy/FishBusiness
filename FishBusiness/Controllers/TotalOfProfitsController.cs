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
    public class TotalOfProfitsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TotalOfProfitsController(ApplicationDbContext context)
        {
            _context = context;
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
            string Datee = Date.ToShortDateString();

            var IMerchantReciepts = _context.IMerchantReciept.Include(m => m.Merchant).ToList();
            model.IMerchantReciepts = IMerchantReciepts.Where(m => m.Date.ToShortDateString() == Datee).ToList();

            var ISellerReciepts = _context.ISellerReciepts.Include(m => m.Merchant).ToList();
            model.ISellerReciepts = ISellerReciepts.Where(m => m.Date.ToShortDateString() == Datee).ToList();
            // labour
            var additionalLabour = _context.AdditionalPayments.ToList().Where(x => x.Date.ToShortDateString() == Date.ToShortDateString() && x.Name == "عمال").Sum(x => x.Value);
            //var debtSarhaLabour = _context.Debts_Sarhas.Include(x=>x.Debt).ToList().Where(x => x.Date.ToShortDateString() == Date.ToShortDateString() && x.Debt.DebtName == "عمال").Sum(x => x.Price);
            var HalakaHalekLabour = _context.HalakaHaleks.Include(x=>x.Debt).ToList().Where(x => x.Date.ToShortDateString() == Date.ToShortDateString() && x.Debt.DebtName == "عمال").Sum(x => x.Price);
            model.Labour = additionalLabour + HalakaHalekLabour;
            // ice
            var additionalIce = _context.AdditionalPayments.ToList().Where(x => x.Date.ToShortDateString() == Date.ToShortDateString() && x.Name == "ثلج").Sum(x => x.Value);
            //var debtSarhaIce = _context.Debts_Sarhas.Include(x => x.Debt).ToList().Where(x => x.Date.ToShortDateString() == Date.ToShortDateString() && x.Debt.DebtName == "ثلج").Sum(x => x.Price);
            var HalakaHalekIce = _context.HalakaHaleks.Include(x => x.Debt).ToList().Where(x => x.Date.ToShortDateString() == Date.ToShortDateString() && x.Debt.DebtName == "ثلج").Sum(x => x.Price);
            model.Ice = additionalIce + HalakaHalekIce;
            var TodayProfit = _context.TotalOfProfits.ToList().Where(x => x.Date.ToShortDateString() == Date.ToShortDateString());
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
            TotalOfProfit t = new TotalOfProfit() { Date = DateTime.Now, Ice = ice, Labour = labour, TotalOfPurchases = totalOfPurchases, TotalOfSales = totalOfSales, Profit = profit };
            _context.TotalOfProfits.Add(t);
            Person p = _context.People.Find(1);
            p.credit -= (decimal)(ice + labour);
            _context.SaveChanges();
         
            return Json(new {message="success" , profits = profit });
        }

        [HttpGet]
        public IActionResult SummationOfAday()
        {

            SummationVm model = new SummationVm();
            string Datee = DateTime.Now.AddDays(10).ToShortDateString();

            var IMerchantReciepts = _context.IMerchantReciept.Include(m => m.Merchant).ToList();
            model.IMerchantReciepts = IMerchantReciepts.Where(m => m.Date.ToShortDateString() == Datee).ToList();

            var ISellerReciepts = _context.ISellerReciepts.Include(m => m.Merchant).ToList();
            model.ISellerReciepts = ISellerReciepts.Where(m => m.Date.ToShortDateString() == Datee).ToList();
            return View(model);
        }
       
        public IActionResult SummationOfAdayDate(DateTime Date)
        {

            System.Threading.Thread.Sleep(2000);
            string Datee = Date.ToShortDateString();
           
            var IMerchantReciepts = _context.IMerchantReciept.Include(m => m.Merchant).ToList();
           var  IMerchantRecieptss = IMerchantReciepts.Where(m => m.Date.ToShortDateString() == Datee).Select(m=>new { merchantName = m.Merchant.MerchantName, totalOfReciept=m.TotalOfReciept });
            var TotalOfPurchases = IMerchantRecieptss.Select(c => c.totalOfReciept).Sum();

            var ISellerReciepts = _context.ISellerReciepts.Include(m => m.Merchant).ToList();
            var ISellerRecieptss = ISellerReciepts.Where(m => m.Date.ToShortDateString() == Datee).Select(m=>new { merchantName=m.Merchant.MerchantName , salesValue =(m.TotalOfPrices-m.Commision) ,commision = m.Commision , totalOfPrices=m.TotalOfPrices , carDistination = m.CarDistination ,carPrice=m.CarPrice});
            var TotalOfSales = ISellerRecieptss.Select(c => (c.totalOfPrices - c.commision)).Sum();

            var ice_labour_profit = _context.TotalOfProfits.ToList().Where(c=>c.Date.ToShortDateString()==Date.ToShortDateString()).FirstOrDefault();

            return Json(new { purchases = IMerchantRecieptss , totalOfReciepts= TotalOfPurchases , sales = ISellerRecieptss , totalOfSales= TotalOfSales ,ice= ice_labour_profit.Ice, labour= ice_labour_profit.Labour ,profit= ice_labour_profit.Profit });
        }

    }
}
