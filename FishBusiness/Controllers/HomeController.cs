using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FishBusiness.Models;
using FishBusiness.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace FishBusiness.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger , ApplicationDbContext context)
        {
            _logger = logger;
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

       [Authorize]
        public IActionResult Index()
        {
           //_context.Roles.Add(new IdentityRole() { Name = "admin" });
           //_context.SaveChanges();


            var TodaysBoatReceipts = _context.BoatOwnerReciepts.ToList().Where(d => d.Date.ToShortDateString() == TimeNow().ToShortDateString());
            if (TodaysBoatReceipts != null)
            {
                ViewBag.TodaysBoatReceipts = TodaysBoatReceipts.Count();
            }
            else
            {
                ViewBag.TodaysBoatReceipts = 0;
            }
            var TodaysMerchantReceipts = _context.MerchantReciepts.ToList().Where(d => d.Date.ToShortDateString() == TimeNow().ToShortDateString());
            if (TodaysMerchantReceipts !=null)
            {
                ViewBag.TodaysMerchantReceipts= TodaysMerchantReceipts.Count();
            }
            else
            {
                ViewBag.TodaysMerchantReceipts = 0;
            }
            var TodaysExternalBoatReceipts = _context.ExternalReceipts.ToList().Where(d => d.Date.ToShortDateString() == TimeNow().ToShortDateString());
            if (TodaysExternalBoatReceipts != null)
            {
                ViewBag.TodaysExternalBoatReceipts = TodaysExternalBoatReceipts.Count();
            }
            else
            {
                ViewBag.TodaysExternalBoatReceipts = 0;
            }
            var Stock = _context.Stocks;
            if (Stock != null)
            {
                ViewBag.Stock = Stock.Count();
            }
            else
            {
                ViewBag.Stock = 0;
            }
         
            



           var ProfitOfDay = _context.TotalOfProfits.ToList().Where(d => d.Date.ToShortDateString() == TimeNow().ToShortDateString()).FirstOrDefault();
            if (ProfitOfDay != null)
            {
                ViewBag.ProfitOfDay = ProfitOfDay.Profit;
            }
            else
            {
                ViewBag.ProfitOfDay = 0;
            }
             return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpGet]
        public IActionResult GetTodaysBoatReceipts()
        {
            var Recs = _context.BoatOwnerReciepts.Include(b => b.Boat).Include(b => b.Sarha).ToList().Where(d => d.Date.ToShortDateString() == TimeNow().ToShortDateString());
            return View(Recs);
        }
        [HttpGet]
        public IActionResult GetTodaysMerchantReceipts()
        {
            var Recs = _context.MerchantReciepts.Include(m => m.Merchant).ToList().Where(d => d.Date.ToShortDateString() == TimeNow().ToShortDateString());
            return View(Recs);
        }
        [HttpGet]
        public IActionResult GetTodaysExternalBoatReceipts()
        {
            var Recs = _context.ExternalReceipts.Include(m => m.Boat).ToList().Where(d => d.Date.ToShortDateString() == TimeNow().ToShortDateString());
            return View(Recs);
        }
        // العمولات
        [HttpGet]
        public IActionResult GetTodaysReceiptsCommission()
        {
            var Recs = _context.BoatOwnerReciepts.Include(b => b.Boat).Include(b => b.Sarha).ToList().Where(d => d.Date.ToShortDateString() == TimeNow().ToShortDateString());
            return View(Recs);
        }
        // فواتير سافرت
        [HttpGet]
        public IActionResult GetTodaysIsellerReceipts()
        {
            var Recs = _context.ISellerReciepts.Include(m => m.Merchant).ToList().Where(d => d.Date.ToShortDateString() == TimeNow().ToShortDateString() && d.TotalOfPrices!=0);
            return View(Recs);
        }
        // فواتير خارجية
        [HttpGet]
        public IActionResult GetTodaysExternalReceiptsForSharedBoats()
        {
            var Recs = _context.ExternalReceipts.Include(m => m.Boat).ToList().Where(d => d.Date.ToShortDateString() == TimeNow().ToShortDateString());
            return View(Recs);
        }
        // ايراد مراكب شريكة
        [HttpGet]
        public IActionResult GetTodaysSharedBoatsIncome()
        {
            var Recs = _context.BoatOwnerReciepts.Include(m => m.Boat).Include(m=>m.Boat.BoatType).ToList().Where(d => d.Date.ToShortDateString() == TimeNow().ToShortDateString() && d.Boat.BoatType.TypeID==2);
            return View(Recs);
        }

        //تصفيه مع مراكب شريكه
        [HttpGet]
        public IActionResult CheckoutsOfSharedBoats()
        {
            var Recs = _context.Checkouts.Include(c=>c.Boat).ToList().Where(c => c.Date.ToShortDateString() == TimeNow().ToShortDateString()).ToList();
            return View(Recs);
        }

        //تصفيه مع مركب شريكه
        public IActionResult CheckoutsOfSharedBoat(int id)
        {
            var Recs = _context.Checkouts.Include(c => c.Boat).ToList().Where(c =>c.BoatID ==id).ToList();
            ViewBag.BoatName = _context.Boats.Find(id).BoatName;
            return View(Recs);
        }
        // مدفوعات ريس المركب
        [HttpGet]
        public IActionResult GetTodaysLeaderLoansPayBack()
        {
            var Recs = _context.LeaderPaybacks.Include(m => m.Boat).ToList().Where(d => d.Date.ToShortDateString() == TimeNow().ToShortDateString());
           
            return View(Recs);
        }
        [HttpGet]
        public IActionResult Office()
        {
            var date = TimeNow().ToShortDateString();
            OfficeVM model = new OfficeVM();
            // income
            model.Commisions = _context.BoatOwnerReciepts.ToList().Where(x => x.Date.ToShortDateString() == date).Sum(x => x.Commission);

            //the same ?
            model.IsellerReceiptsTotal =(decimal) _context.ISellerReciepts.ToList().Where(x => x.Date.ToShortDateString() ==date).Sum(x => x.PaidFromDebt);
            model.SalesTotal =(decimal) _context.ISellerReciepts.ToList().Where(x => x.Date.ToShortDateString() == date ).Sum(x => x.PaidFromDebt);

            model.externalReceiptsTotal = _context.ExternalReceipts.ToList().Where(x => x.Date.ToShortDateString() == date).Sum(x => x.FinalIncome);
            model.SharedBoatsReceiptsTotal = _context.IncomesOfSharedBoats.ToList().Where(x => x.Date.ToShortDateString() == date).Sum(x => x.Income);
            model.collectorForUsTotal = _context.PaidForMerchant.ToList().Where(x => x.Date.ToShortDateString() == date && x.PersonID==3 && x.IsPaidForUs==true).Sum(x => x.Payment);
            model.LeaderLoansPaybackTotal = _context.LeaderPaybacks.ToList().Where(x => x.Date.ToShortDateString() == date ).Sum(x => x.Price);
            model.CheckoutsOfSharedBoats = (decimal)_context.Checkouts.ToList().Where(c => c.Date.ToShortDateString() == date).Sum(c=>c.PaidForUs);
            //outcome
            model.FathallahTotal = _context.FathAllahGifts.ToList().Where(x => x.Date.ToShortDateString() == date).Sum(x => x.charge);
            model.CollectorTotalFromUs = _context.PaidForMerchant.Include(x=>x.Merchant).ToList().Where(x => x.Date.ToShortDateString() == date && x.PersonID == 1 && x.IsPaidForUs == true && x.Merchant.IsOwner==true).Sum(x => x.Payment);
            var CollectorPaidForMerchant = _context.PaidForMerchant.ToList().Where(x => x.Date.ToShortDateString() == date && x.PersonID == 3 && x.IsPaidForUs == false).Sum(x => x.Payment);
            var CollectorPaidForHalek = _context.Debts_Sarhas.ToList().Where(x => x.Date.ToShortDateString() == date && x.PersonID == 3 ).Sum(x => x.Price);
            var CollectorPaidForHalekFromFathallahAndMohamed = _context.Debts_In_Sarhas.ToList().Where(x => x.Date.ToShortDateString() == date).Sum(x => x.Price);

            var CollectorPaidForFathallah = _context.FathAllahGifts.ToList().Where(x => x.Date.ToShortDateString() == date && x.PersonID == 3 ).Sum(x => x.charge);
            var CollectorPaidForAdditional = _context.AdditionalPayments.ToList().Where(x => x.Date.ToShortDateString() == date).Sum(x => x.Value);
            model.CollectorTotalforMerchantsAndHalek = CollectorPaidForMerchant + CollectorPaidForHalek + CollectorPaidForFathallah + CollectorPaidForAdditional;

            var totalOfProfit = _context.TotalOfProfits.ToList().Where(x => x.Date.ToShortDateString() == date).FirstOrDefault();
            var carPrices = _context.ISellerReciepts.ToList().Where(x => x.Date.ToShortDateString() == date).Sum(x => x.CarPrice);

           
            if (totalOfProfit != null)
            {
                model.BuyingTotal = Convert.ToDecimal(totalOfProfit.TotalOfSales + totalOfProfit.Labour + totalOfProfit.Ice + carPrices);
            }
            else
                model.BuyingTotal = 0.0m;
            ViewBag.Credit = _context.People.Find(1).credit;
            return View(model);
        }
        [HttpGet]
        public IActionResult OfficeOfDay()
        {
            return View(new OfficeVM());
        }
        public IActionResult GetTodayOffice(DateTime Date)
        {
            var date = Date.ToShortDateString();
            OfficeVM model = new OfficeVM();
            // income
            model.Commisions = _context.BoatOwnerReciepts.ToList().Where(x => x.Date.ToShortDateString() == date).Sum(x => x.Commission);
            model.IsellerReceiptsTotal = (decimal)_context.ISellerReciepts.ToList().Where(x => x.Date.ToShortDateString() == date).Sum(x => x.PaidFromDebt);
            model.externalReceiptsTotal = _context.ExternalReceipts.ToList().Where(x => x.Date.ToShortDateString() == date).Sum(x => x.FinalIncome);
            model.SharedBoatsReceiptsTotal = _context.BoatOwnerReciepts.ToList().Where(x => x.Date.ToShortDateString() == date).Sum(x => x.FinalIncome);
            model.collectorForUsTotal = _context.PaidForMerchant.ToList().Where(x => x.Date.ToShortDateString() == date && x.PersonID == 3 && x.IsPaidForUs == true).Sum(x => x.Payment);
            model.LeaderLoansPaybackTotal = _context.LeaderPaybacks.ToList().Where(x => x.Date.ToShortDateString() == date).Sum(x => x.Price);
            model.SalesTotal = (decimal)_context.ISellerReciepts.ToList().Where(x => x.Date.ToShortDateString() == date).Sum(x => x.PaidFromDebt);
            model.CheckoutsOfSharedBoats = (decimal)_context.Checkouts.ToList().Where(c => c.Date.ToShortDateString() == date).Sum(c => c.PaidForUs);
            //outcome
            model.FathallahTotal = _context.FathAllahGifts.ToList().Where(x => x.Date.ToShortDateString() == date).Sum(x => x.charge);
            model.CollectorTotalFromUs = _context.PaidForMerchant.Include(x => x.Merchant).ToList().Where(x => x.Date.ToShortDateString() == date && x.PersonID == 1 && x.IsPaidForUs == true && x.Merchant.IsOwner == true).Sum(x => x.Payment);
            var CollectorPaidForMerchant = _context.PaidForMerchant.ToList().Where(x => x.Date.ToShortDateString() == date && x.PersonID == 3 && x.IsPaidForUs == false).Sum(x => x.Payment);
            var CollectorPaidForHalek = _context.Debts_Sarhas.ToList().Where(x => x.Date.ToShortDateString() == date && x.PersonID == 3).Sum(x => x.Price);
            var CollectorPaidForFathallah = _context.FathAllahGifts.ToList().Where(x => x.Date.ToShortDateString() == date && x.PersonID == 3).Sum(x => x.charge);
            var CollectorPaidForAdditional = _context.AdditionalPayments.ToList().Where(x => x.Date.ToShortDateString() == date).Sum(x => x.Value);
            model.CollectorTotalforMerchantsAndHalek = CollectorPaidForMerchant + CollectorPaidForHalek + CollectorPaidForFathallah + CollectorPaidForAdditional;
            var totalOfProfit = _context.TotalOfProfits.ToList().Where(x => x.Date.ToShortDateString() == date).FirstOrDefault();
            var carPrices = _context.ISellerReciepts.ToList().Where(x => x.Date.ToShortDateString() == date).Sum(x => x.CarPrice);
            if (totalOfProfit != null)
            {
                model.BuyingTotal = Convert.ToDecimal(totalOfProfit.TotalOfSales + totalOfProfit.Labour + totalOfProfit.Ice + carPrices);
            }
            else
                model.BuyingTotal = 0.0m;
            var totalIncome = model.Commisions + model.IsellerReceiptsTotal + model.externalReceiptsTotal + model.SharedBoatsReceiptsTotal
                + model.collectorForUsTotal + model.LeaderLoansPaybackTotal + model.SalesTotal+ model.CheckoutsOfSharedBoats;
            var totalOutcome = model.FathallahTotal + model.CollectorTotalFromUs + model.CollectorTotalforMerchantsAndHalek + model.BuyingTotal;
            return Json(new
            {
                message = "success",
                commisions = model.Commisions,
                isellerReceiptsTotal = model.IsellerReceiptsTotal,
                externalReceiptsTotal = model.externalReceiptsTotal,
                sharedBoatsReceiptsTotal = model.SharedBoatsReceiptsTotal,
                collectorForUsTotal = model.collectorForUsTotal,
                leaderLoansPaybackTotal = model.LeaderLoansPaybackTotal,
                salesTotal = model.SalesTotal,
                fathallahTotal = model.FathallahTotal,
                collectorTotalFromUs = model.CollectorTotalFromUs,
                collectorTotalforMerchantsAndHalek = model.CollectorTotalforMerchantsAndHalek,
                buyingTotal = model.BuyingTotal,
                credit = _context.People.Find(1).credit,
                totalIncome = totalIncome,
                totalOutcome = totalOutcome,
                finalTotal = totalIncome - totalOutcome
            });
        }
        public IActionResult Charge(decimal value)
        {
            Person p = _context.People.Find(1);
            p.credit += value;
            _context.SaveChanges();
            return Json(new { message = "success", newCredit = p.credit });
        }
    }
}
