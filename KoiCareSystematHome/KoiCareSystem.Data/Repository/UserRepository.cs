﻿using KoiCareSystem.Data.Base;
using KoiCareSystem.Data.DBContext;
using KoiCareSystem.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiCareSystem.Data.Repository
{
    public class UserRepository : GenericRepository<User>
    {
        //private readonly FA24_SE1702_PRN221_G5_KoiCareSystematHomeContext _context;
        public UserRepository()
        {
        }
        public UserRepository(FA24_SE1702_PRN221_G5_KoiCareSystematHomeContext context) => _context = context;
        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users.Include(x => x.Role).ToListAsync();
        }
        //public async Task<List<User>> GetByEmailAsync(string email)
        //{
        //    return await _context.Users.Include(x => x.Role).Where(x => x.Email == email).ToListAsync();
        //}
        public async Task<User> GetByEmailAsync(string email)
        {
            //return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            return await _context.Users.Include(x => x.Role).Where(u => u.Email == email).FirstOrDefaultAsync();
        }
        //// Kiểm tra User có role gì
        //public async Task<long> GetUserRole(long email)
        //{
        //    var user = await _context.Users.Include(x => x.Role).Where(u => u.Email == email).FirstOrDefaultAsync();
        //    return user.RoleId;
        //}

        // Kiểm tra User có tồn tại không
        public bool UserExists(string email)
        {
            return _context.Users.Any(e => e.Email == email);
        }


    }
}
