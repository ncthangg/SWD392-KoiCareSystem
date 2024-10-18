using KoiCareSystem.Data.Base;
using KoiCareSystem.Data.DBContext;
using KoiCareSystem.Data.Models;

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

    }
}
