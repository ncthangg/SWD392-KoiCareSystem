using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KoiCareSystem.Data.DBContext;
using KoiCareSystem.Data.Models;
using KoiCareSystem.Service;

namespace KoiCareSystem.RazorWebApp.Pages.Admin.Orders
{
    public class EditModel : PageModel
    {
        private readonly OrderService _orderService;
        private readonly OrderStatusService _orderStatusService;

        public EditModel()
        {
            _orderService ??= new OrderService();
            _orderStatusService ??= new OrderStatusService();
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
            Order = (Order)order.Data;

            var status = _orderStatusService.GetAllStatus().Result.Data as IList<OrderStatus>;
            ViewData["StatusId"] = new SelectList(status, "StatusId", "StatusName");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (!_orderService.OrderExists(Order.OrderId))
            {
                return NotFound();
            }
            else
            {
                await _orderService.Save(Order);
            }

            return RedirectToPage("./Index");
        }


    }
}
