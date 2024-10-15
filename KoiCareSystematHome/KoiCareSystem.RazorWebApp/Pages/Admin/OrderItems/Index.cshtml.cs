
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using KoiCareSystem.Data.Models;
using KoiCareSystem.Service;
using AutoMapper;
using MailKit.Search;

namespace KoiCareSystem.RazorWebApp.Pages.Admin.OrderItems
{
    public class IndexModel : PageModel
    {
        private readonly OrderService _orderService;
        private readonly OrderItemService _orderItemService;

        public IndexModel(IMapper mapper)
        {
            _orderService ??= new OrderService();
            _orderItemService ??= new OrderItemService(mapper);
        }

        public IList<OrderItem> OrderItem { get; set; } = default!;
        public long OrderId { get; set; }
        public async Task<IActionResult> OnGetAsync(long? orderId)
        {
            OrderId = (long)orderId; 
            Console.WriteLine(OrderId);
            if (orderId == null)
            {
                return NotFound();
            }

            var orderItems = await _orderItemService.GetAllItemInOrder((long)orderId);
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
