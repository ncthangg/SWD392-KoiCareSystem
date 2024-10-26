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

namespace KoiCareSystem.RazorWebApp.Pages.Admin.WaterParameterLimits
{
    public class IndexModel : BasePageModel
    {
        private readonly KoiCareSystem.Data.DBContext.ApplicationDbContext _context;

        public IndexModel(KoiCareSystem.Data.DBContext.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<WaterParameterLimit> WaterParameterLimit { get;set; } = default!;

        public async Task OnGetAsync()
        {
            WaterParameterLimit = await _context.WaterParameterLimits.ToListAsync();
        }
    }
}
