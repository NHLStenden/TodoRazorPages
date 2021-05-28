using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        public IEnumerable<SelectListItem> Categories { get; set; }

        public void OnGet([FromRoute] int todoId)
        {
            Categories = new CategoryRepository().Get(null, UserId)
                .Select(x => 
                    new SelectListItem(x.Name, x.CategoryId.ToString())
                );
            
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