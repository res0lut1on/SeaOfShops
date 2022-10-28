using Microsoft.EntityFrameworkCore.ChangeTracking;
using SeaOfShops.Models;

namespace SeaOfShops.Services
{
    public interface IOrderItemService<T>
    {
        public Task<List<T>> GetAllItemsAsync();
        public Task<List<T>> GetIncompleteItemsAsync();
        public Task<List<T>> GetCompleteItemsAsync();
        public Task<T> GetByIdItemsAsync(int id);
        public Task<T> GetByIdWithoutIncludeAsync(int id);
        public Task<List<T>> GetSortedItemsAsync(Func<T, bool>? filter = null);
        public Task<EntityEntry> AddItemsAsync(Order order); 

    }
}
