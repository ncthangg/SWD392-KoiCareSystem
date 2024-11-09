using KoiCareSystem.Data.Models;
using KoiCareSystem.Service;
using KoiCareSystem.RazorWebApp.PageBase;

namespace KoiCareSystem.RazorWebApp.Pages.Member.WaterParameters
{
    public class HistoryModel : BasePageModel
    {
        private readonly PondService _pondService;
        private readonly WaterParameterService _waterParameterService;
        public HistoryModel(PondService pondService, WaterParameterService waterParameterService)
        {
            _pondService = pondService;
            _waterParameterService = waterParameterService;
        }
        //========================================
        public IList<WaterParameter> WaterParameter { get;set; } = default!;
        public string PondName { get; set; }
        public int PondId { get; set; }
        //========================================
        public async Task OnGetAsync(int? id)
        {
            PondId = (int)id;
            var pond = (await _pondService.GetById((int)id)).Data as Pond;
            PondName = pond.PondName;

           var WaterParameterData = (await _waterParameterService.GetByPondId((int)id)).Data as IList<WaterParameter>;

            WaterParameter = WaterParameterData.OrderByDescending(wp => wp.CreatedAt).ToList();

        }
    }
}
