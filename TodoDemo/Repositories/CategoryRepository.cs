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

        public Category GetCategoryByName(string name, int userId)
        {
            string sql = @"SELECT * FROM Category 
                        WHERE Name = @name
                            AND 
                        UserId = @userId ORDER BY Name";

            using var connection = GetConnection();
            return connection.QuerySingleOrDefault<Category>(sql, new {userId, name});
        }

        public bool CategoryExists(string name, int userId)
        {
            string sql = @"SELECT COUNT(1) = 1 FROM Category WHERE Name = @name AND UserId = @userId";
            
            using var connection = GetConnection();
            return connection.ExecuteScalar<bool>(sql, new {name, userId});
        }
        
        public int Delete(int categoryId)
        {
            string sql = @"
                DELETE FROM Todo WHERE CategoryId = @CategoryId;
                DELETE FROM Category WHERE CategoryId = @CategoryId;";
            
            using var connection = GetConnection();
            int numRowEffected = connection.Execute(sql, new {CategoryId = categoryId});
            return numRowEffected;         
        }
        
        public Category Update(Category category, int userId)
        {
            category.UserId = userId; //trick
            string sql = 
                @"  UPDATE Category 
                        SET Name = @Name
                        WHERE CategoryId = @CategoryId AND UserId = @UserId;
                    SELECT * FROM Category WHERE CategoryId = @CategoryId";
            using var connection = GetConnection();
            var updatedCategory = connection.QuerySingle<Category>(sql, category);
            return updatedCategory;
        }

        public Category Add(Category category, int userId)
        {
            string sql = @"INSERT INTO Category (Name, UserId) VALUES (@Name, @UserId);
                            SELECT * FROM Category WHERE CategoryId = LAST_INSERT_ID()";
            
            using var connection = GetConnection();
            var addedCategory = connection.QuerySingle<Category>(sql, 
                new {Name = category.Name, UserId = userId, });
            return addedCategory;
        }
    }
}