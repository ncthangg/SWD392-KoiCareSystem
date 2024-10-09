using KoiCareSystem.Common.DTOs.Request;
using KoiCareSystem.Data.Models;
using KoiCareSystem.Service;
using KoiCareSystem.Service.Helper;
using KoiCareSystematHome.Service;
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
        private readonly IUrlHelperFactory _urlHelperFactory;
        public RegisterModel(UserService userService, EmailService emailService, IUrlHelperService urlHelperService, IUrlHelperFactory urlHelperFactory)
        {
            _userService = userService;
            _emailService = emailService;
            _urlHelperFactory = urlHelperFactory;
            _urlHelperService = urlHelperService;
        }

        [BindProperty]
        public RequestRegisterDto RegisterDto { get; set; } = new RequestRegisterDto(); // Khởi tạo RegisterDto

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
                var user = await _userService.Save(RegisterDto);
                if(user != null)
                {
                    var userData = user.Data as User;

                    //Mail Service
                    var verificationLink = _urlHelperService.GenerateVerificationLink(this.PageContext, userData.EmailVerifiedToken);

                    // Tiếp tục logic gửi email
                    await _emailService.SendVerificationEmailAsync(userData.Email, verificationLink);
                }

                return RedirectToPage("/Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}
