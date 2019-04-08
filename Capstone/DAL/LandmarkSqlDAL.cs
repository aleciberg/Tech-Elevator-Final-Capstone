using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.DAL
{
    public class LandmarkSqlDAL : ILandmarkSqlDAL
    {
        public List<Landmark> GetAllLandmarks()
        {
            return new List<Landmark>();
        }

        public Landmark GetLandmarkFromID(int id)
        {
            return new Landmark();
        }

        public int AddLandmark(Landmark landmark)
        {
            return 0;
        }
    }
}
