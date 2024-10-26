using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KoiCareSystem.Data.DBContext;
using KoiCareSystem.Data.Models;
using AutoMapper;
using KoiCareSystem.Service;
using KoiCareSystem.Common.DTOs.Request;
using KoiCareSystem.RazorWebApp.PageBase;

namespace KoiCareSystem.RazorWebApp.Pages.Admin.Users
{
    public class EditModel : BasePageModel
    {
        private readonly UserService _userService;
        private readonly RoleService _roleService;

        public EditModel(IMapper mapper)
        {
            _userService ??= new UserService(mapper);
            _roleService ??= new RoleService(mapper);
        }

        [BindProperty]
        public RequestRegisterAdminDto RequestRegisterAdminDto { get; set; } = default!;
        public User User { get; set; } = default!;
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userService.GetById((int)id);
            if (user == null)
            {
                return NotFound();
            }
            User = (User)user.Data;

            //Role
            var roles = _roleService.GetAllRole().Result.Data as IList<Role>;
            // Kiểm tra null trước khi lọc
            var filteredRoles = roles?.Where(r => r.Name != "Guest").ToList() ?? new List<Role>();
            ViewData["RoleId"] = new SelectList(filteredRoles, "Id", "Name");

            // Điền email của User vào DTO
            RequestRegisterAdminDto = new RequestRegisterAdminDto
            {
                Email = User.Email,
                RoleId = User.RoleId
            };
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            if (!_userService.UserEmailExists(RequestRegisterAdminDto.Email))
            {
                return NotFound();
            }
            else
            {
                await _userService.SaveByAdmin(RequestRegisterAdminDto);
            }

            return RedirectToPage("./Index");
        }

    }
}
