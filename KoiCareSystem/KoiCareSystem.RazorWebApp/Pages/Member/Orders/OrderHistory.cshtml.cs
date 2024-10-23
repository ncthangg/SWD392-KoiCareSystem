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

namespace KoiCareSystem.RazorWebApp.Pages.Member.Orders
{
    public class OrderHistory : PageModel
    {
        private readonly OrderService _orderService;

        public OrderHistory()
        {
            _orderService ??= new OrderService();
        }
        //========================================================
        public IList<Order> Order { get;set; } = default!;
        public int UserId { get; set; }
        //========================================================
        public async Task OnGetAsync()
        {
            UserId = (int)UserSession.UserId; // Lấy UserId từ biến static
            var result = await _orderService.GetByUserId(UserId);
            if (result.Status > 0)
            {
                Order = (IList<Order>)result.Data;
            }
        }
        public async Task<IActionResult> OnPostCreate()
        {
            // Tạo một đơn hàng mới
            Order newOrder = new Order
            {
                    UserId = (int)UserSession.UserId
            };

                await _orderService.Save(newOrder);

                // Điều hướng trở lại trang Index
                return RedirectToPage("./Index");
        }

    }
}
