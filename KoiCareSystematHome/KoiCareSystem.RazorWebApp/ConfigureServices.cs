using KoiCareSystem.Common.AutoMapper;
using KoiCareSystem.Data;
using KoiCareSystem.Data.Models;
using KoiCareSystem.Service;
using KoiCareSystem.Service.Helper;
using KoiCareSystematHome.Service;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc.Routing;

namespace De
{
    public static class ConfigureServices
    {
        public static IServiceCollection ConfigureApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<UserService>();
            services.AddScoped<RoleService>();
            services.AddScoped<AuthenticateService>();

            services.AddScoped<CategoryService>();
            services.AddScoped<ProductService>();

            services.AddScoped<OrderService>();
            services.AddScoped<OrderStatusService>();
            services.AddScoped<OrderItemService>();

            services.AddScoped<KoiFishService>();
            services.AddScoped<PondService>();
            services.AddScoped<WaterStatusService>();
            services.AddScoped<WaterParameterService>();
            services.AddScoped<WaterParameterLimitService>();
            //Helper
            services.AddScoped<EmailService>();
            services.AddScoped<IUrlHelperService, UrlHelperService>();
            services.AddTransient<IFileService, FileService>();

            services.AddTransient<SmtpClient>(provider =>
            {
                var smtpClient = new SmtpClient();
                smtpClient.Connect(configuration["SmtpSettings:Host"], int.Parse(configuration["SmtpSettings:Port"]), SecureSocketOptions.StartTls);
                smtpClient.Authenticate(configuration["SmtpSettings:Username"], configuration["SmtpSettings:Password"]);
                return smtpClient;
            });

            services.AddAutoMapper(typeof(MappingProfile));


            return services;
        }
    }
}
