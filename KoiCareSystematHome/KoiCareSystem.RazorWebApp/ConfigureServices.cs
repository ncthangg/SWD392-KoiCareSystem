using KoiCareSystem.Data.DBContext;
using KoiCareSystem.Service;
using KoiCareSystematHome.Service;

namespace De
{
    public static class ConfigureServices
    {
        public static IServiceCollection ConfigureApiServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddScoped<CategoryService>();
            services.AddScoped<ProductService>();
            services.AddScoped<OrderService>();
            //services.AddScoped<FA24_SE1702_PRN221_G5_KoiCareSystematHomeContext>();


            return services;
        }
    }
}
