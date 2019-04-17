using Capstone.DAL.Interfaces;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.DAL
{
    public class ReviewSqlDal : IReviewDAL
    {
        private string connectionString;
        private const string SQL_GetLandmarkReviewsById = "SELECT * FROM reviews WHERE landmark_id = @landmark_id;";

        public ReviewSqlDal(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public List<LandmarkReview> GetLandmarkReviewsById(int landmarkId)
        {
            List<LandmarkReview> reviews = new List<LandmarkReview>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(SQL_GetLandmarkReviewsById, connection);
                command.Parameters.AddWithValue("@landmark_id", landmarkId);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    LandmarkReview review = new LandmarkReview();

                    review.ID = Convert.ToInt32(reader["landmark_id"]);
                    review.Username = Convert.ToString(reader["username"]);
                    review.Subject = Convert.ToString(reader["subject"]);
                    review.Message = Convert.ToString(reader["message"]);
                    review.PostTime = Convert.ToDateTime(reader["post_date"]);

                    reviews.Add(review);
                }
            }

            return reviews;
        }
    }
}