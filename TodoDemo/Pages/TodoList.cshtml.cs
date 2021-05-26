using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TodoDemo.Models;
using TodoDemo.Repositories;

namespace TodoDemo.Pages
{
    public class TodoList : PageModel
    {
        public IEnumerable<Todo> Todos
        {
            get
            {
                var todos = new TodoRepository().Get(Filter, UserId).ToList();
                return todos;
            }
        }

        public string Filter
        {
            get
            {
                return Request.Query["filter"];
            }
        }
        
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

        public string StatusMessage
        {
            get
            {
                return TempData["StatusMessage"]?.ToString();
            } 
            set
            {
                TempData["StatusMessage"] = value;
            }
        }

        public void OnGet()
        { }

        [BindProperty]
        public Todo NewTodo { get; set; }

        public void OnPostUpdateCheckbox(int todoId, [FromForm(Name = "todo.Done")] string todoDone)
        {
            bool done = !string.IsNullOrWhiteSpace(todoDone) && todoDone.Contains("on");

            new TodoRepository().UpdateDone(todoId, done, UserId);
        }
        
        public IActionResult OnPostAdd()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Todo newTodo = new TodoRepository().Add(NewTodo, UserId);
            StatusMessage = $"Added Todo \"{newTodo.Description}\" with Id: {newTodo.TodoId}";

            //https://stackoverflow.com/questions/52058441/how-can-you-clear-a-bound-property-on-a-razor-pages-model-when-posting
            return RedirectToPage(nameof(TodoList));
        }

        public void OnPostDelete([FromForm] int todoId)
        {
            var todoToDelete = new TodoRepository().Get(todoId);
            
            new TodoRepository().Delete(todoId);

            StatusMessage = $"Deleted Todo \"{todoToDelete.Description}\" with TodoId: {todoToDelete.TodoId}";
        }
    }
}