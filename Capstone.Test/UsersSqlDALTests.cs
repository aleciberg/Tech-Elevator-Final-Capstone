using Capstone.DAL;
using Capstone.DAL.Interfaces;
using Capstone.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Transactions;

namespace Capstone.Test
{
    [TestClass]
    public class UsersSqlDALTests
    {
        private TransactionScope tran;
        private string connectionString = @"Data Source=.\\sqlexpress;Initial Catalog=CityTours;Integrated Security=True";
        private int userCount;
        private int userID;

        [TestInitialize]
        public void Initialize()
        {
            tran = new TransactionScope();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command;

                // Get number of users
                command = new SqlCommand(@"SELECT COUNT(*) FROM users", connection);
                userCount = (int)command.ExecuteScalar();
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            tran.Dispose();
        }

        [TestMethod]
        public void AddUserTest()
        {
            IUsersDAL usersDAL = new UsersSqlDAL(connectionString);
            User dummyUser = new User()
            {
                Username = "dummyuser",
                Role = "visitor",
                Email = "dummyuser@gmail.com",
                Password = "Dummy!"
            };

            userID = usersDAL.AddUser(dummyUser);

            Assert.IsNotNull(userID);
        }
    }
}
