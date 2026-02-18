using System.Security.Cryptography;
using System.Text;

namespace DbUchebPractikNET9.Helpers
{
    public static class PasswordHasher
    {
        public static string Hash(string password)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha.ComputeHash(bytes);

            return Convert.ToHexString(hash); // выдаёт HEX строку
        }

        public static bool Verify(string password, string hash)
        {
            return Hash(password) == hash;
        }
    }
}