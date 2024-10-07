using KoiCareSystem.Common.AutoMapper;
using KoiCareSystem.Data;
using KoiCareSystem.Data.DBContext;
using KoiCareSystem.Data.Models;
using KoiCareSystem.Service;
using KoiCareSystematHome.Service;
using Microsoft.AspNetCore.Identity;

namespace De
{
    public static class ConfigureServices
    {
        public static IServiceCollection ConfigureApiServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddScoped<CategoryService>();
            services.AddScoped<ProductService>();
            services.AddScoped<OrderService>();
            services.AddScoped<UserService>();
            //services.AddScoped<FA24_SE1702_PRN221_G5_KoiCareSystematHomeContext>();

            services.AddAutoMapper(typeof(MappingProfile));


            return services;
        }
    }
}
