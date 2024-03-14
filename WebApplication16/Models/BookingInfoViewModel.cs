using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication16.Models
{
    public class BookingInfoViewModel
    {
        public int IdBooking { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Check In Date")]
        public DateTime CheckInDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name ="Check Out Date")]
        public DateTime CheckOutDate { get; set; }
        [Display(Name = "Number Of Guests")]
        public int NumberOfGuests { get; set; }
        [Display(Name = "Total Price")]
        public decimal TotalPrice { get; set; }
        [Display(Name = "Rating")]
        public int? Rating { get; set; }
        [Display(Name = "Room Type")]
        public string RoomType { get; set; }
        public List<string> PhoneNumbers { get; set; }
        public Property Property { get; set; }
        public Traveller Traveller { get; set; }
        public Listing Listing { get; set; }
        public Room Room { get; set; }
    }
}