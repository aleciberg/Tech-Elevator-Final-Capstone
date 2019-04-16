using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.DAL.Interfaces
{
    public interface IItineraryDAL
    {
        Itinerary GetItineraryById(int id);

        void UpdateItineraryName(int itineraryId, string newItineraryName);
        void UpdateItineraryStartingLocation(int itineraryId, decimal startingLatitude, decimal startingLongitude);
        void UpdateItineraryLandmarks(int itineraryId, List<Landmark> orderedLandmarks);
        void UpdateItineraryLandmark(int itineraryId, int landmarkId, int visitOrder);

        int GetNextItineraryId();
        int CreateItinerary(int id, string name, int userId, decimal lat, decimal lon);
        int DeleteItinerary(int id);
        List<Landmark> GetAllLandmarksByItineraryIdOrderedByVisitOrder(int itineraryId);
        int GetNumberOfLandmarksForItinerary(int itineraryId);
        int AssignLandmarkToBlankItinerary(int itineraryId, int landmarkId);
        Itinerary GetLastItinerary(int itineraryId);
        int AppendLandmarkToItinerary(int itineraryId, int landmarkId, int visitOrder, decimal lat, decimal lon);
        int RemoveLandmarkFromItinerary(int itineraryId, int landmarkId, int visitOrderOfRemovedLandmark);
        List<Itinerary> GetAllItinerariesByUser(int userId);
    }
}
