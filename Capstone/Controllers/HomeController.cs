using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Capstone.Models;
using Capstone.DAL;

namespace Capstone.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILandmarkSqlDAL landmarkDAL; 

        public HomeController(ILandmarkSqlDAL landmarkDAL)
        {
            this.landmarkDAL = landmarkDAL;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(landmarkDAL.GetAllLandmarks());
        }

        [HttpGet]
        public IActionResult Detail(int id)
        {
            Landmark landmark = landmarkDAL.GetLandmarkFromID(id);

            return View(landmark);
        }

        public IActionResult AddLandmark()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
