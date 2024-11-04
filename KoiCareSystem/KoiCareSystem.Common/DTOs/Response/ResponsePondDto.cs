using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiCareSystem.Common.DTOs.Response
{
    public class ResponsePondDto
    {
        public int PondId { get; set; }
        public string PondName { get; set; }
        public string ImageUrl { get; set; }
        public decimal? Size { get; set; }
        public decimal? Depth { get; set; }
        public decimal? Volume { get; set; }
        public int? DrainCount { get; set; }
        public decimal? PumpCapacity { get; set; }
        public int? SkimmerCount { get; set; }
        public string? StatusName { get; set; } // Thêm ở đây
    }

}
