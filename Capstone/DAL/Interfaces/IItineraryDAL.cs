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
        int GetNextItineraryId();
        int CreateItinerary(int id, string name, int userId, decimal lat, decimal lon);
        List<Landmark> GetAllLandmarksByItineraryIdOrderedByVisitOrder(int itineraryId);
        int GetNumberOfLandmarksForItinerary(int itineraryId);
        int AssignLandmarkToBlankItinerary(int itineraryId, int landmarkId);
        Itinerary GetLastItinerary(int itineraryId);
        int AppendLandmarkToItinerary(int itineraryId, int landmarkId, int visitOrder, decimal lat, decimal lon);
    }
}
