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
    public class RoleRepository : GenericRepository<Role>
    {
        //private readonly FA24_SE1702_PRN221_G5_KoiCareSystematHomeContext _context;
        public RoleRepository()
        {
        }
        public RoleRepository(FA24_SE1702_PRN221_G5_KoiCareSystematHomeContext context) => _context = context;

        public async Task<Role> GetByNameAsync(string name)
        {
            //return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            return await _context.Roles.Where(u => u.Name == name).FirstOrDefaultAsync();
        }


        // Kiểm tra Role có tồn tại không
        public bool RoleExists(long id)
        {
            return _context.Roles.Any(e => e.Id == id);
        }
    }
}
