using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication16.Models
{
    public class PropertyOwner : User
    {
        public virtual List<Property> Properties { get; set; }
        public virtual List<Listing> Listings { get; set; }
        public PropertyOwner()
        {
            Properties = new List<Property>();
            Listings = new List<Listing>();
        }
    }
}