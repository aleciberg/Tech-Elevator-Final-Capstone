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
        private const string SQL_AddUser = @"INSERT INTO users (username, role, email, password) VALUES (@username, @role, @email, @password)";

        public UsersSqlDAL(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public int AddUser(User user)
        {
            List<User> users = new List<User>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                int userID = 0;

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        SqlCommand cmd = new SqlCommand(SQL_AddUser, conn);
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
        }
    }
}
