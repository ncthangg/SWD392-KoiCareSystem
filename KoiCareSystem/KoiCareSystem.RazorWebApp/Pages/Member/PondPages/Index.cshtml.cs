using KoiCareSystem.Data.Models;
using KoiCareSystem.Service;
using KoiCareSystem.RazorWebApp.PageBase;
using Microsoft.AspNetCore.Mvc;

namespace KoiCareSystem.RazorWebApp.Pages.Member.PondPages
{
    public class IndexModel : BasePageModel
    {
        private readonly KoiFishService _koiFishService;
        private readonly PondService _pondService;
        public IndexModel(KoiFishService koiFishService, PondService pondService)
        {
            _koiFishService = koiFishService;
            _pondService = pondService;
        }
        //========================================================
        public IList<Pond> Pond { get; set; } = default!;
        //========================================================
        public async Task<IActionResult> OnGetAsync()
        {
            LoadUserIdFromSession();

            if (UserId == null)
            {
                return RedirectToPage("/Guest/Login"); // Điều hướng đến trang đăng nhập nếu không có UserId trong session
            }
            Pond = (await _pondService.GetByUserId((int)UserId)).Data as IList<Pond>;
            return Page();
        }

    }
}
