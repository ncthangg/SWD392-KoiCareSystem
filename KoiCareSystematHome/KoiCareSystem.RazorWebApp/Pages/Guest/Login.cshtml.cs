using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using KoiCareSystem.Common.DTOs.Request;
using Microsoft.AspNetCore.Identity.Data;
using NuGet.Protocol.Plugins;
using KoiCareSystem.Service;
using KoiCareSystem.Data.Models;

namespace KoiCareSystem.RazorWebApp.Pages.Guest
{
    public class LoginModel : PageModel
    {
        private readonly UserService _userService;
        private readonly RoleService _roleService;
        private readonly AuthenticateService _authenticateService;
        
        public LoginModel(UserService userService, RoleService roleService, AuthenticateService authenticateService)
        {
            _userService = userService;
            _roleService = roleService;
            _authenticateService = authenticateService;
            
        }

        //[BindProperty]
        //public InputModel Input { get; set; }

        //public class InputModel
        //{
        //    [Required]
        //    public string Username { get; set; }

        //    [Required]
        //    [DataType(DataType.Password)]
        //    public string Password { get; set; }

        //    public bool RememberMe { get; set; }
        //}
        [BindProperty]
        public RequestLoginDto RequestLoginDto { get; set; } = new RequestLoginDto();

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Kiểm tra sự tồn tại của người dùng dựa trên email
            var user = await _userService.GetUserByEmail(RequestLoginDto.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "User not found.");
                return Page(); // Trả về nếu không tìm thấy user
            }

            // Thực hiện đăng nhập
            var result = await _authenticateService.Login(RequestLoginDto);
            if (result == null || result.Data == null)
            {
                ModelState.AddModelError(string.Empty, $"{result.Status}: {result.Message}");
                return Page(); // Trả về nếu login thất bại
            }

            // Lấy thông tin user từ kết quả đăng nhập
            var userEntity = result.Data as User;
            if (userEntity == null)
            {
                ModelState.AddModelError(string.Empty, "User data is invalid.");
                return Page(); // Trả về nếu thông tin user không hợp lệ
            }

            // Lấy vai trò (role) của người dùng
            var roles = await _roleService.GetRoleById(userEntity.RoleId);
            if (roles == null || roles.Data == null)
            {
                ModelState.AddModelError(string.Empty, "Role not found.");
                return Page(); // Trả về nếu không tìm thấy role
            }

            // Kiểm tra thông tin role
            var roleOfAccount = roles.Data as Role;
            if (roleOfAccount == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid role data.");
                return Page(); // Trả về nếu role không hợp lệ
            }

            // Điều hướng dựa trên vai trò của người dùng
            if (roleOfAccount.Name.Contains("Admin"))
            {
                return RedirectToPage("/Admin/Index");
            }
            else if (roleOfAccount.Name.Contains("Member"))
            {
                return RedirectToPage("/Member/Index");
            }
            else if (roleOfAccount.Name.Contains("Shop"))
            {
                return RedirectToPage("/Shop/Index");
            }

            // Trường hợp không xác định được vai trò
            ModelState.AddModelError(string.Empty, "Invalid role.");
            return Page();
        }
    }
}
