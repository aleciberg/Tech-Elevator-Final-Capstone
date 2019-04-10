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

        [HttpGet]
        public IActionResult AddLandmark()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddLandmark(Landmark landmark)
        {
            if (!ModelState.IsValid)
            {
                return View(landmark);
            }
            else
            {
                int newLandmarkID = landmarkDAL.AddLandmark(landmark);
                return RedirectToAction("Detail", new { id = newLandmarkID });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
