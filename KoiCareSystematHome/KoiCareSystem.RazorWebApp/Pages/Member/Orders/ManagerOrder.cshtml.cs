using AutoMapper;
using KoiCareSystem.Common.DTOs;
using KoiCareSystem.Data;
using KoiCareSystem.Data.Models;
using KoiCareSystem.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KoiCareSystem.RazorWebApp.Pages.Member.Orders
{
    public class ManagerOrderModel : PageModel
    {
        private readonly OrderService _orderService;
        private readonly OrderItemService _orderItemService;

        public ManagerOrderModel(IMapper mapper)
        {
            _orderService ??= new OrderService();
            _orderItemService ??= new OrderItemService(mapper);
        }
        //========================================================
        public IList<OrderItem> OrderItem { get; set; } = default!;
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public bool IsPurchasable { get; set; }
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
            var orderExist = await _orderService.GetByOrderId(OrderId);
            if (orderExist != null)
            {
                var order = orderExist.Data as Order;
                IsPurchasable = order.StatusId == 1; // Chỉ cho phép mua hàng nếu status == 1
            }
            return Page();
        }

        public async Task<IActionResult> OnPostPurchaseOrder(int orderId)
        {
            // Gọi dịch vụ để cập nhật trạng thái đơn hàng
            var result = await _orderService.UpdateOrderStatusAsync(orderId, 2); // 2 là status 'Mua hàng'

            if (result)
            {
                // Chuyển hướng lại trang danh sách hoặc trang khác
                return RedirectToPage("./Index");
            }

            // Nếu có lỗi xảy ra, bạn có thể trả lại thông báo lỗi
            ModelState.AddModelError(string.Empty, "Không thể cập nhật trạng thái đơn hàng.");
            return Page();
        }


    }
}
