using System;
using KoiCareSystem.RazorWebApp.PageBase;
using KoiCareSystem.Data.Models;
using AutoMapper;
using KoiCareSystem.Service;

namespace KoiCareSystem.RazorWebApp.Pages.Admin.Roles
{
    public class IndexModel : BasePageModel
    {
        private readonly UserService _userService;
        private readonly RoleService _roleService;

        public IndexModel(IMapper mapper)
        {
            _userService ??= new UserService(mapper);
            _roleService ??= new RoleService(mapper);
        }

        public IList<Role> Role { get; set; } = default!;

        public async Task OnGetAsync()
        {

            var result = await _roleService.GetAllRole();
            if (result.Status > 0)
            {
                Role = (IList<Role>)result.Data;
            }
        }
    }
}
