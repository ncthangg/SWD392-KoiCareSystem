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
    public class DetailModel : PageModel
    {
        private readonly KoiCareSystem.Data.DBContext.ApplicationDbContext _context;
        private readonly PondService _pondService;

        public DetailModel(PondService pondService, KoiCareSystem.Data.DBContext.ApplicationDbContext context)
        {
            _context = context;
            _pondService = pondService;
        }
        //========================================
        public WaterParameter WaterParameter { get; set; } = default!;
        public List<WaterParameterLimit> WaterParameterLimits { get; set; } = new List<WaterParameterLimit>();
        public Dictionary<string, string> ParameterComparisons { get; set; } = new Dictionary<string, string>();

        public int UserId { get; set; }
        public int PondId { get; set; }
        public string PondName { get; set; }
        //========================================

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var pond = (await _pondService.GetById((int)id)).Data as Pond;
            PondId = (int)id;
            PondName = pond.PondName;
            // Fetch the most recent WaterParameter for the specified PondId
            var waterparameter = await _context.WaterParameters
                .Include(x=> x.Status)
                .Where(m => m.PondId == id) // Filter by PondId
                .OrderByDescending(m => m.UpdatedAt) // Order by UpdatedAt to get the most recent record
                .FirstOrDefaultAsync();

            if (waterparameter == null)
            {
                return NotFound();
            }

            WaterParameter = waterparameter;

            // Fetch WaterParameterLimits for comparisons
            WaterParameterLimits = await _context.WaterParameterLimits.ToListAsync();

            // Perform comparisons and calculate differences
            foreach (var limit in WaterParameterLimits)
            {
                decimal currentValue = 0;
                switch (limit.ParameterName.ToLower())
                {
                    case "temperature":
                        currentValue = (decimal)WaterParameter.Temperature;
                        break;
                    case "salinity":
                        currentValue = (decimal)WaterParameter.Salinity;
                        break;
                    case "ph":
                        currentValue = (decimal)WaterParameter.Ph;
                        break;
                    case "o2":
                        currentValue = (decimal)WaterParameter.O2;
                        break;
                    case "no2":
                        currentValue = (decimal)WaterParameter.No2;
                        break;
                    case "no3":
                        currentValue = (decimal)WaterParameter.No3;
                        break;
                    case "po4":
                        currentValue = (decimal)WaterParameter.Po4;
                        break;
                    default:
                        continue;
                }

                // Compare the current value with the acceptable ranges
                string status;
                if (currentValue < limit.MinAcceptValue)
                {
                    status = "Below acceptable range";
                }
                else if (currentValue > limit.MaxAcceptValue)
                {
                    status = "Above acceptable range";
                }
                else if (currentValue >= limit.MinGoodValue && currentValue <= limit.MaxGoodValue)
                {
                    status = "Within good range";
                }
                else
                {
                    status = "Within acceptable range";
                }

                ParameterComparisons[limit.ParameterName] = status;
            }

            return Page();
        }
    }
}
