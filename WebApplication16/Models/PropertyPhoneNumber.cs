using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication16.Models
{
    public class PropertyPhoneNumber
    {
        [Key]
        [Column("id_property", Order = 0)]
        [ForeignKey("Property")]
        public int IdProperty { get; set; }

        [Key]
        [Column("phone_number", Order = 1)]
        [MaxLength(100)]
        public string PhoneNumber { get; set; }

        public virtual Property Property { get; set; }
    }
}