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
    public class OrderRepository : GenericRepository<Order>
    {
        public OrderRepository()
        {
        }

        public OrderRepository(FA24_SE1702_PRN221_G5_KoiCareSystematHomeContext context) => _context = context;
        public async Task<List<Order>> GetAllAsync()
        {
            return await _context.Orders.Include(x => x.User).Include(x => x.Status).ToListAsync();
        }
        public async Task<List<Order>> GetByUserIdAsync(long userId)
        {
            return await _context.Orders.Include(x => x.User).Include(x => x.Status).Where(x => x.UserId == userId).ToListAsync();
        }
        public async Task<Order> GetByOrderIdAsync(long orderId)
        {
            return await _context.Orders.Include(x => x.User).Include(x => x.Status).FirstOrDefaultAsync(x => x.OrderId == orderId);
        }

        // Kiểm tra sản phẩm có tồn tại không
        public bool OrderExists(long id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }
    }
}
