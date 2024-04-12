using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication16.Models
{
    public class Room
    {
        [Key]
        [Column("id_room")]
        public int Id { get; set; }
        [Column("room_type")]
        [MaxLength(100)]
        [Required]
        [Display(Name = "Room Type")]
        public string RoomType { get; set; }
        [Column("capacity_of_guests")]
        [Required]
        [Display(Name = "Capacity Of Guests")]
        public int CapacityOfGuests { get; set; }

        [Column("id_listing")]
        public int IdListing { get; set; }
        [ForeignKey("IdListing")]
        public virtual Listing Listing { get; set; }
        [Column("id_property")]
        [Required]
        public int IdProperty { get; set; }
        [ForeignKey("IdProperty")]
        public virtual Property Property { get; set; }

        [Column("num_single_beds")]
        [Required]
        [Display(Name = "Number Of Single Beds")]
        public int NumberOfSingleBeds { get; set; }
        [Column("num_double_beds")]
        [Required]
        [Display(Name = "Number Of Double Beds")]
        public int NumberOfDoubleBeds { get; set; }
        [Column("price")]
        [Required]
        public int Price { get; set; }
        [Column("room_number")]
        [Required]
        public int RoomNumber { get; set; }
        public virtual List<RoomAmenity> Amenities { get; set; }
        public Room()
        {
            Amenities=new List<RoomAmenity>();
        }
    }
}