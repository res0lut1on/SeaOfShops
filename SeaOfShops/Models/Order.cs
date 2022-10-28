using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SeaOfShops.Models
{
	public class Order : IEntity
    {
		[Key]
		public int Id { get; set; }
        [Required(ErrorMessage = "Price is required.")]
        public int Price { get; set; }
        [DisplayName("Owner")]
        [Required(ErrorMessage = "Owner is required.")] 
        public User? Owner { get; set; }
        public int? UserId { get; set; }
		public bool Сompleted { get; set; }

		[DisplayName("List of products")]
        [Required(ErrorMessage = "Order cannot be empty")]
        public List<Product>? Products { get; set; }

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
