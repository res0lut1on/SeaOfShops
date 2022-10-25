using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace SeaOfShops.Models
{
    public class User : IdentityUser
    {
        public string? RealName { get; set; }
        public string? LastName { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string? ImageName { get; set; }
    }
}
