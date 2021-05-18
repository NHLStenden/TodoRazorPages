using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
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
                return new TodoRepository().Get(Filter, UserId);
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

        public string StatusMessage { get; set; }
        
        public void OnGet()
        {
            StatusMessage = TempData["updatedObject"]?.ToString();
        }

        [BindProperty]
        public Todo NewTodo { get; set; }

        public void OnPostAdd()
        {
            if (ModelState.IsValid)
            {
                new TodoRepository().Add(NewTodo, UserId);
            }

            StatusMessage = $"Added Todo \"{NewTodo.Description}\" with Id: {NewTodo.TodoId}";
        }

        public void OnPostDelete()
        {
            int todoId = Int32.Parse(Request.Form["todoId"]);

            new TodoRepository().Delete(todoId);

            StatusMessage = $"Deleted Todo \"{NewTodo.Description}\" with Id: {NewTodo.TodoId}";
        }
    }
}