
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
    [Authorize(Roles = "admin")]
    public class ProductController : Controller
    {
        private readonly ApplicationContext _context;
        IOrderItemService<Order> _orderItemService;
        private IMemoryCache cache;
        public ProductController(ApplicationContext context, IMemoryCache memoryCache, IOrderItemService<Order> orderItemService)
        {
            _context = context;
            _orderItemService = orderItemService;
            cache = memoryCache;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var applicationContext = _context.Products.Include(p => p.Shop);
            return View(await applicationContext.ToListAsync());
        }

        // GET: Products/Details/5        
        [ServiceFilter(typeof(ValidateEntityExistsAttribute<Product>))]
        public IActionResult Details(int? id)
        {
            Product? product = null;             
            product = HttpContext.Items["entity"] as Product;
            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["ShopId"] = new SelectList(_context.Shops, "ShopId", "ShopName");
            return View();
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProductName,Color,IsDeleted,Price,ShopId")] Product product)
        {
            _context.Add(product);
            int n = await _context.SaveChangesAsync();
            if(n > 0)
            {
                cache.Set(product.Id, product, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                });
            }
            return RedirectToAction(nameof(Index));                               
        }
        [ServiceFilter(typeof(ValidateEntityExistsAttribute<Product>))]
        public IActionResult Edit(int? id)
        {
            var product = HttpContext.Items["entity"] as Product;
            ViewData["ShopId"] = new SelectList(_context.Shops, "ShopId", "ShopName", product.ShopId);
            return View(product);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductName,Color,IsDeleted,Price,ShopId")] Product product)
        {
            try
            {
                _context.Update(product);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(product.Id))
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
        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }

        // GET: Products/Delete/5
        [ServiceFilter(typeof(ValidateEntityExistsAttribute<Product>))]
        public IActionResult Delete(int? id)
        {
            var product = HttpContext.Items["entity"] as Product;
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ServiceFilter(typeof(ValidateEntityExistsAttribute<Product>))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = HttpContext.Items["entity"] as Product;

            if (product != null)
            {                
                var orders = await _orderItemService.GetAllItemsAsync();
                if (orders.FirstOrDefault(p => p.Products.FirstOrDefault(c => c.Id == product.Id) is not null) is not null) // если продукт есть хотя бы в одном заказе
                {
                    //product.IsDeleted = true; 
                    var a = _context.Products.FirstOrDefault(p => p.Id == product.Id);                                      // не знаю, как решить проблему с отслеживанием в EntityFramework, кроме NoTracking()
                    a.IsDeleted = true;                                                                                     // помечаю, что не нужно выводить его в главном списке
                    _context.Products.Update(a);
                    ViewBag.CountProducts =  _context.Products.Count() - 1;
                }
                else
                {
                    _context.Products.Remove(product);                                                                      // иначе удаляю
                }
            }            
            _context.SaveChanges();
            
            return RedirectToAction(nameof(Index));
        }

        //
        //
        //
        //
        //
        // 
        //
        // Вариант CRUD с одной страницей
        // GET: Transaction/AddOrEdit
        public IActionResult AddOrEdit(int id = 0)
        {
            PopulateShops();
            if (id == 0)
                return View(new Product());
            return View(_context.Products.Find(id));
        }


        // POST: Transaction/AddOrEdit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("TransactionId,CategoryId,Amount,Note,Date")] Product product)
        {
            if (ModelState.IsValid)
            {
                if (product.Id == 0)
                    _context.Add(product);
                else
                    _context.Update(product);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateShops();
            return View(product);
        }        


        [NonAction]
        public void PopulateShops() // вариант SelectList со значением по умолчанию
        {
            var ShopCollection = _context.Shops.ToList();
            Shop DefaultCategory = new Shop() { ShopId = 0, ShopName = "Choose a Shops" };
            ShopCollection.Insert(0, DefaultCategory);
            ViewBag.Shops = ShopCollection;
            ViewData["ShopId"] = new SelectList(ShopCollection, "ShopId", "ShopName", DefaultCategory);
        }
        
        // GET: mini pagination 
        /*public async Task<IActionResult> Index(int pageNo)
        {
            int pageSize = 6;
            var products = await _context.Products.Include(t => t.Shop).ToListAsync();

            ViewBag.CountProducts = Math.Ceiling(((double)products.Count() / (double)pageSize));

            var collect = products.Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

            return View(collect);
        }*/
    }
}
