using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using KoiCareSystem.Data.DBContext;
using KoiCareSystem.Data.Models;
using KoiCareSystematHome.Service;
using KoiCareSystem.Service;
using KoiCareSystem.Common.DTOs.Request;
using AutoMapper;

namespace KoiCareSystem.RazorWebApp.Pages.Admin.Products
{
    public class CreateModel : PageModel
    {
        private readonly ProductService _productService;
        private readonly CategoryService _categoryService;

        public CreateModel(IMapper mapper)
        {
            _productService ??= new ProductService(mapper);
            _categoryService ??= new CategoryService();
        }

        [BindProperty]
        public RequestCreateANewProductDto RequestCreateANewProductDto { get; set; } = default!;

        public  ActionResult OnGet()
        {
            var categories = _categoryService.GetAllCategory().Result.Data as IList<Category>;
            ViewData["CategoryId"] =  new SelectList(categories, "Id", "Description");
            return Page();
        }


        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            await _productService.Save(RequestCreateANewProductDto);

            return RedirectToPage("./Index");
        }
    }
}
