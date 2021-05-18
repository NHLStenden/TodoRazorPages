using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TodoDemo.Pages
{
    public class Login : PageModel
    {
        public int UserId
        {
            get
            {
                string userIdStr = HttpContext.Session.GetString("userid");
                if (!string.IsNullOrWhiteSpace(userIdStr))
                {
                    int userId = int.Parse(userIdStr);
                    return userId;
                }
                
                return -1;
            }
        }

        public void OnGet()
        {
        }

        public IActionResult OnPostLoginUser1()
        {
            HttpContext.Session.SetString("userid", 1.ToString());
            return RedirectToPage(nameof(TodoList));
        }
        
        public IActionResult OnPostLoginUser2()
        {
            HttpContext.Session.SetString("userid", 2.ToString());
            return RedirectToPage(nameof(TodoList));
        }

        public void OnPostLogout()
        {
            HttpContext.Session.Remove("userid");
        }
    }
}