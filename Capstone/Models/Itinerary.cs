using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public class Itinerary
    {
        public int ID { get; set; }
        public string UserEmail { get; set; }
        public string Name { get; set; }
        public decimal StartingLatitude { get; set; }
        public decimal StartingLongitude { get; set; }
        public IEnumerable<Landmark> LandmarksOrderedByVisitOrder { get; set; }
        public Dictionary<int, Landmark> VisitOrderToLandmarkId
        {
            get
            {
                return new Dictionary<int, Landmark> { { 99, new Landmark() }, { 2, new Landmark() } };
            }
            set
            {
                //nothing
            }
        }
    }
}
