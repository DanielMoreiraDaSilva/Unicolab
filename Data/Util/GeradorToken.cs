using System;
using System.Text;

namespace Data.Util
{
    public static class GeradorToken
    {
        public static string GerarToken()
        {
            var token = Guid.NewGuid();
            var novoToken = string.Concat(token);
            var invertido = Reverse(novoToken);
            var base64 = Base64Encode(invertido);
            return base64;
        }

        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        private static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
    }
}
