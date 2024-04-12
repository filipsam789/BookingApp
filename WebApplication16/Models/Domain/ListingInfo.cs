using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication16.Models
{
    public class ListingInfo
    {
        public int IdListing { get; set; }
        public string ListingName { get; set; }
        public int IdProperty { get; set; }
        public int IdRoom { get; set; }
        public string Image { get; set; }
        public string MainImage { get; set; }
        public string Description { get; set; }
        public double? AverageRating { get; set; }
        public int StarRating { get; set; }
        public string Address { get; set; }
        public string RoomType { get; set; }
        public int CapacityOfGuests { get; set; }
        public decimal Price { get; set; }
        public string Amenity { get; set; }
        public int NumberOfSingleBeds { get; set; }
        public int NumberOfDoubleBeds { get; set; }
        public string DestinationName { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int NumberOfNights { get; set; }
        public int NumberOfRooms { get; set; }
        public int NumberOfGuests { get; set; }
    }
}