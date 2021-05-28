using System.Collections.Generic;
using System.Linq;
using Dapper;
using MySql.Data.MySqlClient;
using TodoDemo.Models;

namespace TodoDemo.Repositories
{
    public class CategoryRepository
    {
        private string connectionString = "Server=127.0.0.1;Port=3306;Database=TodoDemo;Uid=root;Pwd=Test@1234!;";

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }

        public Category Get(int categoryId)
        {
            using var connection = GetConnection();
            return connection.QuerySingle<Category>("SELECT * FROM Category WHERE CategoryId = @categoryId",
                new {categoryId});
        }

        public IEnumerable<Category> Get(string filter, int userId)
        {
            string sql = @"SELECT * FROM Category 
                        WHERE (@filter IS NULL OR Name LIKE CONCAT('%', @filter, '%'))
                            AND 
                        UserId = @userId ORDER BY Name";

            using var connection = GetConnection();
            return connection.Query<Category>(sql, new {userId, filter}).ToList();
        }
    }
}