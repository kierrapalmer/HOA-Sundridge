using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HOASunridge.Pages.Shared {

    public static class Extensions {

        public static string CalculateSHA256(string str) {
            StringBuilder hashString = new StringBuilder();

            SHA256 sha256 = SHA256Managed.Create();
            byte[] hashValue;
            UTF8Encoding objUtf8 = new UTF8Encoding();
            hashValue = sha256.ComputeHash(objUtf8.GetBytes(str));

            foreach (byte b in hashValue)
                hashString.Append(b.ToString("x2"));

            return hashString.ToString();
        }

        public static string CleanPhone(string p) //The goal of this method is to return a string of 10 numbers to be hashed for the default password.
        {
            Regex digits = new Regex(@"[^\d]");
            return digits.Replace(p, "");
        }

        public static string FormatPhone(string p) {
            if (string.IsNullOrEmpty(p))
                return null;
            p = CleanPhone(p);
            return string.Format(" {0:###-###-####}", Double.Parse(p));
        }
    }
}