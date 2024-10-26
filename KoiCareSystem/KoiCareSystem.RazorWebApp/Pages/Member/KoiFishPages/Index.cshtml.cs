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
using Microsoft.AspNetCore.Authorization;
using KoiCareSystem.RazorWebApp.PageBase;

namespace KoiCareSystem.RazorWebApp.Pages.Member.KoiFishPages
{
    public class IndexModel : BasePageModel
    {
        private readonly KoiFishService _koiFishService;
        public IndexModel(KoiFishService koiFishService)
        {
            _koiFishService = koiFishService;
        }
        //========================================================
        public IList<KoiFish> KoiFish { get; set; } = default!;
       
        //========================================================
        public async Task<IActionResult> OnGetAsync()
        {
            LoadUserIdFromSession();

            if (UserId == null)
            {
                return RedirectToPage("/Guest/Login"); // Điều hướng đến trang đăng nhập nếu không có UserId trong session
            }
            KoiFish = (await _koiFishService.GetByUserId((int)UserId)).Data as IList<KoiFish>;
            return Page();
        }
    }
}
