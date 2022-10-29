using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using SeaOfShops.Data;
using SeaOfShops.DbInitializer;
using SeaOfShops.DeflateCompressionProvider;
using SeaOfShops.Filters;
using SeaOfShops.Models;
using SeaOfShops.Services;
using System.IO.Compression;
using WEB_3505_MIK;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddFile(Path.Combine(Directory.GetCurrentDirectory(), "logger.txt"));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddTransient<IPasswordValidator<User>,
            CustomPasswordValidator>(serv => new CustomPasswordValidator(6));

builder.Services.AddTransient<IUserValidator<User>, CustomUserValidator>();
builder.Services.AddScoped<ValidationFilterAttribute>();  

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationContext>();

builder.Services.AddOrderService();
builder.Services.AddTimeService();
builder.Services.AddScoped<IEAsyncFilterAttribute>();
builder.Services.AddScoped<ValidateEntityExistsAttribute<Product>>();
builder.Services.AddScoped<ValidateEntityExistsAttribute<Order>>();

builder.Services.AddDbContext<ApplicationContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DevConnection"));
});

builder.Services.AddMemoryCache();

builder.Services.AddControllersWithViews(options =>
{
    options.CacheProfiles.Add("Caching",
        new CacheProfile()
        {
            Duration = 300
        });
    options.CacheProfiles.Add("NoCaching",
        new CacheProfile()
        {
            Location = ResponseCacheLocation.None,
            NoStore = true
        });
    options.Filters.Add(new CustomExceptionFilterAttribute());    
    options.Filters.Add(typeof(CustomExceptionFilterAttribute));  
});

builder.Services.AddResponseCompression(options => 
{
    options.EnableForHttps = true;
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[]
    {
        "image/svg+xml",
    });
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
    options.Providers.Add(new DeflateCompressionProvider());
});

builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.Optimal;    
});

builder.Services.Configure<GzipCompressionProviderOptions>(options => // если клиент не поддерживает сжатие в формат Brotli
{
    options.Level = CompressionLevel.Optimal;
});

builder.Services.AddRazorPages();

builder.Services.AddHttpContextAccessor();


var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(new StaticFileOptions()
{
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers.Add("Cache-Control", "public,max-age=600");
    }
});

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseResponseCompression();

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
