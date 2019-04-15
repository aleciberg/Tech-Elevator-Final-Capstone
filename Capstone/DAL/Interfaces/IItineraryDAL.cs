using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.DAL.Interfaces
{
    public interface IItineraryDAL
    {
        int GetNextItineraryId();
        int CreateItinerary(int id, string name);
        List<Landmark> GetAllLandmarksByItineraryId(int itineraryId);
    }
}
