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

namespace De.Pages.Orders
{
    public class IndexModel : PageModel
    {
        private readonly OrderService _orderService;

        public IndexModel()
        {
            _orderService ??= new OrderService();
        }

        public IList<Order> Order { get;set; } = default!;

        public async Task OnGetAsync()
        {
            var result = await _orderService.GetAllOrder();
            if (result.Status > 0)
            {
                Order = (IList<Order>)result.Data;
            }
        }
    }
}
