using KoiCareSystem.RazorWebApp.PageBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KoiCareSystem.Data.Models;

namespace KoiCareSystem.RazorWebApp.Pages.Admin.WaterParameterLimits
{
    public class DetailsModel : BasePageModel
    {
        private readonly KoiCareSystem.Data.DBContext.ApplicationDbContext _context;

        public DetailsModel(KoiCareSystem.Data.DBContext.ApplicationDbContext context)
        {
            _context = context;
        }

        public WaterParameterLimit WaterParameterLimit { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var waterparameterlimit = await _context.WaterParameterLimits.FirstOrDefaultAsync(m => m.ParameterId == id);
            if (waterparameterlimit == null)
            {
                return NotFound();
            }
            else
            {
                WaterParameterLimit = waterparameterlimit;
            }
            return Page();
        }
    }
}
