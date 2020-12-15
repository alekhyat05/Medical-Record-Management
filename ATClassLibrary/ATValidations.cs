using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;

namespace ATClassLibrary
{
    public static class ATValidations 
    {
        public static string ATCapitalize(string inputString)
        {
            if (string.IsNullOrEmpty(inputString))
            {
                return string.Empty;
            }
            inputString = inputString.ToLower().Trim();

            string[] inputArray = inputString.Split(" ");
            string result = string.Empty;
            foreach(var i in inputArray)
            {
                result += i.Substring(0, 1).ToUpper() + i.Substring(1) + " ";
            }
            return result;
        }

        public static string ATExtractDigits(string inputString)
        {
            string digits = string.Empty;
            
            for (int i = 0; i < inputString.Length; i++)
            {
                if (Char.IsDigit(inputString[i]))
                    digits += inputString[i];
            }
            return digits;
        }
        public static bool ATPostalCodeValidation(string inputString)
        {
            if (string.IsNullOrEmpty(inputString))
            {
                return true;
            }

            var regex = new Regex("[ABCEGHJKLMNPRSTVXY][0-9][ABCEGHJKLMNPRSTVWXYZ] ?[0-9][ABCEGHJKLMNPRSTVWXYZ][0-9]");

            

            bool result = regex.IsMatch(inputString);
            return result;
        }
        public static string ATPostalCodeFormat(string inputString)
        {
            if (string.IsNullOrEmpty(inputString))
            {
                return string.Empty;
            }

            inputString.ToUpper();
            string.Format("[0:### ###]",inputString);
            return inputString;
        }

        public static bool ATZipCodeValidation(string inputString)
        {
            if (string.IsNullOrEmpty(inputString))
            {
                return true;
            }


            var regex = new Regex("\\d{5}([\\-]\\d{4})?");

            bool result = regex.IsMatch(inputString);
            return result;
            
        }
    }
}
