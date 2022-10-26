using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SeaOfShops.Models
{
	public class Product
	{
        [Key]
        public int ProductId { get; set; }

        [DisplayName("Product Name")]
        [Column(TypeName = "nvarchar(50)")]
        [Required(ErrorMessage = "Name is required.")]
        public string ProductName { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Color { get; set; }
        [Required]
        public bool IsDeleted { get; set; } = false;
        public int Price { get; set; }
        public int ShopId { get; set; }
        public Shop? Shop { get; set; }
        public List<Order> Orders { get; set; }

        [NotMapped]
        public string? PriceWithProc
        {
            get
            {
                return Price.ToString() + "$";
            }
        }
    }
}
