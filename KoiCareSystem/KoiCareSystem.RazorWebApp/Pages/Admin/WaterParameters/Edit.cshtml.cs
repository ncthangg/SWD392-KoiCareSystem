using KoiCareSystem.RazorWebApp.PageBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KoiCareSystem.Data.Models;

namespace KoiCareSystem.RazorWebApp.Pages.Admin.WaterParameters
{
    public class EditModel : BasePageModel
    {
        private readonly KoiCareSystem.Data.DBContext.ApplicationDbContext _context;

        public EditModel(KoiCareSystem.Data.DBContext.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public WaterParameter WaterParameter { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var waterparameter =  await _context.WaterParameters.FirstOrDefaultAsync(m => m.ParameterId == id);
            if (waterparameter == null)
            {
                return NotFound();
            }
            WaterParameter = waterparameter;
           ViewData["PondId"] = new SelectList(_context.Ponds, "PondId", "PondName");
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

            _context.Attach(WaterParameter).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WaterParameterExists(WaterParameter.ParameterId))
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

        private bool WaterParameterExists(int id)
        {
            return _context.WaterParameters.Any(e => e.ParameterId == id);
        }
    }
}
