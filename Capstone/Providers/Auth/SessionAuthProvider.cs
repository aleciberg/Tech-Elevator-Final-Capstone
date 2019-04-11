using Capstone.DAL.Interfaces;
using Capstone.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Providers.Auth
{
    public class SessionAuthProvider : IAuthProvider
    {
        private readonly IHttpContextAccessor contextAccessor;
        private readonly IUsersDAL usersDAL;
        public static string SessionKey = "Auth_User";

        public SessionAuthProvider(IHttpContextAccessor contextAccessor, IUsersDAL usersDAL)
        {
            this.contextAccessor = contextAccessor;
            this.usersDAL = usersDAL;
        }

        ISession Session => contextAccessor.HttpContext.Session;

        public bool IsLoggedIn => !String.IsNullOrEmpty(Session.GetString(SessionKey));

        public bool SignIn(string username, string password)
        {
            var user = usersDAL.GetUser(username);
            var hashProvider = new HashProvider();

            if (user != null && hashProvider.VerifyPassword(user.Password, password))
            {
                Session.SetString(SessionKey, user.Username);
                return true;
            }

            return false;
        }

        public void LogOff()
        {
            Session.Clear();
        }

        public User GetCurrentUser()
        {
            var username = Session.GetString(SessionKey);

            if (!String.IsNullOrEmpty(username))
            {
                return usersDAL.GetUser(username);
            }

            return null;
        }

        public int Register(string username, string password, string email, string role)
        {
            int result = 0;
            var hashProvider = new HashProvider();
            var passwordHash = hashProvider.HashPassword(password);

            var user = new User
            {
                Username = username,
                Password = passwordHash.Password,
                Email = email,
                Role = role
            };

            result = usersDAL.AddUser(user);
            Session.SetString(SessionKey, user.Username);
            return result;
        }

        public bool UserHasRole(string[] roles)
        {
            var user = GetCurrentUser();
            return (user != null) &&
                roles.Any(r => r.ToLower() == user.Role.ToLower());
        }
    }
}
