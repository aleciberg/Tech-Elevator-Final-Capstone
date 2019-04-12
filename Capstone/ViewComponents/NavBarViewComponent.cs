using Capstone.Providers.Auth;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.ViewComponents
{
    public class NavBarViewComponent : ViewComponent
    {
        private IAuthProvider authProvider;
        public NavBarViewComponent(IAuthProvider authProvider)
        {
            this.authProvider = authProvider;
        }

        public IViewComponentResult Invoke()
        {
            var user = authProvider.GetCurrentUser();
            return View("_LoginStatus", user);
        }
    }
}
