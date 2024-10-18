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
using KoiCareSystem.Common.DTOs;

namespace KoiCareSystem.RazorWebApp.Pages.Member.KoiFishPages
{
    public class IndexModel : PageModel
    {
        private readonly KoiFishService _koiFishService;
        public IndexModel(KoiFishService koiFishService)
        {
            _koiFishService = koiFishService;
        }
        public IList<KoiFish> KoiFish { get;set; } = default!;
        public int UserId { get; set; }
        public async Task OnGetAsync()
        {
            UserId = (int)UserSession.UserId;
            KoiFish = (await _koiFishService.GetByUserId(UserId)).Data as IList<KoiFish>;
        }
    }
}
