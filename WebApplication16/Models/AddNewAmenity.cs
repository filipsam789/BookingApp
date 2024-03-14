using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication16.Models
{
    public class AddNewAmenity
    {
        public string Amenity { get; set; }
        public int IdRoom { get; set; }
        public Room Room { get; set; }
    }
}