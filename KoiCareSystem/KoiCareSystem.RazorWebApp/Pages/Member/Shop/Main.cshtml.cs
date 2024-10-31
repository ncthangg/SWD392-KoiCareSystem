﻿using AutoMapper;
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
        private readonly CategoryService _categoryService;
        private readonly OrderService _orderService;
        private readonly ProductService _productService;
        private readonly OrderItemService _orderItemService;
        private readonly IMapper _mapper;

        public MainModel(IMapper mapper)
        {
            _categoryService ??= new CategoryService();
            _orderService ??= new OrderService();
            _productService ??= new ProductService(mapper);
            _orderItemService ??= new OrderItemService(mapper);
        }
        //========================================================
        /// <summary>
        ///              Get
        /// </summary>
        public IList<Product> Products { get; set; } = new List<Product>();
        public List<Category> Categories { get; set; }
        public string CategoriesName { get; set; }
        public int OrderId { get; set; }
        public int TotalItems { get; set; }

        [BindProperty]
        public RequestItemToOrderDto RequestItemToOrderDto { get; set; } = default!;

        /// <summary>
        ///          Search
        /// </summary>
        public string SearchQuery { get; set; }
        public int SelectedCategoryId { get; set; }
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
            var categoryListResult = await _categoryService.GetAll();
            Categories = categoryListResult?.Data as List<Category> ?? new List<Category>();

            var existOrder = await _orderService.GetNewOrder((int)UserId);
            if(existOrder != null)
            {
                var id = existOrder.OrderId;
                OrderId = (int)id;

                var order = (await _orderService.GetByOrderId(OrderId)).Data as Order;
                TotalItems = (int)order.Quantity;
            }
            else
            {
                var result = await _orderService.CreateByUserId((int)UserId);
                if (result)
                {
                    var newOrder = await _orderService.GetNewOrder((int)UserId);
                    var id = newOrder.OrderId;
                    OrderId = (int)id;
                    TotalItems = 0;
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

        public async Task<IActionResult> OnGetSearchAsync(string searchQuery, int? categoryId)
        {
            Console.WriteLine("Search handler called");
            SearchQuery = searchQuery;
            SelectedCategoryId = categoryId ?? 0; // Lưu categoryId được chọn

            // Lấy danh sách sản phẩm từ dịch vụ
            var allProducts = (await _productService.GetAll()).Data as List<Product>;

            // Lọc sản phẩm dựa trên tìm kiếm và categoryId
            Products = allProducts
                .Where(p => (string.IsNullOrEmpty(searchQuery) || p.ProductName.Contains(searchQuery, StringComparison.OrdinalIgnoreCase)) &&
                             (!categoryId.HasValue || p.CategoryId == categoryId.Value))
                .ToList();

            // Lấy danh sách category từ cơ sở dữ liệu
            Categories = (await _categoryService.GetAll()).Data as List<Category>;

            return Page();
        }

    }
}
