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
using KoiCareSystem.Common;
using KoiCareSystem.Service;
using KoiCareSystem.Common.DTOs;
using KoiCareSystem.RazorWebApp.PageBase;

namespace KoiCareSystem.RazorWebApp.Pages.Member.KoiFishPages
{
    public class EditModel : BasePageModel
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

        public EditModel(KoiFishService koiFishService, PondService pondService, UserService userService, IWebHostEnvironment webHostEnvironment)
        {
            _koiFishService = koiFishService;
            _pondService = pondService;
            _userService = userService;
            _webHostEnvironment = webHostEnvironment;
        }
        //========================================================
        public async Task<IActionResult> OnGetAsync(int id)
        {
            LoadUserIdFromSession();

            if (UserId == null)
            {
                return RedirectToPage("/Guest/Login"); // Điều hướng đến trang đăng nhập nếu không có UserId trong session
            }
            // Lấy cá Koi theo id
            var result = await _koiFishService.GetByIdWithIncludeAsync(id);
            if (result.Data == null)
            {
                return NotFound();
            }
            KoiFish = (KoiFish)result.Data;

            var ponds = (await _pondService.GetByUserId((int)UserId)).Data as List<Pond>;
            var users = (await _userService.GetAll()).Data as List<User>;

            ViewData["PondId"] = new SelectList(ponds, "PondId", "PondName");
            ViewData["UserId"] = new SelectList(users, "Id", "Email");

            //// Lấy danh sách User
            //var usersResult = await _koiFishService.GetUsers();
            //if (usersResult.Status == Const.SUCCESS_READ_CODE && usersResult.Data != null && ((IEnumerable<User>)usersResult.Data).Any())
            //{
            //    var users = (IEnumerable<User>)usersResult.Data;
            //    var userIds = users.Select(x => new { Id = x.Id }).ToList();
            //    ViewData["UserId"] = new SelectList(userIds, "Id", "Id", KoiFish.UserId);
            //}
            //else
            //{
            //    // Nếu không có dữ liệu hoặc danh sách rỗng
            //    ViewData["UserId"] = new SelectList(Enumerable.Empty<object>(), "Id", "UserId");
            //}

            //// Lấy danh sách Pond
            //var pondsResult = await _koiFishService.GetPonds();
            //if (pondsResult.Status == Const.SUCCESS_READ_CODE && pondsResult.Data != null && ((IEnumerable<Pond>)pondsResult.Data).Any())
            //{
            //    var ponds = (IEnumerable<Pond>)pondsResult.Data;
            //    var pondsId = ponds.Select(x => new { PondId = x.PondId }).ToList();
            //    ViewData["PondId"] = new SelectList(pondsId, "PondId", "PondId", KoiFish.PondId);

            //    var a = KoiFish;
            //}
            //else
            //{
            //    // Nếu không có dữ liệu hoặc danh sách rỗng
            //    ViewData["PondId"] = new SelectList(Enumerable.Empty<object>(), "PondId", "PondId");
            //}

            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var ponds = (await _pondService.GetAll()).Data as List<Pond>;
                var users = (await _userService.GetAll()).Data as List<User>;

                ViewData["PondId"] = new SelectList(ponds, "PondId", "PondName");
                ViewData["UserId"] = new SelectList(users, "Id", "Email");

                ModelState.AddModelError(string.Empty, "Cập nhật không thành công.");
                return Page();
            }

            try
            {
                if (ImageFile != null)
                {
                    // Đường dẫn lưu file trong wwwroot
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/koifishs/");
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
                    KoiFish.ImageUrl = "/images/koifishs/" + uniqueFileName;
                }

                var updateResult = await _koiFishService.Update(KoiFish);
                if (updateResult.Status != Const.SUCCESS_UPDATE_CODE)
                {
                    //Handle error or failure in the service layer
                    return BadRequest(updateResult.Message);
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await KoiFishExists(KoiFish.FishId))
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

        private async Task<bool> KoiFishExists(int id)
        {
            var result = await _koiFishService.GetById(id);
            return result.Data != null;
        }
    }
}
