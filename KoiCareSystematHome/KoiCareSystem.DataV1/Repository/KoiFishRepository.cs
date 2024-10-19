using KoiCareSystem.Data.Base;
using KoiCareSystem.Data.DBContext;
using KoiCareSystem.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace KoiCareSystem.Data.Repository
{
    public class KoiFishRepository : GenericRepository<KoiFish>
    {
        public KoiFishRepository() { }

        public KoiFishRepository(ApplicationDbContext context) => _context = context;

        public void UpdateEntity(KoiFish existingEntity, KoiFish newEntity)
        {
            _context.Entry(existingEntity).CurrentValues.SetValues(newEntity);
        }
        public IQueryable<KoiFish> GetAllQueryableAsync()
        {
            try
            {
                return _context.KoiFishes;  // Trả về IQueryable
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<KoiFish>> GetByUserIdAsync(int userId)
        {
            // Fetch orders based on the userId
            return await _context.KoiFishes
                .Include(e => e.Pond)
                .Where(o => o.UserId == userId)
                .ToListAsync();
        }
        public async Task<List<KoiFish>> GetByPondIdAsync(int pondId)
        {
            // Fetch orders based on the userId
            return await _context.KoiFishes
                .Include(e => e.Pond)
                .Where(o => o.PondId == pondId)
                .ToListAsync();
        }
        public async Task<KoiFish> GetByIdAsync(int id)
        {
            // Fetch orders based on the userId
            return await _context.KoiFishes
                   .Include(e => e.Pond)
                   .FirstOrDefaultAsync(o => o.FishId == id);
        }
    }
}
