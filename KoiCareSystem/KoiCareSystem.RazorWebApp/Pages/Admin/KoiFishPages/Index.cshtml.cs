using KoiCareSystem.Data.Models;
using KoiCareSystem.Service;
using KoiCareSystem.RazorWebApp.PageBase;

namespace KoiCareSystem.RazorWebApp.Pages.Admin.KoiFishPages
{
    public class IndexModel : BasePageModel
    {
        private readonly KoiFishService _koiFishService;
        public IndexModel(KoiFishService koiFishService)
        {
            _koiFishService = koiFishService;
        }
        public IList<KoiFish> KoiFish { get;set; } = default!;

        public async Task OnGetAsync()
        {
            KoiFish = (await _koiFishService.GetAll()).Data as IList<KoiFish>;
        }
    }
}
