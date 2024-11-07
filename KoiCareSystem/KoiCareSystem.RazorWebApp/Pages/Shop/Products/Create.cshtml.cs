using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using KoiCareSystem.Data.Models;
using KoiCareSystem.Service;
using KoiCareSystem.Common.DTOs.Request;
using AutoMapper;
using KoiCareSystem.RazorWebApp.PageBase;
using Microsoft.AspNetCore.Hosting;

namespace KoiCareSystem.RazorWebApp.Pages.Shop.Products
{
    public class CreateModel : BasePageModel
    {
        private readonly ProductService _productService;
        private readonly CategoryService _categoryService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CreateModel(IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _productService ??= new ProductService(mapper);
            _categoryService ??= new CategoryService();
            _webHostEnvironment = webHostEnvironment;
        }
        //========================================================
        [BindProperty]
        public RequestCreateANewProductDto RequestCreateANewProductDto { get; set; } = default!;
        [BindProperty]
        public IFormFile ImageFile { get; set; }
        //========================================================
        public ActionResult OnGet()
        {
            LoadUserIdFromSession();
            var categories = _categoryService.GetAll().Result.Data as IList<Category>;
            ViewData["CategoryId"] =  new SelectList(categories, "Id", "Description");
            return Page();
        }


        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            LoadUserIdFromSession();
            ModelState.Remove("RequestCreateANewProductDto.ImageUrl");
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (ImageFile != null)
            {
                // Đường dẫn lưu file trong wwwroot
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/products/");
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
                RequestCreateANewProductDto.ImageUrl = "/images/products/" + uniqueFileName;
            }
            await _productService.Save(RequestCreateANewProductDto);

            return RedirectToPage("./Index");
        }
    }
}
