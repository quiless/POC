using System;
using System.Text.RegularExpressions;

namespace POC.Artifacts.Helpers
{
	public static class StringHelper
	{
		public static string Encrypt(this string inputString)
		{
            byte[] data = System.Text.Encoding.ASCII.GetBytes(inputString);
            data = System.Security.Cryptography.SHA256.HashData(data);
            return System.Text.Encoding.ASCII.GetString(data).Replace("\u0000", "");
        }

        public static bool IsCNPJ(this string cnpj)
        {
            return new Regex("[0-9]{2}\\.?[0-9]{3}\\.?[0-9]{3}\\/?[0-9]{4}\\-?[0-9]{2}", RegexOptions.IgnoreCase).IsMatch(cnpj);
        }

        public static bool IsDriverLicenseNumber(this string cnh)
        {
            return new Regex("^[0-9]*$", RegexOptions.IgnoreCase).IsMatch(cnh);
        }

        public static bool IsMotorcyclePlateNumber(this string plateNumber)
        {
            return new Regex("[A-Z]{3}[0-9][0-9A-Z][0-9]{2}", RegexOptions.IgnoreCase).IsMatch(plateNumber);
        }

        public static bool IsValidPassword(this string password)
        {
            return new Regex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{12,16}$", RegexOptions.IgnoreCase).IsMatch(password);
        }

        public static string OnlyNumbers(this string val)
        {
           
            try
            {
                Regex regexObj = new Regex(@"[^\d]");
                return regexObj.Replace(val, "");
            }
            catch { }
            return val;

        }
    }
}

