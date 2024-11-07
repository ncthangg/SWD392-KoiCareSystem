using KoiCareSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiCareSystem.Common.DTOs.Request
{
    public class ImportDataDto
    {
        public int? PondId { get; set; }
        public decimal? Temperature { get; set; }

        public decimal? Salinity { get; set; }

        public decimal? Ph { get; set; }

        public decimal? O2 { get; set; }

        public decimal? No2 { get; set; }

        public decimal? No3 { get; set; }

        public decimal? Po4 { get; set; }

        public decimal? WaterVolume { get; set; }
    }
}
