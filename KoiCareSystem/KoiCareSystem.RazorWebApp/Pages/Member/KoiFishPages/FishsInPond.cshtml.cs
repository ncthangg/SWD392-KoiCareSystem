using KoiCareSystem.Common.DTOs;
using KoiCareSystem.Data.Models;
using KoiCareSystem.RazorWebApp.PageBase;
using KoiCareSystem.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KoiCareSystem.RazorWebApp.Pages.Member.KoiFishPages
{
    public class FishsInPondModel : BasePageModel
    {

        private readonly KoiFishService _koiFishService;
        private readonly PondService _pondService;
        public FishsInPondModel(KoiFishService koiFishService, PondService pondService)
        {
            _koiFishService = koiFishService;
            _pondService = pondService;
        }
        //========================================================
        public IList<KoiFish> KoiFish { get; set; } = default!;
        public string PondName { get; set; }
        //========================================================
        public async Task<IActionResult> OnGetAsync(int id)
        {
            LoadUserIdFromSession();

            if (UserId == null)
            {
                return RedirectToPage("/Guest/Login"); // Điều hướng đến trang đăng nhập nếu không có UserId trong session
            }
            KoiFish = (await _koiFishService.GetByPondId(id)).Data as IList<KoiFish>;
            var pond = (await _pondService.GetById(id)).Data as Pond;
            PondName = pond.PondName;
            return Page();
        }

    }
}
