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
    public class WaterParameterLimitRepository : GenericRepository<WaterParameterLimit>
    {
        public WaterParameterLimitRepository()
        {
        }
        public WaterParameterLimitRepository(ApplicationDbContext context) => _context = context;
        public void UpdateEntity(WaterParameterLimit existingEntity, WaterParameterLimit newEntity)
        {
            //_context.Entry(existingEntity).CurrentValues.SetValues(newEntity);
            // Gán giá trị mới cho các thuộc tính của thực thể
            _context.Entry(existingEntity).CurrentValues.SetValues(newEntity);

            // Đánh dấu toàn bộ thực thể là đã thay đổi
            foreach (var property in _context.Entry(existingEntity).Properties)
            {
                if (property.Metadata.Name != nameof(WaterParameterLimit.ParameterId))
                {
                    property.IsModified = true;
                }
            }

            _context.Entry(existingEntity).Property(p => p.UpdatedAt).IsModified = true;
        }
    }
}
