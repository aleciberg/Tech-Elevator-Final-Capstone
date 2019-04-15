﻿using Capstone.DAL.Interfaces;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.DAL
{
    public class ItinerarySqlDAL : IItineraryDAL
    {
        private string connectionString;

        private const string SQL_GetItineraryById = "SELECT * FROM itinerary JOIN itinerary_name ON " + 
            "itinerary.itinerary_id = itinerary_name.itinerary_id JOIN itinerary_user " +
            "ON itinerary.itinerary_id = itinerary_user.itinerary_id JOIN users ON users.user_id = itinerary_user.user_id " + 
            "WHERE itinerary.itinerary_id = @itinerary_id";
        private const string SQL_UpdateItineraryName = "UPDATE itinerary_name SET itinerary_name = @itinerary_name WHERE itinerary_id = @itinerary_id;";
        private const string SQL_UpdateItineraryStartingLocation = "UPDATE itinerary SET start_lat = @start_lat, start_lon = @start_lon WHERE itinerary_id = @itinerary_id;";
        private const string SQL_UpdateItineraryLandmark = "UPDATE itinerary SET visit_order = @visit_order WHERE itinerary_id = @itinerary_id AND landmark_id = @landmark_id;";
        private const string SQL_GetMaxItineraryId = "SELECT MAX(itinerary_id) FROM itinerary";
        private const string SQL_CreateItinerary = "INSERT INTO itinerary (itinerary_id) VALUES (@itinerary_id); INSERT INTO itinerary_name (itinerary_id, itinerary_name) VALUES (@itinerary_id, @itinerary_name);";
        private const string SQL_GetAllLandmarksByItineraryId = "SELECT * FROM landmark JOIN itinerary ON itinerary.landmark_id = landmark.landmark_id WHERE itinerary.itinerary_id = @itinerary_id ORDER BY visit_order";

        public ItinerarySqlDAL(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public Itinerary GetItineraryById(int itineraryId)
        {
            Itinerary itinerary = new Itinerary();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(SQL_GetItineraryById, connection);
                command.Parameters.AddWithValue("@itinerary_id", itineraryId);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    itinerary = new Itinerary()
                    {
                        ID = Convert.ToInt32(reader["itinerary_id"]),
                        Name = Convert.ToString(reader["itinerary_name"]),
                        UserEmail = Convert.ToString(reader["email"]),
                        StartingLatitude = Convert.ToDecimal(reader["start_lat"]),
                        StartingLongitude = Convert.ToDecimal(reader["start_lon"])
                    };
                }

                itinerary.LandmarksOrderedByVisitOrder = GetAllLandmarksByItineraryIdOrderedByVisitOrder(itineraryId);
            }

            return itinerary;
        }

        public void UpdateItineraryName(int itineraryId, string newItineraryName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(SQL_UpdateItineraryName, connection);
                command.Parameters.AddWithValue("@itinerary_id", itineraryId);
                command.Parameters.AddWithValue("@itinerary_name", newItineraryName);

                command.ExecuteNonQuery();
            }
        }

        public void UpdateItineraryStartingLocation(int itineraryId, decimal startingLatitude, decimal startingLongitude)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(SQL_UpdateItineraryStartingLocation, connection);
                command.Parameters.AddWithValue("@itinerary_id", itineraryId);
                command.Parameters.AddWithValue("@start_lat", startingLatitude);
                command.Parameters.AddWithValue("@start_lon", startingLongitude);

                command.ExecuteNonQuery();
            }
        }

        public void UpdateItineraryLandmarks(int itineraryId, List<Landmark> orderedLandmarks)
        {
            for (int order = 0; order < orderedLandmarks.Count; order++)
            {
                UpdateItineraryLandmark(itineraryId, orderedLandmarks[order].ID, order + 1);
            }
        }

        private void UpdateItineraryLandmark(int itineraryId, int landmarkId, int visitOrder)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(SQL_UpdateItineraryLandmark, connection);
                command.Parameters.AddWithValue("@itinerary_id", itineraryId);
                command.Parameters.AddWithValue("@landmark_id", landmarkId);
                command.Parameters.AddWithValue("@visit_order", visitOrder);

                command.ExecuteNonQuery();
            }
        }

        public int GetNextItineraryId()
        {
            int result = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(SQL_GetMaxItineraryId, connection);
                result = (int)command.ExecuteScalar() + 1;
            }

            return result;
        }

        public int CreateItinerary(int id, string name)
        {
            int result = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(SQL_CreateItinerary, connection);
                command.Parameters.AddWithValue("@itinerary_id", id);
                command.Parameters.AddWithValue("@itinerary_name", name);
                result = (int)command.ExecuteScalar();
            }

            return result;
        }

        public List<Landmark> GetAllLandmarksByItineraryIdOrderedByVisitOrder(int itineraryId)
        {
            List<Landmark> landmarks = new List<Landmark>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(SQL_GetAllLandmarksByItineraryId, connection);
                command.Parameters.AddWithValue("@itinerary_id", itineraryId);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Landmark landmark = new Landmark();

                    landmark.Category = Convert.ToString(reader["category"]);
                    landmark.ID = Convert.ToInt32(reader["landmark_id"]);
                    landmark.Name = Convert.ToString(reader["name"]);
                    landmark.Description = Convert.ToString(reader["description"]);
                    landmark.StreetAddress = Convert.ToString(reader["address"]);
                    landmark.City = Convert.ToString(reader["city"]);
                    landmark.State = Convert.ToString(reader["state"]);
                    landmark.ZipCode = Convert.ToString(reader["zip"]);
                    landmark.Latitude = Convert.ToDecimal(reader["latitude"]);
                    landmark.Longitude = Convert.ToDecimal(reader["longitude"]);
                    landmark.HoursOfOperation = Convert.ToString(reader["category"]);
                    landmark.ImageLocation = Convert.ToString(reader["category"]);

                    landmarks.Add(landmark);
                }
            }

            return landmarks;
        }
    }
}