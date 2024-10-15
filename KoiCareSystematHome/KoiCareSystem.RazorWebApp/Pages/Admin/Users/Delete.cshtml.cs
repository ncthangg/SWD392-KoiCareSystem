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
using System.Data;

namespace KoiCareSystem.RazorWebApp.Pages.Admin.Users
{
    public class DeleteModel : PageModel
    {
        private readonly UserService _userService;
        private readonly RoleService _roleService;

        public DeleteModel(IMapper mapper)
        {
            _userService ??= new UserService(mapper);
            _roleService ??= new RoleService(mapper);
        }

        [BindProperty]
        public User User { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user =  await _userService.GetUserById((long)id);

            if (user == null)
            {
                return NotFound();
            }
            else
            {
                User = (User)user.Data;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userService.GetUserById((long)id);
            if (user != null)
            {
                User = (User)user.Data;
                await _userService.DeleteUserById((long)id);
            }

            return RedirectToPage("./Index");
        }
    }
}
