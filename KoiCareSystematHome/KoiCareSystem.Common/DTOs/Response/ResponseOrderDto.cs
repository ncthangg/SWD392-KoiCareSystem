using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiCareSystem.Common.DTOs.Response
{
    public class ResponseOrderDto
    {
        public long OrderId { get; set; }
        public long? Quantity { get; set; }
        public decimal? TotalPrice { get; set; }
        public long? OrderDate { get; set; }
        public long UserId { get; set; }
        public string Email { get; set; }
    }
}
