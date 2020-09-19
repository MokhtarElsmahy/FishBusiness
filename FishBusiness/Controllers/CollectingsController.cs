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
    public class CollectingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CollectingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        #region المحصل
        // GET: Collectings
        //public async Task<IActionResult> Index()
        //{
        //    var applicationDbContext = _context.Collectings.Include(c => c.AdditionalPayment);
        //    return View(await applicationDbContext.ToListAsync());
        //}

        // GET: Collectings/Details/5
        public async Task<IActionResult> Profile(DateTime Date)
        {
            var paid_for_merchant = _context.PaidForMerchant.Include(c => c.Merchant).ToList().Where(c => c.Date.ToShortDateString() == Date.ToShortDateString() && c.IsPaidForUs == false).ToList();
            var paid_for_Us = _context.PaidForMerchant.Include(c => c.Merchant).ToList().Where(c => c.Date.ToShortDateString() == Date.ToShortDateString() && c.IsPaidForUs == true).ToList();
            var halek = _context.Debts_Sarhas.Include(c => c.Sarha).Include(c => c.Debt).Include(c => c.Sarha.Boat).ToList().Where(c => c.Sarha.DateOfSarha.ToShortDateString() == Date.ToShortDateString() && c.PersonID == 3).ToList();
            var expenses = _context.Expenses.Include(c => c.Boat).ToList().Where(c => c.Date.ToShortDateString() == Date.ToShortDateString() && c.PersonID == 3).ToList();

            ViewBag.Merchant = new SelectList(_context.Merchants.ToList(), "MerchantID", "MerchantName");
            ViewBag.Halek = new SelectList(_context.Debts.ToList(), "DebtID", "DebtName");
            ViewBag.Boats = new SelectList(_context.Boats.ToList(), "BoatID", "BoatName");

            CollectorVm model = new CollectorVm() { Debts_Sarha = halek, PaidForMerchant = paid_for_merchant, PaidForUs = paid_for_Us , Expenses= expenses };
            return View(model);
        }

        public async Task<IActionResult> FinalCalc(decimal fathallah)
        {


            //-----------------------------paid for us----------------------------------------------
            var MerchantNameCookie = Request.Cookies["MerchantName"];


            var PriceCookie = Request.Cookies["Price"];

            //--------------------------------------------paid for merchants-------------------------

            var ToMerchantNameCookie = Request.Cookies["ToMerchantName"];

            var ToPriceCookie = Request.Cookies["ToPrice"];

            //-----------------------------------------------paid for halek

            var BoatNameCookie = Request.Cookies["BoatName"];


            var HalekPriceCookie = Request.Cookies["HalekPrice"];

            var HalekNameCookie = Request.Cookies["HalekName"];


            //-------------------------------paid addings

            var AddingCookie = Request.Cookies["Adding"];


            var AddingPriceCookie = Request.Cookies["AddingPrice"];

            //-----------------------------------------------paid for Expenses

            var BoatNameExpensesCookie = Request.Cookies["BoatNameExpensesCookie"];


            var ExpensePriceCookie = Request.Cookies["ExpensePriceCookie"];

            var CauseCookie = Request.Cookies["CauseCookie"];

            //----------------------------------------------------------------------------------------------------------

            Collecting c = new Collecting()
            {
                Date = DateTime.Now,
                TotalForFahAllah = fathallah
            };
            if (MerchantNameCookie != null && PriceCookie !=null)
            {
                string[] MerchantNames = MerchantNameCookie.Split(",").Select(c => Convert.ToString(c)).ToArray();
                decimal[] prices = PriceCookie.Split(",").Select(c => Convert.ToDecimal(c)).ToArray();

                for (int i = 0; i < MerchantNames.Length; i++)
                {
                    var merchant = _context.Merchants.FirstOrDefault(c => c.MerchantName == MerchantNames[i]);
                    PaidForMerchant m;
                    if (merchant.IsOwner == true)
                    {
                        m = new PaidForMerchant()
                        {
                            IsPaidForUs = true,
                            MerchantID = merchant.MerchantID,
                            Payment = prices[i],
                            PreviousDebtsForMerchant = merchant.PreviousDebts - prices[i],
                            Date = DateTime.Now,
                            IsCash = true,
                            PersonID = 1
                        };
                    }
                    else
                    {
                        m = new PaidForMerchant()
                        {
                            IsPaidForUs = true,
                            MerchantID = merchant.MerchantID,
                            Payment = prices[i],
                            PreviousDebtsForMerchant = merchant.PreviousDebts - prices[i],
                            Date = DateTime.Now,
                            IsCash = true,
                            PersonID = 3
                        };
                    }
                    Person pppp = _context.People.Find(3);
                    pppp.credit += prices[i];
                    merchant.PreviousDebts = merchant.PreviousDebts - prices[i];
                    _context.PaidForMerchant.Add(m);
                    _context.SaveChanges();
                    c.TotalPaidFromMerchants = prices.Sum();
                }

            }
            
            if (ToMerchantNameCookie != null && ToPriceCookie!=null)
            {
                string[] ToMerchantNames = ToMerchantNameCookie.Split(",").Select(c => Convert.ToString(c)).ToArray();
                decimal[] Toprices = ToPriceCookie.Split(",").Select(c => Convert.ToDecimal(c)).ToArray();


                for (int i = 0; i < ToMerchantNames.Length; i++)
              {
                var merchant = _context.Merchants.FirstOrDefault(c => c.MerchantName == ToMerchantNames[i]);

                PaidForMerchant m = new PaidForMerchant()
                {
                    IsPaidForUs = false,
                    MerchantID = merchant.MerchantID,
                    Payment = Toprices[i],
                    PreviousDebtsForMerchant = merchant.PreviousDebtsForMerchant - Toprices[i],
                    Date = DateTime.Now,
                    IsCash = true,
                    PersonID = 3
                };
                Person person = _context.People.Find(3);
                person.credit -= Toprices[i];
                merchant.PreviousDebtsForMerchant = merchant.PreviousDebtsForMerchant - Toprices[i];
                _context.PaidForMerchant.Add(m);
                _context.SaveChanges();

            }
                c.TotalPaidForMerchants = Toprices.Sum();
            }
            
            if (HalekNameCookie != null)
            {
                string[] BoatNames = BoatNameCookie.Split(",").Select(c => Convert.ToString(c)).ToArray();
                decimal[] HalekPrices = HalekPriceCookie.Split(",").Select(c => Convert.ToDecimal(c)).ToArray();
                string[] HalekNames = HalekNameCookie.Split(",").Select(c => Convert.ToString(c)).ToArray();

                for (int i = 0; i < HalekNames.Length; i++)
                {
                    var boat = _context.Boats.FirstOrDefault(c => c.BoatName == BoatNames[i]);
                    var lastSarhaID = _context.Sarhas.Where(c => c.BoatID == boat.BoatID && c.IsFinished == false).Max(c => c.SarhaID);
                    var lastSarha = _context.Sarhas.Find(lastSarhaID);

                    var debt_sarha = _context.Debts_Sarhas.Include(c => c.Debt).Where(c => c.SarhaID == lastSarhaID && c.Debt.DebtName == HalekNames[i]).FirstOrDefault();
                    boat.DebtsOfHalek += HalekPrices[i];
                    if (debt_sarha != null)
                    {
                        debt_sarha.Price += HalekPrices[i];
                    }
                    else
                    {
                        var debt = _context.Debts.Where(c => c.DebtName == HalekNames[i]).FirstOrDefault();
                        Debts_Sarha ds = new Debts_Sarha() { Price = HalekPrices[i], DebtID = debt.DebtID, SarhaID = lastSarhaID, PersonID = 3, Date = DateTime.Now };
                        _context.Debts_Sarhas.Add(ds);
                        _context.SaveChanges();

                    }
                    Person person1 = _context.People.Find(3);
                    person1.credit -= HalekPrices[i];

                    _context.SaveChanges();

                }
                c.TotalHalek = HalekPrices.Sum();
            }
            
            if (BoatNameExpensesCookie != null)
            {
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
                        Date = DateTime.Now,
                        PersonID = 3,
                        Price = ExpensePrice[i]
                    };
                    _context.Expenses.Add(ex);
                    _context.SaveChanges();

                    Person person1 = _context.People.Find(3);
                    person1.credit -= ExpensePrice[i];

                    _context.SaveChanges();

                }
                c.TotalOfExpenses = ExpensePrice.Sum();
            }

            var p = _context.People.Find(4);
            FathAllahGift g = new FathAllahGift() { charge = fathallah, CreditBefore = p.credit, CreditAfter = fathallah + p.credit, Date = DateTime.Now, PersonID = 3 };
            p.credit += fathallah;
            Person pp = _context.People.Find(3);
            pp.credit -= fathallah;
            _context.FathAllahGifts.Add(g);
            _context.Collectings.Add(c);
            _context.SaveChanges();
            if (AddingCookie != null)
            {
                string[] AddingNames = AddingCookie.Split(",").Select(c => Convert.ToString(c)).ToArray();
                decimal[] AddingPrices = AddingPriceCookie.Split(",").Select(c => Convert.ToDecimal(c)).ToArray();
                var collecting = _context.Collectings.ToList().Where(x => x.Date.ToShortDateString() == DateTime.Now.ToShortDateString()).FirstOrDefault();
                collecting.TotalOfAdditionalPayment = AddingPrices.Sum();

                for (int i = 0; i < AddingNames.Length; i++)
                {
                    AdditionalPayment ad = new AdditionalPayment()
                    {
                        Name = AddingNames[i],
                        Value = AddingPrices[i],
                        ID = collecting.ID,
                        Date = DateTime.Now
                    };

                    _context.AdditionalPayments.Add(ad);
                    Person ppp = _context.People.Find(3);
                    ppp.credit -= AddingPrices[i];
                    _context.SaveChanges();

                }
            }
        
            var FinalCredit = c.TotalPaidFromMerchants - (c.TotalPaidForMerchants + c.TotalHalek + c.TotalOfAdditionalPayment + fathallah + c.TotalOfExpenses);
            Person halaka = _context.People.Find(1);
            halaka.credit += FinalCredit;
            pp.credit = 0.0m;
            _context.SaveChanges();
            return Json(new { message = "success" });
        }


        public IActionResult ProfileOfDay()
        {
            return View();
        }
        public IActionResult ProfileOfDayData(DateTime Date)
        {
            var paid_for_merchant = _context.PaidForMerchant.
                Include(c => c.Merchant).ToList()
                .Where(c => c.Date.ToShortDateString() == Date.ToShortDateString() && c.IsPaidForUs == false)
                .Select(c => new { merchantName = c.Merchant.MerchantName, Payment = c.Payment, Date = c.Date, IsPaidForUs = c.IsPaidForUs }).ToList();

            var paid_for_Us = _context.PaidForMerchant
                .Include(c => c.Merchant).ToList()
                .Where(c => c.Date.ToShortDateString() == Date.ToShortDateString() && c.IsPaidForUs == true).ToList()
                .Select(c => new { merchantName = c.Merchant.MerchantName, Payment = c.Payment, Date = c.Date, IsPaidForUs = c.IsPaidForUs }).ToList();

            var Halek = _context.Debts_Sarhas.Include(c => c.Sarha)
                .Include(c => c.Debt).Include(c => c.Sarha.Boat)
                .ToList().Select(c => new { PersonID = c.PersonID, Date = c.Sarha.DateOfSarha, BoatName = c.Sarha.Boat.BoatName, Price = c.Price, DebtName = c.Debt.DebtName })
                .Where(c => c.Date.ToShortDateString() == Date.ToShortDateString() && c.PersonID == 3)
                .ToList();

            var Expenses = _context.Expenses.Include(c => c.Boat)
               .ToList().Select(c => new { PersonID = c.PersonID, Date = c.Date, BoatName = c.Boat.BoatName, price = c.Price, cause = c.Cause })
               .Where(c => c.Date.ToShortDateString() == Date.ToShortDateString() && c.PersonID == 3)
               .ToList();


            var collecting = _context.Collectings.ToList().Where(c => c.Date.ToShortDateString() == Date.ToShortDateString()).ToList();
            var collectingItem = collecting.FirstOrDefault();
            if (collectingItem == null)
            {
                return Json(new { message = "Fail" });
            }
            var AdditionalItems = _context.AdditionalPayments.ToList().Where(c => c.ID == collectingItem.ID).Select(c => new { Name = c.Name, Value = c.Value }).ToList();
            var AhmedFathAllah = collectingItem.TotalForFahAllah;
            var TotalPaidFromUs = paid_for_merchant.Sum(c => c.Payment) + Halek.Sum(c => c.Price) + AdditionalItems.Sum(c => c.Value) + AhmedFathAllah;
            var x = paid_for_Us.Sum(c => c.Payment);
            var y = Halek.Sum(c => c.Price);
            var z = AdditionalItems.Sum(c => c.Value);
            var b = paid_for_merchant.Sum(c => c.Payment);
            var ex = Expenses.Sum(c => c.price);
            return Json(new
            {
                message = "success",
                paidForUs = paid_for_Us,
                totalPaidForUs = x,
                paidForMerchant = paid_for_merchant,
                totalPaidForMrchant = b,
                halek = Halek,
                additionalItems = AdditionalItems,
                ahmedFathAllah = AhmedFathAllah,
                totalPaidFromUs = TotalPaidFromUs,
                totalOfHalek = y,
                totalOfAdditional = z,
                expenses = Expenses,
                totalExpenses = ex

            });
        }
        #endregion


        #region احمد فتح الله

        public IActionResult FathAllahMainProfile()
        {
            ViewBag.Leaders = new SelectList(_context.Boats, "BoatID", "BoatLeader");
            FathAllahMainProfileVM model = new FathAllahMainProfileVM();
            model.LeaderPaybacks = _context.LeaderPaybacks.Include(c => c.Boat).ToList();
            model.LeaderLoan = _context.LeaderLoans.Include(c => c.Boat).ToList();
            model.FathAllahGifts = _context.FathAllahGifts.Include(c => c.Person).ToList();

            return View(model);
        }

        public IActionResult FathAllahProfile()
        {
            ViewBag.Leaders = new SelectList(_context.Boats, "BoatID", "BoatLeader");
            ViewBag.Halek = new SelectList(_context.Debts.ToList(), "DebtID", "DebtName");
            ViewBag.Boats = new SelectList(_context.Boats.ToList(), "BoatID", "BoatName");
            ViewBag.People = new SelectList(_context.People.Where(c => c.PersonID != 4), "PersonID", "Name");
            var pp = _context.People.Find(4);
            ViewBag.Credit = pp.credit;
            return View();
        }
        public IActionResult GetLeaderDebts(int BoatID)
        {
            var boat = _context.Boats.Find(BoatID);
            return Json(new { leaderDebts = boat.DebtsOfLeader });
        }
        public IActionResult FathAllahCalc(decimal fathallahFinalCredit)
        {
            //-----------------------------paid for الحلقه----------------------------------------------
            var PlaceCookie = Request.Cookies["Place"];


            var PriceCookie = Request.Cookies["Price"];

            //--------------------------------------------paid for Leader-------------------------

            var ToLeaderCookie = Request.Cookies["ToLeader"];

            var ToPriceCookie = Request.Cookies["ToPrice"];

            //-----------------------------------------------paid for halek

            var BoatNameCookie = Request.Cookies["BoatName"];


            var HalekPriceCookie = Request.Cookies["HalekPrice"];

            var HalekNameCookie = Request.Cookies["HalekName"];
            //-----------------------------------------------paid for Expenses

            var BoatNameExpensesCookie = Request.Cookies["BoatNameExpensesCookie"];


            var ExpensePriceCookie = Request.Cookies["ExpensePriceCookie"];

            var CauseCookie = Request.Cookies["CauseCookie"];

            //-----------------------------------------------------------------------------------------
            var pp = _context.People.Find(4);
            pp.credit = fathallahFinalCredit;

            if (PlaceCookie !=null && PriceCookie != null)
            {
                string[] PlaceHalekNames = PlaceCookie.Split(",").Select(c => Convert.ToString(c)).ToArray();
                decimal[] prices = PriceCookie.Split(",").Select(c => Convert.ToDecimal(c)).ToArray();

                for (int i = 0; i < PlaceHalekNames.Length; i++)
                {
                    int DebtID = _context.Debts.Where(c => c.DebtName == PlaceHalekNames[i]).FirstOrDefault().DebtID;
                    _context.HalakaHaleks.Add(new HalakaHalek { Price = prices[i], DebtID = DebtID, Date = DateTime.Now });
                    pp.credit -= prices[i];

                }
                _context.SaveChanges();
            }

            if (ToLeaderCookie != null && ToPriceCookie != null)
            {
                string[] ToLeaderNames = ToLeaderCookie.Split(",").Select(c => Convert.ToString(c)).ToArray();
                decimal[] Toprices = ToPriceCookie.Split(",").Select(c => Convert.ToDecimal(c)).ToArray();

                for (int i = 0; i < ToLeaderNames.Length; i++)
                {
                    var Boat = _context.Boats.Where(c => c.BoatLeader == ToLeaderNames[i]).FirstOrDefault();
                    _context.LeaderLoans.Add(new LeaderLoan { Price = Toprices[i], BoatID = Boat.BoatID, Date = DateTime.Now });
                    Boat.DebtsOfLeader += Toprices[i];
                    pp.credit -= Toprices[i];
                    _context.SaveChanges();
                }

            }


            if (BoatNameCookie !=null && HalekPriceCookie !=null && HalekNameCookie!=null)
            {
                string[] BoatNames = BoatNameCookie.Split(",").Select(c => Convert.ToString(c)).ToArray();
                decimal[] HalekPrices = HalekPriceCookie.Split(",").Select(c => Convert.ToDecimal(c)).ToArray();
                string[] HalekNames = HalekNameCookie.Split(",").Select(c => Convert.ToString(c)).ToArray();

                for (int i = 0; i < HalekNames.Length; i++)
                {
                    var boat = _context.Boats.FirstOrDefault(c => c.BoatName == BoatNames[i]);

                    var lastSarhaID = _context.Sarhas.Where(c => c.BoatID == boat.BoatID && c.IsFinished == false);
                    if (lastSarhaID != null)
                    {

                        int maxSarahaID = lastSarhaID.Max(c => c.SarhaID);
                        var lastSarha = _context.Sarhas.Find(maxSarahaID);

                        var debt_sarha = _context.Debts_Sarhas.Include(c => c.Debt).Where(c => c.SarhaID == maxSarahaID && c.Debt.DebtName == HalekNames[i]).FirstOrDefault();
                        boat.DebtsOfHalek += HalekPrices[i];
                        if (debt_sarha != null)
                        {
                            debt_sarha.Price += HalekPrices[i];
                        }
                        else
                        {
                            var debt = _context.Debts.Where(c => c.DebtName == HalekNames[i]).FirstOrDefault();
                            Debts_Sarha ds = new Debts_Sarha() { Price = HalekPrices[i], DebtID = debt.DebtID, SarhaID = maxSarahaID, PersonID = 4, Date = DateTime.Now };
                            _context.Debts_Sarhas.Add(ds);
                            _context.SaveChanges();

                        }
                        pp.credit -= HalekPrices[i];
                        _context.SaveChanges();
                    }



                }
            }

            if (BoatNameExpensesCookie != null)
            {
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
                        Date = DateTime.Now,
                        PersonID = 4,
                        Price = ExpensePrice[i]
                    };
                    _context.Expenses.Add(ex);
                    _context.SaveChanges();

                    Person person1 = _context.People.Find(4);
                    person1.credit -= ExpensePrice[i];

                    _context.SaveChanges();

                }
                
            }
            _context.SaveChanges();

            return Json(new { message = "success" });
        }



        public IActionResult FathAllahWorkOfDay()
        {
            return View();
        }
        public IActionResult FathAllahWorkOfDayDate(DateTime Date)
        {
            var halakaHaleks = _context.HalakaHaleks.Include(c => c.Debt).ToList().Where(c => c.Date.ToShortDateString() == Date.ToShortDateString()).Select(c => new { price = c.Price, debtName = c.Debt.DebtName }).ToList();
            decimal sumOfhalakaHaleks = 0;
            if (halakaHaleks != null)
            {
                sumOfhalakaHaleks = halakaHaleks.Sum(c => c.price);
            }
            var Loans = _context.LeaderLoans.Include(c => c.Boat).ToList().Where(c => c.Date.ToShortDateString() == Date.ToShortDateString()).Select(c => new { price = c.Price, leader = c.Boat.BoatLeader }).ToList();
            decimal sumOfLoans = 0;
            if (halakaHaleks != null)
            {
                sumOfLoans = Loans.Sum(c => c.price);
            }


            var Halek = _context.Debts_Sarhas.Include(c => c.Sarha)
               .Include(c => c.Debt).Include(c => c.Sarha.Boat)
               .ToList().Select(c => new { PersonID = c.PersonID, Date = c.Sarha.DateOfSarha, BoatName = c.Sarha.Boat.BoatName, price = c.Price, DebtName = c.Debt.DebtName })
               .Where(c => c.Date.ToShortDateString() == Date.ToShortDateString() && c.PersonID == 4)
               .ToList();

            decimal sumOfHalek = 0;
            if (halakaHaleks != null)
            {
                sumOfHalek = Halek.Sum(c => c.price);
            }

            var Expenses = _context.Expenses.Include(c => c.Boat)
              .ToList().Select(c => new { PersonID = c.PersonID, Date = c.Date, BoatName = c.Boat.BoatName, price = c.Price, cause = c.Cause })
              .Where(c => c.Date.ToShortDateString() == Date.ToShortDateString() && c.PersonID == 4)
              .ToList();

            decimal sumOfExpenses = 0;
            if (Expenses != null)
            {
                sumOfExpenses = Expenses.Sum(c => c.price);
            }

            return Json(new { message = "success", halakaHaleks = halakaHaleks, sumOfhalakaHaleks = sumOfhalakaHaleks, sumOfLoans = sumOfLoans, sumOfHalek = sumOfHalek, loans = Loans, halek = Halek , sumExpenses= sumOfExpenses , expenses = Expenses });
        }
        // هتبقي تبع المكتب
        public IActionResult PayBackLoan(decimal price, int BoatID)
        {
            var Boat = _context.Boats.Find(BoatID);
            Boat.DebtsOfLeader -= price;
            LeaderPayback p = new LeaderPayback() { BoatID = BoatID, Date = DateTime.Now, Price = price };
            _context.LeaderPaybacks.Add(p);
            Person pp = _context.People.Find(4);
            pp.credit += price;
            _context.SaveChanges();
            return Json(new { message = "success" });
        }


        public IActionResult GiveFathAllah(decimal price, int personID)
        {
            System.Threading.Thread.Sleep(1000);
            Person p = _context.People.Find(4);
            FathAllahGift g = new FathAllahGift() { PersonID = personID, Date = DateTime.Now, CreditBefore = p.credit, charge = price, CreditAfter = price + p.credit };
            p.credit += price;
            Person pp = _context.People.Find(personID);
            pp.credit -= price;
            _context.FathAllahGifts.Add(g);
            _context.SaveChanges();
            return Json(new { message = "success", newCredit = g.CreditAfter });
        }
        #endregion
    }
}
