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
        public IActionResult CreateItinerary(string name, decimal startingLatitude, decimal startingLongitude)
        {
            Itinerary itinerary = new Itinerary()
            {
                Name = name,
                ID = itineraryDAL.GetNextItineraryId(),
                StartingLatitude = startingLatitude,
                StartingLongitude = startingLongitude
            };
            //itinerary.RemainingLandmarks = itineraryDAL.GetAllLandmarksByItineraryId(itinerary.ID);

            User user = authProvider.GetCurrentUser();

            int result = itineraryDAL.CreateItinerary(itinerary.ID, itinerary.Name, user.ID, itinerary.StartingLatitude, itinerary.StartingLongitude);

            return RedirectToAction("Itinerary", new { id = itinerary.ID });

            // TODO: Figure out why Itinerary controller action is reading in a 0 for ID
        }

        [HttpGet]
        public IActionResult Itinerary(int id)
        {
            Itinerary itinerary = itineraryDAL.GetItineraryById(id);

            ItineraryViewModel ivm = new ItineraryViewModel()
            {
                Itinerary = itinerary,
                AllLandmarks = landmarkDAL.GetAllLandmarks()
            };

            return View(ivm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddLandmarkToItinerary(int landmarkId, int itineraryId)
        {
            int numberOfLandmarksForThisItinerary = itineraryDAL.GetNumberOfLandmarksForItinerary(itineraryId);
            int numberOfUpdates = 0;
            Itinerary itinerary = new Itinerary();
            ItineraryViewModel ivm = new ItineraryViewModel();

            if (numberOfLandmarksForThisItinerary == 0)
            {
                numberOfUpdates = itineraryDAL.AssignLandmarkToBlankItinerary(landmarkId, itineraryId);
            }
            else
            {
                itinerary = itineraryDAL.GetLastItinerary(itineraryId);
                numberOfUpdates = itineraryDAL.AppendLandmarkToItinerary(itineraryId, landmarkId, itinerary.LastVisitNumber, itinerary.StartingLatitude, itinerary.StartingLongitude);
            }

            itinerary = itineraryDAL.GetItineraryById(itineraryId);
            List<Landmark> landmarks = landmarkDAL.GetAllLandmarks();

            ivm.Itinerary = itinerary;
            ivm.AllLandmarks = landmarks;

            return RedirectToAction("Itinerary", new { id = itineraryId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteLandmarkFromItinerary(int itineraryId, int landmarkId, int visitOrderOfRemovedLandmark)
        {
            itineraryDAL.RemoveLandmarkFromItinerary(itineraryId, landmarkId, visitOrderOfRemovedLandmark);
            return RedirectToAction("Itinerary", new { id = itineraryId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MoveLandmarkUpOnItinerary(int itineraryId, int landmarkId, int fromPosition)
        {
            List<Landmark> originalList = itineraryDAL.GetAllLandmarksByItineraryIdOrderedByVisitOrder(itineraryId);

            Landmark temp = originalList[fromPosition - 1];
            itineraryDAL.UpdateItineraryLandmark(itineraryId, landmarkId, fromPosition - 1);
            itineraryDAL.UpdateItineraryLandmark(itineraryId, temp.ID, fromPosition);

            return RedirectToAction("Itinerary", new { id = itineraryId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MoveLandmarkDownOnItinerary(int itineraryId, int landmarkId, int fromPosition)
        {
            List<Landmark> originalList = itineraryDAL.GetAllLandmarksByItineraryIdOrderedByVisitOrder(itineraryId);

            Landmark temp = originalList[fromPosition + 1];
            itineraryDAL.UpdateItineraryLandmark(itineraryId, landmarkId, fromPosition + 1);
            itineraryDAL.UpdateItineraryLandmark(itineraryId, temp.ID, fromPosition);

            return RedirectToAction("Itinerary", new { id = itineraryId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RenameItinerary(int itineraryId, string newItineraryName)
        {
            itineraryDAL.UpdateItineraryName(itineraryId, newItineraryName);

            return RedirectToAction("Itinerary", new { id = itineraryId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult ChangeItineraryStartingLatLon(int itineraryId, string itineraryStartingLatitude, string itineraryStartingLongitude)
        {
            itineraryDAL.UpdateItineraryStartingLocation(itineraryId, Convert.ToDecimal(itineraryStartingLatitude), Convert.ToDecimal(itineraryStartingLongitude));

            return RedirectToAction("Itinerary", new { id = itineraryId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteItinerary(int itineraryId)
        {
            int result = itineraryDAL.DeleteItinerary(itineraryId);
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
