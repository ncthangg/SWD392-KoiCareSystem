using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using KoiCareSystem.Data.DBContext;
using KoiCareSystem.Data.Models;
using AutoMapper;
using KoiCareSystem.Service;
using KoiCareSystematHome.Service;
using KoiCareSystem.Common.DTOs.Request;
using KoiCareSystem.Common.DTOs;

namespace KoiCareSystem.RazorWebApp.Pages.Member.Orders
{
    public class DeleteModel : PageModel
    {
        private readonly OrderService _orderService;
        private readonly ProductService _productService;
        private readonly OrderItemService _orderItemService;

        public DeleteModel(IMapper mapper)
        {
            _orderService ??= new OrderService();
            _productService ??= new ProductService(mapper);
            _orderItemService ??= new OrderItemService(mapper);
        }


        [BindProperty]
        public OrderItem OrderItem { get; set; } = default!;
        public int UserId { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderItem = await _orderItemService.GetById((int)id);

            if (orderItem == null)
            {
                return NotFound();
            }
            else
            {
                OrderItem = (OrderItem)orderItem.Data;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderitem = await _orderItemService.GetById((int)id);
            if (orderitem != null)
            {
                OrderItem = (OrderItem)orderitem.Data;
                await _orderItemService.DeleteItem((int)id);

            }

            return RedirectToPage("./Index", new { orderId = OrderItem.OrderId });
        }
    }
}
