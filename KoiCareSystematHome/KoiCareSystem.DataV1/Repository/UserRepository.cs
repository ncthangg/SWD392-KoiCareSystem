using KoiCareSystem.Data.Base;
using KoiCareSystem.Data.DBContext;
using KoiCareSystem.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace KoiCareSystem.Data.Repository
{
    public class UserRepository : GenericRepository<User>
    {
        public UserRepository()
        {
        }
        public UserRepository(ApplicationDbContext context) => _context = context;
        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users.Include(x => x.Role).ToListAsync();
        }
        public async Task<User> GetByIdAsync(int id)
        {
            //return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            return await _context.Users.Include(x => x.Role).Where(u => u.Id == id).FirstOrDefaultAsync();
        }
        public async Task<User> GetByEmailAsync(string email)
        {
            //return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            return await _context.Users.Include(x => x.Role).Where(u => u.Email == email).FirstOrDefaultAsync();
        }
        // Kiểm tra User có tồn tại không
        public bool UserExists(string email)
        {
            return _context.Users.Any(e => e.Email == email);
        }
        public bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

    }
}
