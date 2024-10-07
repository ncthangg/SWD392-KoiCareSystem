using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiCareSystem.Common.DTOs.User
{
    public class RegisterAdminDto : RegisterDto
    {
        public long RoleId { get; set; }
    }
}
