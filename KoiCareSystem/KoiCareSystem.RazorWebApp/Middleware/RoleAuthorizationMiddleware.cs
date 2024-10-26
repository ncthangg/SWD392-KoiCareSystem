namespace KoiCareSystem.RazorWebApp.Middleware
{
    public class RoleAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;

        public RoleAuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.ToString().ToLower();
            var userRole = context.Session.GetString("UserRole");

            if (path.StartsWith("/admin") && userRole != "admin")
            {
                context.Response.Redirect("/Guest/AccessDenied");
                return;
            }
            if (path.StartsWith("/shop") && userRole != "shop")
            {
                context.Response.Redirect("/Guest/AccessDenied");
                return;
            }
            if (path.StartsWith("/member") && userRole != "member")
            {
                context.Response.Redirect("/Guest/AccessDenied");
                return;
            }

            await _next(context);
        }
    }

}
