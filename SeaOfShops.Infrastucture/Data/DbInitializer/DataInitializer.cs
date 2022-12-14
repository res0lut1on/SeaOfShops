using Microsoft.AspNetCore.Identity;
using SeaOfShops.Domain.Entities;
using SeaOfShops.Infrastucture;

namespace SeaOfShops.DbInitializer
{
    public class DataInitializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ApplicationContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            // Add Roles

            if (await roleManager.FindByNameAsync("courier") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("courier"));
            }

            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }

            // Add users

            string adminEmail = "admin@gmail.com";
            string adminPassword = "123123";

            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                User admin = new User 
                { 
                    Email = adminEmail, 
                    UserName = adminEmail, 
                    ImageName = "Ainsley.jpg", 
                    RealName = "Admin" 
                };
                IdentityResult result = await userManager.CreateAsync(admin, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }

            string courierEmail = "courier@gmail.com";
            string courierPassword = "123123";

            if (await userManager.FindByNameAsync(courierEmail) == null)
            {
                User courier = new User
                {
                    Email = courierEmail,
                    UserName = courierEmail,
                    ImageName = "avatarochka.jpg",
                    RealName = "Courier"
                };
                IdentityResult result = await userManager.CreateAsync(courier, courierPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(courier, "courier");
                }
            }

            string godEmail = "god@heaven.com";
            string godPassword = "666666";

            if (await userManager.FindByNameAsync(godEmail) == null)
            {
                User god = new User
                {
                    Email = godEmail,
                    UserName = godEmail,
                    RealName = "God"
                };
                IdentityResult result = await userManager.CreateAsync(god, godPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(god, "admin");
                    await userManager.AddToRoleAsync(god, "courier");
                }
            }

            // typical user

            string FriedrichEmail = "toffee_lover@super.man";
            string FriedrichPassword = "314314";

            if (await userManager.FindByNameAsync(FriedrichEmail) == null)
            {
                User fr = new User
                {
                    Email = FriedrichEmail,
                    UserName = FriedrichEmail,
                    RealName = "Friedrich",
                    UserAddress = "San Francisco, City in style Disko, CA 94080",
                };
                IdentityResult result = await userManager.CreateAsync(fr, FriedrichPassword);
            }

            string SashkaEmail = "SashaPetrov@gmail.com";
            string SashkaPassword = "112358";

            if (await userManager.FindByNameAsync(SashkaEmail) == null)
            {
                User fr = new User
                {
                    Email = SashkaEmail,
                    UserName = SashkaEmail,
                    RealName = "Sashka",
                    UserAddress = "California, 7760 Oak St.Fountain Valley, CA 92708",
                };
                IdentityResult result = await userManager.CreateAsync(fr, SashkaPassword);
            }

            string RomchikEmail = "soulkitchen@roma.net";
            string RomchikPassword = "112358";

            if (await userManager.FindByNameAsync(RomchikEmail) == null)
            {
                User fr = new User
                {
                    Email = RomchikEmail,
                    UserName = RomchikEmail,
                    RealName = "Roman",
                    UserAddress = "93 Creek Avenue Commack, NY 11725",
                };
                IdentityResult result = await userManager.CreateAsync(fr, RomchikPassword);
            }

            // Add Shops

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

            // Add Products

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
                ProductName = "Flour",
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
            Product product6 = new Product()
            {
                ProductName = "le Big-Mac",
                Color = "White",
                Price = 3,
                Shop = kfc
            };
            Product product7 = new Product()
            {
                ProductName = "MilkShake Martin Lewis ",
                Color = "Red",
                Price = 5,
                Shop = kfc
            };
            Product product8 = new Product()
            {
                ProductName = "Son of Jame",
                Color = "Green",
                Price = 30,
                Shop = lequint
            };
            Product product9 = new Product()
            {
                ProductName = "Big Kahuna Burger",
                Color = "Wheat",
                Price = 10,
                Shop = lequint
            };

            context.Products.AddRange(product1, product2, product3, product4, product5, product6, product7, product8, product9);

            // Add Orders

            var productList = new List<Product>()
                {
                    product1, product4
                };
            Order order1 = new Order()
            {
                Products = productList,
                Price = productList.Sum(p => p.Price),
                Сompleted = false,
                Owner = context.Users.FirstOrDefault(p => p.Email == RomchikEmail)
            };

            productList = new List<Product>()
                {
                    product2, product7, product9
                };
            Order order2 = new Order()
            {
                Products = productList,
                Price = productList.Sum(p => p.Price),
                Сompleted = false,
                Owner = context.Users.FirstOrDefault(p => p.Email == RomchikEmail)

            };

            productList = new List<Product>()
                {
                    product1, product4, product5
                };
            Order order3 = new Order()
            {
                Products = productList,
                Price = productList.Sum(p => p.Price),
                Сompleted = true,
                Owner = context.Users.FirstOrDefault(p => p.Email == SashkaEmail)
            };

            productList = new List<Product>()
                {
                    product4, product4, product4, product4, product4
                };
            Order order4 = new Order()
            {
                Products = productList,
                Price = productList.Sum(p => p.Price),
                Сompleted = false,
                Owner = context.Users.FirstOrDefault(p => p.Email == FriedrichEmail)
            };

            productList = new List<Product>()
                {
                    product2, product1, product5, product1, product4
                };
            Order order5 = new Order()
            {
                Products = productList,
                Price = productList.Sum(p => p.Price),
                Сompleted = true,
                Owner = context.Users.FirstOrDefault(p => p.Email == SashkaEmail)
            };

            context.Orders.AddRange(order1, order2, order3, order4, order5);
            context.SaveChanges();
        }
    }   
}
