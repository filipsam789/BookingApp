using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using WebApplication16.Models;

namespace WebApplication16.Controllers
{
    public class ListingsController : Controller
    {
        private BookingEntities db = new BookingEntities();

        // GET: Listings
        public ActionResult Index()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (user != null && user is Administrator)
            {
                var listings = db.Listings.Include(l => l.PropertyOwner);
                return View(listings.ToList());
            }
            var returnUrl = Url.Action("Index", "Listings");
            var loginUrl = Url.Action("Login", "Account", new { returnUrl });
            return Redirect(loginUrl);
        }
        // GET: Listings/My
        public ActionResult My()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (user != null && user is PropertyOwner)
            {
                var userId = User.Identity.GetUserId();
                var listings = db.Listings.Include(l => l.PropertyOwner).Where(l => l.IdPropertyOwner == userId).ToList();
                return View(listings);
            }
            var returnUrl = Url.Action("My", "Listings");
            var loginUrl = Url.Action("Login", "Account", new { returnUrl });
            return Redirect(loginUrl);
        }
        // GET: Listings/My
        public ActionResult Search()
        {
            // var destinations = db.Destinations.ToList();
            var user = db.Users.Find(User.Identity.GetUserId());
            return View(new List<Destination>());
        }
        [HttpPost]
        public ActionResult LiveTagSearch(string search)
        {
            // Call your method to search your data source here.
            // I'll use entity framework to query my DB
            var results= new List<Destination>();
            if (search=="")
                return PartialView(results);
            var searches = search.ToLower().Split(' ').ToList();
            results = db.Destinations
                .Where(d => searches.Any(s => d.CityName.ToLower().StartsWith(s) || d.CountryName.ToLower().StartsWith(s)))
                    .ToList();
            // Pass the List of results to a Partial View 
            return PartialView(results);
        }
        // POST: Listings/Search
        [HttpPost]
        public ActionResult Search(string livesearchtags, DateTime check_in_date, DateTime check_out_date, 
            int number_of_guests, int number_of_rooms)
        {
            if (check_in_date < DateTime.Now || check_out_date <= DateTime.Now)
            {
                ViewBag.ErrorMessage = "Please select dates for the future!";
                return View(new List<Destination>());
            }
            if (check_out_date <= check_in_date)
            {
                ViewBag.ErrorMessage = "Check out date must be greater than Check in date";
                return View(new List<Destination>());
            }

            var destination_input = livesearchtags.Split(',')[0];
            string sqlQuery = @"
                      WITH destination_input AS (SELECT @DestinationInput AS destination_name),
                      check_in_input AS (SELECT @CheckInDate AS check_in_date),
                      check_out_input AS (SELECT @CheckOutDate AS check_out_date),
                      num_rooms_input AS (SELECT @NumberOfRooms AS num_rooms),
                      num_guests_input AS (SELECT @NumberOfGuests AS num_guests),
                      unbooked_listings AS (
	                      SELECT l.id_listing, r.id_room
	                      FROM booking b
	                      FULL OUTER JOIN room r ON b.id_room = r.id_room
	                      JOIN listing l ON r.id_listing = l.id_listing
	                      WHERE r.id_room IN (
	  			                      SELECT DISTINCT r.id_room
				                      FROM booking b
				                      FULL OUTER JOIN room r ON b.id_room = r.id_room
				                      WHERE b.check_in_date isnull or (b.check_in_date >= '01-02-2024' OR
                                             b.check_out_date <= '12-31-2023')
                                             )
	                    ), unbooked_rooms AS (
			                      SELECT DISTINCT r.id_room, r.id_listing
			                      FROM booking b
			                      FULL OUTER JOIN room r ON b.id_room = r.id_room
			                      WHERE b.check_in_date isnull or (b.check_in_date >= '01-02-2024' OR
                                             b.check_out_date <= '12-31-2023') 
	                    )
                SELECT
                    destination_input.destination_name as destinationName,
                    check_in_input.check_in_date AS checkIn,
                    check_out_input.check_out_date AS checkOut,
                    (check_out_input.check_out_date::DATE - check_in_input.check_in_date::DATE) AS numberOfNights,
                    num_rooms_input.num_rooms as NumberOfRooms,
                    num_guests_input.num_guests as NumberOfGuests,
                    listing_list_info.id_listing as IdListing,
                    listing_list_info.name,
                    listing_list_info.main_image as MainImage,
                    AVG(listing_list_info.average_rating) AS averageRating,
                    MAX(listing_list_info.star_rating) AS starRating,
                    MIN(listing_list_info.listing_price) AS listingPrice,
                    listing_list_info.city_name as city,
                    listing_list_info.country_name as country,
                    COUNT(DISTINCT unbooked_rooms.id_room) AS roomCount,
                    MAX(listing_list_info.capacity_of_guests) AS capacityOfGuests,
                    MAX(listing_list_info.num_single_beds) AS NumSingleBeds,
                    MAX(listing_list_info.num_double_beds) AS NumDoubleBeds
                FROM
                    listing_list_info, destination_input, check_in_input, check_out_input, num_guests_input, num_rooms_input, unbooked_listings, unbooked_rooms
                WHERE
                    listing_list_info.id_listing IN (SELECT id_listing FROM unbooked_listings) AND
                    capacity_of_guests >= num_guests_input.num_guests AND
                    (listing_list_info.city_name ILIKE destination_input.destination_name OR
                     listing_list_info.country_name ILIKE destination_input.destination_name) AND
                    room_count >= num_rooms_input.num_rooms AND listing_list_info.id_listing = unbooked_rooms.id_listing
                GROUP BY
                    destination_input.destination_name, check_in_input.check_in_date, check_out_input.check_out_date,
                    (check_out_input.check_out_date::DATE - check_in_input.check_in_date::DATE), num_rooms_input.num_rooms,
                    num_guests_input.num_guests, listing_list_info.id_listing, listing_list_info.name, listing_list_info.main_image,
                    listing_list_info.city_name,listing_list_info.country_name
                HAVING COUNT(DISTINCT unbooked_rooms.id_room) >= num_rooms_input.num_rooms;
            ";

            // Execute the SQL query using Database.SqlQuery with parameters
            var result = db.Database.SqlQuery<ListingListInfoViewModel>(
        sqlQuery,
        new Npgsql.NpgsqlParameter("@DestinationInput", NpgsqlTypes.NpgsqlDbType.Text) { Value = destination_input },
        new Npgsql.NpgsqlParameter("@CheckInDate", NpgsqlTypes.NpgsqlDbType.Date) { Value = check_in_date },
        new Npgsql.NpgsqlParameter("@CheckOutDate", NpgsqlTypes.NpgsqlDbType.Date) { Value = check_out_date },
        new Npgsql.NpgsqlParameter("@NumberOfGuests", NpgsqlTypes.NpgsqlDbType.Integer) { Value = number_of_guests },
        new Npgsql.NpgsqlParameter("@NumberOfRooms", NpgsqlTypes.NpgsqlDbType.Integer) { Value = number_of_rooms }
    ).ToList();
            return View("Lookup", result);
        }

        // GET: Listings/Info/...
        public ActionResult Info(string destinationName, DateTime checkInDate, DateTime checkOutDate,
            int numberOfGuests, int numberOfRooms, int selectedIdListing)
        {
            var sqlQuery = "with destination_input as (select @DestinationInput as destination_name)," +
                " check_in_input as (select @CheckInDate as check_in_date)," +
                " check_out_input as (select @CheckOutDate as check_out_date)," +
                "\r\nnum_rooms_input as (select @NumberOfRooms as num_rooms), num_guests_input as (select @NumberOfGuests as num_guests)," +
                " selected_listing as (select @SelectedListing as id_listing)\r\n,booked_rooms as (\r\n" +
                "  select distinct r.id_room\r\n" +
                "  from booking b\r\n" +
                "  join room r on r.id_room = b.id_room\r\n" +
                "  where not (b.check_out_date < @CheckInDate or b.check_in_date > @CheckOutDate)\r\n)" +
                "select destination_input.destination_name as destinationName," +
                " check_in_input.check_in_date as checkInDate, check_out_input.check_out_date as checkOutDate," +
                "\r\n(check_out_input.check_out_date-check_in_input.check_in_date) as numberOfNights," +
                " num_rooms_input.num_rooms as numberOfRooms, num_guests_input.num_guests as numberOfGuests,\r\n" +
                "listing_info.*\r\nfrom listing_info," +
                " destination_input, check_in_input, check_out_input, num_guests_input, num_rooms_input, selected_listing\r\n" +
                "where listing_info.idListing = selected_listing.id_listing and idRoom not in (select id_room from booked_rooms)\r\n";
            
            var listingInfos = db.Database.SqlQuery<ListingInfo>(
                sqlQuery,
                new Npgsql.NpgsqlParameter("@DestinationInput", NpgsqlTypes.NpgsqlDbType.Text) { Value = destinationName },
                new Npgsql.NpgsqlParameter("@CheckInDate", NpgsqlTypes.NpgsqlDbType.Date) { Value = checkInDate },
                new Npgsql.NpgsqlParameter("@CheckOutDate", NpgsqlTypes.NpgsqlDbType.Date) { Value = checkOutDate },
                new Npgsql.NpgsqlParameter("@NumberOfGuests", NpgsqlTypes.NpgsqlDbType.Integer) { Value = numberOfGuests },
                new Npgsql.NpgsqlParameter("@NumberOfRooms", NpgsqlTypes.NpgsqlDbType.Integer) { Value = numberOfRooms },
                new Npgsql.NpgsqlParameter("@SelectedListing", NpgsqlTypes.NpgsqlDbType.Integer) { Value = selectedIdListing }
        ).ToList();
            List<RoomInfo> RoomsInfos = new List<RoomInfo>();
            List<int> roomIds = new List<int>();
            List<string> listingImages = new List<string>();
            foreach (var listingInfo in listingInfos)
            {
                var roomId = listingInfo.IdRoom;
                if (!roomIds.Contains(roomId))
                {
                    roomIds.Add(roomId);
                    var amenities = new List<RoomAmenity>();
                    var roomInfo = new RoomInfo
                    {
                        IdRoom = roomId,
                        Price = listingInfo.Price,
                        CapacityOfGuests = listingInfo.CapacityOfGuests,
                        RoomType = listingInfo.RoomType,
                        NumberOfSingleBeds = listingInfo.NumberOfSingleBeds,
                        NumberOfDoubleBeds = listingInfo.NumberOfDoubleBeds,
                        Amenities = amenities
                    };
                    amenities.AddRange(db.Rooms
                    .Include(r => r.Amenities)
                    .Where(r => r.Id == roomId)
                    .FirstOrDefault()?
                    .Amenities
                    .Where(a => a.Amenity == listingInfo.Amenity)
                    .ToList());
                    RoomsInfos.Add(roomInfo);
                }
                else
                {
                    var roomInfo = RoomsInfos.Where(ri=>ri.IdRoom == roomId).FirstOrDefault();
                    var existingAmenity = roomInfo.Amenities.FirstOrDefault(a => a.Amenity == listingInfo.Amenity);
                    if (existingAmenity == null)
                    {
                        var amenityToAdd = db.Rooms
                            .Include(r => r.Amenities)
                            .Where(r => r.Id == roomId)
                            .FirstOrDefault()?
                            .Amenities
                            .Where(a => a.Amenity == listingInfo.Amenity)
                            .FirstOrDefault();

                        if (amenityToAdd != null)
                        {
                            roomInfo.Amenities.Add(amenityToAdd);
                        }
                    }
                }
                if (!listingImages.Contains(listingInfo.Image))
                    listingImages.Add(listingInfo.Image);
            }
            var templateListingInfo = listingInfos.FirstOrDefault();
            ListingInfoViewModel listingInfoViewModel = new ListingInfoViewModel { IdListing = templateListingInfo.IdListing, Address = templateListingInfo.Address, AverageRating = templateListingInfo.AverageRating,
                IdProperty = templateListingInfo.IdProperty, CheckInDate = templateListingInfo.CheckInDate, CheckOutDate = templateListingInfo.CheckOutDate, Description = templateListingInfo.Description, DestinationName = templateListingInfo.DestinationName,
                MainImage = templateListingInfo.MainImage, NumberOfGuests = templateListingInfo.NumberOfGuests, NumberOfNights = templateListingInfo.NumberOfNights, NumberOfRooms = templateListingInfo.NumberOfRooms, StarRating = templateListingInfo.StarRating,
                RoomInfos = RoomsInfos, Images = listingImages.ToList(), ListingName=templateListingInfo.ListingName
            };

            return View(listingInfoViewModel);
        }
        // GET: Listings/Details/5
        public ActionResult Details(int? id)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (user != null && !(user is Traveller))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Listing listing = db.Listings.Include(l=>l.PropertyOwner).Where(l=>l.Id==id).FirstOrDefault();
                if (listing == null)
                {
                    return HttpNotFound();
                }
                return View(listing);
            }
            var returnUrl = Url.Action("Details", "Listings");
            var loginUrl = Url.Action("Login", "Account", new { returnUrl });
            return Redirect(loginUrl);
        }

        // GET: Listings/Create
        public ActionResult Create()
        {
            //ViewBag.IdPropertyOwner = new SelectList(db.Users, "Id", "Name");
            var user = db.Users.Find(User.Identity.GetUserId());
            if (user != null && user is PropertyOwner)
            {
                return View();
            }
            var returnUrl = Url.Action("Create", "Listings");
            var loginUrl = Url.Action("Login", "Account", new { returnUrl });
            return Redirect(loginUrl);
        }

        // POST: Listings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,MainImageUrl,IdOwner")] Listing listing)
        {
            if (ModelState.IsValid)
            {
                string ownerId = User.Identity.GetUserId();
                PropertyOwner propertyOwner = db.PropertyOwners.FirstOrDefault(po => po.Id == ownerId);
                listing.PropertyOwner = propertyOwner;
                propertyOwner.Listings.Add(listing);
                db.Listings.Add(listing);
                db.SaveChanges();
                return RedirectToAction("My");
            }

            ViewBag.IdPropertyOwner = new SelectList(db.Users, "Id", "Name", listing.IdPropertyOwner);
            return View(listing);
        }

        // GET: Listings/Edit/5
        public ActionResult Edit(int? id)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (user != null && user is PropertyOwner)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Listing listing = db.Listings.Find(id);
                if (listing == null)
                {
                    return HttpNotFound();
                }
                ViewBag.IdPropertyOwner = new SelectList(db.Users, "Id", "Name", listing.IdPropertyOwner);
                return View(listing);
            }
            var returnUrl = Url.Action("Edit", "Listings");
            var loginUrl = Url.Action("Login", "Account", new { returnUrl });
            return Redirect(loginUrl);
        }

        // POST: Listings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,IdPropertyOwner,MainImageUrl")] Listing listing)
        {
            if (ModelState.IsValid)
            {
                db.Entry(listing).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("My");
            }
            ViewBag.IdPropertyOwner = new SelectList(db.Users, "Id", "Name", listing.IdPropertyOwner);
            return View(listing);
        }
        /*
        // GET: Listings/Delete/5
        public ActionResult Delete(int? id)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (user != null &&  !(user is Traveller))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Listing listing = db.Listings.Find(id);
                if (listing == null)
                {
                    return HttpNotFound();
                }
                return View(listing);
            }
            var returnUrl = Url.Action("Delete", "Listings");
            var loginUrl = Url.Action("Login", "Account", new { returnUrl });
            return Redirect(loginUrl);
        }

        // POST: Listings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        */
        public ActionResult Delete(int id)
        {
            Listing listing = db.Listings.Find(id);
            db.Listings.Remove(listing);
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
