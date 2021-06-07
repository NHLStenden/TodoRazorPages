using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TodoDemo.Models;
using TodoDemo.Repositories;

namespace TodoDemo.Pages
{
    public class Login : BasePageModel
    {
        [BindProperty] public LoginVm LoginVm { get; set; }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            User user = new UserRepository().CheckLogin(LoginVm);
            //onderstaande code is vervangen door CheckLoginAttribute (custom validation)
            // if (user == null)
            // {
            //     ModelState.AddModelError("LoginVm.Email", "Incorrect Email Password combination");
            //     return Page();
            // }

            HttpContext.Session.SetString("userid", user.UserId.ToString());
            return RedirectToPage(nameof(TodoList));
        }

        public IActionResult OnPostLoginUser1()
        {
            User user = new UserRepository().CheckLogin(new LoginVm() {Email = "joris@test.com", Password = "Test@1234!"});
            if (user != null)
            {
                HttpContext.Session.SetString("userid", user.UserId.ToString());
                return RedirectToPage(nameof(TodoList));                
            }

            return Page();
        }
        
        public IActionResult OnPostLoginUser2()
        {
            User user = new UserRepository().CheckLogin(new LoginVm() {Email = "joris@test.com", Password = "Test@1234!"});
            if (user != null)
            {
                HttpContext.Session.SetString("userid", user.UserId.ToString());
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