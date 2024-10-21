using KoiCareSystem.Common.DTOs.Request;
using KoiCareSystem.Data.Models;
using KoiCareSystem.Service;
using KoiCareSystem.Service.Helper;
using KoiCareSystem.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Routing;

namespace KoiCareSystem.RazorWebApp.Pages.Guest
{
    public class RegisterModel : PageModel
    {
        private readonly UserService _userService;
        private readonly EmailService _emailService;
        private readonly IUrlHelperService _urlHelperService;
        public RegisterModel(UserService userService, EmailService emailService, IUrlHelperService urlHelperService)
        {
            _userService = userService;
            _emailService = emailService;
            _urlHelperService = urlHelperService;
        }
        //========================================================
        [BindProperty]
        public RequestRegisterDto RegisterDto { get; set; } = new RequestRegisterDto(); // Khởi tạo RegisterDto

        //========================================================

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                // 
                var userExist = _userService.UserEmailExists(RegisterDto.Email);
                if (!userExist)
                {
                    var user = await _userService.Save(RegisterDto);
                    var userData = (User)user.Data;

                    //Mail Service
                    var verificationCode = userData.EmailVerificationToken;
                    var verificationLink = _urlHelperService.GenerateVerificationLink(this.PageContext, verificationCode);

                    // Tiếp tục logic gửi email
                    await _emailService.SendVerificationEmailAsync(userData.Email, verificationCode, verificationLink);

                    return RedirectToPage("/Guest/VerifyEmail", new { email = userData.Email });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, $"Tài khoản đã tồn tại");
                    return Page();
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }

    }
}
