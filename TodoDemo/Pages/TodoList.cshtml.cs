using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TodoDemo.Models;
using TodoDemo.Repositories;

namespace TodoDemo.Pages
{
    public class TodoList : PageModel
    {
        public List<Todo> Todos
        {
            get
            {
                return new TodoRepository().Get(Request.Query["filter"]);   
            }
        }

        public void OnGet()
        {
        }

        [BindProperty]
        public Todo NewTodo { get; set; }
        
        public void OnPostAdd()
        {
            new TodoRepository().Add(NewTodo);
        }

        public void OnPostDelete()
        {
            int todoId = Int32.Parse(Request.Form["todoId"]);

            new TodoRepository().Delete(todoId);
        }
    }
}