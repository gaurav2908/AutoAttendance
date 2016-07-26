using AutoAttendance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AutoAttendance.Controllers
{
    public class ClassNameController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        // GET: ClassName
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View(db.ClassNames.Where(className => className.ApplicationUserId == User.Identity.Name));
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        
        // GET: ClassName
        public ActionResult IndexAttendance()
        {
            if (User.Identity.IsAuthenticated)
            {

                return View(db.ClassNames.Where(className => className.ApplicationUserId == User.Identity.Name));
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult Index(string SelectClass)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Student", new { className = SelectClass });
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult IndexAttendance(string SelectClass)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Attendance", "Student", new { className = SelectClass });
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: ClassName/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ClassName/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ClassName/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: ClassName/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ClassName/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: ClassName/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ClassName/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
