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
using Microsoft.AspNetCore.Identity;

namespace FishBusiness.Controllers
{
    public class MerchantsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public MerchantsController(ApplicationDbContext context , UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Merchants
        public async Task<IActionResult> Index()
        {
            return View(await _context.Merchants.Where(m=>m.IsOwner==false).ToListAsync());
        }
        public DateTime TimeNow()
        {
            TimeZone localZone = TimeZone.CurrentTimeZone;
            DateTime currentDate = DateTime.Now;
            DateTime currentUTC =
           localZone.ToUniversalTime(currentDate);
            return currentUTC.AddHours(2);
        }
        // GET: Merchants/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            MerchantDetailsVm model = new MerchantDetailsVm();
            var merchant = await _context.Merchants
                .FirstOrDefaultAsync(m => m.MerchantID == id);
            if (merchant == null)
            {
                return NotFound();
            }
            model.Merchant = merchant;
            model.MerchantReciepts = await _context.MerchantReciepts.Include(x => x.Merchant).Where(x => x.MerchantID == id).ToListAsync();
            model.IMerchantReciepts = await _context.IMerchantReciept.Include(x => x.Merchant).Where(x => x.MerchantID == id).ToListAsync();
            model.ISellerReciepts = await _context.ISellerReciepts.Include(x => x.Merchant).Where(x => x.MerchantID == id && x.TotalOfPrices == 0).ToListAsync();
            model.ISellerRecieptsMoneytized = await _context.ISellerReciepts.Include(x => x.Merchant).Where(x => x.MerchantID == id && x.TotalOfPrices > 0).ToListAsync();
            model.PaidForMerchantsFromUs = await _context.PaidForMerchant.Include(c=>c.Person).Where(c => c.IsPaidForUs == false).ToListAsync();
            model.PaidForUs = await _context.PaidForMerchant.Include(c => c.Person).Where(c => c.IsPaidForUs == true).ToListAsync();
            model.PaidForSeller = await _context.PaidForSellers.Include(c => c.Person).Where(c => c.MerchantID == id).ToListAsync();
            return View(model);
        }

        [HttpGet]
        public IActionResult LatestRec(int? id)
        {
           
            if (id == null)
            {
                return NotFound();
            }

            var merchantReceipts = _context.MerchantReciepts.Include(b => b.Merchant).Where(x => x.MerchantID == id);
            int merchantReceiptID;
            MerchantReciept mRec;
            if (merchantReceipts.Count()==0)
                return NotFound();
            else
            {
                merchantReceiptID = merchantReceipts.Max(x => x.MerchantRecieptID);
                mRec = _context.MerchantReciepts.Find(merchantReceiptID);
            }
            if (mRec == null)
            {
                return NotFound();
            }

            MerchantRecDetailsVm model = new MerchantRecDetailsVm();
            model.MerchantReciept = mRec;
            model.NormalMerchantItems = _context.MerchantRecieptItems.Include(c => c.Fish).Include(c => c.Boat).Include(c => c.ProductionType).Where(c => c.MerchantRecieptID == mRec.MerchantRecieptID && c.AmountId == null).ToList();
            model.AmountMerchantItems = _context.MerchantRecieptItems.Include(c => c.Fish).Include(c => c.Boat).Include(c => c.ProductionType).Where(c => c.MerchantRecieptID == mRec.MerchantRecieptID && c.AmountId != null).ToList();

            var results = from p in model.AmountMerchantItems
                          group p.MerchantRecieptItemID by p.AmountId into g
                          select new AmountVm { AmountId = g.Key, items = g };

            model.Amounts = results;

            return PartialView(model);
        }

        public IActionResult TableItems()
        {
            return PartialView(_context.Merchants.ToList());
        }


        // GET: Merchants/Create
        public IActionResult Create()
        {
            return PartialView();
        }

        // POST: Merchants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateC([Bind("MerchantID,MerchantName,PreviousDebts,Phone,Address,IsFromOutsideCity,PreviousDebtsForMerchant")] Merchant merchant)
        {
            if (ModelState.IsValid)
            {
                _context.Add(merchant);
                
                await _context.SaveChangesAsync();
                return Json(new { message = "success" });
            }
            return View(merchant);
        }

        // GET: Merchants/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var merchant = await _context.Merchants.FindAsync(id);
            if (merchant == null)
            {
                return NotFound();
            }
            return PartialView(merchant);
        }

        [HttpPost]
        public async Task<IActionResult> CalcDebts(decimal PaidValue, int MerchantID, bool IsCash)
        {

            var merchant = await _context.Merchants.FindAsync(MerchantID);

            if (merchant == null)
            {
                return NotFound();
            }

           
           

            var user = await _userManager.GetUserAsync(User);
         
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains("admin"))
            {
                Person pp = _context.People.Find(1);
                merchant.PreviousDebtsForMerchant -= PaidValue;
                PaidForMerchant p = new PaidForMerchant() { IsPaidForUs = false, Payment = PaidValue, Date = TimeNow(), MerchantID = MerchantID, IsCash = !IsCash, PreviousDebtsForMerchant = (merchant.PreviousDebtsForMerchant), PersonID = 1 };
                _context.PaidForMerchant.Add(p);
                pp.credit -= PaidValue;
                await _context.SaveChangesAsync();


            }
            else if (roles.Contains("partner"))
            {
                Person pp = _context.People.Find(2);
                merchant.PreviousDebtsForMerchant -= PaidValue;
                PaidForMerchant p = new PaidForMerchant() { IsPaidForUs = false, Payment = PaidValue, Date = TimeNow(), MerchantID = MerchantID, IsCash = !IsCash, PreviousDebtsForMerchant = (merchant.PreviousDebtsForMerchant), PersonID = 2 };
                _context.PaidForMerchant.Add(p);
                pp.credit -= PaidValue;
                await _context.SaveChangesAsync();
            }

            return Json(new { message="success", debtsForMerchant = merchant.PreviousDebtsForMerchant });
        }

        [HttpPost]
        public async Task<IActionResult> CalcDebtsAsSeller(decimal PaidValue, int MerchantID, bool IsCash)
        {

            var merchant = await _context.Merchants.FindAsync(MerchantID);

            if (merchant == null)
            {
                return NotFound();
            }




            var user = await _userManager.GetUserAsync(User);

            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains("admin"))
            {
                Person pp = _context.People.Find(1);
                merchant.PreviousDebtsForMerchant -= PaidValue;
                PaidForSeller p = new PaidForSeller() { Payment = PaidValue, Date = TimeNow(), MerchantID = MerchantID, IsCash = !IsCash, PreviousDebtsForSeller = (merchant.PreviousDebtsForMerchant), PersonID = 1 };
                _context.PaidForSellers.Add(p);
                pp.credit -= PaidValue;
                await _context.SaveChangesAsync();
            }
            else if (roles.Contains("partner"))
            {
                Person pp = _context.People.Find(2);
                merchant.PreviousDebtsForMerchant -= PaidValue;
                PaidForSeller p = new PaidForSeller() { Payment = PaidValue, Date = TimeNow(), MerchantID = MerchantID, IsCash = !IsCash, PreviousDebtsForSeller = (merchant.PreviousDebtsForMerchant), PersonID = 2 };
                _context.PaidForSellers.Add(p);
                pp.credit -= PaidValue;
                await _context.SaveChangesAsync();
            }

            return Json(new { message = "success", debtsForMerchant = merchant.PreviousDebtsForMerchant });
        }

        [HttpPost]
        public async Task<IActionResult> CalcDebtForUs(decimal PaidValue, int MerchantID, bool IsCash)
        {

            var merchant = await _context.Merchants.FindAsync(MerchantID);

            if (merchant == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains("admin"))
            {

                merchant.PreviousDebts -= PaidValue;
                PaidForMerchant p = new PaidForMerchant() { IsPaidForUs = true, Payment = PaidValue, Date = TimeNow(), MerchantID = MerchantID, IsCash = !IsCash, PreviousDebtsForMerchant = (merchant.PreviousDebts), PersonID = 1 };
                _context.PaidForMerchant.Add(p);
                Person pp = _context.People.Find(1);
                pp.credit += PaidValue;
                await _context.SaveChangesAsync();

            }
            else if (roles.Contains("partner"))
            {
                merchant.PreviousDebts -= PaidValue;
                PaidForMerchant p = new PaidForMerchant() { IsPaidForUs = true, Payment = PaidValue, Date = TimeNow(), MerchantID = MerchantID, IsCash = !IsCash, PreviousDebtsForMerchant = (merchant.PreviousDebts), PersonID = 2 };
                _context.PaidForMerchant.Add(p);
                Person pp = _context.People.Find(2);
                pp.credit += PaidValue;
                await _context.SaveChangesAsync();
            }

            return Json(new { message = "success", debtsOnMerchant = merchant.PreviousDebts });
        }

        // POST: Merchants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> cEdit([Bind("MerchantID,MerchantName,PreviousDebts,Phone,Address")] Merchant merchant)
        {


            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(merchant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MerchantExists(merchant.MerchantID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                //return RedirectToAction(nameof(Index));
                return Json(new { message = "success" });
            }
            return View(merchant);
        }

        // GET: Merchants/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var merchant = await _context.Merchants
                .FirstOrDefaultAsync(m => m.MerchantID == id);
            if (merchant == null)
            {
                return NotFound();
            }

            _context.Merchants.Remove(merchant);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        // POST: Merchants/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var merchant = await _context.Merchants.FindAsync(id);
        //    _context.Merchants.Remove(merchant);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool MerchantExists(int id)
        {
            return _context.Merchants.Any(e => e.MerchantID == id);
        }
    }
}
