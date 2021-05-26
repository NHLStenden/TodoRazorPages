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
                
                HttpContext.Response.Redirect(nameof(Login));
                
                return -1;
            }
        }
        
        public void OnGet([FromRoute] int todoId)
        {
            Edit = new TodoRepository().Get(todoId);
        }

        public IActionResult OnPost()
        {
            new TodoRepository().Update(Edit, UserId);

            TempData["updatedObject"] = $"Edited Todo \"{Edit.Description}\" with Id: {Edit.TodoId}";
            //Response.Cookies.Append("updatedObject", Edit.Description, new CookieOptions());

            return RedirectToPage("TodoList");
        }
    }
}