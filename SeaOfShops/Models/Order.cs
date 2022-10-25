using System.ComponentModel.DataAnnotations;

namespace SeaOfShops.Models
{
	public class Order
	{
		[Key]
		public int OrderId { get; set; }
		public int Price { get; set; }
		public bool Сompleted { get; set; }
		public List<Product> Products { get; set; }
	}
}
