using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;

namespace BankingManagementSystem.Helpers
{
    public static class PasswordHasher
    {
        public static string HashPassword(string password)
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] salt = new byte[16];
                rng.GetBytes(salt);

                var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100_000, HashAlgorithmName.SHA256);
                byte[] hash = pbkdf2.GetBytes(32);

                // Combine salt + hash
                return Convert.ToBase64String(salt.Concat(hash).ToArray());
            }
        }

        public static bool VerifyPassword(string password, string storedHash)
        {
            byte[] fullHash = Convert.FromBase64String(storedHash);
            byte[] salt = fullHash.Take(16).ToArray();
            byte[] hash = fullHash.Skip(16).ToArray();

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100_000, HashAlgorithmName.SHA256);
            byte[] inputHash = pbkdf2.GetBytes(32);

            return hash.SequenceEqual(inputHash);
        }
    }

}