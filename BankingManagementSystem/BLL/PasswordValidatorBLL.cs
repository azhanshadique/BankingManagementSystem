using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace BankingManagementSystem.BLL
{
    public static class PasswordValidatorBLL
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
    }
}