using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using KoiCareSystem.Data.DBContext;
using KoiCareSystem.Data.Models;
using KoiCareSystem.Service;

namespace KoiCareSystem.RazorWebApp.Pages.Admin.WaterStatusPages
{
    public class CreateModel : PageModel
    {
        private readonly WaterStatusService _waterStatusService;

        public CreateModel(WaterStatusService waterStatusService)
        {
            _waterStatusService = waterStatusService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }
        //=====================================================
        [BindProperty]
        public WaterStatus WaterStatus { get; set; } = default!;
        [BindProperty]
        public string StatusName { get; set; }

        //=====================================================
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _waterStatusService.Create(StatusName);

            return RedirectToPage("./Index");
        }
    }
}
