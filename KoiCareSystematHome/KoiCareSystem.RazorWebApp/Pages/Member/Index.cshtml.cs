using Azure;
using KoiCareSystem.Common.DTOs;
using KoiCareSystem.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KoiCareSystem.RazorWebApp.Pages.Member
{
    public class IndexModel : PageModel
    {

        //public void OnGet()
        //{
        //    // Lấy UserId từ session
        //    var userIdFromSession = HttpContext.Session.GetInt32("UserId");

        //    if (userIdFromSession == null)
        //    {
        //        Response.Redirect("/Guest/Login");
        //        return; // Kết thúc hàm OnGet sau khi điều hướng
        //    }

        //    // Gán giá trị cho UserId vì chắc chắn rằng không phải là null
        //    UserId = (int)userIdFromSession;
        //}
        public int UserId { get; set; } 
        public void OnGet()
        {
            // Lấy UserId từ session
            var userIdFromSession = HttpContext.Session.GetInt32("UserId");

            if (userIdFromSession == null)
            {
                Response.Redirect("/Guest/Login");
                return; // Kết thúc hàm OnGet sau khi điều hướng
            }

            // Gán giá trị cho biến static
            UserSession.UserId = (int)userIdFromSession;
            UserId = (int)userIdFromSession;
        }
    }
}
