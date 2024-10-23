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
using KoiCareSystem.Service;

namespace KoiCareSystem.RazorWebApp.Pages.Member.KoiFishPages
{
    public class DetailsModel : PageModel
    {
        private readonly KoiFishService _koiFishService;
        public DetailsModel(KoiFishService koiFishService)
        {
            _koiFishService = koiFishService;
        }
        //========================================================
        public KoiFish KoiFish { get; set; } = default!;
        public int UserId { get; set; }
        //========================================================
        public async Task<IActionResult> OnGetAsync(int id)
        {
            UserId = (int)HttpContext.Session.GetInt32("UserId");
            if (id == null)
            {
                return NotFound();
            }

            var koifish = await _koiFishService.GetById((int)id);
            if (koifish == null)
            {
                return NotFound();
            }
            else
            {
                KoiFish = (KoiFish)koifish.Data;
            }
            return Page();
        }
    }
}
