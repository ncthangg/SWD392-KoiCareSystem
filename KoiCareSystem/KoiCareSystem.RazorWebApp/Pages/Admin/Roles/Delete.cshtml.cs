using Microsoft.AspNetCore.Mvc;
using KoiCareSystem.RazorWebApp.PageBase;
using KoiCareSystem.Data.Models;
using AutoMapper;
using KoiCareSystem.Service;

namespace KoiCareSystem.RazorWebApp.Pages.Admin.Roles
{
    public class DeleteModel : BasePageModel
    {
        private readonly UserService _userService;
        private readonly RoleService _roleService;

        public DeleteModel(IMapper mapper)
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

            var role = await _roleService.GetById((int)id);

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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _roleService.GetById((int)id);
            if (role != null)
            {
                Role = (Role)role.Data;
                await _roleService.DeleteById((int)id);
            }

            return RedirectToPage("./Index");
        }
    }
}
