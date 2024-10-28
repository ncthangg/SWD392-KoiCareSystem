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

    public class KoiGrowthLogRepository : GenericRepository<KoiGrowthLog>
    {
        public KoiGrowthLogRepository()
        {
        }

        public KoiGrowthLogRepository(ApplicationDbContext context) => _context = context;

        
         public async Task<List<KoiGrowthLog>> GetByFishIdAsync(int fishId)
        {
            // Fetch orders based on the userId
            return await _context.KoiGrowthLogs
                .Include(e => e.Fish)
                .Where(o => o.FishId == fishId)
                .ToListAsync();
        }
    }
}
