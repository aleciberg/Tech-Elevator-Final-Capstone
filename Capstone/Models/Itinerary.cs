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
        public List<Landmark> LandmarksOrderedByVisitOrder { get; set; }
        public int LastVisitNumber { get; set; }
    }
}
