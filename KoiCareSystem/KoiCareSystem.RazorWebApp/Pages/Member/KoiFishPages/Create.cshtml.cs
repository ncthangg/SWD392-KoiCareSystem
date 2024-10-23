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
using KoiCareSystem.Common.DTOs;

namespace KoiCareSystem.RazorWebApp.Pages.Member.KoiFishPages
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public KoiFish KoiFish { get; set; } = default!;
        [BindProperty]
        public IFormFile ImageFile { get; set; }
        public int UserId { get; set; }
        //========================================================
        private readonly KoiFishService _koiFishService;
        private readonly PondService _pondService;
        private readonly UserService _userService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CreateModel(KoiFishService koiFishService, PondService pondService, UserService userService, IWebHostEnvironment webHostEnvironment)
        {
            _koiFishService = koiFishService;
            _pondService = pondService;
            _userService = userService;
            _webHostEnvironment = webHostEnvironment;
        }
        //========================================================
        public async Task<IActionResult> OnGet()
        {
            UserId = (int)UserSession.UserId;
            var ponds = (await _pondService.GetByUserId(UserId)).Data as List<Pond>;

            if (ponds == null || !ponds.Any())
            {
                TempData["ErrorMessage"] = "Bạn cần tạo ít nhất một hồ cá trước khi tạo thông tin cá Koi.";
                return RedirectToPage("/Member/KoiFishPages/Index"); // Chuyển hướng về trang danh sách hồ cá
            }

            ViewData["PondId"] = new SelectList(ponds, "PondId", "PondName");

            return Page();
        }

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            UserId = (int)UserSession.UserId; // Get the UserId from the session
            KoiFish.UserId = UserId; // Set the UserId for KoiFish

            if (!ModelState.IsValid)
            {
                var ponds = (await _pondService.GetByUserId(UserId)).Data as List<Pond>;

                ViewData["PondId"] = new SelectList(ponds, "PondId", "PondName");
                ModelState.AddModelError(string.Empty, "Cập nhật không thành công.");
                return Page();
            }

            if (ImageFile != null)
            {
                // Đường dẫn lưu file trong wwwroot
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/koifishs");
                Directory.CreateDirectory(uploadsFolder);  // Tạo thư mục nếu chưa có

                // Đặt tên file duy nhất
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + ImageFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Lưu file vào thư mục
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(fileStream);
                }

                // Cập nhật đường dẫn ảnh trong model
                KoiFish.ImageUrl = "/images/koifishs" + uniqueFileName;
            }

            var a = _koiFishService.Create(KoiFish);

            return RedirectToPage("./Index");
        }
    }
}
