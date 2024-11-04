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
    public class WaterParameterRepository : GenericRepository<WaterParameter>
    {
        public WaterParameterRepository()
        {
        }
        public WaterParameterRepository(ApplicationDbContext context) => _context = context;
        public async Task<WaterParameter> GetByIdAsync(int id)
        {
            // Fetch orders based on the userId
            return await _context.WaterParameters
                .Include(e => e.Status)
                .FirstOrDefaultAsync(o => o.ParameterId == id);
        }
        public async Task<List<WaterParameter>> GetByPondIdAsync(int pondId)
        {
            // Fetch orders based on the userId
            return await _context.WaterParameters
                .Include(e => e.Status)
                .Where(o => o.PondId == pondId)
                .ToListAsync();
        }
        public async Task<WaterParameter> GetLatestByPondIdAsync(int pondId)
        {
            return await _context.WaterParameters
                .Include(e => e.Status)
                .Where(param => param.PondId == pondId) // Lọc theo PondId
                .OrderByDescending(param => param.CreatedAt) // Sắp xếp theo thời gian tạo giảm dần
                .FirstOrDefaultAsync(); // Lấy bản ghi đầu tiên (mới nhất)
        }

        public void UpdateEntity(WaterParameter existingEntity, WaterParameter newEntity)
        {
            //_context.Entry(existingEntity).CurrentValues.SetValues(newEntity);
            // Gán giá trị mới cho các thuộc tính của thực thể
            _context.Entry(existingEntity).CurrentValues.SetValues(newEntity);

            // Đánh dấu toàn bộ thực thể là đã thay đổi
            foreach (var property in _context.Entry(existingEntity).Properties)
            {
                if (property.Metadata.Name != nameof(WaterParameter.ParameterId))
                {
                    property.IsModified = true;
                }
            }

            _context.Entry(existingEntity).Property(p => p.UpdatedAt).IsModified = true;
        }
    }
}
