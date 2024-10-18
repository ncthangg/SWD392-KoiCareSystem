using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using KoiCareSystem.Data.DBContext;
using KoiCareSystem.Data.Models;
using KoiCareSystem.Service;

namespace KoiCareSystem.RazorWebApp.Pages.Admin.KoiFishPages
{
    public class IndexModel : PageModel
    {
        private readonly KoiFishService _koiFishService;
        public IndexModel(KoiFishService koiFishService)
        {
            _koiFishService = koiFishService;
        }
        public IList<KoiFish> KoiFish { get;set; } = default!;

        public async Task OnGetAsync()
        {
            KoiFish = (await _koiFishService.GetAll()).Data as IList<KoiFish>;
        }
    }
}
