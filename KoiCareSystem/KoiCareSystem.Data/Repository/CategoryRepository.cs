using KoiCareSystem.Data.Base;
using KoiCareSystem.Data.DBContext;
using KoiCareSystem.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace KoiCareSystem.Data.Repository
{
   public class CategoryRepository : GenericRepository<Category>
    {
        public CategoryRepository()
        {
        }

        public CategoryRepository(ApplicationDbContext context) => _context = context;

        public async Task<Category> GetByIdAsync(int id)
        {
            //return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            return await _context.Categories.Where(u => u.Id == id).FirstOrDefaultAsync();
        }
        public async Task<Category> GetByNameAsync(string name)
        {
            //return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            return await _context.Categories.Where(u => u.Name == name).FirstOrDefaultAsync();
        }
    }
}
