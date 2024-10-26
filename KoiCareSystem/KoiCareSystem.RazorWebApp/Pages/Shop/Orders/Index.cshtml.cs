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
using Microsoft.AspNetCore.Mvc.Rendering;
using KoiCareSystem.RazorWebApp.PageBase;

namespace KoiCareSystem.RazorWebApp.Pages.Shop.Orders
{
    public class IndexModel : BasePageModel
    {
        private readonly OrderService _orderService;
        private readonly OrderStatusService _orderStatusService;
        public IndexModel()
        {
            _orderService ??= new OrderService();
            _orderStatusService ??= new OrderStatusService();
        }
        //========================================================
        //List Order
        public IList<Order> Order { get; set; } = default!;
        public IList<OrderStatus> OrderStatuses { get; set; } = default!;
        //Order

        // Define the search properties
        [BindProperty(SupportsGet = true)]
        public string SearchOrderId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchUserName { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SearchOrderStatus { get; set; }

        //========================================================
        public async Task OnGetAsync()
        {
            var result = await _orderService.GetAllOrder();
            if (result.Status > 0)
            {
                Order = (IList<Order>)result.Data;
            }
            OrderStatuses = (await _orderStatusService.GetAll()).Data as List<OrderStatus>;
            OrderStatuses.Insert(0, new OrderStatus { StatusId = 0, StatusName = "Select an Order Status" });

            ViewData["OrderStatus"] = new SelectList(OrderStatuses, "StatusId", "StatusName");


            // Convert to queryable to enable search filtering
            var query = Order.AsQueryable();
            // Search by OrderID
            if (!string.IsNullOrEmpty(SearchOrderId) && int.TryParse(SearchOrderId, out int id))
            {
                query = query.Where(p => p.OrderId == id);
            }

            // Search by User.Email
            if (!string.IsNullOrEmpty(SearchUserName))
            {
                query = query.Where(p => p.User.Email.Contains(SearchUserName, StringComparison.OrdinalIgnoreCase));
            }

            // Search by OrderStatus
            if (!string.IsNullOrEmpty(SearchOrderStatus) && int.TryParse(SearchOrderStatus, out int statusId))
            {
                if (statusId != 0)
                {
                    query = query.Where(p => p.StatusId == statusId);
                }
            }

            Order = query.ToList();
        }
    }
}
