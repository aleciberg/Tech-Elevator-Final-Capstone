using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Providers.Auth
{
    public class AuthorizationFilterAttribute : Attribute, IActionFilter
    {
        private string[] roles;

        public AuthorizationFilterAttribute(params string[] roles)
        {
            this.roles = roles;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var authProvider = context.HttpContext.RequestServices.GetService<IAuthProvider>();

            if (!authProvider.IsLoggedIn)
            {
                context.Result = new RedirectToRouteResult(new
                {
                    controller = "home",
                    action = "login",
                });
                return;
            }

            // If they are logged in and the user doesn't have any of the roles
            // give them a 403
            if (roles.Length > 0 && !authProvider.UserHasRole(roles))
            {
                // User shouldn't have access
                context.Result = new StatusCodeResult(403);
                return;
            }

            // If our code gets this far then the filter "lets them through".
        }
    }
}
