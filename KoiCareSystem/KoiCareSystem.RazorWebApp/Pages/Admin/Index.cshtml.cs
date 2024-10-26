using AutoMapper;
using KoiCareSystem.Data.Models;
using KoiCareSystem.RazorWebApp.PageBase;
using KoiCareSystem.Service;
using Microsoft.AspNetCore.Mvc;

namespace KoiCareSystem.RazorWebApp.Pages.Admin
{
    public class IndexModel : BasePageModel
    {
        private readonly UserService _userService;
        private readonly RoleService _roleService;

        public IndexModel(IMapper mapper)
        {
            _userService ??= new UserService(mapper);
            _roleService ??= new RoleService(mapper);
        }
        public string Email { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            LoadUserIdFromSession();

            if (UserId == null)
            {
                return RedirectToPage("/Guest/Login");
            }

            // Lấy thông tin người dùng từ service
            var userResult = await _userService.GetById((int)UserId);
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
