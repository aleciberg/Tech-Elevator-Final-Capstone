using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.DAL
{
    public interface ILandmarkSqlDAL
    {
        List<Landmark> GetAllLandmarks();
        Landmark GetLandmarkFromID(int id);
        int AddLandmark(Landmark landmark);
    }
}
