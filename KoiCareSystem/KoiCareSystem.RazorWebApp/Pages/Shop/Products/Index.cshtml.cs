using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using KoiCareSystem.Data.Models;
using KoiCareSystem.Service;
using AutoMapper;
using KoiCareSystem.RazorWebApp.PageBase;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KoiCareSystem.RazorWebApp.Pages.Shop.Products
{
    public class IndexModel : BasePageModel
    {
        private readonly ProductService _productService;
        private readonly CategoryService _categoryService;
        private readonly IMapper _mapper;

        public IndexModel(IMapper mapper)
        {
            _productService ??= new ProductService(mapper);
            _categoryService ??= new CategoryService();
            _mapper = mapper;
        }
        //========================================================
        public IList<Product> Products { get; set; } = new List<Product>();
        public IList<Category> Category { get; set; } = new List<Category>();

        // Define the search properties
        [BindProperty(SupportsGet = true)]
        public string SearchId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchName { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchCategory { get; set; }
        //========================================================
        public async Task<IActionResult> OnGetAsync()
        {
            LoadUserIdFromSession();

            if (UserId == null)
            {
                return RedirectToPage("/Guest/Login");
            }
            Category = (await _categoryService.GetAll()).Data as List<Category>;
            Category.Insert(0, new Category { Id = 0, Name = "Select an Category" });

            ViewData["Category"] = new SelectList(Category, "Id", "Name");

            // Fetch all products using the ProductService
            var productListResult = await _productService.GetAll();
            var products = productListResult?.Data as IList<Product> ?? new List<Product>();

            // Convert to queryable to enable search filtering
            var query = products.AsQueryable();

            // Search by ProductId
            if (!string.IsNullOrEmpty(SearchId) && int.TryParse(SearchId, out int id))
            {
                query = query.Where(p => p.ProductId == id);
            }

            // Search by ProductName
            if (!string.IsNullOrEmpty(SearchName))
            {
                query = query.Where(p => p.ProductName.Contains(SearchName, StringComparison.OrdinalIgnoreCase));
            }

            // Search by ProductType
            if (!string.IsNullOrEmpty(SearchCategory) && int.TryParse(SearchCategory, out int Id))
            {
                if (Id != 0)
                {
                    query = query.Where(p => p.CategoryId == Id);
                }
            }

            Products = query.ToList();
            return Page();
        }
    }
}
