using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using KoiCareSystem.Data.DBContext;
using KoiCareSystem.Data.Models;

namespace KoiCareSystem.RazorWebApp.Pages.Admin.WaterStatusPages
{
    public class IndexModel : PageModel
    {
        private readonly KoiCareSystem.Data.DBContext.ApplicationDbContext _context;

        public IndexModel(KoiCareSystem.Data.DBContext.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<WaterStatus> WaterStatus { get;set; } = default!;

        public async Task OnGetAsync()
        {
            WaterStatus = await _context.WaterStatuses.ToListAsync();
        }
    }
}
