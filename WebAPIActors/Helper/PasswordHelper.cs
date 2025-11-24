using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;


namespace WebAPIActors.Helper
{
    public static class PasswordHelper
    {
        public static string HashPassword(string password, string salt)
        {
            using (var sha = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password + salt);
                var hash = sha.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }

        }

        public static string GenerateSalt()
        {
            byte[] saltBytes = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }
    }
}
