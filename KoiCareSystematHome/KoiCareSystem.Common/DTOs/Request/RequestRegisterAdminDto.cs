using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiCareSystem.Common.DTOs.Request
{
    public class RequestRegisterAdminDto : RequestRegisterDto
    {
        public int RoleId { get; set; }
    }
}
