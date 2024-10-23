using AutoMapper;
using KoiCareSystem.Data.Models;
using KoiCareSystem.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KoiCareSystem.RazorWebApp.Pages.AdminShop
{
    public class IndexModel : PageModel
    {
        private readonly UserService _userService;
        private readonly RoleService _roleService;

        public IndexModel(IMapper mapper)
        {
            _userService ??= new UserService(mapper);
            _roleService ??= new RoleService(mapper);
        }
        public int UserId { get; set; }
        public string Email { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            // Lấy UserId từ session
            var userIdFromSession = HttpContext.Session.GetInt32("UserId");

            if (userIdFromSession == null)
            {
                return RedirectToPage("/Guest/Login"); // Điều hướng đến trang đăng nhập nếu không có UserId trong session
            }

            // Gán giá trị cho biến cục bộ và static
            UserId = (int)userIdFromSession;

            // Lấy thông tin người dùng từ service
            var userResult = await _userService.GetById(UserId);
            if (userResult.Data is User user)
            {
                Email = user.Email;
            }
            else
            {
                return RedirectToPage("/Error"); // Chuyển hướng nếu có lỗi
            }

            return Page(); // Hiển thị trang sau khi xử lý xong
        }

    }
}
