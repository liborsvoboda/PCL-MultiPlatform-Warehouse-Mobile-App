using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Terminal.Functions
{
    class MathFunctions
    {
        private static SByte[] byteToSbyte(String content)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(content);
            sbyte[] sbytes = new sbyte[bytes.Length];
            for (int i = 0; i < bytes.Length; i++)
                sbytes[i] = (sbyte)bytes[i];
            return sbytes;
        }


        public static string HashSHA512(string value)
        {
            using (var sha = SHA512.Create())
            {
                return Convert.ToBase64String(sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(value)));
            }
        }

        public static bool IsNumeric(string strNumber)
        {
            if (string.IsNullOrEmpty(strNumber))
            { return false; }
            else
            {
                int numberOfChar = strNumber.Count();
                if (numberOfChar > 0)
                {
                    bool r = strNumber.All(char.IsDigit);
                    return r;
                }
                else { return false; }
            }
        }

        public static string DecimalToHexadecimal(int dec)
        {
            if (dec < 1) return "0";

            int hex = dec;
            string hexStr = string.Empty;

            while (dec > 0)
            {
                hex = dec % 16;

                if (hex < 10)
                    hexStr = hexStr.Insert(0, Convert.ToChar(hex + 48).ToString());
                else
                    hexStr = hexStr.Insert(0, Convert.ToChar(hex + 55).ToString());

                dec /= 16;
            }

            return hexStr;
        }

        public static string GetNumberPart(string input)
        {
            return Regex.Replace(input, "[^.0-9]", "");
        }

    }
}
