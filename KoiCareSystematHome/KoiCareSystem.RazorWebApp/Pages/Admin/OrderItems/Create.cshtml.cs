using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using KoiCareSystem.Data.Models;
using KoiCareSystem.Service;
using KoiCareSystematHome.Service;
using AutoMapper;
using KoiCareSystem.Common.DTOs.Request;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace KoiCareSystem.RazorWebApp.Pages.Admin.OrderItems
{
    public class CreateModel : PageModel
    {
        private readonly OrderService _orderService;
        private readonly ProductService _productService;
        private readonly OrderItemService _orderItemService;

        public CreateModel(IMapper mapper)
        {
            _orderService ??= new OrderService();
            _productService ??= new ProductService(mapper);
            _orderItemService ??= new OrderItemService(mapper);
        }

        [BindProperty]
        public RequestItemToOrderDto RequestItemToOrderDto { get; set; } = default!;
        public IActionResult OnGet(long orderId)
        {
            if (RequestItemToOrderDto == null)
            {
                RequestItemToOrderDto = new RequestItemToOrderDto();
            }

            RequestItemToOrderDto.OrderId = orderId;

            var products = _productService.GetAllProduct().Result.Data as IList<Product>;
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
