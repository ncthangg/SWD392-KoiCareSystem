using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using KoiCareSystem.Data.DBContext;
using KoiCareSystem.Data.Models;
using KoiCareSystem.Service;

namespace KoiCareSystem.RazorWebApp.Pages.Member.WaterParameters
{
    public class DetailModel : PageModel
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
        public Dictionary<string, string> Evaluations { get; set; }

        public int UserId { get; set; }
        public int PondId { get; set; }
        public string PondName { get; set; }
        //========================================

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var pond = (await _pondService.GetById((int)id)).Data as Pond;
            if (pond == null) {
                return NotFound();
            }
            PondId = (int)id;
            PondName = pond.PondName;
            // Fetch the most recent WaterParameter for the specified PondId
            var waterParameter = await _context.WaterParameters
                .Include(x=> x.Status)
                .Where(m => m.PondId == id) // Filter by PondId
                .OrderByDescending(m => m.UpdatedAt) // Order by UpdatedAt to get the most recent record
                .FirstOrDefaultAsync();

            if (waterParameter == null)
            {
                return NotFound();
            }
            // Gọi hàm Evaluate để lấy các đánh giá
            var evaluationResult = await _waterParameterService.Evaluate(waterParameter);
            var evaluations = evaluationResult;

            WaterParameter = waterParameter;
            Evaluations = evaluations;


            return Page();
        }
    }
}
