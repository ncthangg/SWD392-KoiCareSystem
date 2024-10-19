using KoiCareSystem.Data.Base;
using KoiCareSystem.Data.DBContext;
using KoiCareSystem.Data.Models;
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
    }
}
