using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Bakery_Software
{
    public class utils
    {
        public static string hashPassword(string password)
        {
            // SHA-1 (Secure Hash Algorithm 1)
            SHA1CryptoServiceProvider SHA1 = new SHA1CryptoServiceProvider();
            byte[] password_bytes = Encoding.ASCII.GetBytes(password);
            byte[] encrypted_bytes = SHA1.ComputeHash(password_bytes);
            return Convert.ToBase64String(encrypted_bytes);
        }
    }
}
