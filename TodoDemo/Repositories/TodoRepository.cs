using System;
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
        
        public Todo Get(int todoId, int userId)
        {
            return Get(null, userId, todoId).FirstOrDefault();
        }

        public IEnumerable<Todo> Get(string filter, int userId, int? todoId = null)
        {
            string sql = @" SELECT * 
                            FROM Todo T 
                                LEFT JOIN TodoUser TU on T.TodoId = TU.TodoId
                                    LEFT JOIN User U on TU.UserId = U.UserId                                
                                JOIN Category C on T.CategoryId = C.CategoryId                                
                            WHERE 
                                  (@todoId IS NULL OR T.TodoId = @todoId)
                                AND                                  
                                  (@filter IS NULL OR Description LIKE CONCAT('%', @filter, '%'))
                                AND 
                            T.UserId = @userId ORDER BY T.TodoId";

            using var connection = GetConnection();
            var result = connection.Query<Todo, User, Category, Todo>(sql, (todo, user, category) =>
            {
                todo.Category = category;
                
                todo.AssignedUsers = new List<User>() { user };
                
                return todo;
            }, splitOn:"TodoId, CategoryId", param: new { userId, filter, todoId });

            var groupedResult = result.GroupBy(t => t.TodoId).Select(g =>
            {
                var groupedTodo = g.First();
                var assignedUsers = g.SelectMany(x => x.AssignedUsers).ToList(); 
                groupedTodo.AssignedUsers = assignedUsers;
                groupedTodo.AssignedUserIds = assignedUsers.Select(x => x.UserId).ToList();
                return groupedTodo;
            });
            
            return groupedResult;
        }

        public Todo Update(Todo edit, int userId)
        {
            string sql = 
                @"  UPDATE Todo 
                        SET Description = @Description, Done = @Done,
                            CategoryId = @CategoryId
                        WHERE TodoId = @TodoId AND UserId = @UserId;";
            using var connection = GetConnection();

            int numRowsEffected = connection.Execute(sql, 
                new {edit.Description, edit.Done, edit.CategoryId, userId, edit.TodoId});

            UpdateTodoUsers(connection, edit, edit.TodoId);

            return Get(edit.TodoId, userId);
        }

        private void UpdateTodoUsers(MySqlConnection connection, Todo todo, int todoId)
        {
            string sqlDeleteTodoUsers = "DELETE FROM TodoUser WHERE TodoId = @TodoId";
            int numTodoUserDeleted = connection.Execute(sqlDeleteTodoUsers, new {TodoId = todoId});

            string sqlInsertTodoUser = "INSERT INTO TodoUser (TodoId, UserId) VALUES (@todoId, @userId)";

            foreach (var assignedUserId in todo.AssignedUserIds)
            {
                connection.Execute(sqlInsertTodoUser, new {todoId = todoId, userId = assignedUserId});
            }
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
            string sql = @"INSERT INTO Todo (Description, Done, UserId, CategoryId) 
                                VALUES (@Description, @Done, @UserId, @CategoryId);
                            SELECT LAST_INSERT_ID();";
            
            using var connection = GetConnection();
            var newTodoId = connection.ExecuteScalar<int>(sql, 
                new {Description = newTodo.Description, Done = newTodo.Done, CategoryId = newTodo.CategoryId, 
                    UserId = userId, });

            UpdateTodoUsers(connection, newTodo, newTodoId);
            
            return Get(newTodoId, userId);
        }

        public bool Delete(int todoId, int userId)
        {
            string sql = @"DELETE FROM Todo WHERE TodoId = @TodoId AND UserId = @userId";
            
            using var connection = GetConnection();
            int numRowEffected = connection.Execute(sql, new {TodoId = todoId, userId});
            return numRowEffected == 1;         
        }
        
        
        public StatsVm GetStats(int userId)
        {
            string sql = @"SELECT
                                COUNT(CASE WHEN DONE IS TRUE THEN 1 END) AS CompletedCount,
                                COUNT(CASE WHEN DONE IS FALSE THEN 1 END) AS NotCompletedCount
                            FROM Todo WHERE UserId = @userId";
            
            using var connection = GetConnection();
            return connection.QuerySingle<StatsVm>(sql, new {userId});
        }

        public IEnumerable<User> GetAssignedUsers(int todoId, int userId)
        {
            string sql =
                @"  SELECT U.* 
                    FROM TodoUser TU 
                        JOIN User U on TU.UserId = U.UserId
                            JOIN Todo T on TU.TodoId = T.TodoId
                    WHERE T.TodoId = @todoId AND T.UserId = @userId";
            
            using var connection = GetConnection();
            return connection.Query<User>(sql, new {userId, todoId});
        }
    }
}