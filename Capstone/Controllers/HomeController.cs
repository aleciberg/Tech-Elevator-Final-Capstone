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

        public IActionResult Index()
        {
            return View(landmarkDAL.GetAllLandmarks());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Detail()
        {
            return View();
        }

        public IActionResult AddLandmark()
        {
            return View();
        }

    }
}
