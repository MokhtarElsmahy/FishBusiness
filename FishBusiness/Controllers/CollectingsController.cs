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
    public class CollectingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CollectingsController(ApplicationDbContext context)
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
            // var paid_for_merchant = _context.PaidForMerchant.Include(c => c.Merchant).ToList().Where(c => c.Date.ToShortDateString() == Date.ToShortDateString() && c.IsPaidForUs == false).ToList();
            // var paid_for_Us = _context.PaidForMerchant.Include(c => c.Merchant).ToList().Where(c => c.Date.ToShortDateString() == Date.ToShortDateString() && c.IsPaidForUs == true).ToList();
            // var halek = _context.Debts_Sarhas.Include(c => c.Sarha).Include(c => c.Debt).Include(c => c.Sarha.Boat).ToList().Where(c => c.Sarha.DateOfSarha.ToShortDateString() == Date.ToShortDateString() && c.PersonID == 3).ToList();
            // var expenses = _context.Expenses.Include(c => c.Boat).ToList().Where(c => c.Date.ToShortDateString() == Date.ToShortDateString() && c.PersonID == 3).ToList();

            // ViewBag.Merchant = new SelectList(_context.Merchants.Where(c=>c.IsFromOutsideCity==false).ToList(), "MerchantID", "MerchantName");
            // ViewBag.PayForMerchant = new SelectList(_context.Merchants.Where(c=>c.IsOwner==false && c.IsFromOutsideCity == false).ToList(), "MerchantID", "MerchantName");
            // ViewBag.Halek = new SelectList(_context.Debts.ToList(), "DebtID", "DebtName");
            // var UnfinishedSarhas = _context.Sarhas.Where(x => x.IsFinished == false).Select(x => x.BoatID);
            // ViewBag.Boats = new SelectList(_context.Boats.Where(b => b.IsActive == true && b.BoatLicenseNumber != "0").Where(b => UnfinishedSarhas.Contains(b.BoatID)).ToList(), "BoatID", "BoatName");
            // ViewBag.SharedBoats = new SelectList(_context.Boats.Where(b => b.IsActive == true && b.TypeID == 2).Where(b => UnfinishedSarhas.Contains(b.BoatID)).ToList(), "BoatID", "BoatName");

            // CollectorVm model = new CollectorVm() { Debts_Sarha = halek, PaidForMerchant = paid_for_merchant, PaidForUs = paid_for_Us, Expenses = expenses };
            //var TodayCash = _context.PersonReciepts.ToList().Where(x => x.Date.ToShortDateString() == TimeNow().ToShortDateString()).FirstOrDefault();
            // if (TodayCash != null)
            //     ViewBag.TotalOfCashes = TodayCash.TotalPrice;
            // else
            //     ViewBag.TotalOfCashes = 0;


            var paid_for_merchant =await _context.PaidForMerchant.Where(c => c.Date.Date == Date.Date && c.IsPaidForUs == false).Include(c => c.Merchant).ToListAsync();

            var paid_for_Us = await _context.PaidForMerchant.Where(c => c.Date.Date == Date.Date && c.IsPaidForUs == true).Include(c => c.Merchant).ToListAsync();

            var halek = await _context.Debts_Sarhas.Where(c => c.Sarha.DateOfSarha.Date == Date.Date && c.PersonID == 3).Include(c => c.Sarha).Include(c => c.Debt).Include(c => c.Sarha.Boat).ToListAsync();

            var expenses = await _context.Expenses.Include(c => c.Boat).Where(c => c.Date.Date == Date.Date && c.PersonID == 3).ToListAsync();

            ViewBag.Merchant = new SelectList(await _context.Merchants.Where(c => c.IsFromOutsideCity == false).ToListAsync(), "MerchantID", "MerchantName");
            ViewBag.PayForMerchant = new SelectList(await _context.Merchants.Where(c => c.IsOwner == false && c.IsFromOutsideCity == false).ToListAsync(), "MerchantID", "MerchantName");
            ViewBag.Halek = new SelectList(await _context.Debts.ToListAsync(), "DebtID", "DebtName");
            var UnfinishedSarhas = await _context.Sarhas.Where(x => x.IsFinished == false).Select(x => x.BoatID).ToListAsync();
            ViewBag.Boats = new SelectList(await _context.Boats.Where(b => b.IsActive == true && b.BoatLicenseNumber != "0").Where(b => UnfinishedSarhas.Contains(b.BoatID)).ToListAsync(), "BoatID", "BoatName");
            ViewBag.SharedBoats = new SelectList(await _context.Boats.Where(b => b.IsActive == true && b.TypeID == 2).Where(b => UnfinishedSarhas.Contains(b.BoatID)).ToListAsync(), "BoatID", "BoatName");

            CollectorVm model = new CollectorVm() { Debts_Sarha = halek, PaidForMerchant = paid_for_merchant, PaidForUs = paid_for_Us, Expenses = expenses };
            var TodayCash = await _context.PersonReciepts.Where(x => x.Date.Date == TimeNow().Date).FirstOrDefaultAsync();
            if (TodayCash != null)
                ViewBag.TotalOfCashes = TodayCash.TotalPrice;
            else
                ViewBag.TotalOfCashes = 0;
            return View(model);
        }

        public  IActionResult FinalCalc(decimal fathallah, decimal Cash, string MerchantName, string Price, string ToMerchantName, string ToPrice, string BoatName, string HalekPrice,
            string HalekName, string Adding, string AddingPrice, string BoatNameExpenses, string ExpensePricee, string Cause)
        {

            Collecting c = new Collecting()
            {
                Date = TimeNow(),
                TotalForFahAllah = fathallah,
                TotalOfCashes = Cash
            };

           

            if (MerchantName != null && Price != null)
            {
                var MerchantNameCookie = MerchantName.TrimEnd(MerchantName[MerchantName.Length - 1]);
                var PriceCookie = Price.TrimEnd(Price[Price.Length - 1]);
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
                            Date = TimeNow(),
                            IsCash = true,
                            PersonID = 3
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
                            Date = TimeNow(),
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

            if (ToMerchantName != null && ToPrice != null)
            {
                var ToMerchantNameCookie = ToMerchantName.TrimEnd(ToMerchantName[ToMerchantName.Length - 1]);
                var ToPriceCookie = ToPrice.TrimEnd(ToPrice[ToPrice.Length - 1]);
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
                        Date = TimeNow(),
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

            if (HalekName!= null)
            {
                var BoatNameCookie = BoatName.TrimEnd(BoatName[BoatName.Length - 1]);
                var HalekPriceCookie = HalekPrice.TrimEnd(HalekPrice[HalekPrice.Length - 1]);
                var HalekNameCookie = HalekName.TrimEnd(HalekName[HalekName.Length - 1]);
                string[] BoatNames = BoatNameCookie.Split(",").Select(c => Convert.ToString(c)).ToArray();
                decimal[] HalekPrices = HalekPriceCookie.Split(",").Select(c => Convert.ToDecimal(c)).ToArray();
                string[] HalekNames = HalekNameCookie.Split(",").Select(c => Convert.ToString(c)).ToArray();

                for (int i = 0; i < HalekNames.Length; i++)
                {
                    var boat = _context.Boats.FirstOrDefault(c => c.BoatName == BoatNames[i]);
                    var lastSarhaaa =  _context.Sarhas.Where(c => c.BoatID == boat.BoatID && c.IsFinished == false).FirstOrDefault();
                    var lastSarhaID= lastSarhaaa.SarhaID;
                  
                    var lastSarha = _context.Sarhas.Find(lastSarhaID);

                    var debt_sarha = _context.Debts_Sarhas.Include(c => c.Debt).
                        Where(c => c.SarhaID == lastSarhaID && c.Debt.DebtName == HalekNames[i]&&c.PersonID==3 && c.Date.Date== TimeNow().Date).FirstOrDefault();
                    boat.DebtsOfHalek += HalekPrices[i];
                    if (debt_sarha != null)
                    {
                        debt_sarha.Price += HalekPrices[i];
                    }
                    else
                    {
                        var debt = _context.Debts.Where(c => c.DebtName == HalekNames[i]).FirstOrDefault();
                        Debts_Sarha ds = new Debts_Sarha() { Price = HalekPrices[i], DebtID = debt.DebtID, SarhaID = lastSarhaID, PersonID = 3, Date = TimeNow() };
                        _context.Debts_Sarhas.Add(ds);
                        _context.SaveChanges();

                    }
                    Person person1 = _context.People.Find(3);
                    person1.credit -= HalekPrices[i];

                    _context.SaveChanges();

                }
                c.TotalHalek = HalekPrices.Sum();
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
            FathAllahGift g = new FathAllahGift() { charge = fathallah, CreditBefore = p.credit, CreditAfter = fathallah + p.credit, Date = TimeNow(), PersonID = 3 };
            p.credit += fathallah;
            Person pp = _context.People.Find(3);
            pp.credit -= fathallah;
            _context.FathAllahGifts.Add(g);
            _context.Collectings.Add(c);
             _context.SaveChanges();
            if (Adding != null)
            {
                var AddingCookie = Adding.TrimEnd(Adding[Adding.Length - 1]);
                var AddingPriceCookie = AddingPrice.TrimEnd(AddingPrice[AddingPrice.Length - 1]);
                string[] AddingNames = AddingCookie.Split(",").Select(c => Convert.ToString(c)).ToArray();
                decimal[] AddingPrices = AddingPriceCookie.Split(",").Select(c => Convert.ToDecimal(c)).ToArray();
                var collecting = _context.Collectings.ToList().Where(x => x.Date.ToShortDateString() == TimeNow().ToShortDateString()).FirstOrDefault();
                collecting.TotalOfAdditionalPayment = AddingPrices.Sum();

                for (int i = 0; i < AddingNames.Length; i++)
                {
                    AdditionalPayment ad = new AdditionalPayment()
                    {
                        Name = AddingNames[i],
                        Value = AddingPrices[i],
                        ID = collecting.ID,
                        Date = TimeNow()
                    };

                    _context.AdditionalPayments.Add(ad);
                    Person ppp = _context.People.Find(3);
                    ppp.credit -= AddingPrices[i];
                    _context.SaveChanges();

                }
            }

            var FinalCredit = c.TotalPaidFromMerchants - (c.TotalPaidForMerchants + c.TotalHalek + c.TotalOfAdditionalPayment + fathallah + c.TotalOfExpenses);
            //Person halaka = _context.People.Find(1);
            //halaka.credit += FinalCredit;
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
            //var paid_for_merchant = _context.PaidForMerchant.
            //    Include(c => c.Merchant).ToList()
            //    .Where(c => c.Date.ToShortDateString() == Date.ToShortDateString() && c.IsPaidForUs == false)
            //    .Select(c => new { merchantName = c.Merchant.MerchantName, Payment = c.Payment, Date = c.Date, IsPaidForUs = c.IsPaidForUs }).ToList();
            var paid_for_merchant = _context.PaidForMerchant.
                Include(c => c.Merchant).Where(c => c.Date.Date == Date.Date && c.IsPaidForUs == false)
                .Select(c => new { merchantName = c.Merchant.MerchantName, Payment = c.Payment, Date = c.Date, IsPaidForUs = c.IsPaidForUs }).ToList();
            //-----------------------------------------------------------------------------------------------------------------------------------------------------------


            //var paid_for_Us = _context.PaidForMerchant
            //    .Include(c => c.Merchant).ToList()
            //    .Where(c => c.Date.ToShortDateString() == Date.ToShortDateString() && c.IsPaidForUs == true).ToList()
            //    .Select(c => new { merchantName = c.Merchant.MerchantName, Payment = c.Payment, Date = c.Date, IsPaidForUs = c.IsPaidForUs }).ToList();
            var paid_for_Us = _context.PaidForMerchant
                .Include(c => c.Merchant).Where(c => c.Date.Date == Date.Date && c.IsPaidForUs == true)
                .Select(c => new { merchantName = c.Merchant.MerchantName, Payment = c.Payment, Date = c.Date, IsPaidForUs = c.IsPaidForUs }).ToList();
            //-----------------------------------------------------------------------------------------------------------------------------------------------------------

            //var Halek = _context.Debts_Sarhas.Include(c => c.Sarha)
            //    .Include(c => c.Sarha.Boat)
            //    .Include(c => c.Debt).ToList()
            //    .Where(c => c.Date.ToShortDateString() == Date.ToShortDateString() && c.PersonID == 3).OrderBy(c=>c.Sarha.BoatID)
            //    .Select(c => new { PersonID = c.PersonID, Date = c.Sarha.DateOfSarha, BoatName = c.Sarha.Boat.BoatName, Price = c.Price, DebtName = c.Debt.DebtName }).ToList();
            var Halek = _context.Debts_Sarhas.Where(c => c.Date.Date== Date.Date&& c.PersonID == 3)
                .Include(c => c.Sarha)
               .Include(c => c.Sarha.Boat)
               .Include(c => c.Debt).ToList()
               .OrderBy(c => c.Sarha.BoatID)
               .Select(c => new { PersonID = c.PersonID, Date = c.Sarha.DateOfSarha, BoatName = c.Sarha.Boat.BoatName, Price = c.Price, DebtName = c.Debt.DebtName }).ToList();
            //-----------------------------------------------------------------------------------------------------------------------------------------------------------

            //var Expenses = _context.Expenses.Include(c => c.Boat)
            //   .ToList().Select(c => new { PersonID = c.PersonID, Date = c.Date, BoatName = c.Boat.BoatName, price = c.Price, cause = c.Cause })
            //   .Where(c => c.Date.ToShortDateString() == Date.ToShortDateString() && c.PersonID == 3)
            //   .ToList();
            var Expenses = _context.Expenses.Where(c => c.Date.Date == Date.Date && c.PersonID == 3)
                .Include(c => c.Boat)
                .Select(c => new { PersonID = c.PersonID, Date = c.Date, BoatName = c.Boat.BoatName, price = c.Price, cause = c.Cause })
                .ToList();
            //-----------------------------------------------------------------------------------------------------------------------------------------------------------

            var collecting = _context.Collectings.Where(c => c.Date.Date == Date.Date).ToList();
            var collectingItem = collecting.FirstOrDefault();
            if (collectingItem == null)
            {
                return Json(new { message = "Fail" });
            }
            var AdditionalItems = _context.AdditionalPayments.ToList().Where(c => c.ID == collectingItem.ID).Select(c => new { Name = c.Name, Value = c.Value }).ToList();
            var AhmedFathAllah = collecting.Sum(c => c.TotalForFahAllah);
            var Cash = collecting.Sum(c => c.TotalOfCashes);
            var TotalPaidFromUs = paid_for_merchant.Sum(c => c.Payment) + Halek.Sum(c => c.Price) + AdditionalItems.Sum(c => c.Value) + AhmedFathAllah+ Expenses.Sum(c => c.price);
            var x = paid_for_Us.Sum(c => c.Payment) + Cash;
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
                cash = Cash,
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
            ViewBag.Leaders = new SelectList(_context.Boats.Where(b => b.IsActive == true && b.BoatLicenseNumber != "0").ToList(), "BoatID", "BoatLeader");
            ViewBag.Halek = new SelectList(_context.Debts.ToList(), "DebtID", "DebtName");
            var UnfinishedSarhas = _context.Sarhas.Where(x => x.IsFinished == false).Select(x => x.BoatID);
            ViewBag.Boats = new SelectList(_context.Boats.Where(b => b.IsActive == true && b.BoatLicenseNumber != "0").Where(b => UnfinishedSarhas.Contains(b.BoatID)).ToList(), "BoatID", "BoatName");
            ViewBag.SharedBoats = new SelectList(_context.Boats.Where(b => b.IsActive == true && b.TypeID == 2).Where(b => UnfinishedSarhas.Contains(b.BoatID)).ToList(), "BoatID", "BoatName");
            ViewBag.People = new SelectList(_context.People.Where(c => c.PersonID != 4 &&c.PersonID!=2), "PersonID", "Name");
            var pp = _context.People.Find(4);
            ViewBag.Credit = pp.credit;
            return View();
        }
        public IActionResult GetLeaderDebts(int BoatID)
        {
            var boat = _context.Boats.Find(BoatID);
            return Json(new { leaderDebts = boat.DebtsOfLeader });
        }
        public IActionResult FathAllahCalc(decimal fathallahFinalCredit, string Place, string Price , string ToLeader, string ToPrice, string BoatName, string HalekPrice,
            string HalekName, string BoatNameExpenses, string ExpensePricee, string Cause)
        {

            //-----------------------------------------------------------------------------------------
            var pp = _context.People.Find(4);
            //pp.credit = fathallahFinalCredit;

            if (Place != null && Price != null)
            {
                var PlaceCookie = Place.TrimEnd(Place[Place.Length - 1]);
                var PriceCookie = Price.TrimEnd(Price[Price.Length - 1]);

                string[] PlaceHalekNames = PlaceCookie.Split(",").Select(c => Convert.ToString(c)).ToArray();
                decimal[] prices = PriceCookie.Split(",").Select(c => Convert.ToDecimal(c)).ToArray();

                for (int i = 0; i < PlaceHalekNames.Length; i++)
                {
                    int DebtID = _context.Debts.Where(c => c.DebtName == PlaceHalekNames[i]).FirstOrDefault().DebtID;
                    _context.HalakaHaleks.Add(new HalakaHalek { Price = prices[i], DebtID = DebtID, Date = TimeNow() });
                    pp.credit -= prices[i];

                }
                _context.SaveChanges();
            }

            if (ToLeader != null && ToPrice != null)
            {
                var ToLeaderCookie = ToLeader.TrimEnd(ToLeader[ToLeader.Length - 1]);
                var ToPriceCookie = ToPrice.TrimEnd(ToPrice[ToPrice.Length - 1]);

                string[] ToLeaderNames = ToLeaderCookie.Split(",").Select(c => Convert.ToString(c)).ToArray();
                decimal[] Toprices = ToPriceCookie.Split(",").Select(c => Convert.ToDecimal(c)).ToArray();

                for (int i = 0; i < ToLeaderNames.Length; i++)
                {
                    var Boat = _context.Boats.Where(c => c.BoatLeader == ToLeaderNames[i]).FirstOrDefault();
                    _context.LeaderLoans.Add(new LeaderLoan { Price = Toprices[i], BoatID = Boat.BoatID, Date = TimeNow(),PersonID=4 });
                    Boat.DebtsOfLeader += Toprices[i];
                    pp.credit -= Toprices[i];
                    _context.SaveChanges();
                }

            }


            if (BoatName != null && HalekPrice != null && HalekName != null)
            {
                var BoatNameCookie = BoatName.TrimEnd(BoatName[BoatName.Length - 1]);
                var HalekPriceCookie = HalekPrice.TrimEnd(HalekPrice[HalekPrice.Length - 1]);
                var HalekNameCookie = HalekName.TrimEnd(HalekName[HalekName.Length - 1]);
                string[] BoatNames = BoatNameCookie.Split(",").Select(c => Convert.ToString(c)).ToArray();
                decimal[] HalekPrices = HalekPriceCookie.Split(",").Select(c => Convert.ToDecimal(c)).ToArray();
                string[] HalekNames = HalekNameCookie.Split(",").Select(c => Convert.ToString(c)).ToArray();

                for (int i = 0; i < HalekNames.Length; i++)
                {
                    var boat = _context.Boats.FirstOrDefault(c => c.BoatName == BoatNames[i]);

                    var lastSarhaID = _context.Sarhas.ToList().Where(c => c.BoatID == boat.BoatID && c.IsFinished == false).ToList();
                    if (lastSarhaID != null)
                    {

                        int maxSarahaID = lastSarhaID.FirstOrDefault().SarhaID;
                        var lastSarha = _context.Sarhas.Find(maxSarahaID);

                        var debt_sarha = _context.Debts_Sarhas.Include(c => c.Debt).ToList()
                            .Where(c => c.SarhaID == maxSarahaID && c.Debt.DebtName == HalekNames[i]&&c.PersonID==4 && c.Date.ToShortDateString() == TimeNow().ToShortDateString()).FirstOrDefault();
                        boat.DebtsOfHalek += HalekPrices[i];
                        if (debt_sarha != null)
                        {
                            debt_sarha.Price += HalekPrices[i];
                        }
                        else
                        {
                            var debt = _context.Debts.Where(c => c.DebtName == HalekNames[i]).FirstOrDefault();
                            Debts_Sarha ds = new Debts_Sarha() { Price = HalekPrices[i], DebtID = debt.DebtID, SarhaID = maxSarahaID, PersonID = 4, Date = TimeNow() };
                            _context.Debts_Sarhas.Add(ds);
                            _context.SaveChanges();

                        }
                        pp.credit -= HalekPrices[i];
                        _context.SaveChanges();
                    }



                }
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
                        PersonID = 4,
                        Price = ExpensePrice[i]
                    };
                    _context.Expenses.Add(ex);
                    _context.SaveChanges();

                    
                    pp.credit -= ExpensePrice[i];

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


            //var Halek = _context.Debts_Sarhas.Include(c => c.Sarha)
            //   .Include(c => c.Debt).Include(c => c.Sarha.Boat)
            //   .ToList().Select(c => new { PersonID = c.PersonID, Date = c.Sarha.DateOfSarha, BoatName = c.Sarha.Boat.BoatName, price = c.Price, DebtName = c.Debt.DebtName })
            //   .Where(c => c.Date.ToShortDateString() == Date.ToShortDateString() && c.PersonID == 4)
            //   .ToList();

            var Halek = _context.Debts_Sarhas.Include(c => c.Sarha)
               .Include(c => c.Sarha.Boat)
               .Include(c => c.Debt).ToList()
               .Where(c => c.Date.ToShortDateString() == Date.ToShortDateString() && c.PersonID == 4).OrderBy(c => c.Sarha.BoatID)
               .Select(c => new { PersonID = c.PersonID, Date = c.Sarha.DateOfSarha, BoatName = c.Sarha.Boat.BoatName, Price = c.Price, DebtName = c.Debt.DebtName }).ToList();


            decimal sumOfHalek = 0;
            if (halakaHaleks != null)
            {
                sumOfHalek = Halek.Sum(c => c.Price);
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

            return Json(new { message = "success", halakaHaleks = halakaHaleks, sumOfhalakaHaleks = sumOfhalakaHaleks, sumOfLoans = sumOfLoans, sumOfHalek = sumOfHalek, loans = Loans, halek = Halek, sumExpenses = sumOfExpenses, expenses = Expenses });
        }
        // هتبقي تبع المكتب
        public IActionResult PayBackLoan(decimal price, int BoatID)
        {
            var Boat = _context.Boats.Find(BoatID);
            Boat.DebtsOfLeader -= price;
            LeaderPayback p = new LeaderPayback() { BoatID = BoatID, Date = TimeNow(), Price = price };
            _context.LeaderPaybacks.Add(p);
            Person pp = _context.People.Find(4);
            pp.credit += price;
            _context.SaveChanges();
            return Json(new { message = "success" });
        }


        public IActionResult GiveFathAllah(decimal price, int personID)
        {
            Person p = _context.People.Find(4);
            FathAllahGift g = new FathAllahGift() { PersonID = personID, Date = TimeNow(), CreditBefore = p.credit, charge = price, CreditAfter = price + p.credit };
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
