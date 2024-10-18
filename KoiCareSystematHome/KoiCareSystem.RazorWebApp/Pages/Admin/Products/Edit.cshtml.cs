using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KoiCareSystem.Data.DBContext;
using KoiCareSystem.Data.Models;
using KoiCareSystematHome.Service;
using KoiCareSystem.Service;
using AutoMapper;
using KoiCareSystem.Common.DTOs.Request;

namespace KoiCareSystem.RazorWebApp.Pages.Admin.Products
{
    public class EditModel : PageModel
    {
        private readonly ProductService _productService;
        private readonly CategoryService _categoryService;
        private readonly IMapper _mapper;
        public EditModel(IMapper mapper)
        {
            _productService ??= new ProductService(mapper);
            _categoryService ??= new CategoryService();
            _mapper = mapper;
        }

        [BindProperty]
        public RequestCreateANewProductDto RequestCreateANewProductDto { get; set; } = default!;
        public Product Product { get; set; } = default!;
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product =  await _productService.GetProductById((int)id);
            if (product == null)
            {
                return NotFound();
            }
            Product = (Product)product.Data;

            RequestCreateANewProductDto = _mapper.Map<RequestCreateANewProductDto>(Product);
            var category = _categoryService.GetAllCategory().Result.Data as IList<Category>;
            ViewData["CategoryId"] = new SelectList(category, "Id", "Description");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (!_productService.ProductExists(RequestCreateANewProductDto.ProductId))
            {
                return NotFound();
            }
            else
            {
                await _productService.Save(RequestCreateANewProductDto);
            }

            if (!_productService.ProductExists(RequestCreateANewProductDto.ProductId))
            {
                return NotFound();
            }
            return RedirectToPage("./Index");
        }

    }
}
