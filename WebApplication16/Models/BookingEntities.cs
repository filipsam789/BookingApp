using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebApplication16.Models
{
    public class BookingEntities:DbContext
    {
        public DbSet<Destination> Destinations { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Listing> Listings { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Administrator> Administrators { get; set; }
        public DbSet<PropertyOwner> PropertyOwners { get; set; }
        public DbSet<Traveller> Travellers { get; set; }
        public BookingEntities() : base("name=BookingEntities")
        {
            //Configuration.LazyLoadingEnabled = true;
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Destination>().ToTable("destination", "public");
            modelBuilder.Entity<Booking>().ToTable("booking", "public");
            modelBuilder.Entity<Listing>().ToTable("listing", "public");
            modelBuilder.Entity<Property>().ToTable("property", "public");
            modelBuilder.Entity<Room>().ToTable("room", "public");
            modelBuilder.Entity<User>().ToTable("booking_user", "public");
            modelBuilder.Entity<Administrator>().ToTable("booking_administrator", "public");
            modelBuilder.Entity<PropertyOwner>().ToTable("booking_owner", "public");
            modelBuilder.Entity<Traveller>().ToTable("booking_traveller", "public");
            modelBuilder.Entity<ListingImage>().ToTable("image", "public");
            modelBuilder.Entity<RoomAmenity>().ToTable("amenity", "public");
            modelBuilder.Entity<PropertyFacility>().ToTable("facilities", "public");
            modelBuilder.Entity<PropertyPhoneNumber>().ToTable("phone_number", "public");
        }
        public static BookingEntities Create()
        {
            return new BookingEntities();
        }
    }
}