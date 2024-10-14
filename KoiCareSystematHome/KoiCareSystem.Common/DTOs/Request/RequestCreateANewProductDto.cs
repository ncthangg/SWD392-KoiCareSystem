using KoiCareSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiCareSystem.Common.DTOs.Request
{
    public class RequestCreateANewProductDto
    {
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductType { get; set; }
        public decimal? Price { get; set; }
        public long? StockQuantity { get; set; }
        public long CategoryId { get; set; }
    }
}
