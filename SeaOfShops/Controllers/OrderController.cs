using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SeaOfShops.Data;
using SeaOfShops.Filters;
using SeaOfShops.Models;
using SeaOfShops.Services;

namespace SeaOfShops.Controllers
{
    [Authorize(Roles = "admin" + "," + "courier")]
    public class OrderController : Controller
    {
        private readonly ApplicationContext _context;
        private IMemoryCache cache;
        private readonly IOrderItemService<Order> _orderItemService;
        private static bool _flagForCangeCache = false;
        public OrderController(ApplicationContext context, IOrderItemService<Order> orderItemService, IMemoryCache memoryCache)
        {
            cache = memoryCache;
            _context = context;
            _orderItemService = orderItemService;
        }
        
        // GET: Order
        public async Task<IActionResult> Index()
        {
            string id = "all";
            List<Order>? orders = null;
            if (!cache.TryGetValue(id, out orders) || _flagForCangeCache == true)
            {
                orders = await _orderItemService.GetAllItemsAsync() ?? throw new ArgumentNullException(nameof(orders));

                if (orders != null)
                {
                    cache.Set(id, orders,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                    _flagForCangeCache = false;
                }
                else
                {
                    return NotFound();
                }
            }
            //var orders = await _orderItemService.GetAllItemsAsync();
            var sortOrders = orders.OrderBy(p => p.Сompleted == true);
            return View(sortOrders);
        }

        // GET: Order/Details/5        
        [ServiceFilter(typeof(ValidateEntityExistsAttribute<Order>))]
        public IActionResult Details(int? id)
        {            
            var order = HttpContext.Items["entity"] as Order;            
            return View(order);
        }


        [Authorize(Roles = "admin")]
        [ServiceFilter(typeof(ValidateEntityExistsAttribute<Order>))]
        public async Task<IActionResult> Complete(int? id)
        {
            var order = HttpContext.Items["entity"] as Order;
            order.Сompleted = true;
            await _context.SaveChangesAsync();
            _flagForCangeCache = true;
            return RedirectToAction(nameof(Index));
        }        

        // GET: Order/Delete/5
        [Authorize(Roles = "admin")]
        [ServiceFilter(typeof(ValidateEntityExistsAttribute<Order>))]
        public IActionResult Delete(int? id)
        {
            var order = HttpContext.Items["entity"] as Order;
            return View(order);
        }

        // POST: Order/Delete/5
        [Authorize(Roles = "admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Orders == null)
            {
                return Problem("Entity set 'ApplicationContext.Orders'  is null.");
            }           
            var order = await _orderItemService.GetByIdWithoutIncludeAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }            
            await _context.SaveChangesAsync();
            _flagForCangeCache = true;
            return RedirectToAction(nameof(Index));
        }        

        // GET: Order/Create
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Order/Create
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Create([Bind("Id,Price,Сompleted")] Order order)
        {
            await _orderItemService.AddItemsAsync(order);
            await _context.SaveChangesAsync();
            _flagForCangeCache = true;
            return RedirectToAction(nameof(Index));
        }
        /*[Authorize(Roles = "admin")]
                // GET: Order/Edit/5
                public async Task<IActionResult> Edit(int? id)
                {
                    if (id == null || _context.Orders == null)
                    {
                        return NotFound();
                    }

                    var order = await _context.Orders.FindAsync(id);
                    if (order == null)
                    {
                        return NotFound();
                    }
                    var _owner = await _context.Users.FindAsync(order.UserId);
                    _owner = await _context.Users.FindAsync(order.Owner);
                    await _context.Entry(order).Collection(p => p.Products).LoadAsync();
                    return View(order);
                }*/
        /*[Authorize(Roles = "admin")]
        // POST: Order/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Price,Сompleted")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }*/
    }
}
