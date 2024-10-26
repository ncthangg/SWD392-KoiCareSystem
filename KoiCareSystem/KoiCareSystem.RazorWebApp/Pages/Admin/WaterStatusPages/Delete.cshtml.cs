using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using KoiCareSystem.Data.DBContext;
using KoiCareSystem.Data.Models;
using KoiCareSystem.RazorWebApp.PageBase;

namespace KoiCareSystem.RazorWebApp.Pages.Admin.WaterStatusPages
{
    public class DeleteModel : BasePageModel
    {
        private readonly KoiCareSystem.Data.DBContext.ApplicationDbContext _context;

        public DeleteModel(KoiCareSystem.Data.DBContext.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public WaterStatus WaterStatus { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var waterstatus = await _context.WaterStatuses.FirstOrDefaultAsync(m => m.StatusId == id);

            if (waterstatus == null)
            {
                return NotFound();
            }
            else
            {
                WaterStatus = waterstatus;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var waterstatus = await _context.WaterStatuses.FindAsync(id);
            if (waterstatus != null)
            {
                WaterStatus = waterstatus;
                _context.WaterStatuses.Remove(WaterStatus);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
