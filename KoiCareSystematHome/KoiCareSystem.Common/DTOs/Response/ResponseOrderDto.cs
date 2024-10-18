using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiCareSystem.Common.DTOs.Response
{
    public class ResponseOrderDto
    {
        public int OrderId { get; set; }
        public int? Quantity { get; set; }
        public decimal? TotalPrice { get; set; }
        public int? OrderDate { get; set; }
        public int UserId { get; set; }
        public string Email { get; set; }
    }
}
