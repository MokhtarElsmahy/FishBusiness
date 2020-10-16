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
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace FishBusiness.Controllers
{
    public class ISellerRecieptsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IHostingEnvironment _hosting { get; set; }

        public ISellerRecieptsController(ApplicationDbContext context, IHostingEnvironment hosting)
        {
            _context = context;
            _hosting = hosting;
        }

        // GET: ISellerReciepts
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ISellerReciepts.Include(i => i.Merchant);
            return View(await applicationDbContext.ToListAsync());
        }
        public DateTime TimeNow()
        {
            TimeZone localZone = TimeZone.CurrentTimeZone;
            DateTime currentDate = DateTime.Now;
            DateTime currentUTC =
           localZone.ToUniversalTime(currentDate);
            return currentUTC.AddHours(2);
        }
        // GET: ISellerReciepts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var iSellerReciept = await _context.ISellerReciepts
                .Include(i => i.Merchant)
                .FirstOrDefaultAsync(m => m.ISellerRecieptID == id);

            if (iSellerReciept == null)
            {
                return NotFound();
            }
            var items = _context.ISellerRecieptItems.Include(i => i.Fish).Include(i => i.ProductionType).Where(i => i.ISellerRecieptID == iSellerReciept.ISellerRecieptID).ToList();
            ISellerRecieptDetailsVm model = new ISellerRecieptDetailsVm();
            model.ISellerReciept = iSellerReciept;
            model.ISellerRecieptItems = items;
            ViewBag.MerchantDebts = _context.Merchants.Where(c => c.MerchantID == iSellerReciept.MerchantID).FirstOrDefault().PreviousDebts;
            if (iSellerReciept.ReceiptImage != null)
                ViewBag.ImageExists = true;
            else
                ViewBag.ImageExists = false;
            return View(model);
        }

        public async Task<IActionResult> Moneytization(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var iSellerReciept = await _context.ISellerReciepts
                .Include(i => i.Merchant)
                .FirstOrDefaultAsync(m => m.ISellerRecieptID == id);

            if (iSellerReciept == null)
            {
                return NotFound();
            }
            var items = _context.ISellerRecieptItems.Include(i => i.Fish).Include(i => i.ProductionType).Where(i => i.ISellerRecieptID == iSellerReciept.ISellerRecieptID).ToList();
            ISellerRecieptDetailsVm model = new ISellerRecieptDetailsVm();
            model.ISellerReciept = iSellerReciept;
            model.ISellerRecieptItems = items;
            ViewBag.PreviousDebts = _context.Merchants.FirstOrDefault(m => m.MerchantID == iSellerReciept.MerchantID).PreviousDebts;
            return View(model);
        }

        [HttpPost]
        public IActionResult MoneytizationSave(int IsellerRecieptID, double TotalOfPrices, double Commision, double TotalOfPricesAfterCommision, double PaidFromDebt, double DebtsAfterCommisionAndPayment, string Pricescookie, string ImageUrl)
        {
            decimal[] prices = Pricescookie.Split(",").Select(c => Convert.ToDecimal(c)).ToArray();


            var iSellerReciept = _context.ISellerReciepts
                .Include(i => i.Merchant)
                .FirstOrDefault(m => m.ISellerRecieptID == IsellerRecieptID);
            iSellerReciept.Commision = Commision;
            iSellerReciept.PaidFromDebt = PaidFromDebt;
            iSellerReciept.TotalOfPrices = TotalOfPrices;
            if (ImageUrl != null)
            {
                iSellerReciept.ReceiptImage = ImageUrl;
            }
            else
            {
                iSellerReciept.ReceiptImage = "/assets/img/defaultRecImage.png";
            }

            iSellerReciept.DateOfMoneytization = TimeNow();
            PaidForMerchant p = new PaidForMerchant() { Date = TimeNow(), IsCash = true, MerchantID = iSellerReciept.MerchantID, Payment = (decimal)PaidFromDebt, IsPaidForUs = true, PreviousDebtsForMerchant = (decimal)(DebtsAfterCommisionAndPayment - PaidFromDebt), PersonID = 1 };
            _context.PaidForMerchant.Add(p);
            Person pp = _context.People.Find(1);
            pp.credit += Convert.ToDecimal(PaidFromDebt);
            if (iSellerReciept == null)
            {
                return NotFound();
            }
            var items = _context.ISellerRecieptItems.Include(i => i.Fish).Include(i => i.ProductionType).Where(i => i.ISellerRecieptID == iSellerReciept.ISellerRecieptID).ToList();

            for (int i = 0; i < items.Count(); i++)
            {
                items[i].UnitPrice = prices[i];
            }
            _context.SaveChanges();

            var debts = DebtsAfterCommisionAndPayment - PaidFromDebt;
            var merchant = _context.Merchants.FirstOrDefault(m => m.MerchantID == iSellerReciept.MerchantID);
            merchant.PreviousDebts = (decimal)debts;
            _context.SaveChanges();

            return Json(new { message = "success", totalDebts = debts, id = iSellerReciept.ISellerRecieptID });

        }
        [HttpPost]
        public async Task<IActionResult> UploadImg(int id, IList<IFormFile> files)
        {
            var rec = _context.ISellerReciepts.Find(id);
            foreach (IFormFile source in files)
            {
                string filename = ContentDispositionHeaderValue.Parse(source.ContentDisposition).FileName.Trim('"');
                string newFileName = Guid.NewGuid() + filename;
                using (FileStream output = System.IO.File.Create(this.GetPathAndFilename(newFileName)))
                    await source.CopyToAsync(output);
                rec.ReceiptImage = newFileName;
            }

            _context.SaveChanges();
            return Json(new { message = "success" });
        }
      
        public async Task<IActionResult> GetStockInfo(int StockID, string FishName)
        {

            var stock = await _context.Stocks.Include(s => s.Fish).Include(s => s.ProductionType).Where(s => s.StockID == StockID && s.Fish.FishName == FishName).FirstOrDefaultAsync();

            if (stock != null)
            {
                return Json(new { message = "success", totalWeight = stock.TotalWeight, productionTypeId = stock.ProductionTypeID });
            }
            else
            {
                return Json(new { message = "fail" });
            }

        }
        // GET: ISellerReciepts/Create
        public async Task<IActionResult> Create()
        {
            ViewData["MerchantID"] = new SelectList(_context.Merchants.Where(m => m.IsFromOutsideCity == true), "MerchantID", "MerchantName");
            ViewData["ProductionTypeID"] = new SelectList(_context.ProductionTypes, "ProductionTypeID", "ProductionName");

            IsellerRecVm model = new IsellerRecVm();
            model.Stocks = await _context.Stocks.Include(r => r.Fish).ToListAsync();

            return View(model);
        }

        // POST: ISellerReciepts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //FishNames: FishNames,
        //ProductionTypes: ProductionTypes,
        //                qtys: qtys,
        //                NOfBoxes: NOfBoxes
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Create(int MerchantID, DateTime Date, double CarPrice, string FishNames, string ProductionTypes, string qtyss, string NOfBoxess)
        {
            if (ModelState.IsValid)
            {
                ISellerReciept sellerReciept = new ISellerReciept();
                sellerReciept.MerchantID = MerchantID;
                sellerReciept.Date = Date;
                sellerReciept.CarPrice = CarPrice;
                sellerReciept.CarDistination = _context.Merchants.Find(MerchantID).Address;
                _context.Add(sellerReciept);


                /*
                
                دى مش هتتحسب دلوقت لان السواق مش شرط يتحاسب وقتها ممكن يتحاسب بعد كذا يوم 

                Person p = _context.People.Find(1);
                p.credit -= (decimal)CarPrice;

                */

                _context.SaveChanges();


                var FishesCookie = FishNames.TrimEnd(FishNames[FishNames.Length - 1]);
                var ProductionTypesCookie = ProductionTypes.TrimEnd(ProductionTypes[ProductionTypes.Length - 1]);
                var qtysCookie = qtyss.TrimEnd(qtyss[qtyss.Length - 1]);
                var NOfBoxesCookie = NOfBoxess.TrimEnd(NOfBoxess[NOfBoxess.Length - 1]);

                string[] Fishes = FishesCookie.Split(",").Select(c => Convert.ToString(c)).ToArray();
                string[] Productions = ProductionTypesCookie.Split(",").Select(c => Convert.ToString(c)).ToArray();
                double[] qtys = qtysCookie.Split(",").Select(c => Convert.ToDouble(c)).ToArray();
                //decimal[] unitPrices = unitpricesCookie.Split(",").Select(c => Convert.ToDecimal(c)).ToArray();
                int[] NOfBoxes = NOfBoxesCookie.Split(",").Select(c => Convert.ToInt32(c)).ToArray();

                //var latestReceipt = _context.ISellerReciepts.Max(x => x.ISellerRecieptID);
                for (int i = 0; i < Fishes.Length; i++)
                {
                    var fish = _context.Fishes.Single(x => x.FishName == Fishes[i]);
                    var Produc = _context.ProductionTypes.Single(x => x.ProductionName == Productions[i]);

                    ISellerRecieptItem ISellerRecieptItem = new ISellerRecieptItem()
                    {
                        ISellerRecieptID = sellerReciept.ISellerRecieptID,
                        FishID = fish.FishID,
                        ProductionTypeID = Produc.ProductionTypeID,
                        Qty = qtys[i],
                        BoxQty = NOfBoxes[i],
                    };
                    var stock = _context.Stocks.Where(i => i.FishID == fish.FishID && i.ProductionTypeID == Produc.ProductionTypeID).FirstOrDefault();
                    stock.Qty -= qtys[i];
                    if (stock.ProductionTypeID == 3)
                    {
                        stock.TotalWeight -= qtys[i];
                    }
                    _context.SaveChanges();
                    stock = _context.Stocks.Where(i => i.FishID == fish.FishID && i.ProductionTypeID == Produc.ProductionTypeID).FirstOrDefault();
                    if (stock.Qty == 0)
                    {
                        _context.Stocks.Remove(stock);

                    }
                    _context.ISellerRecieptItems.Add(ISellerRecieptItem);
                    _context.SaveChanges();
                }

                return Json(new { message = "success", id = sellerReciept.ISellerRecieptID });

            }

            return Json(new { message = "fail" });

        }

        // GET: ISellerReciepts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var iSellerReciept = await _context.ISellerReciepts.FindAsync(id);
            if (iSellerReciept == null)
            {
                return NotFound();
            }
            ViewData["MerchantID"] = new SelectList(_context.Merchants, "MerchantID", "MerchantName", iSellerReciept.MerchantID);
            return View(iSellerReciept);
        }

        // POST: ISellerReciepts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ISellerRecieptID,Date,MerchantID")] ISellerReciept iSellerReciept)
        {
            if (id != iSellerReciept.ISellerRecieptID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(iSellerReciept);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ISellerRecieptExists(iSellerReciept.ISellerRecieptID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MerchantID"] = new SelectList(_context.Merchants, "MerchantID", "MerchantName", iSellerReciept.MerchantID);
            return View(iSellerReciept);
        }

        // GET: ISellerReciepts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var ISellerReciept = await _context.ISellerReciepts.FirstOrDefaultAsync(ww => ww.ISellerRecieptID == id);
            if (ISellerReciept == null)
            {
                return NotFound();
            }
            var ISellerRecieptItems = _context.ISellerRecieptItems.Where(x => x.ISellerRecieptID == id).ToList();
            _context.ISellerRecieptItems.RemoveRange(ISellerRecieptItems);
            _context.Remove(ISellerReciept);



            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        //// POST: ISellerReciepts/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var iSellerReciept = await _context.ISellerReciepts.FindAsync(id);
        //    _context.ISellerReciepts.Remove(iSellerReciept);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool ISellerRecieptExists(int id)
        {
            return _context.ISellerReciepts.Any(e => e.ISellerRecieptID == id);
        }
        #region MyRegion
        //[HttpPost]
        //public async Task<IActionResult> UploadImage(IList<IFormFile> files)
        //{
        //    foreach (IFormFile source in files)
        //    {
        //        string filename = ContentDispositionHeaderValue.Parse(source.ContentDisposition).FileName.Trim('"');

        //        filename = this.EnsureCorrectFilename(filename);

        //        using (FileStream output = System.IO.File.Create(this.GetPathAndFilename(filename)))
        //            await source.CopyToAsync(output);
        //        return Json(new { ImageURL = GetPathAndFilename(filename) });
        //    }

        //    return Json(new { ImageURL = "/img/defaultRecImage.png" });
        //}

        //private string EnsureCorrectFilename(string filename)
        //{
        //    if (filename.Contains("\\"))
        //        filename = filename.Substring(filename.LastIndexOf("\\") + 1);

        //    return filename;
        //} 
        #endregion
        private string GetPathAndFilename(string filename)
        {
            return this._hosting.WebRootPath + "\\img\\" + filename;
        }
        //private string GetPathAndFilename(string filename)
        //{
        //    return _hosting.WebRootPath + "\\img\\" + filename;
        //}
        public IActionResult UploadImage()
        {

            string result = "defaultRecImage.png";

            try
            {
                var file = Request.Form.Files;
                string uploads = Path.Combine(_hosting.WebRootPath, @"uploads");
                var filename = ContentDispositionHeaderValue.Parse(file[0].ContentDisposition).FileName.Trim('"');
                var newFileName = Guid.NewGuid() + filename;
                string fullPath = Path.Combine(uploads, newFileName);
                file[0].CopyTo(new FileStream(fullPath, FileMode.Create));


                return Json(new { image = $"/uploads/{newFileName}" });
            }
            catch (Exception ex)
            {
                result = ex.Message;
                return Json(new { image = $"/uploads/{result}" });
            }
          

        }
    }
}
