using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication16.Models
{
    public class Booking
    {
        [Key]
        [Column("id_booking")]
        public int Id { get; set; }

        [Column("rating")]
        [Range(0, 11, ErrorMessage = "Value must be greater than or equal to 0 and smaller or equal to 10")]
        public int? Rating { get; set; }
        [Column("discount")]
        [Required]
        [Range(0, 101, ErrorMessage = "Value must be greater than or equal to 0 and smaller than or equal to 100")]
        [Display(Name ="Discount Percentage")]
        public int DiscountPercent { get; set; }

        [Column("check_in_date")]
        [Required]
        [Display(Name = "Check in date")]
        public DateTime CheckInDate { get; set; }
        [Column("check_out_date")]
        [Required]
        [Display(Name = "Check out date")]
        public DateTime CheckOutDate { get; set; }
        
        [Column("id_user")]
        [Required]
        [MaxLength(200)]
        public string IdTraveller { get; set; }
        [ForeignKey("IdTraveller")]
        public virtual Traveller Traveller { get; set; }
        [Column("id_listing")]
        [Required]
        public int IdListing { get; set; }
        [ForeignKey("IdListing")]
        public virtual Listing Listing { get; set; }
        [Column("number_of_guests")]
        [Required]
        [Display(Name = "Number of guests")]
        public int NumberOfGuests { get; set; }
        [Column("id_room")]
        [Required]
        public int IdRoom { get; set; }
        [ForeignKey("IdRoom")]
        public virtual Room Room { get; set; }
    }
}