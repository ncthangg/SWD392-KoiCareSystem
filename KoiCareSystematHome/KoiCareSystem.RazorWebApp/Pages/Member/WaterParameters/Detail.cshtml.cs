using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using KoiCareSystem.Data.DBContext;
using KoiCareSystem.Data.Models;

namespace KoiCareSystem.RazorWebApp.Pages.Member.WaterParameters
{
    public class DetailModel : PageModel
    {
        private readonly KoiCareSystem.Data.DBContext.ApplicationDbContext _context;

        public DetailModel(KoiCareSystem.Data.DBContext.ApplicationDbContext context)
        {
            _context = context;
        }
        //========================================
        public WaterParameter WaterParameter { get; set; } = default!;
        public int UserId { get; set; }
        //========================================

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            // Fetch the most recent WaterParameter for the specified PondId
            var waterparameter = await _context.WaterParameters
                .Where(m => m.PondId == id) // Filter by PondId
                .OrderByDescending(m => m.UpdatedAt) // Order by UpdatedAt to get the most recent record
                .FirstOrDefaultAsync();

            if (waterparameter == null)
            {
                return NotFound();
            }

            WaterParameter = waterparameter;
            return Page();
        }
    }
}
