using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TodoDemo.Pages
{
    public class Login : PageModel
    {
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