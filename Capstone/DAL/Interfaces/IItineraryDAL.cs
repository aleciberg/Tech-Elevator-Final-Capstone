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
        int CreateItinerary(int id, string name);
        List<Landmark> GetAllLandmarksByItineraryIdOrderedByVisitOrder(int itineraryId);
    }
}
