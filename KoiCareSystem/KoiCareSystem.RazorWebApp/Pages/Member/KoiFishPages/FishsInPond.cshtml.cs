using KoiCareSystem.Common.DTOs;
using KoiCareSystem.Data.Models;
using KoiCareSystem.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KoiCareSystem.RazorWebApp.Pages.Member.KoiFishPages
{
    public class FishsInPondModel : PageModel
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
        public int UserId { get; set; }
        //========================================================
        public async Task OnGetAsync(int id)
        {
            UserId = (int)HttpContext.Session.GetInt32("UserId");
            KoiFish = (await _koiFishService.GetByPondId(id)).Data as IList<KoiFish>;
            var pond = (await _pondService.GetById(id)).Data as Pond;
            PondName = pond.PondName;
        }

    }
}
