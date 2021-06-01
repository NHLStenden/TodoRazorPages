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
    public class TodoEdit : BasePageModel
    {
        [BindProperty]
        public Todo Todo { get; set; }
        
        public EditOrCreateMode Mode { get ; set; }
        
        public enum EditOrCreateMode
        {
            Edit, Create
        }

        public IEnumerable<SelectListItem> Categories
        {
            get
            {
                return new CategoryRepository().Get(null, UserId)
                    .Select(x => 
                        new SelectListItem(x.Name, x.CategoryId.ToString())
                    );
            }
        }

        public IEnumerable<SelectListItem> Users
        {
            get
            {
                var users = new UserRepository().Get().Select(x => 
                    new SelectListItem(x.Email, x.UserId.ToString(), 
                        Todo.AssignedUserIds.FirstOrDefault(assignedId => assignedId == x.UserId) != null));

                return users;
            }
        }

        public void OnGet([FromRoute] int? todoId)
        {
            if (todoId != null)
            {
                Todo = new TodoRepository().Get(todoId.Value, UserId);
                Mode = EditOrCreateMode.Edit;
            }
            else
            {
                Todo = new Todo();
                Mode = EditOrCreateMode.Create;
            }
        }

        public IActionResult OnPostUpdate()
        {
            Mode = EditOrCreateMode.Edit;
            
            if (!ModelState.IsValid)
                return Page();
            
            var todo = new TodoRepository().Update(Todo, UserId);

            TempData["updatedObject"] = $"Edited Todo \"{todo.Description}\" with Id: {todo.TodoId}";
            //Response.Cookies.Append("updatedObject", Edit.Description, new CookieOptions());

            return RedirectToPage(nameof(TodoList));
        }

        public IActionResult OnPostCreate()
        {
            Mode = EditOrCreateMode.Create;
            
            if (!ModelState.IsValid)
                return Page();

            var todo = new TodoRepository().Add(Todo, UserId);

            TempData["updatedObject"] = $"Todo create \"{todo.Description}\" with Id: {todo.TodoId}";
            
            return RedirectToPage(nameof(TodoList));
        }
    }
}