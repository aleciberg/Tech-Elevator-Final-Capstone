using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public class Itinerary
    {
        public Dictionary<int, Landmark> Landmarks { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
        public List<Landmark> RemainingLandmarks { get; set; }
    }
}