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

namespace KoiCareSystem.RazorWebApp.Pages.Member.PondPages
{
    public class IndexModel : PageModel
    {
        private readonly KoiFishService _koiFishService;
        private readonly PondService _pondService;
        public IndexModel(KoiFishService koiFishService, PondService pondService)
        {
            _koiFishService = koiFishService;
            _pondService = pondService;
        }
        //========================================================
        public IList<Pond> Pond { get; set; } = default!;
        public int UserId { get; set; }
        //========================================================
        public async Task OnGetAsync()
        {
            UserId = (int)UserSession.UserId;
            Pond = (await _pondService.GetByUserId(UserId)).Data as IList<Pond>;
        }

    }
}
