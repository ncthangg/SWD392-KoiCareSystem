using Microsoft.AspNetCore.Mvc;
using KoiCareSystem.Data.Models;
using KoiCareSystem.Service;
using AutoMapper;
using KoiCareSystem.RazorWebApp.PageBase;

namespace KoiCareSystem.RazorWebApp.Pages.Shop.Orders
{
    public class CreateModel : BasePageModel
    {
        private readonly OrderService _orderService;
        private readonly IMapper _mapper;
        public CreateModel(IMapper mapper)
        {
            _orderService ??= new OrderService();
            _mapper = mapper;
        }
        //========================================================
        [BindProperty]
        public Order Order { get; set; } = default!;
        //public RequestCreateOrderDto RequestCreateOrderDto { get; set; } = default!;

        //========================================================
        public IActionResult OnGet()
        {
            return Page();
        }
        
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            //Order = _mapper.Map<Order>(RequestCreateOrderDto);
            await _orderService.Save(Order);

            return RedirectToPage("./Index");
        }
    }
}
