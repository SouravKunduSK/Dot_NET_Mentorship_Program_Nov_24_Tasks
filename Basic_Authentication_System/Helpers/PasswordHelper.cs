using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;

namespace Basic_Authentication_System.Helpers
{
    public static class PasswordHelper
    {
        //Hashing Password
        public static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            password = Convert.ToBase64String(hash);
            return password;
        }

        //Verifying password
        public static bool VerifyPassword(string realPassword, string password)
        {
            var result = (realPassword == HashPassword(password)) ? false : true;
            return result;
        }

    }
}
