using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiCareSystem.Common.DTOs.Response
{
    public class ResponseUserDto
    {
        public int? Id { get; set; }
        public string Email { get; set; }
        public int? Role { get; set; }
        public bool IsVerified { get; set; }
        public string EmailVerificationToken { get; set; }
        public string VerificationLink { get; set; }
    }
}
