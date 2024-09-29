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

namespace KoiCareSystem.RazorWebApp.Pages.Products
{
    public class DetailsModel : PageModel
    {
        //private readonly KoiCareSystem.Data.DBContext.FA24_SE1702_PRN221_G5_KoiCareSystematHomeContext _context;
        private readonly ProductService _productService;

        public DetailsModel(KoiCareSystem.Data.DBContext.FA24_SE1702_PRN221_G5_KoiCareSystematHomeContext context)
        {
            //_context = context;
            _productService ??= new ProductService();
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
