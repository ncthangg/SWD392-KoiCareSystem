using KoiCareSystem.Data.Base;
using KoiCareSystem.Data.DBContext;
using KoiCareSystem.Data.Models;

namespace KoiCareSystem.Data.Repository
{
    public class PaymentRepository : GenericRepository<Payment>
    {
        public PaymentRepository()
        {
        }

        public PaymentRepository(ApplicationDbContext context) => _context = context;

    }
}
