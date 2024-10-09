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


            //Helper
            services.AddScoped<EmailService>();
            services.AddScoped<IUrlHelperService, UrlHelperService>();

            services.AddTransient<SmtpClient>(provider =>
            {
                var smtpClient = new SmtpClient();
                smtpClient.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                smtpClient.Authenticate("thangncse172630@fpt.edu.vn", "mbrx fwmj lxxn vdst");
                return smtpClient;
            });

            services.AddAutoMapper(typeof(MappingProfile));


            return services;
        }
    }
}
