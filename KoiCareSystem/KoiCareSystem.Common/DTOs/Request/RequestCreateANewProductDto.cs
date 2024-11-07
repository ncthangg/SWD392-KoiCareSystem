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
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductType { get; set; }
        public string ImageUrl { get; set; }
        public decimal? Price { get; set; }
        public int? StockQuantity { get; set; }
        public int CategoryId { get; set; }
    }
}
