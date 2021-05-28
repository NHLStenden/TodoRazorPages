using System.Collections.Generic;
using System.Linq;
using Dapper;
using MySql.Data.MySqlClient;
using TodoDemo.Models;

namespace TodoDemo.Repositories
{
    public class TodoRepository
    {
        private string connectionString = "Server=127.0.0.1;Port=3306;Database=TodoDemo;Uid=root;Pwd=Test@1234!;";

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }
        
        public Todo Get(int todoId)
        {
            using var connection = GetConnection();
            return connection.QuerySingle<Todo>("SELECT * FROM Todo WHERE TodoId = @todoId",
                new {todoId});
        }
        
        public IEnumerable<Todo> Get(string filter, int userId)
        {
            string sql = @" SELECT * 
                            FROM Todo T 
                                JOIN Category C on T.CategoryId = C.CategoryId
                            WHERE (@filter IS NULL OR Description LIKE CONCAT('%', @filter, '%'))
                                AND 
                            T.UserId = @userId ORDER BY TodoId";

            using var connection = GetConnection();
            var result = connection.Query<Todo, Category, Todo>(sql, (todo, category) =>
            {
                todo.Category = category;
                return todo;
            }, splitOn:"CategoryId", param: new { userId, filter });
            return result;
        }

        public Todo Update(Todo edit, int userId)
        {
            edit.UserId = userId; //trick
            string sql = 
                @"  UPDATE Todo 
                        SET Description = @Description, Done = @Done,
                            CategoryId = @CategoryId
                        WHERE TodoId = @TodoId AND UserId = @UserId;
                    SELECT * FROM Todo WHERE TodoId = @TodoId";
            using var connection = GetConnection();
            var todo = connection.QuerySingle<Todo>(sql, edit);
            return todo;
        }

        public bool UpdateDone(int todoId, bool done, int userId)
        {
            string sql = @"UPDATE Todo 
                SET Done = @Done
                WHERE TodoId = @TodoId AND UserId = @UserId";
            
            using var connection = GetConnection();
            int numRowEffected = connection.Execute(sql, new {TodoId = todoId, Done = done, UserId = userId});
            return numRowEffected == 1;
        }

        public Todo Add(Todo newTodo, int userId)
        {
            string sql = @"INSERT INTO Todo (Description, Done, UserId, CategoryId) VALUES (@Description, @Done, @UserId, @CategoryId);
                            SELECT * FROM Todo WHERE TodoId = LAST_INSERT_ID()";
            
            using var connection = GetConnection();
            var todo = connection.QuerySingle<Todo>(sql, 
                new {Description = newTodo.Description, Done = newTodo.Done, CategoryId = newTodo.CategoryId, 
                    UserId = userId, });
            return todo;
        }

        public bool Delete(int todoId)
        {
            string sql = @"DELETE FROM Todo WHERE TodoId = @TodoId ";
            
            using var connection = GetConnection();
            int numRowEffected = connection.Execute(sql, new {TodoId = todoId});
            return numRowEffected == 1;         
        }
    }
}