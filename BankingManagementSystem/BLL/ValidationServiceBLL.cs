using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace BankingManagementSystem.BLL
{
    public static class ValidationServiceBLL
    {
        public static bool IsStrongPassword(string password, string confirmPassword, out string errorMessage)
        {
            errorMessage = "";

            if (password.Length < 8)
            {
                errorMessage = "Password must be at least 8 characters long.";
                return false;
            }

            if (!Regex.IsMatch(password, "[A-Z]"))
            {
                errorMessage = "Password must contain at least one uppercase letter.";
                return false;
            }

            if (!Regex.IsMatch(password, "[a-z]"))
            {
                errorMessage = "Password must contain at least one lowercase letter.";
                return false;
            }

            if (!Regex.IsMatch(password, "[0-9]"))
            {
                errorMessage = "Password must contain at least one digit.";
                return false;
            }

            if (!Regex.IsMatch(password, "[^a-zA-Z0-9]")) // special char
            {
                errorMessage = "Password must contain at least one special character.";
                return false;
            }

            if (password != confirmPassword)
            {
                errorMessage = "Passwords do not match!";
                return false;
            }
            return true;
        }
        public static bool IsValidDOB(string dobInput, out string errorMessage)
        {
            errorMessage = "";
            DateTime dob;

            if (!DateTime.TryParse(dobInput, out dob))
            {
                errorMessage = "Invalid Date of Birth format.";
                return false;
            }

            if (dob > DateTime.Now)
            {
                errorMessage = "Date of Birth cannot be in the future.";
                return false;
            }

            int age = DateTime.Now.Year - dob.Year;
            if (dob > DateTime.Now.AddYears(-age)) age--;

            if (age < 18)
            {
                errorMessage = "You must be at least 18 years old to register.";
                return false;
            }

            return true;
        }
    }
}