using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Capstone.Models;
using Capstone.DAL;
using Microsoft.AspNetCore.Http;
using Capstone.DAL.Interfaces;
using Capstone.Providers.Auth;

namespace Capstone.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILandmarkSqlDAL landmarkDAL;
        private readonly IAuthProvider authProvider;
        private readonly IUsersDAL usersDAL;
        private readonly IItineraryDAL itineraryDAL;
        public static string SessionKey = "Auth_User";


        public HomeController(ILandmarkSqlDAL landmarkDAL, IAuthProvider authProvider, IUsersDAL usersDAL, IItineraryDAL itineraryDAL)
        {
            this.landmarkDAL = landmarkDAL;
            this.authProvider = authProvider;
            this.usersDAL = usersDAL;
            this.itineraryDAL = itineraryDAL;
        }

        private void SetSession()
        {
            if (HttpContext.Session.GetString(SessionKey) == null)
            {
                HttpContext.Session.SetString(SessionKey, "");
            }
        }

        [HttpGet]
        public IActionResult Index()
        {
            SetSession();
            return View(landmarkDAL.GetAllLandmarks());
        }

        [HttpGet]
        public IActionResult Detail(int id)
        {
            SetSession();
            Landmark landmark = landmarkDAL.GetLandmarkFromID(id);
            return View(landmark);
        }

        [HttpGet]
        public IActionResult AddLandmark()
        {
            SetSession();

            User user = authProvider.GetCurrentUser();

            Landmark landmark = new Landmark();

            LandmarkUserViewModel luvm = new LandmarkUserViewModel()
            {
                Landmark = landmark,
                User = user
            };

            return View(luvm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddLandmark(Landmark landmark)
        {
            //TODO: Add user to model before passing
            User user = authProvider.GetCurrentUser();

            LandmarkUserViewModel luvm = new LandmarkUserViewModel()
            {
                User = user,
                Landmark = landmark
            };

            SetSession();
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

        [HttpGet]
        public IActionResult Register()
        {
            SetSession();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegistrationViewModel rvm)
        {
            SetSession();

            User user = new User()
            {
                Username = rvm.Username,
                Password = rvm.Password,
                Email = rvm.Email,
                Role = "visitor"
            };

            if (ModelState.IsValid)
            {
                if (authProvider.Register(user.Username, user.Password, user.Email, user.Role) == 0)
                {
                    ViewBag.RegistrationError = "Error, please try again.";
                    return View(rvm);
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(rvm);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Login(LoginViewModel lvm)
        {
            if (ModelState.IsValid)
            {
                bool validLogin = authProvider.SignIn(lvm.Email, lvm.Password);
                if (validLogin)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(lvm);
        }

        [HttpGet]
        public IActionResult LogOff()
        {
            authProvider.LogOff();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult CreateItinerary()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateItinerary(string name)
        {
            Itinerary itinerary = new Itinerary()
            {
                Name = name,
                ID = itineraryDAL.GetNextItineraryId()
            };
            //itinerary.RemainingLandmarks = itineraryDAL.GetAllLandmarksByItineraryId(itinerary.ID);

            int result = itineraryDAL.CreateItinerary(itinerary.ID, itinerary.Name);

            return RedirectToAction("Itinerary", itinerary.ID);
        }

        [HttpGet]
        public IActionResult Itinerary(int id)
        {
            Itinerary itinerary = itineraryDAL.GetItineraryById(id);

            return View(itinerary);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RenameItinerary(Itinerary itinerary, string name)
        {
            Itinerary newItinerary = itinerary;
            newItinerary.Name = name;

            return RedirectToAction("Itinerary", newItinerary);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
