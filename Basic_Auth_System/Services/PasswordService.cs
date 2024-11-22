using System.Security.Cryptography;
using System.Text;

namespace Basic_Auth_System.Services
{
    public class PasswordService
    {
        public string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            var hashPassword = Convert.ToBase64String(hash);
            return hashPassword;
        }

        public bool ValidatePassword(string hashedPassword, string providedPassword)
        {
            var convertedPassword = HashPassword(providedPassword);
            return hashedPassword == convertedPassword;
        }
    }
}
