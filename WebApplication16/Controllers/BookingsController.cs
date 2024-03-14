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
    public class BookingsController : Controller
    {
        private BookingEntities db = new BookingEntities();

        // GET: Bookings
        public ActionResult Index()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (user != null && user is Traveller)
            {
               // var bookings = db.Bookings.Include(b => b.Listing).Include(b => b.Traveller).Where(b=>b.IdTraveller == user.Id);
                var sqlQuery = "select *\r\n" +
                    "from booking_info\r\n" +
                    "where booking_info.idUser = @TravellerId";
                var bookingInfos = db.Database.SqlQuery<BookingInfo>(
                    sqlQuery,
                    new Npgsql.NpgsqlParameter("@TravellerId", NpgsqlTypes.NpgsqlDbType.Text) { Value = user.Id }
                    ).ToList();
                List<BookingInfoViewModel> list = new List<BookingInfoViewModel>();
                Dictionary<int, List<string>> PhoneNumbersPerBooking = new Dictionary<int, List<string>>();
                foreach (var bookingInfo in bookingInfos)
                {
                    PhoneNumbersPerBooking.TryGetValue(bookingInfo.IdBooking, out List<string> PhoneNumbers);
                    if(PhoneNumbers == null) PhoneNumbers= new List<string>();
                    if (!PhoneNumbers.Contains(bookingInfo.PhoneNumber))
                        PhoneNumbers.Add(bookingInfo.PhoneNumber);
                    PhoneNumbersPerBooking[bookingInfo.IdBooking] = PhoneNumbers;
                    var templateBookingInfo = bookingInfo;
                    var property = db.Properties.Find(templateBookingInfo.IdProperty);
                    var listing = db.Listings.Find(templateBookingInfo.IdListing);

                    BookingInfoViewModel bookingInfoViewModel = new BookingInfoViewModel
                    {
                        Listing = listing,
                        IdBooking = templateBookingInfo.IdBooking,
                        Traveller = (Traveller)user,
                        CheckInDate = templateBookingInfo.CheckInDate,
                        CheckOutDate = templateBookingInfo.CheckOutDate,
                        NumberOfGuests = templateBookingInfo.NumberOfGuests,
                        PhoneNumbers = PhoneNumbers,
                        TotalPrice = templateBookingInfo.TotalPrice,
                        Rating = templateBookingInfo.Rating,
                        Property = property,
                        Room = db.Rooms.Find(templateBookingInfo.IdRoom),
                        RoomType = templateBookingInfo.RoomType
                    };
                    if(!list.Any(bivm=>bivm.IdBooking == bookingInfo.IdBooking))
                        list.Add(bookingInfoViewModel);
                }
                return View(list);
             }
            var returnUrl = Url.Action("Index", "Bookings");
            var loginUrl = Url.Action("Login", "Account", new { returnUrl });
            return Redirect(loginUrl);
        }

        // GET: Bookings/Details/5
        public ActionResult Details(int? id)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (user != null && user is Traveller)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Booking booking = db.Bookings.Include(b => b.Listing).Include(b=>b.Room).FirstOrDefault(b => b.Id == id);
                if (booking == null)
                {
                    return HttpNotFound();
                }
                return View(booking);
            }
            var returnUrl = Url.Action("Details", "Bookings");
            var loginUrl = Url.Action("Login", "Account", new { returnUrl });
            return Redirect(loginUrl);
        }
/*
        // GET: Bookings/Create
        public ActionResult Create()
        {
            ViewBag.IdListing = new SelectList(db.Listings, "Id", "Name");
            ViewBag.IdTraveller = new SelectList(db.Users, "Id", "Name");
            return View();
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Rating,DiscountPercent,CheckInDate,CheckOutDate,IdTraveller,IdListing,NumberOfGuests")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                db.Bookings.Add(booking);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdListing = new SelectList(db.Listings, "Id", "Name", booking.IdListing);
            ViewBag.IdTraveller = new SelectList(db.Users, "Id", "Name", booking.IdTraveller);
            return View(booking);
        } 
*/
        // POST: Bookings/Reserve
        [HttpPost]
        public ActionResult Reserve(DateTime CheckInDate, DateTime CheckOutDate, int IdListing, int NumberOfGuests, int IdRoom)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (user != null && user is Traveller)
            {
                if (ModelState.IsValid)
                {
                    Random rand = new Random();
                    var booking = new Booking { IdListing = IdListing, Listing=db.Listings.Find(IdListing) ,CheckInDate = CheckInDate, CheckOutDate = CheckOutDate,
                        NumberOfGuests = NumberOfGuests, IdTraveller = user.Id, Traveller = (Traveller) user, DiscountPercent = rand.Next(0,16), IdRoom=IdRoom, Room=db.Rooms.Find(IdRoom)
                    };
                    db.Bookings.Add(booking);
                    db.SaveChanges();
                    return View("Details", booking);
                }
            }
            var returnUrl = Url.Action("Reserve", "Bookings");
            var loginUrl = Url.Action("Login", "Account", new { returnUrl });
            return Redirect(loginUrl);
        }
        /*
                // GET: Bookings/Edit/5
                public ActionResult Edit(int? id)
                {
                    var user = db.Users.Find(User.Identity.GetUserId());
                    if (user != null && user is Traveller)
                    {
                        if (id == null)
                        {
                            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                        }
                        Booking booking = db.Bookings.Find(id);
                        if (booking == null)
                        {
                            return HttpNotFound();
                        }
                        ViewBag.IdListing = new SelectList(db.Listings, "Id", "Name", booking.IdListing);
                        ViewBag.IdTraveller = new SelectList(db.Users, "Id", "Name", booking.IdTraveller);
                        return View(booking);
                    }
                    return RedirectToAction("Login", "Account");
                }

                // POST: Bookings/Edit/5
                // To protect from overposting attacks, enable the specific properties you want to bind to, for 
                // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
                [HttpPost]
                [ValidateAntiForgeryToken]
                public ActionResult Edit([Bind(Include = "Id,Rating,DiscountPercent,CheckInDate,CheckOutDate,IdTraveller,IdListing,NumberOfGuests")] Booking booking)
                {
                    var user = db.Users.Find(User.Identity.GetUserId());
                    if (user != null && user is Traveller)
                    {
                        if (ModelState.IsValid)
                        {
                            db.Entry(booking).State = EntityState.Modified;
                            db.SaveChanges();
                            return RedirectToAction("Index");
                        }
                        ViewBag.IdListing = new SelectList(db.Listings, "Id", "Name", booking.IdListing);
                        ViewBag.IdTraveller = new SelectList(db.Users, "Id", "Name", booking.IdTraveller);
                        return View(booking);
                    }
                    return RedirectToAction("Login", "Account");
                }

                // GET: Bookings/Delete/5
                public ActionResult Delete(int? id)
                {
                    var user = db.Users.Find(User.Identity.GetUserId());
                    if (user != null && user is Traveller)
                    {
                        if (id == null)
                        {
                            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                        }
                        Booking booking = db.Bookings.Find(id);
                        if (booking == null)
                        {
                            return HttpNotFound();
                        }
                        return View(booking);
                    }
                    var returnUrl = Url.Action("Delete", "Bookings");
                    var loginUrl = Url.Action("Login", "Account", new { returnUrl });
                    return Redirect(loginUrl);
                }

                // POST: Bookings/Delete/5
                [HttpPost, ActionName("Delete")]
                [ValidateAntiForgeryToken]
        */
        // GET: Bookings/Rate
        public ActionResult Rate(int? id)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (user != null && user is Traveller)
            {
                if (id == null)
                {
                    return HttpNotFound();
                }
                var viewModel = new RateBooking { IdBooking = (int)id, Booking = db.Bookings.Include(b=>b.Listing).Where(b=>b.Id==(int)id).FirstOrDefault() };
                if (viewModel.Booking == null)
                {
                    return HttpNotFound();
                }
                return View(viewModel);
            }
            var returnUrl = Url.Action("RateBooking", "Bookings");
            var loginUrl = Url.Action("Login", "Account", new { returnUrl });
            return Redirect(loginUrl);
        }

        // POST: Bookings/RateBooking
        [HttpPost]
        public ActionResult RateBooking(RateBooking rateBooking)
        {
            if (ModelState.IsValid)
            {
                var booking = db.Bookings.Find(rateBooking.IdBooking);
                booking.Rating = rateBooking.Rating;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(rateBooking);

        }
        public ActionResult Delete(int id)
        {
            Booking booking = db.Bookings.Find(id);
            db.Bookings.Remove(booking);
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
