using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Capstone.Providers.Auth
{
    public class HashProvider
    {
        private const int WorkFactor = 10000;

        public HashedPassword HashPassword(string plainTextPassword)
        {            
            Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes(plainTextPassword, 0, WorkFactor);

            byte[] hash = rfc.GetBytes(20);

            string salt = Convert.ToBase64String(rfc.Salt);

            return new HashedPassword(Convert.ToBase64String(hash));
        }

        public bool VerifyPassword(string existingHashedPassword, string plainTextPassword)
        {
            Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes(plainTextPassword, 0, WorkFactor);

            byte[] hash = rfc.GetBytes(20);

            string newHashedPassword = Convert.ToBase64String(hash);

            return (existingHashedPassword == newHashedPassword);
        }
    }

    public class HashedPassword
    {
        public HashedPassword(string password)
        {
            this.Password = password;
        }

        public string Password { get; }
    }
}
