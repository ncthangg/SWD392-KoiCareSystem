using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using KoiCareSystem.Data.Models;
using KoiCareSystem.Service;
using AutoMapper;

namespace KoiCareSystem.RazorWebApp.Pages.Shop.Products
{
    public class IndexModel : PageModel
    {
        private readonly ProductService _productService;
        private readonly IMapper _mapper;

        public IndexModel(IMapper mapper)
        {
            _productService ??= new ProductService(mapper);
            _mapper = mapper;
        }
        //========================================================
        public IList<Product> Products { get; set; } = new List<Product>();

        // Define the search properties
        [BindProperty(SupportsGet = true)]
        public string SearchId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchName { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchType { get; set; }
        //========================================================
        public async Task OnGetAsync()
        {
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
            if (!string.IsNullOrEmpty(SearchType))
            {
                query = query.Where(p => p.ProductType.Contains(SearchType, StringComparison.OrdinalIgnoreCase));
            }

            Products = query.ToList();
        }
    }
}
