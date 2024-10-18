﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using KoiCareSystem.Data.DBContext;
using KoiCareSystem.Data.Models;
using KoiCareSystem.Service;

namespace KoiCareSystem.RazorWebApp.Pages.Admin.Orders
{
    public class DeleteModel : PageModel
    {
        private readonly OrderService _orderService;

        public DeleteModel()
        {
            _orderService ??= new OrderService();
        }

        [BindProperty]
        public Order Order { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _orderService.GetOrderByOrderId((int)id);

            if (order == null)
            {
                return NotFound();
            }
            else
            {
                Order = (Order)order.Data;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _orderService.GetOrderByOrderId((int)id);
            if (order != null)
            {
                await _orderService.DeleteOrderByOrderId((int)id);
            }

            return RedirectToPage("./Index");
        }
    }
}
