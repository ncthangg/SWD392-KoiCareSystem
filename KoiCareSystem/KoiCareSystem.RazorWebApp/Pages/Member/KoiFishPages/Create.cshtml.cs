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
using KoiCareSystem.RazorWebApp.PageBase;

namespace KoiCareSystem.RazorWebApp.Pages.Member.KoiFishPages
{
    public class CreateModel : BasePageModel
    {
        [BindProperty]
        public KoiFish KoiFish { get; set; } = default!;

        [BindProperty]
        public IFormFile ImageFile { get; set; }
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
            LoadUserIdFromSession();

            if (UserId == null)
            {
                return RedirectToPage("/Guest/Login"); // Điều hướng đến trang đăng nhập nếu không có UserId trong session
            }
            var ponds = (await _pondService.GetByUserId((int)UserId)).Data as List<Pond>;

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
            LoadUserIdFromSession();

            if (UserId == null)
            {
                return RedirectToPage("/Guest/Login"); // Điều hướng đến trang đăng nhập nếu không có UserId trong session
            }

            if (!ModelState.IsValid)
            {
                var ponds = (await _pondService.GetByUserId((int)UserId)).Data as List<Pond>;

                ViewData["PondId"] = new SelectList(ponds, "PondId", "PondName");
                ModelState.AddModelError(string.Empty, "Cập nhật không thành công.");
                return Page();
            }

            if (ImageFile != null)
            {
                // Đường dẫn lưu file trong wwwroot
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/koi/");
                Directory.CreateDirectory(uploadsFolder);

                // Đường dẫn lưu file trong mobile
                string externalUploadsFolder = Path.Combine("/app/external_images/koi");
                Directory.CreateDirectory(externalUploadsFolder); 

                // Đặt tên file duy nhất
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + ImageFile.FileName;

                // Lưu file vào wwwroot
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(fileStream);
                }

                // Lưu file vào mobile
                string externalFilePath = Path.Combine(externalUploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(externalFilePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(fileStream);
                }

                // Cập nhật đường dẫn ảnh trong model
                KoiFish.ImageUrl = "/images/koi/" + uniqueFileName;
            }
            KoiFish.UserId = UserId;
            var a = _koiFishService.Create(KoiFish);

            return RedirectToPage("./Index");
        }
    }
}
