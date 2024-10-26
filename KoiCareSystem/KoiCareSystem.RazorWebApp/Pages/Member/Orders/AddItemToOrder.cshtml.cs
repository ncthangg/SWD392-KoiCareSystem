using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using KoiCareSystem.Data.Models;
using KoiCareSystem.Service;
using KoiCareSystem.Service;
using AutoMapper;
using KoiCareSystem.Common.DTOs.Request;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using KoiCareSystem.Common.DTOs;
using KoiCareSystem.RazorWebApp.PageBase;

namespace KoiCareSystem.RazorWebApp.Pages.Member.Orders
{
    public class AddItemModel : BasePageModel
    {
        private readonly OrderService _orderService;
        private readonly ProductService _productService;
        private readonly OrderItemService _orderItemService;

        public AddItemModel(IMapper mapper)
        {
            _orderService ??= new OrderService();
            _productService ??= new ProductService(mapper);
            _orderItemService ??= new OrderItemService(mapper);
        }
        //========================================================
        [BindProperty]
        public RequestItemToOrderDto RequestItemToOrderDto { get; set; } = default!;
        //========================================================
        public IActionResult OnGet(int id)
        {
            if (RequestItemToOrderDto == null)
            {
                RequestItemToOrderDto = new RequestItemToOrderDto();
            }

            RequestItemToOrderDto.OrderId = id;

            var products = _productService.GetAll().Result.Data as IList<Product>;
            ViewData["ProductId"] = new SelectList(products, "ProductId", "ProductName");
            return Page();
        }

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            await _orderItemService.AddItemToOrder(RequestItemToOrderDto);

            return RedirectToPage("./Index", new { orderId = RequestItemToOrderDto.OrderId });
        }
    }
}
