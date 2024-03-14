using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System.Security.Claims;

namespace WebApplication16.Models
{
    public class User:IUser<string>
    {
        [Key]
        [MaxLength(200)]
        [Column("id_user")]
        public string Id { get; set; }
        [MaxLength(50)]
        [Column("name")]
        public string Name { get; set; }
        [MaxLength(50)]
        [Column("surname")]
        public string Surname { get; set; }
        [MaxLength(30)]
        [Column("username")]
        [Required]
        public string UserName { get; set; }
        [MaxLength(150)]
        [Column("password")]
        [Required]
        public string PasswordHash { get; set; }
        [MaxLength(100)]
        [Column("email")]
        [Required]
        public string Email { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(ApplicationUserManager manager)
        {
            // Note: This example assumes that you have a UserManager with the name ApplicationUserManager

            // Create a claims identity
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            // Add custom claims based on your user properties
            //userIdentity.AddClaim(new Claim("FullName", $"{FirstName} {LastName}"));
            userIdentity.AddClaim(new Claim("Email", Email));

            // Add additional claims as needed...

            return userIdentity;
        }
    }
}