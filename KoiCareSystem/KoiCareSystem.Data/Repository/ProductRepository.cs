using KoiCareSystem.Data.Base;
using KoiCareSystem.Data.DBContext;
using KoiCareSystem.Data.Models;
using Microsoft.EntityFrameworkCore;
namespace KoiCareSystem.Data.Repository
{
    public class ProductRepository : GenericRepository<Product>
    {
        public ProductRepository()
        {
        }

        public ProductRepository(ApplicationDbContext context) => _context = context;

        // Nối bảng Category
        public async Task<Product> GetByIdAsync(int id)
        {
            return await _context.Products.Include(e => e.Category).FirstOrDefaultAsync(x => x.ProductId == id);
        }
        public async Task<List<Product>> GetByCategoryNameAsync(List<string> names)
        {
            return await _context.Products
                .Include(e => e.Category) // Lấy thông tin danh mục
                .Where(p => names.Contains(p.Category.Name)) // Lọc sản phẩm theo tên danh mục
                .ToListAsync(); // Trả về danh sách sản phẩm
        }
        public async Task<List<Product>> GetAllAsync()
        {
            return await _context.Products.Include(e => e.Category).ToListAsync();
        }
        // Kiểm tra sản phẩm có tồn tại không
        public bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
