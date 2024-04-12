using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication16.Models
{
    public class Traveller : User
    {
        [Key]
        [Column("phone_number")]
        [MaxLength(20)]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }
        virtual public List<Booking> Bookings { get; set; }
        public Traveller()
        {
            Bookings = new List<Booking>();
        }
    }
}