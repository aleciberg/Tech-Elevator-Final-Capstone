using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public class ItineraryViewModel
    {
        public Itinerary Itinerary { get; set; }
        public List<Landmark> AllLandmarks { get; set; }
        public List<Landmark> UnusedLandmarks
        {
            get
            {
                List<Landmark> result = new List<Landmark>(AllLandmarks);

                foreach(Landmark landmark in Itinerary.LandmarksOrderedByVisitOrder)
                {
                    if (result.Contains(landmark))
                    {
                        result.Remove(landmark);
                    }
                }

                return result;
            }
        }
    }
}
