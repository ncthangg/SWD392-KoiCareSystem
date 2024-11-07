using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace KoiCareSystem.Common.DTOs
{
    public class ApiResponseDto<T>
    {
        public HttpStatusCode StatusCode { get; set; }

        public string Message { get; set; } = null!;

        public T? Data { get; set; }
    }
}
