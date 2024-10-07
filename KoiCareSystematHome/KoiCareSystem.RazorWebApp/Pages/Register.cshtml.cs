using KoiCareSystem.Common.DTOs.User;
using KoiCareSystem.Data.Models;
using KoiCareSystem.Service;
using KoiCareSystematHome.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace De.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly UserService _userService;
        public RegisterModel(UserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public RegisterDto RegisterDto { get; set; } = new RegisterDto(); // Khởi tạo RegisterDto

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
                await _userService.Save(RegisterDto);
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
