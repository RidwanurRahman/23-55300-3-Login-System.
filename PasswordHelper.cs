using System.Security.Cryptography;
using System.Text;

namespace Login_and_Register
{
    internal static class PasswordHelper
    {
        public static string ComputeSha256(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder(bytes.Length * 2);

                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("X2"));
                }

                return builder.ToString();
            }
        }
    }
}
