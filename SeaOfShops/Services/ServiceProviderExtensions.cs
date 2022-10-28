using SeaOfShops.Models;

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
        }
    }
}
