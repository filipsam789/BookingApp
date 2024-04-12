using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication16.Models
{
    public class RateBooking
    {
        [Range(0,10)]
        public int Rating { get; set; }
        public int IdBooking { get; set; }
        public Booking Booking { get; set; }
    }
}