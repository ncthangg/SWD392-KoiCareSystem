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
    public class PondRepository : GenericRepository<Pond>
    {
        public PondRepository()
        {

        }

        public PondRepository(ApplicationDbContext context) => _context = context;

        public void UpdateEntity(Pond existingEntity, Pond newEntity)
        {
            //_context.Entry(existingEntity).CurrentValues.SetValues(newEntity);
            // Gán giá trị mới cho các thuộc tính của thực thể
            _context.Entry(existingEntity).CurrentValues.SetValues(newEntity);

            // Đánh dấu toàn bộ thực thể là đã thay đổi
            foreach (var property in _context.Entry(existingEntity).Properties)
            {
                if (property.Metadata.Name != nameof(Pond.PondId))
                {
                    property.IsModified = true;
                }
            }

            _context.Entry(existingEntity).Property(p => p.UpdatedAt).IsModified = true;
        }
        public void Update(Pond pondToUpdate)
        {
            _context.Entry(pondToUpdate).State = EntityState.Modified;
        }
        public IQueryable<Pond> GetAllQueryableAsync()
        {
            try
            {
                return _context.Ponds;  // Trả về IQueryable
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<Pond>> GetByUserIdAsync(int userId)
        {
            // Fetch orders based on the userId
            var ponds = await _context.Ponds.Where(o => o.UserId == userId).ToListAsync();
            
            return ponds;
        }
        public async Task<Pond> GetByIdAsync(int id)
        {
            //return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            return await _context.Ponds.Include(x => x.User).Where(u => u.PondId == id).FirstOrDefaultAsync();
        }
    }
}
