using Microsoft.AspNetCore.Identity;
using SeaOfShops.Data;
using SeaOfShops.Models;

namespace SeaOfShops.DbInitializer
{
    public class DataInitializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ApplicationContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            string adminEmail = "admin@gmail.com";
            string password = "123123";

            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }

            if (await roleManager.FindByNameAsync("courier") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("courier"));
            }

            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                User admin = new User 
                { 
                    Email = adminEmail, 
                    UserName = adminEmail, 
                    ImageName = "niko.jpg", 
                    RealName = "Admin" 
                };
                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }

            Shop lequint = new Shop() 
            { 
                ShopName = "LeQuint Dickey", 
                StoreAddress = "Texas, Galle Road, Colombo 3", 
                User = userManager.FindByNameAsync(adminEmail).Result 
            };
            Shop kfc = new Shop()
            {
                ShopName = "KFC",
                StoreAddress = "CA, North Vermont, Tarantino House",
                User = userManager.FindByNameAsync(adminEmail).Result,
            };

            context.Shops.AddRange(lequint, kfc);

            Product product1 = new Product()
            {
                ProductName = "Beautiful stone",
                Color = "Yellow",
                Price =  100,
                Shop = lequint
            };
            Product product2 = new Product()
            {
                ProductName = "Nice tiny stone",
                Color = "Pink",
                Price = 85,
                Shop = lequint
            };
            Product product3 = new Product()
            {
                ProductName = "Rakam rock",
                Color = "Red",
                Price = 185,
                Shop = lequint
            };
            Product product4 = new Product()
            {
                ProductName = "Nice table",
                Color = "Black",
                Price = 82,
                Shop = kfc
            };
            Product product5 = new Product()
            {
                ProductName = "Comfortable table",
                Color = "Gray",
                Price = 156,
                Shop = kfc
            };

            context.Products.AddRange(product1, product2, product3, product4, product5);

            var productList = new List<Product>()
                {
                    product1, product4
                };

            Order order = new Order()
            {
                Products = productList,
                Price = productList.Sum(p => p.Price),
                Сompleted = false
            };
            context.Orders.Add(order);
            context.SaveChanges();

            product1.Orders.Add(order);
            product4.Orders.Add(order);

            context.SaveChanges();
        }
    }   
}
