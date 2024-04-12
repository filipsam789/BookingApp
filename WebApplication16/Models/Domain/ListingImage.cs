using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication16.Models
{
    public class ListingImage
    {
        [Key]
        [Column("id_listing", Order = 0)]
        [ForeignKey("Listing")]
        public int IdListing { get; set; }

        [Key]
        [Column("image", Order = 1)]
        [MaxLength(200)]
        public string ImagePath { get; set; }

        public virtual Listing Listing { get; set; }
    }
}