using System.Security.Cryptography;
using System.Text;

namespace User_Authentication_Service.Helpers
{
    public static class Md5Helper
    {
        public static string HashPassword(string password)
        {
            using var md5 = MD5.Create();
            var inputBytes = Encoding.UTF8.GetBytes(password);
            var hashBytes = md5.ComputeHash(inputBytes);
            return Convert.ToHexString(hashBytes); 
        }
        public static bool VerifyPassword(string password, string hash)
        {
            return HashPassword(password) == hash;
        }
    }
}
