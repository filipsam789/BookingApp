using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication16.Models
{
    public class Listing
    {
        [Key]
        [Column("id_listing")]
        public int Id { get; set; }
        [Column("name")]
        [MaxLength(100)]
        [Required]
        [Display(Name = "Listing name")]
        public string Name { get; set; }
        [Column ("description")]
        [MaxLength(1000)]
        [Required]
        public string Description { get; set; }

        [Column("id_user")]
        [MaxLength(200)]
        public string IdPropertyOwner { get; set; }
        [ForeignKey("IdPropertyOwner")]
        public virtual PropertyOwner PropertyOwner { get; set; }
        [Column("main_image")]
        [MaxLength(200)]
        [Required]
        [Display(Name = "Main image")]
        public string MainImageUrl { get; set; }
        public virtual List<ListingImage> Images { get; set; }
        public Listing()
        {
            Images = new List<ListingImage>();
        }
    }
}