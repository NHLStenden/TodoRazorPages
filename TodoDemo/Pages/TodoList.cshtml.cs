using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TodoDemo.Models;
using TodoDemo.Repositories;

namespace TodoDemo.Pages
{
    public class TodoList : BasePageModel
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

        public IEnumerable<SelectListItem> Categories { get; set; }

        public void OnGet()
        {
            Categories = new CategoryRepository().Get(null, UserId)
                .Select(x => 
                    new SelectListItem(x.Name, x.CategoryId.ToString())
                );
        }

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
            var todoToDelete = new TodoRepository().Get(todoId, UserId);
            
            new TodoRepository().Delete(todoId, UserId);

            StatusMessage = $"Deleted Todo \"{todoToDelete.Description}\" with TodoId: {todoToDelete.TodoId}";
        }
    }
}