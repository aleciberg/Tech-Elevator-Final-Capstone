using Capstone.DAL.Interfaces;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.DAL
{
    public class UsersSqlDAL : IUsersDAL
    {
        private string connectionString;
        private const string SQL_AddUser = @"INSERT INTO users (username, role, email, password) VALUES (@username, @role, @email, @password); SELECT CAST(SCOPE_IDENTITY() as int);";
        private const string SQL_GetUser = @"SELECT * FROM users WHERE email = @email";

        public UsersSqlDAL(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public int AddUser(User user)
        {
            List<User> users = new List<User>();
            int userID = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(SQL_AddUser, connection);
                    cmd.Parameters.AddWithValue("@username", user.Username);
                    cmd.Parameters.AddWithValue("@role", user.Role);
                    cmd.Parameters.AddWithValue("@email", user.Email);
                    cmd.Parameters.AddWithValue("@password", user.Password);

                    userID = (int)cmd.ExecuteScalar();
                }
            }

            catch
            {
                throw;
            }

            return userID;
        }

        public User GetUser(string email)
        {
            User user = new User();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(SQL_GetUser, connection);
                    cmd.Parameters.AddWithValue("@email", email);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        user = new User()
                        {
                            Username = Convert.ToString(reader["username"]),
                            Password = Convert.ToString(reader["password"]),
                            Email = Convert.ToString(reader["email"]),
                            Role = Convert.ToString(reader["role"]),
                        };
                    }
                }

            }
            catch (SqlException ex)
            {
                user = null;
            }

            return user;
        }

    }
}
