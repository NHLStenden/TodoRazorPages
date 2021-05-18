using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TodoDemo.Models;
using TodoDemo.Repositories;

namespace TodoDemo.Pages
{
    public class TodoEdit : PageModel
    {
        [BindProperty]
        public Todo Edit { get; set; }
        
        public void OnGet(int todoId)
        {
            Edit = new TodoRepository().Get(todoId);
        }

        public IActionResult OnPost()
        {
            new TodoRepository().Update(Edit);

            TempData["updatedObject"] = $"Edited Todo \"{Edit.Description}\" with Id: ${Edit.TodoId}";
            //Response.Cookies.Append("updatedObject", Edit.Description, new CookieOptions());

            return RedirectToPage("TodoList");
        }
    }
}