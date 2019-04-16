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
    }
}
