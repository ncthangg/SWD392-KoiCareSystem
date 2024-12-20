﻿using KoiCareSystem.RazorWebApp.PageBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KoiCareSystem.Data.Models;

namespace KoiCareSystem.RazorWebApp.Pages.Admin.WaterParameterLimits
{
    public class DeleteModel : BasePageModel
    {
        private readonly KoiCareSystem.Data.DBContext.ApplicationDbContext _context;

        public DeleteModel(KoiCareSystem.Data.DBContext.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var waterparameterlimit = await _context.WaterParameterLimits.FindAsync(id);
            if (waterparameterlimit != null)
            {
                WaterParameterLimit = waterparameterlimit;
                _context.WaterParameterLimits.Remove(WaterParameterLimit);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
