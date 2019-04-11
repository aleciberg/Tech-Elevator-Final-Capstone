using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Providers.Auth
{
    public interface IAuthProvider
    {
        bool IsLoggedIn { get; }

        User GetCurrentUser();
        bool SignIn(string username, string password);
        void LogOff();
        int Register(string username, string password, string email, string role);
        bool UserHasRole(string[] roles);
    }
}
