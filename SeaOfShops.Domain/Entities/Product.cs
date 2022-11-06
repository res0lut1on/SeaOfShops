using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

#nullable disable

namespace SeaOfShops.Domain.Entities
{
	public class Product : EntityBase<int>
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Product Name")]
        [Column(TypeName = "nvarchar(50)")]
        [Required(ErrorMessage = "Name is required.")]
        public string ProductName { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Color { get; set; }
        [Required]
        public bool IsDeleted { get; set; } = false;

        [Range(1, int.MaxValue, ErrorMessage = "Price should be greater than 0.")]
        public int Price { get; set; }
        public int? ShopId { get; set; }
        public Shop? Shop { get; set; }
        public List<Order>? Orders { get; set; }

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
