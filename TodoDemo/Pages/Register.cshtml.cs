using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TodoDemo.Models;
using TodoDemo.Repositories;

namespace TodoDemo.Pages
{
    public class Register : PageModel
    {
        [BindProperty] public RegisterUserVm RegisterUser { get; set; }
        
        public void OnGet()
        {
            
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            bool emailOccupied = new UserRepository().IsEmailOccupied(RegisterUser.Email);
            if (emailOccupied)
            {
                ModelState.AddModelError("RegisterUser.Email", "Email address is already in use!");
                return Page();
            }

            var registeredUser = new UserRepository().Add(RegisterUser.ConvertToUser());
            
            return RedirectToPage(nameof(Login), new {message = $"Account aangemaakt voor {registeredUser.Email}"});
        }
    }
}