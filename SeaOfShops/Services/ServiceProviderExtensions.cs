using Microsoft.AspNetCore.Cors.Infrastructure;
using SeaOfShops.Domain.Entities;
using SeaOfShops.Domain.Interfaces;
using SeaOfShops.Domain.Interfaces.Services;
using SeaOfShops.Infrastucture;
using SeaOfShops.Models;
using SeaOfShops.Service;

namespace SeaOfShops.Services
{
    public static class ServiceProviderExtensions
    {
        public static void AddTimeService(this IServiceCollection services)
        {
            services.AddTransient<TimeService>();
        }
        public static void AddOrderService(this IServiceCollection services)
        {
            services.AddScoped<IOrderItemService<Order>, OrderItemService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<DbFactory>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
