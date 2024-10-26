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
using KoiCareSystem.RazorWebApp.PageBase;

namespace KoiCareSystem.RazorWebApp.Pages.Member.KoiFishPages
{
    public class DetailsModel : BasePageModel
    {
        private readonly KoiFishService _koiFishService;
        public DetailsModel(KoiFishService koiFishService)
        {
            _koiFishService = koiFishService;
        }
        //========================================================
        public KoiFish KoiFish { get; set; } = default!;
        //========================================================
        public async Task<IActionResult> OnGetAsync(int id)
        {
            LoadUserIdFromSession();

            if (UserId == null)
            {
                return RedirectToPage("/Guest/Login"); // Điều hướng đến trang đăng nhập nếu không có UserId trong session
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
