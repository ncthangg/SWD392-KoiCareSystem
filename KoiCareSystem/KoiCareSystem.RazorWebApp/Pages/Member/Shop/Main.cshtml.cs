using AutoMapper;
using Humanizer;
using KoiCareSystem.Common.DTOs.Request;
using KoiCareSystem.Data.DBContext;
using KoiCareSystem.Data.Models;
using KoiCareSystem.RazorWebApp.PageBase;
using KoiCareSystem.Service;
using MailKit.Search;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis;
using System;

namespace KoiCareSystem.RazorWebApp.Pages.Member.Shop
{
    public class MainModel : BasePageModel
    {
        private readonly OrderService _orderService;
        private readonly ProductService _productService;
        private readonly OrderItemService _orderItemService;
        private readonly IMapper _mapper;

        public MainModel(IMapper mapper)
        {
            _orderService ??= new OrderService();
            _productService ??= new ProductService(mapper);
            _orderItemService ??= new OrderItemService(mapper);
        }
        //========================================================
        public IList<Product> Products { get; set; } = new List<Product>();
        [BindProperty]
        public RequestItemToOrderDto RequestItemToOrderDto { get; set; } = default!;
        public int OrderId { get; set; }
        //========================================================

        public async Task<IActionResult> OnGetAsync()
        {
            LoadUserIdFromSession();
            if (UserId == null)
            {
                return RedirectToPage("/Guest/Login");
            }
            var productListResult = await _productService.GetAll();
            Products = productListResult?.Data as IList<Product> ?? new List<Product>();

            var existOrder = await _orderService.GetNewOrder((int)UserId);
            if(existOrder != null)
            {
                var id = existOrder.OrderId;
                OrderId = (int)id;
            }
            else
            {
                var result = await _orderService.CreateByUserId((int)UserId);
                if (result)
                {
                    var newOrder = await _orderService.GetNewOrder((int)UserId);
                    var id = newOrder.OrderId;
                    OrderId = (int)id;
                }
                RedirectToPage("/Error");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAddToCart(RequestItemToOrderDto requestItemToOrderDto)
        {
            Console.WriteLine("Send");
            if (requestItemToOrderDto != null)
            {
                var productId = requestItemToOrderDto.ProductId;
                var orderId = requestItemToOrderDto.OrderId;
                var quantity = requestItemToOrderDto.Quantity;

                // Ghi log các giá trị để kiểm tra
                Console.WriteLine($"ProductId: {productId}, OrderId: {orderId}, Quantity: {quantity}");

                var result = await _orderItemService.AddItemToOrder(requestItemToOrderDto);
                if (result.Status > 0)
                {
                    //return new JsonResult(new { success = true, message = "Đã thêm vào giỏ hàng thành công!" }); 
                    return RedirectToPage("./Main", new { message = "Đã thêm vào giỏ hàng thành công!" });
                }
            }
            //return new JsonResult(new { success = false, message = "Không thể thêm sản phẩm vào giỏ hàng." });
            return RedirectToPage("/Main", new { message = "Không thể thêm sản phẩm vào giỏ hàng." });
        }

    }
}
