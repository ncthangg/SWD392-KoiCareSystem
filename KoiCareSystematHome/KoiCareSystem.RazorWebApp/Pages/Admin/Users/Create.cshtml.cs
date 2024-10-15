using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using KoiCareSystem.Data.DBContext;
using KoiCareSystem.Data.Models;
using KoiCareSystem.Service;
using KoiCareSystematHome.Service;
using AutoMapper;
using KoiCareSystem.Common.DTOs.Request;

namespace KoiCareSystem.RazorWebApp.Pages.Admin.Users
{
    public class CreateModel : PageModel
    {
        private readonly UserService _userService;
        private readonly RoleService _roleService;

        public CreateModel(IMapper mapper)
        {
            _userService ??= new UserService(mapper);
            _roleService ??= new RoleService(mapper);
        }

        public IActionResult OnGet()
        {
            var roles = _roleService.GetAllRole().Result.Data as IList<Role>;
            // Kiểm tra null trước khi lọc
            var filteredRoles = roles?.Where(r => r.Name != "Guest").ToList() ?? new List<Role>();

            ViewData["RoleId"] = new SelectList(filteredRoles, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public RequestRegisterAdminDto RegisterAdminDto { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _userService.SaveByAdmin(RegisterAdminDto);

            return RedirectToPage("./Index");
        }
    }
}
