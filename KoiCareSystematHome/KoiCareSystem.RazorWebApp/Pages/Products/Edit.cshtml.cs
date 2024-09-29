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

namespace KoiCareSystem.RazorWebApp.Pages.Products
{
    public class EditModel : PageModel
    {
        //private readonly KoiCareSystem.Data.DBContext.FA24_SE1702_PRN221_G5_KoiCareSystematHomeContext _context;
        private readonly ProductService _productService;
        private readonly CategoryService _categoryService;

        public EditModel(KoiCareSystem.Data.DBContext.FA24_SE1702_PRN221_G5_KoiCareSystematHomeContext context)
        {
            //_context = context;
            _productService ??= new ProductService();
            _categoryService ??= new CategoryService();
        }

        [BindProperty]
        public Product Product { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product =  await _productService.GetProductById((long)id);
            if (product == null)
            {
                return NotFound();
            }
            Product = (Product)product.Data;
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

            //_context.Attach(Product).State = EntityState.Modified;
            await _productService.Save(Product);

            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!ProductExists(Product.ProductId))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}
            //if (!_productService.ProductExists(Product.ProductId))
            //{
            //    return NotFound();
            //}
            return RedirectToPage("./Index");
        }

        //private bool ProductExists(long id)
        //{
        //    return _context.Products.Any(e => e.ProductId == id);
        //}
    }
}
