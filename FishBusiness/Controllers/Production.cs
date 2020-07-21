using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FishBusiness.Controllers
{
    public class Production : Controller
    {
        // GET: Production
        public ActionResult Index()
        {
            return View();
        }

        // GET: Production/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Production/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Production/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Production/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Production/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Production/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Production/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
