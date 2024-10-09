using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiCareSystem.Service.Helper
{
    public interface IUrlHelperService
    {
        string GenerateVerificationLink(ActionContext actionContext, string token);
    }

    public class UrlHelperService : IUrlHelperService
    {
        public string GenerateVerificationLink(ActionContext actionContext, string token)
        {
            var urlHelperFactory = new UrlHelperFactory();
            var urlHelper = urlHelperFactory.GetUrlHelper(actionContext);

            // Tạo liên kết xác thực
            return urlHelper.Page("/Guest/VerifyEmail", null, new { token }, actionContext.HttpContext.Request.Scheme);
        }
    }

}
