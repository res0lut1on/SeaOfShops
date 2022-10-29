using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SeaOfShops.Data;
using SeaOfShops.Models;
using System.Linq.Expressions;

namespace SeaOfShops.Services
{
    public class OrderItemService : IOrderItemService<Order>
    {
        private readonly ApplicationContext _context;

        public OrderItemService(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetAllItemsAsync()
        {
            var orders = await _context.Orders
                            .Include(p => p.Owner)
                            .Include(p => p.Products).ToListAsync();
            return orders;
        }
        
        public async Task<List<Order>> GetCompleteItemsAsync()
        {
            var items = await _context.Orders
                            .Where(x => x.Сompleted == true)
                            .ToListAsync();
            return items;
        }

        public async Task<List<Order>> GetIncompleteItemsAsync()
        {
            var items = await _context.Orders
                .Where(x => x.Сompleted == false)
                .ToListAsync();
            return items;
        }
        public Task<List<Order>> GetSortedItemsAsync(Func<Order, bool>? filter = null)
        {
            var items = _context.Orders
                .Include(p => p.Owner)
                .Include(p => p.Products)
                            .Where(filter ?? (s => true))
                            .ToList();
            return Task.FromResult(items);
        }
        public async Task<Order> GetByIdItemsAsync(int id)
        {
            var items = await _context.Orders
                .Include(p => p.Owner)
                .Include(p => p.Products)
                .FirstOrDefaultAsync(p => p.Id == id);
            return items;
        }

        public async Task<EntityEntry> AddItemsAsync(Order order)
        {
            var res = await _context.AddAsync(order);
            return res;
        }
        public async Task<Order> GetByIdWithoutIncludeAsync(int id)
        {
            var item = await _context.Orders
                .FindAsync(id);
            return item;
        }
    }
}
