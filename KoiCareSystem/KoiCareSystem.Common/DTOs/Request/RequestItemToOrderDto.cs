using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiCareSystem.Common.DTOs.Request
{
    public class RequestItemToOrderDto
    {

        public int? OrderId { get; set; }

        public int? ProductId { get; set; }

        public int? Quantity { get; set; }

    }
}
