using KoiCareSystem.RazorWebApp.PageBase;
using KoiCareSystem.Data.Models;
using KoiCareSystem.Service;

namespace KoiCareSystem.RazorWebApp.Pages.Admin.PondPages
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

        public IList<Pond> Pond { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Pond = (await _pondService.GetAll()).Data as IList<Pond>;
        }

    }
}
