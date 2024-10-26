using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using KoiCareSystem.Data.Models;
using KoiCareSystem.RazorWebApp.PageBase;

namespace KoiCareSystem.RazorWebApp.Pages.Admin.PondPages
{
    public class CreateModel : BasePageModel
    {
        private readonly KoiCareSystem.Data.DBContext.ApplicationDbContext _context;

        public CreateModel(KoiCareSystem.Data.DBContext.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email");
            return Page();
        }

        [BindProperty]
        public Pond Pond { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Ponds.Add(Pond);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
