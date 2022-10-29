using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SeaOfShops.Data;
using SeaOfShops.Models;
using SeaOfShops.Services;

namespace SeaOfShops.Filters
{
    public class ValidateEntityExistsAttribute<T> : IActionFilter where T : class, IEntity
    {
        private readonly ApplicationContext _context;
        private readonly IOrderItemService<Order> _orderItemService;
        private IMemoryCache _cache;
        public ValidateEntityExistsAttribute(ApplicationContext context, IMemoryCache cache, IOrderItemService<Order> orderItemService)
        {
            _context = context;
            _cache = cache;
            _orderItemService = orderItemService;
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var id = 0;

            if (context.ActionArguments.ContainsKey("id"))
            {
                id = (int)context.ActionArguments["id"];
            }
            else
            {
                context.Result = new BadRequestObjectResult("Bad id parameter");
                return;
            }
            /*var entity = _context.Set<T>().SingleOrDefault(x => x.Id.Equals(id));
            if(entity == null)
            {
                context.Result = new NotFoundResult();
            }
            else
            {
                context.HttpContext.Items.Add("entity", entity);
            }*/

            if (typeof(T) == typeof(Product))                                       // я знаю, что можно лучше абстрагироваться с рефлексией, но не знаю как
            {
                Product? product = null;
                if (!_cache.TryGetValue(id, out product))
                {
                    product =  _context.Products
                       .Include(p => p.Shop)
                       .ThenInclude(p => p.User)
                       .FirstOrDefault(x => x.Id.Equals(id));
                    if (product is not null)
                    {
                        _cache.Set(product.Id, product,
                        new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                        context.HttpContext.Items.Add("entity", product);
                        return;
                    }
                    else
                    {
                        context.Result = new NotFoundResult();
                    }
                }
                context.HttpContext.Items.Add("entity", product);
            }

            if (typeof(T) == typeof(Order))                                       
            {
                Order? order = null;
                if (!_cache.TryGetValue(id, out order))
                {
                    order =  _orderItemService.GetByIdItemsAsync(id).Result;                                        
                    if (order is not null)
                    {   
                        _cache.Set(order.Id, order,                                                                                     // ?k?e?y?
                        new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));                                  
                        context.HttpContext.Items.Add("entity", order);
                        return;
                    }
                    else
                    {
                        context.Result = new NotFoundResult();
                    }
                }
                context.HttpContext.Items.Add("entity", order);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {   }

    }
}
