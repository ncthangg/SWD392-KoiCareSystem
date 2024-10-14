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
    public class OrderItemRepository : GenericRepository<OrderItem>
    {
        public OrderItemRepository()
        {
        }

        public OrderItemRepository(FA24_SE1702_PRN221_G5_KoiCareSystematHomeContext context) => _context = context;

        public async Task<List<OrderItem>> GetByOrderIdAsync(long orderId)
        {
            return await _context.OrderItems.Include(x => x.Order)
                                           .Include(x => x.Product)
                                           .Where(x => x.OrderId == orderId)
                                           .ToListAsync();
        }
        public async Task<OrderItem> GetByIdAsync(long id)
        {
            return await _context.OrderItems.Where(x => x.Id == id).FirstOrDefaultAsync();
        }
        public async Task<OrderItem> GetByProductIdAsync(long productId)
        {
            return await _context.OrderItems.Where(x => x.ProductId == productId).FirstOrDefaultAsync();
        }
        public async Task<OrderItem> GetItemByOrderIdAndProductIdAsync(long orderId, long productId)
        {
            return await _context.OrderItems
                .Where(x => x.OrderId == orderId && x.ProductId == productId)
                .FirstOrDefaultAsync();
        }

        // Kiểm tra Role có tồn tại không
        public bool ProductExists(long id)
        {
            return _context.OrderItems.Any(e => e.ProductId == id);
        }

    }
}
