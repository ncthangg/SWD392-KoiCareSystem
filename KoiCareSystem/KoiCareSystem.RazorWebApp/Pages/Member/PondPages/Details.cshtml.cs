using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using KoiCareSystem.Data.DBContext;
using KoiCareSystem.Data.Models;
using KoiCareSystem.Common.DTOs;

namespace KoiCareSystem.RazorWebApp.Pages.Member.PondPages
{
    public class DetailsModel : PageModel
    {
        private readonly KoiCareSystem.Data.DBContext.ApplicationDbContext _context;

        public DetailsModel(KoiCareSystem.Data.DBContext.ApplicationDbContext context)
        {
            _context = context;
        }
        //========================================================

        public Pond Pond { get; set; } = default!;
        public int UserId { get; set; }
        //========================================================
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            UserId = (int)HttpContext.Session.GetInt32("UserId");
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
