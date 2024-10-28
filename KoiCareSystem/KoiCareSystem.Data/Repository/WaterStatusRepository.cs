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
    public class WaterStatusRepository : GenericRepository<WaterStatus>
    {
        public WaterStatusRepository()
        {
        }
        public WaterStatusRepository(ApplicationDbContext context) => _context = context;
        public async Task<WaterStatus> GetByNameAsync(string name)
        {
            return await _context.WaterStatuses.Where(u => u.StatusName == name).FirstOrDefaultAsync();
        }
        // Kiểm tra Status có tồn tại không
        public bool StatusExists(int id)
        {
            return _context.WaterStatuses.Any(e => e.StatusId == id);
        }
    }
}
