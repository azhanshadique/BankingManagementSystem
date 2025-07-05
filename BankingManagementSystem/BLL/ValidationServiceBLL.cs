using System;
using System.Linq;
using System.Text.RegularExpressions;

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
        public static bool IsValidDOB(DateTime? dob, out string errorMessage)
        {
            errorMessage = "";

            if (dob == null)
            {
                errorMessage = "Date of Birth is required.";
                return false;
            }

            if (dob > DateTime.Now)
            {
                errorMessage = "Date of Birth cannot be in the future.";
                return false;
            }

            int age = DateTime.Now.Year - dob.Value.Year;
            if (dob > DateTime.Now.AddYears(-age)) age--;

            if (age < 18)
            {
                errorMessage = "You must be at least 18 years old to register.";
                return false;
            }

            if (dob.Value.Year < 1900)
            {
                errorMessage = "Year of birth is too far in the past. Please check again.";
                return false;
            }

            return true;
        }
        public static bool IsValidGender(string gender, out string errorMessage)
        {
            errorMessage = "";

            if (string.IsNullOrWhiteSpace(gender))
            {
                errorMessage = "Gender is required.";
                return false;
            }

            string[] validGenders = { "Male", "Female", "Other", "Prefer not to say" };

            if (!validGenders.Contains(gender.Trim(), StringComparer.OrdinalIgnoreCase))
            {
                errorMessage = "Invalid gender. Accepted values are: Male, Female, Other or Prefer not to say.";
                return false;
            }

            return true;
        }

        public static bool IsValidUsername(string username, out string errorMessage)
        {
            errorMessage = "";
            if (username.Length < 6)
            {
                errorMessage = "Username must be at least 6 characters long.";
                return false;
            }
            if (Regex.IsMatch(username, "[^a-zA-Z0-9]")) // special char
            {
                errorMessage = "Username must not contain any special character.";
                return false;
            }

            return true;
        }
        public static bool IsValidAadhaar(string aadhaar, out string errorMessage)
        {
            errorMessage = "";

            if (string.IsNullOrWhiteSpace(aadhaar))
            {
                errorMessage = "Aadhaar number is required.";
                return false;
            }

            // Aadhaar must be exactly 12 digits
            if (!Regex.IsMatch(aadhaar, @"^\d{12}$"))
            {
                errorMessage = "Invalid Aadhaar number format. It must be exactly 12 digits.";
                return false;
            }

            return true;
        }

        public static bool IsValidPAN(string pan, out string errorMessage)
        {
            errorMessage = "";

            if (string.IsNullOrWhiteSpace(pan))
            {
                errorMessage = "PAN number is required.";
                return false;
            }

            if (!Regex.IsMatch(pan, @"^[A-Z]{5}[0-9]{4}[A-Z]$"))
            {
                errorMessage = "Invalid PAN number format. It must be 10 characters: 5 uppercase letters, 4 digits, and 1 uppercase letter.";
                return false;
            }

            return true;
        }

        public static bool IsValidMobileNumber(string mobileNumber, out string errorMessage)
        {
            errorMessage = "";

            if (string.IsNullOrWhiteSpace(mobileNumber))
            {
                errorMessage = "Mobile number is required.";
                return false;
            }

            // Check if it's exactly 10 digits and starts with 6-9
            if (!Regex.IsMatch(mobileNumber, @"^[6-9]\d{9}$"))
            {
                errorMessage = "Invalid mobile number format. It must be 10 digits starting with 6, 7, 8, or 9.";
                return false;
            }

            return true;
        }

        public static bool IsValidPincode(string pincode, out string errorMessage)
        {
            errorMessage = "";

            if (string.IsNullOrWhiteSpace(pincode))
            {
                errorMessage = "Pincode is required.";
                return false;
            }

            if (!Regex.IsMatch(pincode, @"^\d{6}$"))
            {
                errorMessage = "Invalid pincode format. It must be exactly 6 digits.";
                return false;
            }

            return true;
        }

    }
}

