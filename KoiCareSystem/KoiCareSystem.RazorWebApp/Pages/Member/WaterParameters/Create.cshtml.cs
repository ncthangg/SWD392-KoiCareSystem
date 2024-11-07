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
using KoiCareSystem.Service.Helper;
using KoiCareSystem.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using KoiCareSystem.RazorWebApp.PageBase;

namespace KoiCareSystem.RazorWebApp.Pages.Member.WaterParameters
{
    public class CreateModel : BasePageModel
    {
        private readonly KoiCareSystem.Data.DBContext.ApplicationDbContext _context;
        private readonly PondService _pondService;
        private readonly WaterParameterService _waterParameterService;
        private readonly IFileService _fileService;

        public CreateModel(KoiCareSystem.Data.DBContext.ApplicationDbContext context, WaterParameterService waterParameterService, PondService pondService, IFileService fileService)
        {
            _context = context;
            _pondService = pondService;
            _waterParameterService = waterParameterService;
            _fileService = fileService;
        }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Khởi tạo WaterParameter nếu chưa có
            WaterParameter = new WaterParameter
            {
                PondId = (int)id
            };

            PondIdGet = (int)id;
            ViewData["StatusId"] = new SelectList(_context.WaterStatuses, "StatusId", "StatusName");
            return Page();
        }

        //=============================================
        [BindProperty]
        public WaterParameter WaterParameter { get; set; } = default!;
        [BindProperty]
        public int PondIdGet { get; set; }
        //=============================================


        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostCreateAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _waterParameterService.Create(WaterParameter);

            return RedirectToPage("/Member/PondPages/Index");
        }

        public async Task<IActionResult> OnPostImportExcelAsync(IFormFile excelFile)
        {
            if (excelFile == null || excelFile.Length == 0)
            {
                ModelState.AddModelError(string.Empty, "Please select a valid Excel file.");
                return Page();
            }

            try
            {
                // Đọc dữ liệu từ file Excel
                using (var stream = new MemoryStream())
                {
                    await excelFile.CopyToAsync(stream);
                    stream.Position = 0; // Đặt vị trí về đầu stream

                    // Chuyển đổi dữ liệu từ Excel sang danh sách ImportDataDto
                    var importDataList = _fileService.ReadFromExcel(stream);

                    // Kiểm tra nếu danh sách dữ liệu trống
                    if (importDataList == null || !importDataList.Any())
                    {
                        ModelState.AddModelError(string.Empty, "The uploaded Excel file does not contain valid data.");
                        return Page();
                    }

                    // Lưu dữ liệu vào cơ sở dữ liệu
                    foreach (var data in importDataList)
                    {
                        var waterParameter = new WaterParameter
                        {
                            PondId = PondIdGet,
                            WaterVolume = data.WaterVolume,
                            Temperature = data.Temperature,
                            Salinity = data.Salinity,
                            Ph = data.Ph,
                            O2 = data.O2,
                            No2 = data.No2,
                            No3 = data.No3,
                            Po4 = data.Po4,
                            MeasurementDate = DateTime.Now, // Thêm ngày đo lường
                            CreatedAt = DateTime.Now, // Thêm ngày tạo
                            UpdatedAt = DateTime.Now // Thêm ngày cập nhật
                        };

                        // Thêm vào cơ sở dữ liệu (cần dùng DbContext)
                        await _waterParameterService.Create(waterParameter);
                    }
                }

                TempData["SuccessMessage"] = "Data imported successfully!";

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error importing Excel file: {ex.Message}");
            }

            return RedirectToPage("/Member/WaterParameters/Detail", new { id = PondIdGet });
        }
        public async Task<IActionResult> OnPostImportCsvAsync(IFormFile csvFile)
        {
            if (csvFile == null || csvFile.Length == 0)
            {
                ModelState.AddModelError(string.Empty, "Please select a valid CSV file.");
                return Page();
            }

            try
            {
                // Đọc dữ liệu từ file Csv
                using (var stream = new MemoryStream())
                {
                    await csvFile.CopyToAsync(stream);
                    stream.Position = 0; // Đặt vị trí về đầu stream

                    // Chuyển đổi dữ liệu từ Csv sang danh sách ImportDataDto
                    var importDataList = _fileService.ReadFromCsv(stream);

                    // Kiểm tra nếu danh sách dữ liệu trống
                    if (importDataList == null || !importDataList.Any())
                    {
                        ModelState.AddModelError(string.Empty, "The uploaded Csv file does not contain valid data.");
                        return Page();
                    }

                    // Lưu dữ liệu vào cơ sở dữ liệu
                    foreach (var data in importDataList)
                    {
                        var waterParameter = new WaterParameter
                        {
                            PondId = PondIdGet,
                            WaterVolume = data.WaterVolume,
                            Temperature = data.Temperature,
                            Salinity = data.Salinity,
                            Ph = data.Ph,
                            O2 = data.O2,
                            No2 = data.No2,
                            No3 = data.No3,
                            Po4 = data.Po4,
                            MeasurementDate = DateTime.Now, // Thêm ngày đo lường
                            CreatedAt = DateTime.Now, // Thêm ngày tạo
                            UpdatedAt = DateTime.Now // Thêm ngày cập nhật
                        };

                        // Thêm vào cơ sở dữ liệu (cần dùng DbContext)
                        await _waterParameterService.Create(waterParameter);
                    }
                }

                TempData["SuccessMessage"] = "Data imported successfully!";

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error importing Excel file: {ex.Message}");
            }

            return RedirectToPage("/Member/WaterParameters/Detail", new { id = PondIdGet });

        }
    }
}
