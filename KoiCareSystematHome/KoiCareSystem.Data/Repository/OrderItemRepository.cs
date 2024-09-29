using KoiCareSystem.Data.Base;
using KoiCareSystem.Data.DBContext;
using KoiCareSystem.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiCareSystem.Data.Repository
{
    public class OrderItemRepository : GenericRepository<OrderItem>
    {
        public OrderItemRepository()
        {
        }

        public OrderItemRepository(FA24_SE1702_PRN221_G5_KoiCareSystematHomeContext context) => _context = context;
    }
}
