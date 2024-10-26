using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KoiCareSystem.Data.DBContext;
using KoiCareSystem.Data.Models;
using KoiCareSystem.RazorWebApp.PageBase;

namespace KoiCareSystem.RazorWebApp.Pages.Admin.WaterStatusPages
{
    public class EditModel : BasePageModel
    {
        private readonly KoiCareSystem.Data.DBContext.ApplicationDbContext _context;

        public EditModel(KoiCareSystem.Data.DBContext.ApplicationDbContext context)
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

            var waterstatus =  await _context.WaterStatuses.FirstOrDefaultAsync(m => m.StatusId == id);
            if (waterstatus == null)
            {
                return NotFound();
            }
            WaterStatus = waterstatus;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(WaterStatus).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WaterStatusExists(WaterStatus.StatusId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool WaterStatusExists(int id)
        {
            return _context.WaterStatuses.Any(e => e.StatusId == id);
        }
    }
}
