using System.Collections.Generic;
using Dapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using MySql.Data.MySqlClient;
using TodoDemo.Models;

namespace TodoDemo.Repositories
{
    public class UserRepository
    {
        private string connectionString = "Server=127.0.0.1;Port=3306;Database=TodoDemo;Uid=root;Pwd=Test@1234!;";

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }

        public User CheckLogin(LoginVm loginVm)
        {
            string sql = "SELECT * FROM User WHERE Email = @email AND Password = @Password";

            using var connection = GetConnection();
            return connection.QuerySingleOrDefault<User>(sql, loginVm);
        }

        public bool IsEmailOccupied(string email)
        {
            string sql = "SELECT COUNT(1) = 1 FROM User Where Email = @email";
            
            using var connection = GetConnection();
            return connection.ExecuteScalar<bool>(sql, new {email});
        }

        public User Add(User user)
        {
            string sql = @"INSERT INTO User (Email, Password) VALUES (@Email, @Password);
                            SELECT * FROM User WHERE UserId = LAST_INSERT_ID()";
            
            using var connection = GetConnection();
            var addedUser = connection.QuerySingle<User>(sql, user);
            return addedUser;  
        }

        public IEnumerable<User> GetAllUsers()
        {
            string sql = @"SELECT * FROM User";

            using var connection = GetConnection();
            return connection.Query<User>(sql);
        }
    }
}