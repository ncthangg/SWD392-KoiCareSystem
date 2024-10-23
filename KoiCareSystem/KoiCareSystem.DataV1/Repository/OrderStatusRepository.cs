using KoiCareSystem.Data.Base;
using KoiCareSystem.Data.DBContext;
using KoiCareSystem.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace KoiCareSystem.Data.Repository
{
    public class OrderStatusRepository : GenericRepository<OrderStatus>
    {
        public OrderStatusRepository()
        {
        }

        public OrderStatusRepository(ApplicationDbContext context) => _context = context;
        public async Task<List<OrderStatus>> GetAllAsync()
        {
            return await _context.OrderStatuses.ToListAsync();
        }

    }
}
