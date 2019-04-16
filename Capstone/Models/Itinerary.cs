using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public class Itinerary
    {
        public int ID { get; set; }

        [Display(Name = ("User Email: "))]
        public string UserEmail { get; set; }

        [Display(Name = ("Name of Itinerary: "))]
        public string Name { get; set; }

        public decimal StartingLatitude { get; set; }

        public decimal StartingLongitude { get; set; }

        public List<Landmark> LandmarksOrderedByVisitOrder { get; set; }
        public int LastVisitNumber { get; set; }
    }
}
