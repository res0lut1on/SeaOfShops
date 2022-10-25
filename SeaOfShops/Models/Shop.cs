using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SeaOfShops.Models
{
	public class Shop
	{
        [Key]
        public int ShopId { get; set; }
        [DisplayName("Shop Name")]
        [Column(TypeName = "nvarchar(50)")]
        [Required(ErrorMessage = "Name is required.")]
        public string ShopName { get; set; }
        [DisplayName("Store Address")]
        [Column(TypeName = "nvarchar(50)")]
        public string StoreAddress { get; set; }
        public List<Product> Products { get; set; } = new();
        public string UserId { get; set; }
        public User? User { get; set; }
    }
}
