using AutoMapper;
using Azure;
using KoiCareSystem.Common.DTOs;
using KoiCareSystem.Data.Models;
using KoiCareSystem.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KoiCareSystem.RazorWebApp.Pages.Member
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
        public void OnGet()
        {
            // Lấy UserId từ session
            var userIdFromSession = HttpContext.Session.GetInt32("UserId");

            if (userIdFromSession == null)
            {
                Response.Redirect("/Guest/Login");
                return; // Kết thúc hàm OnGet sau khi điều hướng
            }

            // Gán giá trị cho biến static
            UserSession.UserId = (int)userIdFromSession;
            UserId = (int)userIdFromSession;

            var user = _userService.GetById(UserId);
            if (user != null)
            {
                Email = ((user.Result.Data) as User).Email;
            }
        }
    }
}
