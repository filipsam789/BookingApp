using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication16.Models
{
    public class AddNewFacilityViewModel
    {
        public string Facility { get; set; }
        public int IdProperty { get; set; }
        public Property Property { get; set; }
    }
}