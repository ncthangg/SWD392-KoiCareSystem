using KoiCareSystem.Data.DBContext;
using KoiCareSystem.RazorWebApp.Middleware;

namespace KoiCareSystem.RazorWebApp
{
    public class Program
    {

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages().AddJsonOptions(options =>
            {
                // Tuỳ chỉnh cài đặt JSON nếu cần, ví dụ: đặt PropertyNamingPolicy
                options.JsonSerializerOptions.PropertyNamingPolicy = null; // Mặc định là camelCase
            });

            builder.Services.ConfigureApiServices(builder.Configuration);

            builder.Services.AddScoped<ApplicationDbContext>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment()) 
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseSession();
            app.UseMiddleware<RoleAuthorizationMiddleware>();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}
