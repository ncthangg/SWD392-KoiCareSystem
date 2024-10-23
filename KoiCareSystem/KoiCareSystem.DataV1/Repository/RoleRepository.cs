using KoiCareSystem.Data.Base;
using KoiCareSystem.Data.DBContext;
using KoiCareSystem.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace KoiCareSystem.Data.Repository
{
    public class RoleRepository : GenericRepository<Role>
    {
        public RoleRepository()
        {
        }
        public RoleRepository(ApplicationDbContext context) => _context = context;

        public async Task<Role> GetByNameAsync(string name)
        {
            //return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            return await _context.Roles.Where(u => u.Name == name).FirstOrDefaultAsync();
        }


        // Kiểm tra Role có tồn tại không
        public bool RoleExists(int id)
        {
            return _context.Roles.Any(e => e.Id == id);
        }
    }
}
