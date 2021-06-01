using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TodoDemo.Models;
using TodoDemo.Repositories;

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

        [BindProperty] public LoginVm LoginVm { get; set; }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            User user = new UserRepository().CheckLogin(LoginVm);
            if (user == null)
            {
                ModelState.AddModelError("IncorrectEmailPasswordCombination", "Incorrect Email Password combination");
                return Page();
            }

            HttpContext.Session.SetString("userid", user.UserId);
            return RedirectToPage(nameof(TodoList));
        }

        public IActionResult OnPostLoginUser1()
        {
            User user = new UserRepository().CheckLogin(new LoginVm() {Email = "joris@test.com", Password = "Test@1234!"});
            if (user != null)
            {
                HttpContext.Session.SetString("userid", user.UserId);
                return RedirectToPage(nameof(TodoList));                
            }

            return Page();
        }
        
        public IActionResult OnPostLoginUser2()
        {
            User user = new UserRepository().CheckLogin(new LoginVm() {Email = "joris@test.com", Password = "Test@1234!"});
            if (user != null)
            {
                HttpContext.Session.SetString("userid", user.UserId);
                return RedirectToPage(nameof(TodoList));                
            }

            return Page();
        }

        public void OnPostLogout()
        {
            HttpContext.Session.Remove("userid");
        }
    }


}