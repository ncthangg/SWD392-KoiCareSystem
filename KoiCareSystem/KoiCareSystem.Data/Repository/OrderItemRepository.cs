using KoiCareSystem.Data.Base;
using KoiCareSystem.Data.DBContext;
using KoiCareSystem.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace KoiCareSystem.Data.Repository
{
    public class OrderItemRepository : GenericRepository<OrderItem>
    {
        public OrderItemRepository()
        {
        }

        public OrderItemRepository(ApplicationDbContext context) => _context = context;

        public async Task<List<OrderItem>> GetByOrderIdAsync(int orderId)
        {
            return await _context.OrderItems.Include(x => x.Order)
                                           .Include(x => x.Product)
                                           .Where(x => x.OrderId == orderId)
                                           .ToListAsync();
        }
        public async Task<OrderItem> GetByIdAsync(int id)
        {
            return await _context.OrderItems.Where(x => x.Id == id).FirstOrDefaultAsync();
        }
        public async Task<OrderItem> GetByProductIdAsync(int productId)
        {
            return await _context.OrderItems.Where(x => x.ProductId == productId).FirstOrDefaultAsync();
        }
        public async Task<OrderItem> GetItemByOrderIdAndProductIdAsync(int orderId, int productId)
        {
            return await _context.OrderItems
                .Where(x => x.OrderId == orderId && x.ProductId == productId)
                .FirstOrDefaultAsync();
        }

        // Kiểm tra Role có tồn tại không
        public bool ProductExists(int orderId, int productId)
        {
            return _context.OrderItems.Any(e => e.OrderId == orderId && e.ProductId == productId);
        }

    }
}
