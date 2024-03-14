using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNetCore.Identity;
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
    public class AllPropertiesController : Controller
    {
        private BookingEntities db = new BookingEntities();

        
        // GET: AllProperties
        public ActionResult Index()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if(user != null && user is Administrator)
            {
                var properties = db.Properties.Include(p => p.Destination).Include(p => p.PropertyOwner).ToList();
                return View(properties);
            }
            var returnUrl = Url.Action("Index", "AllProperties");
            var loginUrl = Url.Action("Login", "Account", new { returnUrl });
            return Redirect(loginUrl);
        }
        public ActionResult MyProperties()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (user != null && user is PropertyOwner)
            {
                var loggedInOwnerId = User.Identity.GetUserId();
                var properties = db.Properties.Include(p => p.Destination).Include(p => p.PropertyOwner).Where(p => p.IdOwner == loggedInOwnerId).ToList();
                return View(properties);
            }
            var returnUrl = Url.Action("MyProperties", "AllProperties");
            var loginUrl = Url.Action("Login", "Account", new { returnUrl });
            return Redirect(loginUrl);
        }
        // GET: AllProperties/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Property property = db.Properties.Find(id);
            if (property == null)
            {
                return HttpNotFound();
            }
            return View(property);
        }

        // GET: AllProperties/Create
        public ActionResult Create()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (user != null && user is PropertyOwner)
            {
                ViewBag.DestinationList = new SelectList(db.Destinations.ToList(), "Id", "CityName");
                //ViewBag.OwnerList = new SelectList(db.PropertyOwners.ToList(), "Id", "Email");
                return View();
            }
            var returnUrl = Url.Action("Create", "AllProperties");
            var loginUrl = Url.Action("Login", "Account", new { returnUrl });
            return Redirect(loginUrl);
        }

        // POST: AllProperties/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,NumberOfStars,IdDestination,IdOwner,Address")] Property property)
        {
            if (ModelState.IsValid)
            {
                string ownerId = User.Identity.GetUserId();
                PropertyOwner propertyOwner = db.PropertyOwners.FirstOrDefault(po => po.Id == ownerId);
                property.PropertyOwner = propertyOwner;
                var destination = db.Destinations.Find(property.IdDestination);
                property.Destination = destination;
                destination.Properties.Add(property);
                propertyOwner.Properties.Add(property);
                db.Properties.Add(property);
                db.SaveChanges();
                return RedirectToAction("MyProperties");
            }

            ViewBag.DestinationList = new SelectList(db.Destinations.ToList(), "Id", "CityName");
            //ViewBag.OwnerList = new SelectList(db.PropertyOwners.ToList(), "Id", "Email");
            return View(property);

        }

        // GET: AllProperties/AddNewPhoneNumber
        public ActionResult AddNewPhoneNumber(int? id)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (user != null && user is PropertyOwner)
            {
                //ViewBag.OwnerList = new SelectList(db.PropertyOwners.ToList(), "Id", "Email");
                if (id == null)
                {
                    return HttpNotFound();
                }
                var viewModel = new AddNewPhoneViewModel { IdProperty = (int)id, Property = db.Properties.Find((int)id) };
                if (viewModel.Property == null)
                {
                    return HttpNotFound();
                }
                return View(viewModel);
            }
            var returnUrl = Url.Action("AddNewPhoneNumber", "AllProperties");
            var loginUrl = Url.Action("Login", "Account", new { returnUrl });
            return Redirect(loginUrl);
        }

        // POST: AllProperties/InsertNewPhoneNumber
        [HttpPost]
        public ActionResult InsertNewPhoneNumber(AddNewPhoneViewModel addNewPhoneViewModel)
        {
            if (ModelState.IsValid)
            {
                var property = db.Properties.Find(addNewPhoneViewModel.IdProperty);
                var propertyPhoneNumber = new PropertyPhoneNumber { IdProperty = (int)property.Id, PhoneNumber=addNewPhoneViewModel.PhoneNumber, Property=property };
                property.PhoneNumbers.Add(propertyPhoneNumber);
                db.SaveChanges();
                return RedirectToAction("MyProperties");
            }
            //ViewBag.OwnerList = new SelectList(db.PropertyOwners.ToList(), "Id", "Email");
            return View(addNewPhoneViewModel);

        }
        // GET: AllProperties/AddNewFacility
        public ActionResult AddNewFacility(int? id)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (user != null && user is PropertyOwner)
            {
                if (id == null)
                {
                    return HttpNotFound();
                }
                var viewModel = new AddNewFacilityViewModel { IdProperty = (int)id, Property = db.Properties.Find((int)id) };
                if (viewModel.Property == null)
                {
                    return HttpNotFound();
                }
                return View(viewModel);
            }
            var returnUrl = Url.Action("AddNewFacility", "AllProperties");
            var loginUrl = Url.Action("Login", "Account", new { returnUrl });
            return Redirect(loginUrl);
        }

        // POST: AllProperties/InsertNewFacility
        [HttpPost]
        public ActionResult InsertNewFacility(AddNewFacilityViewModel addNewFacilityViewModel)
        {
            if (ModelState.IsValid)
            {
                var property = db.Properties.Find(addNewFacilityViewModel.IdProperty);

                var propertyFacility = new PropertyFacility { IdProperty = (int)property.Id, Facility = addNewFacilityViewModel.Facility, Property = property };
                property.Facilities.Add(propertyFacility);
                db.SaveChanges();
                return RedirectToAction("MyProperties");
            }
            //ViewBag.OwnerList = new SelectList(db.PropertyOwners.ToList(), "Id", "Email");
            return View(addNewFacilityViewModel);

        }

        // GET: AllProperties/Edit/5
        public ActionResult Edit(int? id)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (user != null && user is PropertyOwner)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Property property = db.Properties.Find(id);
                if (property == null)
                {
                    return HttpNotFound();
                }
                ViewBag.IdDestination = new SelectList(db.Destinations, "Id", "CountryName", property.IdDestination);
                ViewBag.IdOwner = new SelectList(db.Users, "Id", "Name", property.IdOwner);
                return View(property);
            }
            var returnUrl = Url.Action("Edit", "AllProperties");
            var loginUrl = Url.Action("Login", "Account", new { returnUrl });
            return Redirect(loginUrl);
        }

        // POST: AllProperties/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,NumberOfStars,IdDestination,IdOwner,Address")] Property property)
        {
            if (ModelState.IsValid)
            {
                db.Entry(property).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("MyProperties");
            }
            ViewBag.IdDestination = new SelectList(db.Destinations, "Id", "CountryName", property.IdDestination);
            ViewBag.IdOwner = new SelectList(db.Users, "Id", "Name", property.IdOwner);
            return View(property);
        }
/*
        // GET: AllProperties/Delete/5
        public ActionResult Delete(int? id)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (user != null && user is PropertyOwner)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Property property = db.Properties.Find(id);
                if (property == null)
                {
                    return HttpNotFound();
                }
                return View(property);
            }
            var returnUrl = Url.Action("Delete", "AllProperties");
            var loginUrl = Url.Action("Login", "Account", new { returnUrl });
            return Redirect(loginUrl);
        }

        // POST: AllProperties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
*/
        public ActionResult Delete(int id)
        {
            Property property = db.Properties.Find(id);
            db.Properties.Remove(property);
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
