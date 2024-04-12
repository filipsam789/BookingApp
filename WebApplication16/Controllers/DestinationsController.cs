using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication16.Models;

namespace WebApplication16.Controllers
{
    public class DestinationsController : Controller
    {
        private BookingEntities db = new BookingEntities();

        // GET: Destinations
        public ActionResult Index()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (user != null && user is Administrator)
            {
                return View(db.Destinations.ToList());
            }
            var returnUrl = Url.Action("Index", "Destinations");
            var loginUrl = Url.Action("Login", "Account", new { returnUrl });
            return Redirect(loginUrl);
        }

        // GET: Destinations/Details/5
        public ActionResult Details(int? id)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (user != null && user is Administrator)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Destination destination = db.Destinations.Find(id);
                if (destination == null)
                {
                    return HttpNotFound();
                }
                return View(destination);
            }
            var returnUrl = Url.Action("Details", "Destinations");
            var loginUrl = Url.Action("Login", "Account", new { returnUrl });
            return Redirect(loginUrl);
        }

        // GET: Destinations/Create
        public ActionResult Create()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (user != null && user is Administrator)
            {
                return View();
            }
            var returnUrl = Url.Action("Create", "Destinations");
            var loginUrl = Url.Action("Login", "Account", new { returnUrl });
            return Redirect(loginUrl);
        }

        // POST: Destinations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CountryName,CityName")] Destination destination)
        {
            if (ModelState.IsValid)
            {
                db.Destinations.Add(destination);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(destination);
        }

        // GET: Destinations/Edit/5
        public ActionResult Edit(int? id)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (user != null && user is Administrator)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Destination destination = db.Destinations.Find(id);
                if (destination == null)
                {
                    return HttpNotFound();
                }
                return View(destination);
            }
            var returnUrl = Url.Action("Edit", "Destinations");
            var loginUrl = Url.Action("Login", "Account", new { returnUrl });
            return Redirect(loginUrl);
        }

        // POST: Destinations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CountryName,CityName")] Destination destination)
        {
            if (ModelState.IsValid)
            {
                db.Entry(destination).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(destination);
        }

        public ActionResult Delete(int id)
        {
            Destination destination = db.Destinations.Find(id);
            db.Destinations.Remove(destination);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
