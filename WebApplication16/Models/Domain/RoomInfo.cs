using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication16.Models
{
    public class RoomInfo
    {
        public int IdRoom { get; set; }
        [Display(Name ="Room Type")]
        public string RoomType { get; set; }
        [Display(Name = "Capacity Of Guests")]
        public int CapacityOfGuests { get; set; }
        [Display(Name = "Price")]
        public decimal Price { get; set; }
        [Display(Name = "Number Of Single Beds")]
        public int NumberOfSingleBeds { get; set; }
        [Display(Name = "Number Of Double Beds")]
        public int NumberOfDoubleBeds { get; set; }
        public List<RoomAmenity> Amenities { get; set; }
        public RoomInfo() { 
            Amenities = new List<RoomAmenity>();
        }
    }
}