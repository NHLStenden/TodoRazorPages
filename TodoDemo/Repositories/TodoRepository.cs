using System.Collections.Generic;
using System.Linq;
using TodoDemo.Models;

namespace TodoDemo.Repositories
{
    public class TodoRepository
    {
        private static List<Todo> _todos = new List<Todo>()
        {
            new Todo() {TodoId = 1, Description = "Aa 1", Done = false, UserId = 1},
            new Todo() {TodoId = 2, Description = "Bb 2", Done = true, UserId = 2},
            new Todo() {TodoId = 3, Description = "Cc 3", Done = true, UserId = 1}
        };

        public List<Todo> Get(string filter, int userId)
        {
            var result = _todos.Where(x => x.UserId == userId);
            
            if (!string.IsNullOrWhiteSpace(filter))
            {
                return result.Where(x => x.Description.Contains(filter)).ToList(); 
            }

            return result.ToList();
        }
        
        public Todo Get(int todoId)
        {
            return _todos.Find(x => x.TodoId == todoId);
        }

        public void Add(Todo newTodo, int userId)
        {
            newTodo.TodoId = _todos.Max(x => x.TodoId) + 1;
            newTodo.UserId = userId;
            _todos.Add(newTodo);    
        }

        public void Delete(int todoId)
        {
            _todos = _todos.Where(x => x.TodoId != todoId).ToList();
        }

        public void Update(Todo edit)
        {
            Todo itemToUpdate = Get(edit.TodoId);
            itemToUpdate.Description = edit.Description;
            itemToUpdate.Done = edit.Done;
        }
    }
}