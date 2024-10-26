using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;

namespace KoiCareSystem.RazorWebApp.PageBase
{
    public class BasePageModel : PageModel
    {
        public int? UserId { get; set; }

        public void LoadUserIdFromSession()
        {
            UserId = HttpContext.Session.GetInt32("UserId");
        }
    }
}
