using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using KoiCareSystem.Common.DTOs.Request;
using Microsoft.AspNetCore.Identity.Data;
using NuGet.Protocol.Plugins;
using KoiCareSystem.Service;
using KoiCareSystem.Data.Models;
using KoiCareSystem.Service.Helper;
using Microsoft.AspNetCore.Http;

namespace KoiCareSystem.RazorWebApp.Pages.Guest
{
    public class LoginModel : PageModel
    {
        private readonly UserService _userService;
        private readonly RoleService _roleService;
        private readonly AuthenticateService _authenticateService;
        private readonly EmailService _emailService;
        private readonly IUrlHelperService _urlHelperService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LoginModel(UserService userService, RoleService roleService, AuthenticateService authenticateService, EmailService emailService, IUrlHelperService urlHelperService, IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _roleService = roleService;
            _authenticateService = authenticateService;
            _emailService = emailService;
            _urlHelperService = urlHelperService;
            _httpContextAccessor = httpContextAccessor;
        }
        //========================================================
        [BindProperty]
        public RequestLoginDto RequestLoginDto { get; set; } = new RequestLoginDto();
        //========================================================
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Kiểm tra sự tồn tại của người dùng dựa trên email
            var user = await _userService.GetByEmail(RequestLoginDto.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Không tìm thấy User");
                return Page();
            }

            // Thực hiện đăng nhập
            var result = await _authenticateService.Login(RequestLoginDto);
            if (result == null)
            {
                ModelState.AddModelError(string.Empty, "Login failed");
                return Page();
            }

            var userExist = result.Data as User;
            if (userExist == null)
            {
                ModelState.AddModelError(string.Empty, "Không lấy được User.");
                return Page();
            }

            // Nếu user chưa xác thực email
            if (!userExist.IsVerified)
            {
                await HandleEmailVerification(userExist);
                return RedirectToPage("/Guest/VerifyEmail", new { email = userExist.Email });
            }

            // Lưu thông tin vào session
            SaveUserSession(userExist);

            // Điều hướng dựa trên vai trò
            return RedirectBasedOnRole(userExist.Role.Name.ToLower());
        }

        // Gửi mã xác thực qua email
        private async Task HandleEmailVerification(User userExist)
        {
            userExist.EmailVerificationToken = Guid.NewGuid().ToString();
            await _userService.UpdateVerifyCode(userExist.Email, userExist.EmailVerificationToken);

            // Gửi email xác thực
            var verificationLink = _urlHelperService.GenerateVerificationLink(this.PageContext, userExist.EmailVerificationToken);
            await _emailService.SendVerificationEmailAsync(userExist.Email, userExist.EmailVerificationToken, verificationLink);
        }
        // Hàm lưu session
        private void SaveUserSession(User user)
        {
            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserRole", user.Role.Name.ToLower());
        }

        //Chuyển trang dựa theo Role
        private IActionResult RedirectBasedOnRole(string role)
        {
            if (role.Contains("admin"))
            {
                return RedirectToPage("/Admin/Index");
            }
            else if (role.Contains("member"))
            {
                return RedirectToPage("/Member/Index");
            }
            else if (role.Contains("shop"))
            {
                return RedirectToPage("/Shop/Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid role.");
                return Page();
            }
        }
    }
}
