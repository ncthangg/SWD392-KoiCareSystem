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
    public class IndexModel : PageModel
    {
        private readonly KoiCareSystem.Data.DBContext.FA24_SE1702_PRN221_G5_KoiCareSystematHomeContext _context;
        private readonly ProductService _productService;

        public IndexModel(KoiCareSystem.Data.DBContext.FA24_SE1702_PRN221_G5_KoiCareSystematHomeContext context)
        {
            // _context = context;
            _productService ??= new ProductService();
        }
        //public IndexModel(ProductService productService)
        //{
        //    // _context = context;
        //    _productService ??= productService;
        //}

        public IList<Product> Product { get; set; } = default!;

        public async Task OnGetAsync()
        {
            //Product = await _context.Products
            //    .Include(p => p.Category).ToListAsync();

            //var result = await _productService.GetAllProduct();
            //if(result.Status > 0)
            //{
            //    Product =(IList<Product>) result.Data;
            //}    


            Product = (await _productService.GetAllProduct()).Data as IList<Product>;

        }
    }
}
