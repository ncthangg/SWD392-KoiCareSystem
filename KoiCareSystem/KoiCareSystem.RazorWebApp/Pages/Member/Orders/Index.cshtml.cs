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
                Order = ((IList<Order>)result.Data)
                         .OrderByDescending(o => o.CreatedAt) // Sắp xếp từ mới nhất đến cũ
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
