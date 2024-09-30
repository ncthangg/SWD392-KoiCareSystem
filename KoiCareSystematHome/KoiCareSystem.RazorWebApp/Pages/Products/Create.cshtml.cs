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

namespace KoiCareSystem.RazorWebApp.Pages.Products
{
    public class CreateModel : PageModel
    {
        private readonly ProductService _productService;
        private readonly CategoryService _categoryService;

        public CreateModel()
        {
            _productService ??= new ProductService();
            _categoryService ??= new CategoryService();
        }

        public ActionResult OnGet()
        {
            //var categories = (await _categoryService.GetAllCategory()).Data as IList<Category>;
            var categories =  _categoryService.GetAllCategory().Result.Data as IList<Category>;
            ViewData["CategoryId"] =  new SelectList(categories, "Id", "Description");
            return Page();
        }

        [BindProperty]
        public Product Product { get; set; } = default!;


        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _productService.Save(Product);

            return RedirectToPage("./Index");
        }
    }
}
