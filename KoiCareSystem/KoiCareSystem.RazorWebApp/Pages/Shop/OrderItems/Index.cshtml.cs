
using Microsoft.AspNetCore.Mvc;
using KoiCareSystem.Data.Models;
using KoiCareSystem.Service;
using AutoMapper;
using KoiCareSystem.RazorWebApp.PageBase;

namespace KoiCareSystem.RazorWebApp.Pages.Shop.OrderItems
{
    public class IndexModel : BasePageModel
    {
        private readonly OrderService _orderService;
        private readonly OrderItemService _orderItemService;

        public IndexModel(IMapper mapper)
        {
            _orderService ??= new OrderService();
            _orderItemService ??= new OrderItemService(mapper);
        }
        //========================================================
        public IList<OrderItem> OrderItem { get; set; } = default!;
        public int OrderId { get; set; }
        //========================================================
        public async Task<IActionResult> OnGetAsync(int? orderId)
        {
            OrderId = (int)orderId; 
            Console.WriteLine(OrderId);
            if (orderId == null)
            {
                return NotFound();
            }

            var orderItems = await _orderItemService.GetAllItemInOrder((int)orderId);
            if (orderItems == null || orderItems.Data == null)
            {
                return NotFound();
            }
            else
            {
                OrderItem = orderItems.Data as List<OrderItem>;
            }

            return Page();
        }

    }
}
