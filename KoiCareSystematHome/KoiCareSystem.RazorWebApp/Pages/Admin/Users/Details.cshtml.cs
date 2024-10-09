using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using KoiCareSystem.Data.DBContext;
using KoiCareSystem.Data.Models;

namespace KoiCareSystem.RazorWebApp.Pages.Users
{
    public class DetailsModel : PageModel
    {
        private readonly KoiCareSystem.Data.DBContext.FA24_SE1702_PRN221_G5_KoiCareSystematHomeContext _context;

        public DetailsModel(KoiCareSystem.Data.DBContext.FA24_SE1702_PRN221_G5_KoiCareSystematHomeContext context)
        {
            _context = context;
        }

        public User User { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                User = user;
            }
            return Page();
        }
    }
}
