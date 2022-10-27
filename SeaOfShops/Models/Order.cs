﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SeaOfShops.Models
{
	public class Order
	{
		[Key]
		public int OrderId { get; set; }
		public int Price { get; set; }
		public bool Сompleted { get; set; }
		[DisplayName("List of products")]
		public List<Product> Products { get; set; }
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
