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
    public class RoomsController : Controller
    {
        private BookingEntities db = new BookingEntities();

        // GET: Rooms
        public ActionResult Index()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (user != null && user is Administrator)
            {
                var rooms = db.Rooms.Include(r => r.Listing).Include(r => r.Property);
                return View(rooms.ToList());
            }
            var returnUrl = Url.Action("Index", "Rooms");
            var loginUrl = Url.Action("Login", "Account", new { returnUrl });
            return Redirect(loginUrl);
        }
        // GET: Rooms/My
        public ActionResult My()
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            if (user != null && user is PropertyOwner)
            {
                var ownerPropertyIds = db.Properties
                .Where(p => p.IdOwner == userId)
                .Select(p => p.Id)
                .ToList();

                var rooms = db.Rooms
                    .Include(r => r.Listing)
                    .Include(r => r.Property)
                    .Where(r => ownerPropertyIds.Contains(r.IdProperty))
                    .ToList();
                return View(rooms.ToList());
            }
            var returnUrl = Url.Action("My", "Rooms");
            var loginUrl = Url.Action("Login", "Account", new { returnUrl });
            return Redirect(loginUrl);
        }

        // GET: Rooms/Details/5
        public ActionResult Details(int? id)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (user != null && !( user is Traveller))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Room room = db.Rooms.Find(id);
                if (room == null)
                {
                    return HttpNotFound();
                }
                return View(room);
            }
            var returnUrl = Url.Action("Details", "Rooms");
            var loginUrl = Url.Action("Login", "Account", new { returnUrl });
            return Redirect(loginUrl);
        }

        // GET: Rooms/Create
        public ActionResult Create()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (user != null && user is PropertyOwner)
            {
                ViewBag.IdListing = new SelectList(db.Listings, "Id", "Name");
                ViewBag.IdProperty = new SelectList(db.Properties, "Id", "Name");
                return View();
            }
            var returnUrl = Url.Action("Create", "Rooms");
            var loginUrl = Url.Action("Login", "Account", new { returnUrl });
            return Redirect(loginUrl);
        }
        // GET: AllProperties/AddNewPhoneNumber
        public ActionResult AddNewAmenity(int? id)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (user != null && user is PropertyOwner)
            {
                if (id == null)
                {
                    return HttpNotFound();
                }
                var viewModel = new AddNewAmenity { IdRoom = (int)id, Room = db.Rooms.Find((int)id) };
                return View(viewModel);
            }
            var returnUrl = Url.Action("AddNewAmenity", "Rooms");
            var loginUrl = Url.Action("Login", "Account", new { returnUrl });
            return Redirect(loginUrl);
        }

        // POST: AllProperties/InsertNewPhoneNumber
        [HttpPost]
        public ActionResult InsertNewAmenity(AddNewAmenity addNewAmenity)
        {
            if (ModelState.IsValid)
            {
                var room = db.Rooms.Find(addNewAmenity.IdRoom);
                var roomAmenity = new RoomAmenity { IdRoom = (int)room.Id, Amenity = addNewAmenity.Amenity, Room = room };
                room.Amenities.Add(roomAmenity);
                db.SaveChanges();
                return RedirectToAction("My");
            }
            //ViewBag.OwnerList = new SelectList(db.PropertyOwners.ToList(), "Id", "Email");
            return View(addNewAmenity);

        }
        // POST: Rooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,RoomType,CapacityOfGuests,IdListing,IdProperty,NumberOfSingleBeds,NumberOfDoubleBeds,Price,RoomNumber")] Room room)
        {
            if (ModelState.IsValid)
            {
                db.Rooms.Add(room);
                db.SaveChanges();
                return RedirectToAction("My");
            }

            ViewBag.IdListing = new SelectList(db.Listings, "Id", "Name", room.IdListing);
            ViewBag.IdProperty = new SelectList(db.Properties, "Id", "Name", room.IdProperty);
            return View(room);
        }

        // GET: Rooms/Edit/5
        public ActionResult Edit(int? id)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (user != null && user is PropertyOwner)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Room room = db.Rooms.Find(id);
                if (room == null)
                {
                    return HttpNotFound();
                }
                ViewBag.IdListing = new SelectList(db.Listings, "Id", "Name", room.IdListing);
                ViewBag.IdProperty = new SelectList(db.Properties, "Id", "Name", room.IdProperty);
                return View(room);
            }
            var returnUrl = Url.Action("Edit", "Rooms");
            var loginUrl = Url.Action("Login", "Account", new { returnUrl });
            return Redirect(loginUrl);
        }

        // POST: Rooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,RoomType,CapacityOfGuests,IdListing,IdProperty,NumberOfSingleBeds,NumberOfDoubleBeds,Price,RoomNumber")] Room room)
        {
            if (ModelState.IsValid)
            {
                db.Entry(room).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("My");
            }
            ViewBag.IdListing = new SelectList(db.Listings, "Id", "Name", room.IdListing);
            ViewBag.IdProperty = new SelectList(db.Properties, "Id", "Name", room.IdProperty);
            return View(room);
        }
        public ActionResult Delete(int id)
        {
            Room room = db.Rooms.Find(id);
            db.Rooms.Remove(room);
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
