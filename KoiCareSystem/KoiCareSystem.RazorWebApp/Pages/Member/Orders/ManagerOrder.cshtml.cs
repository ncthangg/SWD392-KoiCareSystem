using AutoMapper;
using KoiCareSystem.Common.DTOs;
using KoiCareSystem.Data;
using KoiCareSystem.Data.Models;
using KoiCareSystem.RazorWebApp.PageBase;
using KoiCareSystem.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KoiCareSystem.RazorWebApp.Pages.Member.Orders
{
    public class ManagerOrderModel : BasePageModel
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
        public Order Order { get; set; } = default!;
        public int OrderId { get; set; }
        /// <summary>
        /// status
        /// </summary>
        public bool New { get; set; }
        public bool Pending { get; set; }
        public bool Confirmed { get; set; }
        public bool Shipping { get; set; }
        public bool Delivered { get; set; }
        public bool Completed { get; set; }
        public bool Cancelled { get; set; }
        public bool HasItems { get; set; }
        //========================================================
        public async Task<IActionResult> OnGetAsync(int? orderId)
        {
            LoadUserIdFromSession();

            if (UserId == null)
            {
                return RedirectToPage("/Guest/Login"); // Điều hướng đến trang đăng nhập nếu không có UserId trong session
            }

            OrderId = (int)orderId;
            if (orderId == null)
            {
                return NotFound();
            }
            var orderInfo = (await _orderService.GetByOrderId(OrderId)).Data as Order;
            Order = orderInfo;
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
                SetStatus(order);
               HasItems = order.Quantity != null;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostPendingOrder(int orderId)
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
        public async Task<IActionResult> OnPostCancelOrder(int orderId)
        {
            // Gọi dịch vụ để cập nhật trạng thái đơn hàng
            var result = await _orderService.UpdateOrderStatusAsync(orderId, 7);

            if (result)
            {
                // Chuyển hướng lại trang danh sách hoặc trang khác
                return RedirectToPage("./Index");
            }

            // Nếu có lỗi xảy ra, bạn có thể trả lại thông báo lỗi
            ModelState.AddModelError(string.Empty, "Không thể cập nhật trạng thái đơn hàng.");
            return Page();
        }

        private void SetStatus(Order order)
        {
            New = order.StatusId == 1;
            Pending = order.StatusId == 2;
            Confirmed = order.StatusId == 3;
            Shipping = order.StatusId == 4;
            Delivered = order.StatusId == 5;
            Completed = order.StatusId == 6;
            Cancelled = order.StatusId == 7;

        }
    }
}
