using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FishBusiness.Models;
using Microsoft.EntityFrameworkCore;

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

        public IActionResult Index()
        {
            ViewBag.TodaysBoatReceipts = _context.BoatOwnerReciepts.ToList().Where(d=>d.Date.ToShortDateString()== DateTime.Now.ToShortDateString()).Count();
            ViewBag.TodaysMerchantReceipts = _context.MerchantReciepts.ToList().Where(d => d.Date.ToShortDateString() == DateTime.Now.ToShortDateString()).Count();
            ViewBag.TodaysExternalBoatReceipts = _context.ExternalReceipts.ToList().Where(d => d.Date.ToShortDateString() == DateTime.Now.ToShortDateString()).Count();
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
            var Recs = _context.BoatOwnerReciepts.Include(b => b.Boat).Include(b => b.Sarha).ToList().Where(d => d.Date.ToShortDateString() == DateTime.Now.ToShortDateString());
            return View(Recs);
        }
        [HttpGet]
        public IActionResult GetTodaysMerchantReceipts()
        {
            var Recs = _context.MerchantReciepts.Include(m => m.Merchant).ToList().Where(d => d.Date.ToShortDateString() == DateTime.Now.ToShortDateString());
            return View(Recs);
        }
        [HttpGet]
        public IActionResult GetTodaysExternalBoatReceipts()
        {
            var Recs = _context.ExternalReceipts.Include(m => m.Boat).ToList().Where(d => d.Date.ToShortDateString() == DateTime.Now.ToShortDateString());
            return View(Recs);
        }
    }
}
