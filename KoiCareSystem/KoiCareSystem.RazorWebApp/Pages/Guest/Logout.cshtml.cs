using KoiCareSystem.Service.Helper;
using KoiCareSystem.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KoiCareSystem.RazorWebApp.Pages.Guest
{
    public class LogoutModel : PageModel
    {
        private readonly AuthenticateService _authenticateService;
        public LogoutModel( AuthenticateService authenticateService)
        {
            _authenticateService = authenticateService;
        }
        public async Task<IActionResult> OnPost()
        {
            // Kiểm tra xem session có tồn tại không
            if (HttpContext.Session.IsAvailable)
            {
                _authenticateService.Logout();
            }

            // Redirect về trang đăng nhập
            return RedirectToPage("/Guest/Login");
        }
    }
}
