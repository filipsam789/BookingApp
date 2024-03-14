using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication16.Models
{
    public class PropertyFacility
    {

        [Key]
        [Column("id_property", Order = 0)]
        [ForeignKey("Property")]
        public int IdProperty { get; set; }

        [Key]
        [Column("facility", Order = 1)]
        [MaxLength(100)]
        public string Facility { get; set; }

        public virtual Property Property { get; set; }
    }
}