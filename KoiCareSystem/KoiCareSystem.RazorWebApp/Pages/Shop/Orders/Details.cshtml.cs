
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using KoiCareSystem.Data.DBContext;
using KoiCareSystem.Data.Models;
using KoiCareSystem.Service;
using KoiCareSystem.RazorWebApp.PageBase;

namespace KoiCareSystem.RazorWebApp.Pages.Shop.Orders
{
    public class DetailsModel : BasePageModel
    {
        private readonly OrderService _orderService;

        public DetailsModel()
        {
            _orderService ??= new OrderService();
        }
        //========================================================
        public Order Order { get; set; } = default!;
        //========================================================

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _orderService.GetByOrderId((int)id);
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
    }
}
