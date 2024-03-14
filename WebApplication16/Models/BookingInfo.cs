using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication16.Models
{
    public class BookingInfo
    {
        public string IdUser { get; set; }
        public int IdBooking { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int NumberOfGuests { get; set; }
        public int IdListing { get; set; }
        public string RoomType { get; set; }
        public decimal TotalPrice { get; set; }
        public int? Rating { get; set; }
        public string PhoneNumber { get; set; }
        public int IdProperty { get; set; }
        public int IdRoom { get; set; }
        
    }
}