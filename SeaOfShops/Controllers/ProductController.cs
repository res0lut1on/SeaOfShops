using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SeaOfShops.Data;
using SeaOfShops.Models;

namespace SeaOfShops.Controllers
{
    [Authorize(Roles = "admin")]
    public class ProductController : Controller
    {
        private readonly ApplicationContext _context;
        private int pageSize = 6;
        public ProductController(ApplicationContext context)
        {
            _context = context;
        }

        /*// GET: Products
        public async Task<IActionResult> Index()
        {

            var applicationContext = _context.Products.Include(p => p.Shop);
            return View(await applicationContext.ToListAsync());
        }*/

        // GET: mini pagination 
        public async Task<IActionResult> Index(int pageNo)
        {
            var products = await _context.Products.Include(t => t.Shop).ToListAsync();

            ViewBag.CountProducts = Math.Ceiling(((double)products.Count() / (double)pageSize));

            var collect = products.Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

            return View(collect);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Shop)      // Select с Магазинами
                .ThenInclude(p => p.User)  // Владельц Магазина
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["ShopId"] = new SelectList(_context.Shops, "ShopId", "ShopName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,Color,IsDeleted,Price,ShopId")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ShopId"] = new SelectList(_context.Shops, "ShopId", "ShopName", product.ShopId);
            return View(product);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["ShopId"] = new SelectList(_context.Shops, "ShopId", "ShopName", product.ShopId);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,Color,IsDeleted,Price,ShopId")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
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
            ViewData["ShopId"] = new SelectList(_context.Shops, "ShopId", "ShopName", product.ShopId);
            return View(product);
        }
        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Shop)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'ApplicationContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);

            if (product != null)
            {
                var orders = _context.Orders.Include(p => p.Products).ToList();

                if (orders.FirstOrDefault(p => p.Products.FirstOrDefault(c => c.ProductId == product.ProductId) is not null) is not null) // если продукт есть хотя бы в одном заказе
                {
                    product.IsDeleted = true; // помечаю, что не нужно выводить его в главном списке
                    ViewBag.CountProducts =  _context.Products.Count() - 1;
                }
                else
                {
                    _context.Products.Remove(product); // иначе удаляю
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

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
                if (product.ProductId == 0)
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
    }
}
