using SeaOfShops.Domain.Entities;
using System.Collections;

namespace SeaOfShops.Domain.Interfaces.Services
{
    public interface IProductService
    {
        Task<IList<Product>> GetAll();
        Task<Product> GetOne(int productId);
        Task Update(Product product);
        Task Add(Product product);
        Task Delete(int productId);
    }
}
