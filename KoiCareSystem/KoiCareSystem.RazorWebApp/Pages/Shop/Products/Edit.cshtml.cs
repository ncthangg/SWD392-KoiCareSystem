
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KoiCareSystem.Data.DBContext;
using KoiCareSystem.Data.Models;
using KoiCareSystem.Service;
using AutoMapper;
using KoiCareSystem.Common.DTOs.Request;
using KoiCareSystem.RazorWebApp.PageBase;

namespace KoiCareSystem.RazorWebApp.Pages.Shop.Products
{
    public class EditModel : BasePageModel
    {
        private readonly ProductService _productService;
        private readonly CategoryService _categoryService;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EditModel(IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _productService ??= new ProductService(mapper);
            _categoryService ??= new CategoryService();
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }
        //========================================================
        [BindProperty]
        public RequestCreateANewProductDto RequestCreateANewProductDto { get; set; } = default!;
        public Product Product { get; set; } = default!;
        [BindProperty]
        public IFormFile ImageFile { get; set; }
        //========================================================
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product =  await _productService.GetById((int)id);
            if (product == null)
            {
                return NotFound();
            }
            Product = (Product)product.Data;

            RequestCreateANewProductDto = _mapper.Map<RequestCreateANewProductDto>(Product);
            var category = _categoryService.GetAll().Result.Data as IList<Category>;
            ViewData["CategoryId"] = new SelectList(category, "Id", "Description");
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

            if (!_productService.ProductExists(RequestCreateANewProductDto.ProductId))
            {
                return NotFound();
            }
            else
            {
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
            }

            if (!_productService.ProductExists(RequestCreateANewProductDto.ProductId))
            {
                return NotFound();
            }
            return RedirectToPage("./Index");
        }

    }
}
