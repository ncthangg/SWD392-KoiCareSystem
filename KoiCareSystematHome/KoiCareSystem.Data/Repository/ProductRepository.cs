using KoiCareSystem.Data.Base;
using KoiCareSystem.Data.DBContext;
using KoiCareSystem.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiCareSystem.Data.Repository
{
    public class ProductRepository : GenericRepository<Product>
    {
        public ProductRepository()
        {
        }

        public ProductRepository(FA24_SE1702_PRN221_G5_KoiCareSystematHomeContext context) => _context = context;

        // Kiểm tra sản phẩm có tồn tại không
        public bool ProductExists(long id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }

        // Nối bảng Category
        public async Task<Product> GetByIdAsync(long id)
        {
            return await _context.Products.Include(e => e.Category).FirstOrDefaultAsync(x => x.ProductId == id);    
        }
        public async Task<List<Product>> GetAllAsync()
        {
            return await _context.Products.Include(e=>e.Category).ToListAsync();
        }
    }
}
