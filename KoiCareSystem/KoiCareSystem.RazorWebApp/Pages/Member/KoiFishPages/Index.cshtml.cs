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

        // size page
        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }

        public const int PageSize = 5;

        //========================================================
        public async Task<IActionResult> OnGetAsync()
        {
            LoadUserIdFromSession();

            if (UserId == null)
            {
                return RedirectToPage("/Guest/Login"); // Điều hướng đến trang đăng nhập nếu không có UserId trong session
            }
            var list = (await _koiFishService.GetByUserId((int)UserId)).Data as IList<KoiFish>;

            // Calculate total pages
            int totalRecords = list.Count;

            TotalPages = (int)Math.Ceiling(totalRecords / (double)PageSize);

            // Get only the players for the current page
            KoiFish = list
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            return Page();
        }
    }
}
