using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TodoDemo.Pages
{
    public class Login : PageModel
    {
        public void OnGet()
        {
            
        }

        public void OnPostLoginUser1()
        {
            HttpContext.Session.SetString("userid", 1.ToString());
        }
        
        public void OnPostLoginUser2()
        {
            HttpContext.Session.SetString("userid", 2.ToString());
        }

        public void OnPostLogout()
        {
            HttpContext.Session.Remove("userid");
        }
    }
}