using KoiCareSystem.RazorWebApp.PageBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KoiCareSystem.RazorWebApp.Pages
{
    public class IndexModel : BasePageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> OnGet()
        {
            LoadUserIdFromSession();

            if (UserId == null)
            {
                return Page();
            }
            else
            {
                return RedirectToPage("/Member/Index");
            }    
        }
    }

}
