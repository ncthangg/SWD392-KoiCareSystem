using KoiCareSystem.Service.AutoMapper;
using KoiCareSystem.Service;
using KoiCareSystem.Service.Helper;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Server.Kestrel.Core;

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

            //services.AddSession(options =>
            //{
            //    options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian hết hạn của session
            //    options.Cookie.HttpOnly = true;
            //    options.Cookie.IsEssential = true;
            //});
            services.Configure<KestrelServerOptions>(options =>
            {
                options.Limits.MaxRequestHeadersTotalSize = 64 * 1024; // Ví dụ: 64KB
            });

            var durationInMinutes = configuration["JwtSettings:Audience"]; // Lấy DurationInMinutes từ appsettings


            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["JwtSettings:Issuer"],
                    ValidAudience = configuration["JwtSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:SecretKey"]))
                };
            });


            return services;
        }
    }
}
