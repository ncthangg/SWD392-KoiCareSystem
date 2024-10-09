﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using KoiCareSystem.Data.DBContext;
using KoiCareSystem.Data.Models;
using KoiCareSystematHome.Service;
using KoiCareSystem.Service;

namespace KoiCareSystem.RazorWebApp.Pages.Products
{
    public class DetailsModel : PageModel
    {
        private readonly ProductService _productService;
        private readonly CategoryService _categoryService;

        public DetailsModel()
        {
            _productService ??= new ProductService();
            _categoryService ??= new CategoryService();
        }
        public Product Product { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productService.GetProductById((long)id);
            if (product == null)
            {
                return NotFound();
            }
            else
            {
                Product = (Product)product.Data;
            }
            return Page();
        }
    }
}