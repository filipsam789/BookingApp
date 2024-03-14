using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication16.Models
{
    public class RoomAmenity
    {
        [Key]
        [Column("id_room", Order = 0)]
        [ForeignKey("Room")]
        public int IdRoom { get; set; }

        [Key]
        [Column("amenity", Order = 1)]
        [MaxLength(100)]
        public string Amenity { get; set; }

        public virtual Room Room { get; set; }
    }
}