using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication16.Models
{
    public class ListingListInfoViewModel
    {
        public string DestinationName { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CheckIn { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CheckOut { get; set; }
        public int NumberOfNights { get; set; }
        public int NumberOfRooms { get; set; }
        public int NumberOfGuests { get; set; }
        public int IdListing { get; set; }
        public string Name { get; set; }
        public string MainImage { get; set; }
        public double? AverageRating { get; set; }
        public int StarRating { get; set; }
        public int ListingPrice { get; set; }
        public string City{ get; set; }
        public string Country { get; set; }
        public int RoomCount { get; set; }
        public int CapacityOfGuests { get; set; }
        public int NumSingleBeds { get; set; }
        public int NumDoubleBeds { get; set; }
        public int selectedListingId { get; set; }
    }
}