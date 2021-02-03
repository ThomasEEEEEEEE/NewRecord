using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevOne.Security.Cryptography.BCrypt;

namespace NewRecord_Backend.Database
{
    public class Hashing
    {
        private static string GetSalt()
        {
            return BCryptHelper.GenerateSalt(10);
        }

        public static string HashPassword(string password)
        {
            return BCryptHelper.HashPassword(password, GetSalt());
        }

        public static bool VerifyPassword(string password, string hash)
        {
            return BCryptHelper.CheckPassword(password, hash);
        }
    }
}
