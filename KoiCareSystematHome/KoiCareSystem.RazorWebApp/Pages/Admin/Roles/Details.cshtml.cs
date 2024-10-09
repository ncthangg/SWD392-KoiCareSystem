using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using KoiCareSystem.Data.DBContext;
using KoiCareSystem.Data.Models;
using AutoMapper;
using KoiCareSystem.Service;

namespace KoiCareSystem.RazorWebApp.Pages.Roles
{
    public class DetailsModel : PageModel
    {
        private readonly UserService _userService;
        private readonly RoleService _roleService;

        public DetailsModel(IMapper mapper)
        {
            _userService ??= new UserService(mapper);
            _roleService ??= new RoleService(mapper);
        }

        public Role Role { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _roleService.GetRoleById((long)id);
            if (role == null)
            {
                return NotFound();
            }
            else
            {
                Role = (Role)role.Data;
            }
            return Page();
        }
    }
}
