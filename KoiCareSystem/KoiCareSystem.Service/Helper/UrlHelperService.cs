using Microsoft.AspNetCore.Http;
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
        string GenerateVerificationLink(HttpContext httpContext, string token);
    }

    public class UrlHelperService : IUrlHelperService
    {
        private readonly IUrlHelperFactory _urlHelperFactory;

        public UrlHelperService(IUrlHelperFactory urlHelperFactory)
        {
            _urlHelperFactory = urlHelperFactory;
        }
        public string GenerateVerificationLink(ActionContext actionContext, string token)
        {
            var urlHelper = _urlHelperFactory.GetUrlHelper(actionContext);

            // Tạo liên kết xác thực
            return urlHelper.Page("/Guest/VerifyEmail", null, new { token }, actionContext.HttpContext.Request.Scheme);
        }
        public string GenerateVerificationLink(HttpContext httpContext, string token)
        {
            // Kiểm tra httpContext không phải là null
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext), "HttpContext cannot be null.");
            }

            var urlHelper = _urlHelperFactory.GetUrlHelper(new ActionContext
            {
                HttpContext = httpContext
            });

            // Tạo liên kết xác thực
            return urlHelper.Action("VerifyEmail", "Guest", new { token }, httpContext.Request.Scheme);
        }

    }

}
