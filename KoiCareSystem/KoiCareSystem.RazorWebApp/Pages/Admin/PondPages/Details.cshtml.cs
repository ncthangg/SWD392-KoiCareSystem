using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KoiCareSystem.Data.Models;
using KoiCareSystem.RazorWebApp.PageBase;

namespace KoiCareSystem.RazorWebApp.Pages.Admin.PondPages
{
    public class DetailsModel : BasePageModel
    {
        private readonly KoiCareSystem.Data.DBContext.ApplicationDbContext _context;

        public DetailsModel(KoiCareSystem.Data.DBContext.ApplicationDbContext context)
        {
            _context = context;
        }

        public Pond Pond { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pond = await _context.Ponds.FirstOrDefaultAsync(m => m.PondId == id);
            if (pond == null)
            {
                return NotFound();
            }
            else
            {
                Pond = pond;
            }
            return Page();
        }
    }
}
