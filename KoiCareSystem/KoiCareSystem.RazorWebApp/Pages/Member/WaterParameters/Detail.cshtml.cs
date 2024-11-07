using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KoiCareSystem.Data.Models;
using KoiCareSystem.Service;
using KoiCareSystem.RazorWebApp.PageBase;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KoiCareSystem.RazorWebApp.Pages.Member.WaterParameters
{
    public class DetailModel : BasePageModel
    {
        private readonly KoiCareSystem.Data.DBContext.ApplicationDbContext _context;
        private readonly PondService _pondService;
        private readonly WaterParameterService _waterParameterService;

        public DetailModel(PondService pondService, WaterParameterService waterParameterService, KoiCareSystem.Data.DBContext.ApplicationDbContext context)
        {
            _context = context;
            _pondService = pondService;
            _waterParameterService = waterParameterService;
        }
        //========================================
        public WaterParameter WaterParameter { get; set; } = default!;
        public Dictionary<string, string> Evaluations { get; set; } = new Dictionary<string, string>();
        public int PondId { get; set; }
        public string PondName { get; set; }
        public string Status { get; set; }
        //========================================

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var pond = (await _pondService.GetById((int)id)).Data as Pond;
            if (pond == null)
            {
                return NotFound();
            }
            PondId = (int)id;
            PondName = pond.PondName;

            // Fetch the most recent WaterParameter for the specified PondId
            var waterParameter = await _context.WaterParameters
                .Include(x => x.Status)
                .Where(m => m.PondId == id) // Filter by PondId
                .OrderByDescending(m => m.UpdatedAt) // Order by UpdatedAt to get the most recent record
                .FirstOrDefaultAsync();

            if (waterParameter == null)
            {
                // Gán WaterParameter mặc định nếu không có bản ghi nào
                WaterParameter = new WaterParameter()
                {
                    ParameterId = 0,
                    PondId = id,
                    MeasurementDate = null,
                    Temperature = 0,
                    Salinity = 0,
                    Ph = 0,
                    O2 = 0,
                    No2 = 0,
                    No3 = 0,
                    Po4 = 0,
                    WaterVolume = 0,
                    CreatedAt = null,
                    UpdatedAt = null,
                    StatusId = 0,
                };
                Status = "unknown"; // hoặc giá trị mặc định khác cho Status
                Evaluations = new Dictionary<string, string>();
            }
            else
            {
                Status = waterParameter.Status.StatusName;
                // Gọi hàm Evaluate để lấy các đánh giá
                var evaluationResult = await _waterParameterService.Evaluate(waterParameter);
                var evaluations = evaluationResult;

                WaterParameter = waterParameter;
                Evaluations = evaluations;

                
            }
            return Page();

        }
    }
}
