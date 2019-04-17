using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public class LandmarkReview
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public DateTime PostTime { get; set; }
    }
}