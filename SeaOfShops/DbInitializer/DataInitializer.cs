using Microsoft.AspNetCore.Identity;
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
                User admin = new User { Email = adminEmail, UserName = adminEmail, ImageName = "niko.jpg", RealName = "Admin" };
                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }
        }
    }   
}
