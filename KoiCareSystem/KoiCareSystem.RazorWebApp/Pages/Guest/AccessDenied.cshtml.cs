using KoiCareSystem.Data.Models;
using KoiCareSystem.RazorWebApp.PageBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace KoiCareSystem.RazorWebApp.Pages.Guest
{
    public class AccessDeniedModel : BasePageModel
    {
        private readonly KoiCareSystem.Data.DBContext.ApplicationDbContext _context;
        public AccessDeniedModel(KoiCareSystem.Data.DBContext.ApplicationDbContext context)
        {
            _context = context;
        }
        public User User { get; set; } = default!;
        public async Task<IActionResult> OnGetAsync()
        {
            LoadUserIdFromSession();

            if (UserId == null)
            {
                return RedirectToPage("/Guest/Login"); // Điều hướng đến trang đăng nhập nếu không có UserId trong session
            }

            var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == UserId);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                User = user;
            }
            return Page();
        }
    }
}
