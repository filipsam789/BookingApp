using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication16.Models
{

    public class Destination
    {
        [Key]
        [Column("id_destination")]
        public int Id { get; set; }

        [Column("country_name")]
        [Required]
        [Display(Name = "Country")]
        public string CountryName { get; set; }

        [Column("city_name")]
        [Required]
        [Display(Name = "City")]
        public string CityName { get; set; }
        public List<Property> Properties { get; set; }
        public Destination() {
            Properties = new List<Property>();
        }
    }
}