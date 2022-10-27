using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace SeaOfShops.Models
{
    public class User : IdentityUser
    {
        [DisplayName("Name")]
        [Column(TypeName = "nvarchar(20)")]
        public string? RealName { get; set; }
        [DisplayName("Last Name")]
        [Column(TypeName = "nvarchar(25)")]
        public string? LastName { get; set; }
        [DisplayName("User Address")]
        [Column(TypeName = "nvarchar(100)")]
        public string? UserAddress { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string? ImageName { get; set; }


        [NotMapped]
        public string? FullName
        {
            get
            {
                return RealName + " " + LastName;
            }
        }
    }
}
