using KoiCareSystem.Service.AutoMapper;
using KoiCareSystem.Service;
using KoiCareSystem.Service.Helper;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc.Routing;

namespace KoiCareSystem.Api
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
            services.AddScoped<ITokenService,TokenService>();
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

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian hết hạn của session
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            return services;
        }
    }
}
