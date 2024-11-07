using KoiCareSystem.Data.Base;
using KoiCareSystem.Data.DBContext;
using KoiCareSystem.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace KoiCareSystem.Data.Repository
{
    public class OrderRepository : GenericRepository<Order>
    {
        public OrderRepository()
        {
        }

        public OrderRepository(ApplicationDbContext context) => _context = context;
        public async Task<List<Order>> GetAllAsync()
        {
            return await _context.Orders.Include(x => x.User).Include(x => x.Status).ToListAsync();
        }
        public async Task<List<Order>> GetByUserIdAsync(int userId)
        {
            return await _context.Orders.Include(x => x.User).Include(x => x.Status).Where(x => x.UserId == userId).ToListAsync();
        }
        public async Task<Order> GetByOrderIdAsync(int orderId)
        {
            return await _context.Orders.Include(x => x.User).Include(x => x.Status).FirstOrDefaultAsync(x => x.OrderId == orderId);
        }

        // Kiểm tra sản phẩm có tồn tại không
        public bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }
    }
}