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
        public List<Todo> Todos
        {
            get
            {
                return new TodoRepository().Get(Request.Query["filter"], UserId);
            }
        }

        public void OnGet()
        {
            Description = TempData["updatedObject"]?.ToString();
            //Description = Request.Cookies["updatedObject"];

//            Response.Cookies.Append("updateObject", String.Empty);
            //Response.Cookies.Delete("updatedObject");
        }

        public string? Description { get; set; }

        [BindProperty]
        public Todo NewTodo { get; set; }
        
        public void OnPostAdd()
        {
            if (ModelState.IsValid)
            {
                new TodoRepository().Add(NewTodo, UserId);
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
                    //return new TodoRepository().Get(Request.Query["filter"], userId);   
                }

                return -1;
            }
        }

        public void OnPostDelete()
        {
            int todoId = Int32.Parse(Request.Form["todoId"]);

            new TodoRepository().Delete(todoId);
        }
    }
}