using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using KoiCareSystem.Data.DBContext;
using KoiCareSystem.Data.Models;
using KoiCareSystem.Service;
using KoiCareSystem.Common.DTOs.Request;
using AutoMapper;

namespace KoiCareSystem.RazorWebApp.Pages.Shop.Orders
{
    public class CreateModel : PageModel
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
