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

namespace KoiCareSystem.RazorWebApp.Pages.Guest
{
    public class LoginModel : PageModel
    {
        private readonly UserService _userService;
        private readonly RoleService _roleService;
        private readonly AuthenticateService _authenticateService;
        private readonly EmailService _emailService;
        private readonly IUrlHelperService _urlHelperService;

        public LoginModel(UserService userService, RoleService roleService, AuthenticateService authenticateService, EmailService emailService, IUrlHelperService urlHelperService)
        {
            _userService = userService;
            _roleService = roleService;
            _authenticateService = authenticateService;
            _emailService = emailService;
            _urlHelperService = urlHelperService;
        }

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
                ModelState.AddModelError(string.Empty, "Không tìm thấy User");
                return Page(); // Trả về nếu không tìm thấy user
            }

            // Thực hiện đăng nhập
            var result = await _authenticateService.Login(RequestLoginDto);
            if (result == null)
            {
                ModelState.AddModelError(string.Empty, $"Login fail");
                return Page(); // Trả về nếu login thất bại
            }

            var userExist = (User)result.Data;
            if (userExist != null && !userExist.IsVerified)
            {
                userExist.EmailVerificationToken = Guid.NewGuid().ToString();
                await _userService.UpdateVerifyCode(userExist.Email, userExist.EmailVerificationToken);

                //Mail Service
                var verificationCode = userExist.EmailVerificationToken;
                var verificationLink = _urlHelperService.GenerateVerificationLink(this.PageContext, verificationCode);

                // Tiếp tục logic gửi email
                await _emailService.SendVerificationEmailAsync(userExist.Email, verificationCode, verificationLink);
                
                return RedirectToPage("/Guest/VerifyEmail", new { email = userExist.Email});
            }

            // Lấy thông tin user từ kết quả đăng nhập
            if (userExist == null)
            {
                ModelState.AddModelError(string.Empty, "Không lấy được User.");
                return Page(); // Trả về nếu thông tin user không hợp lệ
            }

            // Lấy vai trò (role) của người dùng
            var roles = await _roleService.GetRoleById(userExist.RoleId);
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

            // Lưu UserId vào session sau khi đăng nhập thành công
            HttpContext.Session.SetInt32("UserId", userExist.Id);

            // Điều hướng dựa trên vai trò của người dùng
            if (roleOfAccount.Name.ToLower().Contains("admin"))
            {
                return RedirectToPage("/Admin/Index");
            }
            else if (roleOfAccount.Name.ToLower().Contains("user"))
            {
                return RedirectToPage("/Member/Index");
            }
            else if (roleOfAccount.Name.ToLower().Contains("shop"))
            {
                return RedirectToPage("/Shop/Index");
            }

            // Trường hợp không xác định được vai trò
            ModelState.AddModelError(string.Empty, "Invalid role.");
            return Page();
        }
    }
}
