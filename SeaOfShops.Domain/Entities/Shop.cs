using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace SeaOfShops.Domain.Entities
{
	public class Shop : EntityBase<int>
    {
        [Key]
        public int ShopId { get; set; }

        [DisplayName("Shop Name")]
        [Column(TypeName = "nvarchar(50)")]
        [Required(ErrorMessage = "Name is required.")]
        public string ShopName { get; set; }

        [DisplayName("Store Address")]
        [Column(TypeName = "nvarchar(50)")]
        [Required(ErrorMessage = "Store address is required.")]
        public string StoreAddress { get; set; }
        public List<Product> Products { get; set; } = new();
        public string UserId { get; set; }
        [DisplayName("Store Administrator")]
        public User User { get; set; }
    }
}
