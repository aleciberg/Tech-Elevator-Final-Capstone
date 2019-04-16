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
        private string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=CityTours;Integrated Security=True";
        private int userID;

        // Dummy user for AddUser
        private User dummyUserOne = new User()
        {
            Username = "dummyuserone",
            Role = "visitor",
            Email = "dummyuserone@gmail.com",
            Password = "Dummy!"
        };

        // Dummy user for GetUser
        private User dummyUserTwo = new User()
        {
            Username = "dummyusertwo",
            Role = "visitor",
            Email = "dummyusertwo@gmail.com",
            Password = "Dummy!"
        };

        [TestInitialize]
        public void Initialize()
        {
            tran = new TransactionScope();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command;

                // Insert dummy user for GetUser
                command = new SqlCommand(@"INSERT INTO users (username, role, email, password) VALUES ('dummyusertwo', 'visitor', 'dummyusertwo@gmail.com', 'Dummy!');", connection);
                command.ExecuteScalar();
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

            userID = usersDAL.AddUser(dummyUserOne);

            Assert.IsTrue(userID > 0);
        }

        [TestMethod]
        public void GetUserTest()
        {
            IUsersDAL usersDAL = new UsersSqlDAL(connectionString);

            User user = usersDAL.GetUser(dummyUserTwo.Email);

            Assert.IsNotNull(user);
            Assert.AreEqual(user.Username, dummyUserTwo.Username);
            Assert.AreEqual(user.Email, dummyUserTwo.Email);
        }
    }
}
