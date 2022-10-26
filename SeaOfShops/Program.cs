using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SeaOfShops.Data;
using SeaOfShops.DbInitializer;
using SeaOfShops.Models;
using WEB_3505_MIK;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddFile(Path.Combine(Directory.GetCurrentDirectory(), "logger.txt"));

// Add services to the container.
builder.Services.AddControllersWithViews();
//
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(\"Mgo+DSMBaFt/QHJqVVhjWlpFdEBBXHxAd1p/VWJYdVt5flBPcDwsT3RfQF9jQXxRdkFjWHteeHRUTw==;Mgo+DSMBPh8sVXJ0S0R+XE9HcFRDX3xKf0x/TGpQb19xflBPallYVBYiSV9jS3xTfkdhWXtecXBVTmFeWA==;ORg4AjUWIQA/Gnt2VVhiQlFadVlJXGFWfVJpTGpQdk5xdV9DaVZUTWY/P1ZhSXxRdk1hW39acHVRRmheWU0=;NzAxNTQ5QDMyMzAyZTMyMmUzMFVtTklsZUJVd0lxUVJZYjhLS0tWblhuQnpwS2ZCTVBtQzlOTWVKdG9JbUU9;NzAxNTUwQDMyMzAyZTMyMmUzMEtjc0JSNG5zTDN5c1FMdkszd1Y1ekg5Sml5c0drTVJHbTJqNE1qWEFoL0k9;NRAiBiAaIQQuGjN/V0Z+Xk9EaFxEVmJLYVB3WmpQdldgdVRMZVVbQX9PIiBoS35RdEVrW31edHRUQmBUWUx/;NzAxNTUyQDMyMzAyZTMyMmUzMGlLem90QWEzVkpreVkwQTM2Q2EvNFRMU3ViamJFYXBheERFZmlRUEp2dXM9;NzAxNTUzQDMyMzAyZTMyMmUzME1xSi91c3NyUnBZR2VOaDhrekt4cTFTS1lYWVVGMDFvU1RmQ21uWmppN0E9;Mgo+DSMBMAY9C3t2VVhiQlFadVlJXGFWfVJpTGpQdk5xdV9DaVZUTWY/P1ZhSXxRdk1hW39acHVRRmlaVU0=;NzAxNTU1QDMyMzAyZTMyMmUzMEtjeXRSd1V5QXRLelZSejRLZHhRZVJjc1JTaVVZOTFyU0krVGtSeG9JUVE9;NzAxNTU2QDMyMzAyZTMyMmUzMG9GL3U2SG1LRmYxeFEzUmtzMWZKL3hqK1R2Z3FvTGpUVFowREpNWUlGb2c9;NzAxNTU3QDMyMzAyZTMyMmUzMGlLem90QWEzVkpreVkwQTM2Q2EvNFRMU3ViamJFYXBheERFZmlRUEp2dXM9==\");");
//

builder.Services.AddTransient<IPasswordValidator<User>,
            CustomPasswordValidator>(serv => new CustomPasswordValidator(6));

builder.Services.AddTransient<IUserValidator<User>, CustomUserValidator>();


builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationContext>();

builder.Services.AddDbContext<ApplicationContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DevConnection"));
});

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Order}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var userManager = services.GetRequiredService<UserManager<User>>();
        var rolesManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var dataManager = services.GetRequiredService<ApplicationContext>();
        await DataInitializer.InitializeAsync(userManager, rolesManager, dataManager);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

app.Run();
