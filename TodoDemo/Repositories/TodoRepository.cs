using System.Collections.Generic;
using System.Linq;
using TodoDemo.Models;

namespace TodoDemo.Repositories
{
    public class TodoRepository
    {
        private static List<Todo> _todos = new List<Todo>()
        {
            new Todo() {TodoId = 1, Description = "Aa 1", Done = false},
            new Todo() {TodoId = 2, Description = "Bb 2", Done = true}
        };

        public List<Todo> Get(string filter)
        {
            if (!string.IsNullOrWhiteSpace(filter))
            {
                return _todos.Where(x => x.Description.Contains(filter)).ToList(); 
            }

            return _todos;
        }
        
        public Todo Get(int todoId)
        {
            return _todos.Find(x => x.TodoId == todoId);
        }

        public void Add(Todo newTodo)
        {
            newTodo.TodoId = _todos.Max(x => x.TodoId) + 1;
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