using KoiCareSystem.Data.Models;
using KoiCareSystem.Service;
using KoiCareSystem.RazorWebApp.PageBase;
using Microsoft.AspNetCore.Mvc;
using KoiCareSystem.Common.DTOs.Response;
using AutoMapper;
using System.Collections.Generic;

namespace KoiCareSystem.RazorWebApp.Pages.Member.PondPages
{
    public class IndexModel : BasePageModel
    {
        private readonly KoiFishService _koiFishService;
        private readonly WaterParameterService _waterParameterService;
        private readonly PondService _pondService;
        private readonly IMapper _mapper;
        public IndexModel(KoiFishService koiFishService, PondService pondService, WaterParameterService waterParameterService, IMapper mapper)
        {
            _koiFishService = koiFishService;
            _pondService = pondService;
            _waterParameterService = waterParameterService;
            _mapper = mapper;
        }
        //========================================================
        public IList<Pond> Pond { get; set; } = default!;
        public IList<ResponsePondDto> ResponsePondDto { get; set; } = default!;
        // size page
        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }

        public const int PageSize = 5;
        //========================================================
        public async Task<IActionResult> OnGetAsync()
        {
            LoadUserIdFromSession();

            if (UserId == null)
            {
                return RedirectToPage("/Guest/Login"); // Điều hướng đến trang đăng nhập nếu không có UserId trong session
            }
            var ponds = (await _pondService.GetByUserId((int)UserId)).Data as IList<Pond>;
            var pondsDto = _mapper.Map<IList<ResponsePondDto>>(ponds);

            foreach (var pondDto in pondsDto)
            {
                var waterParam = (await _waterParameterService.GetLastestByPondId(pondDto.PondId)).Data as WaterParameter ;


                pondDto.StatusName = waterParam?.Status?.StatusName ?? "N/A";
            }

            // Calculate total pages
            int totalRecords = pondsDto.Count;
            TotalPages = (int)Math.Ceiling(totalRecords / (double)PageSize);

            // Get only the players for the current page
            ResponsePondDto = pondsDto
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            return Page();
        }

    }
}
