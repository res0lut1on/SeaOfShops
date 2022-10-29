using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SeaOfShops.Models
{
	public class Product : IEntity
	{
        [Key]
        public int Id { get; set; }

        [DisplayName("Product Name")]
        [Column(TypeName = "nvarchar(50)")]
        [Required(ErrorMessage = "Name is required.")]
        public string ProductName { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Color { get; set; }
        /// <summary>
        /// Вариант с наличием товара и удалением, ещё можно решить с помощью создания еще таблицы "Склад", где будут храниться все товары. 
        /// Соответсвенно оперировать над доступностью и количеством. 
        /// В таблице Product, хранить ссылки на заказы с many to many, 
        /// А выводить в общий список продукты с таблицы Store с _context.ProductInStores.Where( p => p.IsAvaible == true)
        /// Но так как здесь неполноценный функционал, обошелся с добавлением свойства доступности в самой таблице Product
        /// 
        /// Заказы оставил с каскадным удалением для (заказчика) пользователя
        /// </summary>
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
