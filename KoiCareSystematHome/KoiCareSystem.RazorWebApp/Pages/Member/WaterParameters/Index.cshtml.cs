using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using KoiCareSystem.Data.DBContext;
using KoiCareSystem.Data.Models;
using KoiCareSystem.Service;

namespace KoiCareSystem.RazorWebApp.Pages.Member.WaterParameters
{
    public class IndexModel : PageModel
    {
        private readonly PondService _pondService;
        private readonly WaterParameterService _waterParameterService;
        public IndexModel(PondService pondService, WaterParameterService waterParameterService)
        {
            _pondService = pondService;
            _waterParameterService = waterParameterService;
        }
        //========================================
        public IList<WaterParameter> WaterParameter { get;set; } = default!;
        public int UserId { get; set; }
        public string PondName { get; set; }
        //========================================
        public async Task OnGetAsync(int? id)
        {
            var pond = (await _pondService.GetById((int)id)).Data as Pond;
            PondName = pond.PondName;

            WaterParameter = (await _waterParameterService.GetByPondId((int)id)).Data as IList<WaterParameter>;

        }
    }
}
