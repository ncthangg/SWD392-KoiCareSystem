using System;
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
using AutoMapper;

namespace KoiCareSystem.RazorWebApp.Pages.Admin.Products
{
    public class IndexModel : PageModel
    {
        private readonly ProductService _productService;
        private readonly CategoryService _categoryService;

        public IndexModel(IMapper mapper)
        {
            _productService ??= new ProductService(mapper);
            _categoryService ??= new CategoryService();
        }


        public IList<Product> Product { get; set; } = default!;

        public async Task OnGetAsync()
        {
            
            var result = await _productService.GetAllProduct();
            if (result.Status > 0)
            {
                Product = (IList<Product>)result.Data;
            }

            //Product = (await _productService.GetAllProduct()).Data as IList<Product>;

        }
    }
}
