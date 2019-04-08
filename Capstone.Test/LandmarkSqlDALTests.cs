using Capstone.DAL;
using Capstone.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Transactions;
using System.Data.SqlClient;

namespace Capstone.Test
{
    [TestClass]
    public class LandmarkSqlDALTests
    {
        private TransactionScope tran;
        private string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=CityTours;Integrated Security=True";
        ILandmarkSqlDAL landmarkSqlDAL;

        [TestInitialize]
        public void Initialize()
        {
            tran = new TransactionScope();
            landmarkSqlDAL = new LandmarkSqlDAL(connectionString);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd;

                //make a test landmark
                cmd = new SqlCommand(
                    @"INSERT INTO landmark (submitter_id, name, category, description, address, " +
                    "city, state, zip, latitude, longitude, hours_of_operation) " +
                    "VALUES (1, 'TestName', 'TestCategory', 'TestDescription', 'TestAddress', " +
                    "'TestCity', 'OH', 'TestZip', 39.961720, -83.061591, 'TestHours');", connection);
                cmd.ExecuteNonQuery();
            }
        }

        /*
        * CLEANUP
        * Rollback the existing transaction.
        */
        [TestCleanup]
        public void Cleanup()
        {
            tran.Dispose();
        }

        [TestMethod]
        public void GetLandmarkTests()
        {
            //Arrange
            Landmark landmark = new Landmark();

            //Act
            landmark = landmarkSqlDAL.GetLandmarkFromID(1);

            //Assert
            Assert.AreEqual(1, landmark.ID);
        }

        [TestMethod]
        public void GetAllLandmarksTest()
        {
            //Arrange
            List<Landmark> landmarks = new List<Landmark>();

            //Act
            landmarks = landmarkSqlDAL.GetAllLandmarks();

            //Assert
            Assert.AreNotEqual(0, landmarks.Count);
        }
    }
}
