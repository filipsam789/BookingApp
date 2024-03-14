using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication16.Models
{
    public class Property
    {
        [Key]
        [Column("id_property")]
        public int Id { get; set; }

        [Column("name")]
        [MaxLength(100)]
        [Required]
        [Display(Name = "Property Name")]
        public string Name { get; set; }

        [Column("star_rating")]
        [Range(1, 6, ErrorMessage = "Value must be greater than 0 and smaller than or equal to 5")]
        [Display(Name = "Stars")]
        public int NumberOfStars { get; set; }
        [Column("id_destination")]
        [Required]
        public int IdDestination { get; set; }
        [Column("id_user")]
        
        public string IdOwner { get; set; }
        [ForeignKey("IdOwner")]
        public virtual PropertyOwner PropertyOwner { get; set; }
        [ForeignKey("IdDestination")]
        public virtual Destination Destination { get; set; }
        [Column("address")]
        [MaxLength(200)]
        [Required]
        public string Address { get; set; }
        public virtual List<PropertyFacility> Facilities { get; set; }
        public virtual List<PropertyPhoneNumber> PhoneNumbers { get; set; }
        public Property()
        {
            Facilities = new List<PropertyFacility>();
            PhoneNumbers = new List<PropertyPhoneNumber>();
        }
    }
}