using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FishBusiness.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using FishBusiness.Models;

namespace FishBusiness.Controllers
{
    public class BranchOfficesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public BranchOfficesController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
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
        public async Task<IActionResult> OfficeToday()
        {
            var user = await _userManager.GetUserAsync(User);
            var roles = await _userManager.GetRolesAsync(user);
            int PID = 1;
            if (roles.Contains("partner"))
            {
                PID = 2;
                ViewBag.title = "المكتب الفرعي / يومية علاء";
            }
            else
                ViewBag.title = "المكتب";
            var branch = _context.BranchOffices.ToList().Where(x => x.Date.ToShortDateString() == TimeNow().AddDays(-1).ToShortDateString()).FirstOrDefault();
            var c = _context.Collectings.ToList().Where(x => x.Date.ToShortDateString() == TimeNow().ToShortDateString()).FirstOrDefault();
            BranchOfficeVM model = new BranchOfficeVM();
            if (c != null)
            {
                model.Collecting = (c.TotalPaidFromMerchants + c.TotalOfCashes) - (c.TotalPaidForMerchants + c.TotalHalek + c.TotalOfAdditionalPayment + c.TotalForFahAllah + c.TotalOfExpenses);

            }
            else
                model.Collecting = 0.0m;
            var halek = _context.Debts_Sarhas.Include(c => c.Sarha).Include(c => c.Debt).Include(c => c.Sarha.Boat).ToList().Where(c => c.Date.ToShortDateString() == TimeNow().ToShortDateString() && c.PersonID == PID).ToList();
            model.CurrentCredit = 0.0m;
            if (branch != null)
            {
              model.CurrentCredit = branch.CurrentCredit;

            }
            model.IsellerReciepts = _context.ISellerReciepts.Include(x => x.Merchant).ToList().Where(x => x.Date.ToShortDateString() == TimeNow().ToShortDateString() && x.TotalOfPrices != 0 && x.PersonID == PID).ToList();
            model.PaidForSellers = _context.PaidForSellers.Include(x => x.Merchant).ToList().Where(x => x.Date.ToShortDateString() == TimeNow().ToShortDateString() && x.PersonID == PID).ToList();
            model.PaidForMerchants = _context.PaidForMerchant.Include(x => x.Merchant).ToList().Where(x => x.Date.ToShortDateString() == TimeNow().ToShortDateString() && x.PersonID == PID && x.IsCash==true && x.IsPaidForUs==true).ToList();
            model.PaidForBoats = _context.PaidForBoats.Include(x => x.Boat).ToList().Where(x => x.Date.ToShortDateString() == TimeNow().ToShortDateString() && x.PersonID == PID).ToList();
            model.HalekDifferences = _context.HalekDifferences.Include(x => x.Boat).ToList().Where(x => x.Date.ToShortDateString() == TimeNow().ToShortDateString() && x.PersonID == PID).ToList();
            model.Debts_Sarha = halek;
            ViewBag.Merchant = new SelectList(_context.Merchants.Where(c => c.IsFromOutsideCity == false).ToList(), "MerchantID", "MerchantName");
            ViewBag.Halek = new SelectList(_context.Debts.ToList(), "DebtID", "DebtName");
            var UnfinishedSarhas = _context.Sarhas.Where(x => x.IsFinished == false).Select(x => x.BoatID);
            ViewBag.Boats = new SelectList(_context.Boats.Where(b => b.IsActive == true && b.BoatLicenseNumber != "0").Where(b => UnfinishedSarhas.Contains(b.BoatID)).ToList(), "BoatID", "BoatName");
            ViewBag.SharedBoats = new SelectList(_context.Boats.Where(b => b.IsActive == true && b.TypeID == 2).Where(b => UnfinishedSarhas.Contains(b.BoatID)).ToList(), "BoatID", "BoatName");
            ViewBag.Operators = new SelectList(_context.Operators.ToList(), "OperatorID", "OperatorName");

            return View(model);
        }

        public async Task<IActionResult> FinalCalc(decimal PricePaidFromMagdy, decimal driversTotal, decimal PricePaidForFathallah,
    decimal PaidFathallahSalary, decimal totalOfIncome, decimal totalSarhas, decimal totalOfDailyExpense, decimal FinalTotal,
    string InternalMerchant, string InternalMerchantPrice, string ToLeaderBoatName, string ToLeaderPrice, string BoatNameForHalek,
    string HalekPrice, string HalekName, string Adding, string AddingPrice, string BoatNameExpenses, string ExpensePricee,
   string OperatorName, string OperatorPrice, string Cause)
        {
            var c = _context.Collectings.ToList().Where(x => x.Date.ToShortDateString() == TimeNow().ToShortDateString()).FirstOrDefault();
            decimal collectedToday = 0.0m;
            if (c != null)
            {
                collectedToday = (c.TotalPaidFromMerchants + c.TotalOfCashes) - (c.TotalPaidForMerchants + c.TotalHalek + c.TotalOfAdditionalPayment + c.TotalForFahAllah + c.TotalOfExpenses);

            }
            BranchOffice branch = new BranchOffice()
            {
                Date = TimeNow(),
                Collecting = collectedToday,
                DriversSalary = driversTotal,
                ExpensesTotal = totalOfDailyExpense,
                FathallahSalary = PaidFathallahSalary,
                OfficeMoney = PricePaidFromMagdy,
                SarhasTotal = totalSarhas,
                CurrentCredit = FinalTotal,
                IncomeTotal = totalOfIncome


            };
            _context.BranchOffices.Add(branch);
            _context.SaveChanges();
            var user = await _userManager.GetUserAsync(User);
            var roles = await _userManager.GetRolesAsync(user);
            int PID = 1;
            if (roles.Contains("partner"))
                PID = 2;
            if (InternalMerchant != null && InternalMerchantPrice != null)
            {
                var InternalMerchantCookie = InternalMerchant.TrimEnd(InternalMerchant[InternalMerchant.Length - 1]);
                var InternalMerchantPriceCookie = InternalMerchantPrice.TrimEnd(InternalMerchantPrice[InternalMerchantPrice.Length - 1]);
                string[] InternalMerchantNames = InternalMerchantCookie.Split(",").Select(c => Convert.ToString(c)).ToArray();
                decimal[] InternalMerchantprices = InternalMerchantPriceCookie.Split(",").Select(c => Convert.ToDecimal(c)).ToArray();

                for (int i = 0; i < InternalMerchantNames.Length; i++)
                {
                    var merchant = _context.Merchants.FirstOrDefault(c => c.MerchantName == InternalMerchantNames[i]);
                    PaidForMerchant m;
                    m = new PaidForMerchant()
                    {
                        IsPaidForUs = true,
                        MerchantID = merchant.MerchantID,
                        Payment = InternalMerchantprices[i],
                        PreviousDebtsForMerchant = merchant.PreviousDebts - InternalMerchantprices[i],
                        Date = TimeNow(),
                        IsCash = true,
                        PersonID = PID
                    };

                    //Person pppp = _context.People.Find(PID);
                    //pppp.credit += InternalMerchantprices[i];
                    merchant.PreviousDebts = merchant.PreviousDebts - InternalMerchantprices[i];
                    _context.PaidForMerchant.Add(m);
                    _context.SaveChanges();
                }

            }

            if (ToLeaderBoatName != null && ToLeaderPrice != null)
            {
                var ToLeaderCookie = ToLeaderBoatName.TrimEnd(ToLeaderBoatName[ToLeaderBoatName.Length - 1]);
                var ToPriceCookie = ToLeaderPrice.TrimEnd(ToLeaderPrice[ToLeaderPrice.Length - 1]);

                string[] ToLeaderBoatNames = ToLeaderCookie.Split(",").Select(c => Convert.ToString(c)).ToArray();
                decimal[] Toprices = ToPriceCookie.Split(",").Select(c => Convert.ToDecimal(c)).ToArray();

                for (int i = 0; i < ToLeaderBoatNames.Length; i++)
                {
                    var Boat = _context.Boats.Where(c => c.BoatName == ToLeaderBoatNames[i]).FirstOrDefault();
                    _context.LeaderLoans.Add(new LeaderLoan { Price = Toprices[i], BoatID = Boat.BoatID, Date = TimeNow(), PersonID = PID });
                    Boat.DebtsOfLeader += Toprices[i];
                    //pp.credit -= Toprices[i];
                    _context.SaveChanges();
                }

            }

            if (HalekName != null)
            {
                var BoatNameCookie = BoatNameForHalek.TrimEnd(BoatNameForHalek[BoatNameForHalek.Length - 1]);
                var HalekPriceCookie = HalekPrice.TrimEnd(HalekPrice[HalekPrice.Length - 1]);
                var HalekNameCookie = HalekName.TrimEnd(HalekName[HalekName.Length - 1]);
                string[] BoatNames = BoatNameCookie.Split(",").Select(c => Convert.ToString(c)).ToArray();
                decimal[] HalekPrices = HalekPriceCookie.Split(",").Select(c => Convert.ToDecimal(c)).ToArray();
                string[] HalekNames = HalekNameCookie.Split(",").Select(c => Convert.ToString(c)).ToArray();

                for (int i = 0; i < HalekNames.Length; i++)
                {
                    var boat = _context.Boats.FirstOrDefault(c => c.BoatName == BoatNames[i]);
                    var lastSarhaID = _context.Sarhas.Where(c => c.BoatID == boat.BoatID && c.IsFinished == false).FirstOrDefault().SarhaID;
                    var lastSarha = _context.Sarhas.Find(lastSarhaID);

                    var debt_sarha = _context.Debts_Sarhas.Include(c => c.Debt).Where(c => c.SarhaID == lastSarhaID && c.Debt.DebtName == HalekNames[i] && c.PersonID == PID && c.Date.ToShortDateString()==TimeNow().ToShortDateString()).FirstOrDefault();
                    boat.DebtsOfHalek += HalekPrices[i];
                    if (debt_sarha != null)
                    {
                        debt_sarha.Price += HalekPrices[i];
                    }
                    else
                    {
                        var debt = _context.Debts.Where(c => c.DebtName == HalekNames[i]).FirstOrDefault();
                        Debts_Sarha ds = new Debts_Sarha() { Price = HalekPrices[i], DebtID = debt.DebtID, SarhaID = lastSarhaID, PersonID = PID, Date = TimeNow() };
                        _context.Debts_Sarhas.Add(ds);
                        _context.SaveChanges();

                    }
                    //Person person1 = _context.People.Find(3);
                    //person1.credit -= HalekPrices[i];

                    _context.SaveChanges();

                }
                //c.TotalHalek = HalekPrices.Sum();
            }

            if (BoatNameExpenses != null)
            {
                var BoatNameExpensesCookie = BoatNameExpenses.TrimEnd(BoatNameExpenses[BoatNameExpenses.Length - 1]);
                var ExpensePriceCookie = ExpensePricee.TrimEnd(ExpensePricee[ExpensePricee.Length - 1]);
                var CauseCookie = Cause.TrimEnd(Cause[Cause.Length - 1]);
                string[] BoatNamesExpenses = BoatNameExpensesCookie.Split(",").Select(c => Convert.ToString(c)).ToArray();
                decimal[] ExpensePrice = ExpensePriceCookie.Split(",").Select(c => Convert.ToDecimal(c)).ToArray();
                string[] Causes = CauseCookie.Split(",").Select(c => Convert.ToString(c)).ToArray();
                for (int i = 0; i < BoatNamesExpenses.Length; i++)
                {
                    var boat = _context.Boats.FirstOrDefault(c => c.BoatName == BoatNamesExpenses[i]);
                    Expense ex = new Expense()
                    {
                        BoatID = boat.BoatID,
                        Cause = Causes[i],
                        Date = TimeNow(),
                        PersonID = PID,
                        Price = ExpensePrice[i]
                    };
                    _context.Expenses.Add(ex);
                    await _context.SaveChangesAsync();

                    //Person person1 = _context.People.Find(3);
                    //person1.credit -= ExpensePrice[i];

                    await _context.SaveChangesAsync();

                }
            }

            var p = _context.People.Find(4);
            FathAllahGift g = new FathAllahGift() { charge = PricePaidForFathallah, CreditBefore = p.credit, CreditAfter = PricePaidForFathallah + p.credit, Date = TimeNow(), PersonID = PID };
            p.credit += PricePaidForFathallah;
            //Person pp = _context.People.Find(3);
            //pp.credit -= fathallah;
            _context.FathAllahGifts.Add(g);
            //_context.Collectings.Add(c);
            await _context.SaveChangesAsync();
            if (Adding != null)
            {
                var AddingCookie = Adding.TrimEnd(Adding[Adding.Length - 1]);
                var AddingPriceCookie = AddingPrice.TrimEnd(AddingPrice[AddingPrice.Length - 1]);
                string[] AddingNames = AddingCookie.Split(",").Select(c => Convert.ToString(c)).ToArray();
                decimal[] AddingPrices = AddingPriceCookie.Split(",").Select(c => Convert.ToDecimal(c)).ToArray();
                //var collecting = _context.Collectings.ToList().Where(x => x.Date.ToShortDateString() == TimeNow().ToShortDateString()).FirstOrDefault();
                //collecting.TotalOfAdditionalPayment = AddingPrices.Sum();

                for (int i = 0; i < AddingNames.Length; i++)
                {
                    AdditionalForOffice ad = new AdditionalForOffice()
                    {
                        Name = AddingNames[i],
                        Value = AddingPrices[i],
                        PersonID = PID,
                        Date = TimeNow()
                    };

                    _context.AdditionalForOffices.Add(ad);
                    //Person ppp = _context.People.Find(3);
                    //ppp.credit -= AddingPrices[i];
                    _context.SaveChanges();

                }
            }
            if (OperatorName != null)
            {
                var OperatorNameCookie = OperatorName.TrimEnd(OperatorName[OperatorName.Length - 1]);
                var OperatorPriceCookie = OperatorPrice.TrimEnd(AddingPrice[AddingPrice.Length - 1]);
                string[] OperatorNames = OperatorNameCookie.Split(",").Select(c => Convert.ToString(c)).ToArray();
                decimal[] OperatorPrices = OperatorPriceCookie.Split(",").Select(c => Convert.ToDecimal(c)).ToArray();
                //var collecting = _context.Collectings.ToList().Where(x => x.Date.ToShortDateString() == TimeNow().ToShortDateString()).FirstOrDefault();
                //collecting.TotalOfAdditionalPayment = AddingPrices.Sum();

                for (int i = 0; i < OperatorNames.Length; i++)
                {
                    var operatorr = _context.Operators.Where(x => x.OperatorName == OperatorNames[i]).FirstOrDefault();
                    PaidForOperator pp = new PaidForOperator()
                    {
                        OperatorID = operatorr.OperatorID,
                        Payment = OperatorPrices[i],
                        DebtsAfterPayment = operatorr.Credit - OperatorPrices[i],
                        PersonID = PID,
                        Date = TimeNow()
                    };

                    _context.PaidForOperators.Add(pp);
                    //Person ppp = _context.People.Find(3);
                    //ppp.credit -= AddingPrices[i];
                    _context.SaveChanges();

                }
            }

            Person pppp = _context.People.Find(PID);
            pppp.credit = FinalTotal;
            await _context.SaveChangesAsync();
            return Json(new { message = "success" });
        }

        public async Task<IActionResult> OfficeOfDay()
        {
            var user = await _userManager.GetUserAsync(User);
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains("partner"))
            {
                ViewBag.title = "المكتب الفرعي / يومية علاء";
            }
            else
                ViewBag.title = "المكتب";
            return View();
        }

        public async Task<IActionResult> OfficeOfDayData(DateTime Date)
        {
            var user = await _userManager.GetUserAsync(User);
            var roles = await _userManager.GetRolesAsync(user);
            int PID = 1;
            if (roles.Contains("partner"))
                PID = 2;
            // الايرادات
            var Yesterdaybranch = _context.BranchOffices.ToList().Where(x => x.Date.ToShortDateString() == Date.AddDays(-1).ToShortDateString()).FirstOrDefault();
            var Todaybranch = _context.BranchOffices.ToList().Where(x => x.Date.ToShortDateString() == Date.ToShortDateString()).FirstOrDefault();
            decimal collecting = 0.0m;
            decimal PaidFromMagdy = 0.0m;
            decimal TotalIncome = 0.0m;
            decimal TotalDrivers = 0.0m;
            decimal PaidForFathallah = 0.0m;
            decimal FathallahSalary = 0.0m;
            decimal totalDailyExpense = 0.0m;
            decimal totalSarhas = 0.0m;
            decimal FinalTotal = 0.0m;
            decimal CurrentCredit = 0.0m;
            if (Todaybranch != null)
            {
                collecting = Todaybranch.Collecting;
                PaidFromMagdy = Todaybranch.OfficeMoney;
                TotalIncome = Todaybranch.IncomeTotal;
                TotalDrivers = Todaybranch.DriversSalary;
                FathallahSalary = Todaybranch.FathallahSalary;
                totalDailyExpense = Todaybranch.ExpensesTotal;
                totalSarhas = Todaybranch.SarhasTotal;
                FinalTotal = Todaybranch.CurrentCredit; // رصيد مترحل
            }
            var halek = _context.Debts_Sarhas.Include(c => c.Sarha).Include(c => c.Debt).Include(c => c.Sarha.Boat).ToList().Where(c => c.Date.ToShortDateString() == TimeNow().ToShortDateString() && c.PersonID == PID).ToList();

            if (Yesterdaybranch != null)
            {
                CurrentCredit = Yesterdaybranch.CurrentCredit;

            }
            var IsellerReciepts = _context.ISellerReciepts.Include(x => x.Merchant).ToList().Where(x => x.Date.ToShortDateString() == Date.ToShortDateString() && x.TotalOfPrices != 0 && x.PersonID == PID).Select(c => new { merchantName = c.Merchant.MerchantName, paidFromDebt = c.PaidFromDebt }).ToList();
            var paid_for_Us = _context.PaidForMerchant
               .Include(c => c.Merchant).ToList()
               .Where(c => c.Date.ToShortDateString() == Date.ToShortDateString() && c.IsPaidForUs == true && c.PersonID == PID).ToList()
               .Select(c => new { merchantName = c.Merchant.MerchantName, payment = c.Payment }).ToList();

            ///////////////////////////////////////////////////////////////////
            // المصروف اليومي
            var Halek = _context.Debts_Sarhas.Include(c => c.Sarha)
                .Include(c => c.Sarha.Boat)
                .Include(c => c.Debt).ToList()
                .Where(c => c.Date.ToShortDateString() == Date.ToShortDateString() && c.PersonID == PID).OrderBy(c => c.Sarha.BoatID)
                .Select(c => new { boatName = c.Sarha.Boat.BoatName, price = c.Price, debtName = c.Debt.DebtName }).ToList();



            //فيها مشكله
            var Loans = _context.LeaderLoans.Include(c => c.Boat).ToList()
               .Where(c => c.Date.ToShortDateString() == Date.ToShortDateString() && c.PersonID == PID).Select(c => new { price = c.Price, boatName = c.Boat.BoatName }).ToList();

          
            


            var SharedBoatExpenses = _context.Expenses.Include(c => c.Boat)
             .ToList().Where(c => c.Date.ToShortDateString() == Date.ToShortDateString() && c.PersonID == PID)
             .Select(c => new { boatName = c.Boat.BoatName, price = c.Price, cause = c.Cause })
             .ToList();

            PaidForFathallah = _context.FathAllahGifts.ToList().Where(c => c.Date.ToShortDateString() == Date.ToShortDateString() && c.PersonID == PID).Sum(c => c.charge);

            var AdditionalItems = _context.AdditionalForOffices.ToList().Where(c => c.Date.ToShortDateString() == Date.ToShortDateString() && c.PersonID == PID).Select(c => new { name = c.Name, value = c.Value }).ToList();

            var OperatorItems = _context.PaidForOperators.Include(c => c.Operator).ToList().Where(c => c.Date.ToShortDateString() == Date.ToShortDateString() && c.PersonID == PID).Select(c => new { operatorName = c.Operator.OperatorName, payment = c.Payment }).ToList();

            //////////////////////////////////////////////////////////////////
            /// قبض السرح
            var paid_for_seller = _context.PaidForSellers.
                Include(c => c.Merchant).ToList()
                .Where(c => c.Date.ToShortDateString() == Date.ToShortDateString() && c.PersonID == PID)
                .Select(c => new { merchantName = c.Merchant.MerchantName, payment = c.Payment }).ToList();
            var paid_for_boat = _context.PaidForBoats.
              Include(c => c.Boat).ToList()
              .Where(c => c.Date.ToShortDateString() == Date.ToShortDateString() && c.PersonID == PID)
              .Select(c => new { boatName = c.Boat.BoatName, payment = c.Payment }).ToList();

            /////////////////////////////////////////////////////////////////

            var HalekTotal = halek.Sum(c => c.Price);
            var LeaderLoanTotal = Loans.Sum(c => c.price);
            var SharedExpensesTotal = SharedBoatExpenses.Sum(c => c.price);
            var AdditionalTotal = AdditionalItems.Sum(c => c.value);
            var AllAditionalTotal = TotalDrivers + PaidForFathallah + FathallahSalary + AdditionalTotal;
            var OperatorsTotal = OperatorItems.Sum(c => c.payment);
            /////////////////////////////////////////////////////////////////
            return Json(new
            {
                message = "success",
                currentCredit = CurrentCredit,
                collecting = collecting,
                paidFromMagdy = PaidFromMagdy,
                isellerReciepts = IsellerReciepts,
                paid_for_Us = paid_for_Us,
                totalIncome = TotalIncome,
                halek = Halek,
                halekTotal = HalekTotal,
                loans = Loans,
                leaderLoanTotal = LeaderLoanTotal,
                sharedBoatExpenses = SharedBoatExpenses,
                sharedExpensesTotal = SharedExpensesTotal,
                totalDrivers = TotalDrivers,
                paidForFathallah = PaidForFathallah,
                fathallahSalary = FathallahSalary,
                additionalItems = AdditionalItems,
                allAditionalTotal = AllAditionalTotal,
                operatorItems = OperatorItems,
                operatorsTotal = OperatorsTotal,
                paid_for_seller = paid_for_seller,
                paid_for_boat = paid_for_boat,
                totalSarhas = totalSarhas,
                totalDailyExpense = totalDailyExpense,
                finalTotal = FinalTotal,
            });
        }
    }


}
