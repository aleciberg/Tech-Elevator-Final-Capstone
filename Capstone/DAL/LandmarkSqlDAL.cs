using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.DAL
{
    public class LandmarkSqlDAL : ILandmarkSqlDAL
    {
        private string connectionString;
        private const string SQL_GetAllLandmarks = @"SELECT * FROM landmark ORDER BY landmark.name;";
        private const string SQL_GetLandmarkFromID = @"SELECT * FROM landmark where landmark_id = @landmark_id;";
        private const string SQL_AddLandmark =
            @"INSERT INTO landmark (submitter_id, name, category, description, address, " +
            "city, state, zip, latitude, longitude, hours_of_operation) " +
            "VALUES (@submitter_id, @name, @category, @description, @address, " +
            "@city, @state, @zip, @latitude, @longitude, @hours_of_operation);" +
            "SELECT CAST(SCOPE_IDENTITY() as int);";

        public LandmarkSqlDAL(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public List<Landmark> GetAllLandmarks()
        {
            List<Landmark> landmarks = new List<Landmark>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(SQL_GetAllLandmarks, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    landmarks.Add(ReadLandmarkFromReader(reader));
                }
            }
            return landmarks;
        }

        public Landmark GetLandmarkFromID(int id)
        {
            Landmark landmark = new Landmark();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(SQL_GetLandmarkFromID, conn);
                cmd.Parameters.AddWithValue("@landmark_id", id);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    landmark = ReadLandmarkFromReader(reader);
                }
            }
            return landmark;
        }

        public int AddLandmark(Landmark landmark)
        {
            List<Landmark> landmarks = new List<Landmark>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                int landmarkID = 0;
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        SqlCommand cmd = new SqlCommand(SQL_AddLandmark, connection);
                        cmd.Parameters.AddWithValue("@submitter_id", landmark.UserID);
                        cmd.Parameters.AddWithValue("@name", landmark.Name);
                        cmd.Parameters.AddWithValue("@category", landmark.Category);
                        cmd.Parameters.AddWithValue("@description", landmark.Description);
                        cmd.Parameters.AddWithValue("@address", landmark.StreetAddress);
                        cmd.Parameters.AddWithValue("@city", landmark.City);
                        cmd.Parameters.AddWithValue("@state", landmark.State);
                        cmd.Parameters.AddWithValue("@zip", landmark.ZipCode);
                        cmd.Parameters.AddWithValue("@latitude", landmark.Latitude);
                        cmd.Parameters.AddWithValue("@longitude", landmark.Longitude);
                        cmd.Parameters.AddWithValue("@hours_of_operation", landmark.HoursOfOperation);

                        landmarkID = (int)cmd.ExecuteScalar();
                    }
                }
                catch
                {
                    throw;
                }
                return landmarkID;
            }
        }

        private Landmark ReadLandmarkFromReader(SqlDataReader reader)
        {
            return new Landmark
            {
                Category = Convert.ToString(reader["category"]),
                City = Convert.ToString(reader["city"]),
                Description = Convert.ToString(reader["description"]),
                HoursOfOperation = Convert.ToString(reader["hours_of_operation"]),
                ID = Convert.ToInt32(reader["landmark_id"]),
                ImageLocation = Convert.ToString(reader["image_location"]),
                Latitude = Convert.ToDecimal(reader["latitude"]),
                Longitude = Convert.ToDecimal(reader["longitude"]),
                Name = Convert.ToString(reader["name"]),
                State = Convert.ToString(reader["state"]),
                StreetAddress = Convert.ToString(reader["address"]),
                UserID = Convert.ToInt32(reader["submitter_id"]),
                ZipCode = Convert.ToString(reader["zip"])
            };
        }
    }
}
