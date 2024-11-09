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
using KoiCareSystem.Common.DTOs;
using KoiCareSystem.Service;
using KoiCareSystem.Common;
using KoiCareSystem.RazorWebApp.PageBase;

namespace KoiCareSystem.RazorWebApp.Pages.Member.PondPages
{
    public class EditModel : BasePageModel
    {
        private readonly KoiFishService _koiFishService;
        private readonly PondService _pondService;
        private readonly UserService _userService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EditModel(KoiFishService koiFishService, PondService pondService, UserService userService, IWebHostEnvironment webHostEnvironment)
        {
            _koiFishService = koiFishService;
            _pondService = pondService;
            _userService = userService;
            _webHostEnvironment = webHostEnvironment;
        }
        //========================================================
        [BindProperty]
        public Pond Pond { get; set; } = default!;
        [BindProperty]
        public IFormFile ImageFile { get; set; }
        //========================================================
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            LoadUserIdFromSession();

            if (UserId == null)
            {
                return RedirectToPage("/Guest/Login"); // Điều hướng đến trang đăng nhập nếu không có UserId trong session
            }

            var pond =  await _pondService.GetById((int)id);
            if (pond == null)
            {
                return NotFound();
            }
            Pond = (Pond)pond.Data;
            Pond.UserId = (int)UserId;
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
            try
            {
                if (ImageFile != null)
                {
                    // Đường dẫn lưu file trong wwwroot
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/ponds/");
                    Directory.CreateDirectory(uploadsFolder);  // Tạo thư mục nếu chưa có

                    // Đường dẫn lưu file trong mobile
                    string externalUploadsFolder = Path.Combine("/app/external_images/ponds");
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
                    Pond.ImageUrl = "/images/ponds/" + uniqueFileName;
                }

                var updateResult = await _pondService.Update(Pond);
                if (updateResult.Status != Const.SUCCESS_UPDATE_CODE)
                {
                    //Handle error or failure in the service layer
                    return BadRequest(updateResult.Message);
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await PondExists(Pond.PondId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            
            return RedirectToPage("./Index");
        }

        private async Task<bool> PondExists(int id)
        {
            var result = await _pondService.GetById(id);
            return result.Data != null;
        }
    }
}
