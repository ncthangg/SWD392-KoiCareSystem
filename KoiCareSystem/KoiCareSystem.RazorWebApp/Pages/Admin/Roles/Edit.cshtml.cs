using System;
using Microsoft.AspNetCore.Mvc;
using KoiCareSystem.RazorWebApp.PageBase;
using KoiCareSystem.Data.Models;
using AutoMapper;
using KoiCareSystem.Service;

namespace KoiCareSystem.RazorWebApp.Pages.Admin.Roles
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
        public Role Role { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role =  await _roleService.GetById((int)id);
            if (role == null)
            {
                return NotFound();
            }
            Role = (Role)role.Data;
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

            if (!_roleService.RoleExists(Role.Id))
            {
                return NotFound();
            }
            else
            {
                await _roleService.Save(Role);
            }

            if (!_roleService.RoleExists(Role.Id))
            {
                return NotFound();
            }

            return RedirectToPage("./Index");
        }


    }
}
