using KoiCareSystem.Common;
using KoiCareSystem.Common.DTOs.Request;
using KoiCareSystem.Data;
using KoiCareSystem.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace KoiCareSystem.RazorWebApp.Pages.Guest
{
    public class VerifyEmailModel : PageModel
    {
        private readonly UserService _userService;
        private readonly RoleService _roleService;
        private readonly AuthenticateService _authenticateService;

        public VerifyEmailModel(UserService userService, RoleService roleService, AuthenticateService authenticateService)
        {
            _userService = userService;
            _roleService = roleService;
            _authenticateService = authenticateService;

        }
        [BindProperty]
        public RequestVerifyEmailDto RequestVerifyEmailDto { get; set; }
        public void OnGet(string email)
        {
            if (RequestVerifyEmailDto == null)
            {
                RequestVerifyEmailDto = new RequestVerifyEmailDto();
            }

            RequestVerifyEmailDto.Email = email;
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var user = await _userService.GetUserByEmail(RequestVerifyEmailDto.Email);
            if (user == null)
            {
                return NotFound();
            }

            // Kiểm tra mã xác minh
            var result = _userService.VerifyCode(RequestVerifyEmailDto);
            if (result.Result.Status == Const.SUCCESS_UPDATE_CODE)
            {
                ModelState.AddModelError(string.Empty, "Thành công.");
                return Page();
            }
            if (result.Result.Status == Const.FAIL_UPDATE_CODE)
            {
                ModelState.AddModelError(string.Empty, "Sai mã xác minh.");
                return Page();
            }
            if (result.Result.Status == Const.ERROR_INVALID_DATA)
            {
                ModelState.AddModelError(string.Empty, "Sai thông tin hoặc Đã xác minh");
                return Page();
            }


            return RedirectToPage("/Guest/Login");
        }
    }
}
