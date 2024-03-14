using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication16.Models
{
    public class ListingInfoViewModel
    {
        public int IdListing { get; set; }
        public int IdProperty { get; set; }
        public string ListingName { get; set; }
        public string MainImage { get; set; }
        public string Description { get; set; }
        public double? AverageRating { get; set; }
        public int StarRating { get; set; }
        public string Address { get; set; }
        [Display(Name = "Destination Name")]
        public string DestinationName { get; set; }
        [Display(Name = "Check In Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CheckInDate { get; set; }
        [Display(Name = "Check Out Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CheckOutDate { get; set; }
        [Display(Name = "Number Of Nights")]
        public int NumberOfNights { get; set; }
        [Display(Name = "Number Of Rooms")]
        public int NumberOfRooms { get; set; }
        [Display(Name = "Number Of Guests")]
        public int NumberOfGuests { get; set; }
        public virtual List<RoomInfo> RoomInfos { get; set; }
        public virtual List<string> Images { get; set; }
        public ListingInfoViewModel()
        {
            RoomInfos = new List<RoomInfo>();
            Images = new List<string>();
        }
    }
}