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
using KoiCareSystem.RazorWebApp.PageBase;
using System.Drawing.Printing;

namespace KoiCareSystem.RazorWebApp.Pages.Member.Orders
{
    public class IndexModel : BasePageModel
    {
        private readonly OrderService _orderService;

        public IndexModel()
        {
            _orderService ??= new OrderService();
        }
        //========================================================
        public IList<Order> Order { get; set; } = default!;

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

            var result = await _orderService.GetByUserId((int)UserId);

            if (result.Status > 0)
            {

                var list = ((IList<Order>)result.Data)
                         .OrderByDescending(o => o.CreatedAt) // Sắp xếp từ mới nhất đến cũ
                         .ToList();

                int totalRecords = list.Count;

                TotalPages = (int)Math.Ceiling(totalRecords / (double)PageSize);

                // Get only the players for the current page
                Order = list
                    .Skip((CurrentPage - 1) * PageSize)
                    .Take(PageSize)
                    .ToList();

            }
            return Page();
        }

        public async Task<IActionResult> OnPostCreate()
        {
            // Tạo một đơn hàng mới
            Order newOrder = new Order
            {
                UserId = (int)HttpContext.Session.GetInt32("UserId")
            };

            await _orderService.Save(newOrder);

            // Điều hướng trở lại trang Index
            return RedirectToPage("./Index");
        }

    }
}
