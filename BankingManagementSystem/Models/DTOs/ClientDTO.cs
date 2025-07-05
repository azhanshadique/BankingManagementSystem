//using BankingManagementSystem.Models.ConstraintTypes;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;

//namespace BankingManagementSystem.Models.DTOs
//{
//    public class ClientDTO
//    {
//        public string FullName { get; set; }
//        public string ParentName { get; set; }
//        public string DOB { get; set; }
//        public string Gender { get; set; }
//        public string Nationality { get; set; }
//        public string Occupation { get; set; }
//        public string AadhaarNumber { get; set; }
//        public string PANNumber { get; set; }
//        public string MobileNumber { get; set; }
//        public string EmailId { get; set; }
//        public string Address { get; set; }
//        public string State { get; set; }
//        public string City { get; set; }
//        public string Pincode { get; set; }
//        public string AccountType { get; set; }
//        public bool IsJointAccount { get; set; }
//        public int JointClientId { get; set; }
//        //public string JointRelationship { get; set; }
//        public string Username { get; set; }
//        public string Password { get; set; }
//        public string ConfirmPassword { get; set; }
//        public bool CoHolderApproved { get; set; }
//        public bool AdminApproved { get; set; }
//    }

//}


using System;
using System.ComponentModel.DataAnnotations;

namespace BankingManagementSystem.Models.DTOs
{
    public class ClientDTO
    {
        public int ClientId { get; set; }

        // Personal Details
        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required, StringLength(100)]
        public string ParentName { get; set; }

        [Required]
        //[RegularExpression(@"^\d{4}-\d{2}-\d{2}$", ErrorMessage = "DOB must be in YYYY-MM-DD format")]
        [DataType(DataType.Date)]
        public DateTime? DOB { get; set; }

        [Required]
        //[RegularExpression("^(Male|Female|Other)$", ErrorMessage = "Gender must be Male, Female, or Other.")]
        public string Gender { get; set; }

        [Required]
        public string Nationality { get; set; }

        [Required]
        public string Occupation { get; set; }

        [Required]
        //[RegularExpression(@"^\d{12}$", ErrorMessage = "Aadhaar must be exactly 12 digits.")]
        public string AadhaarNumber { get; set; }

        [Required]
        //[RegularExpression(@"^[A-Z]{5}[0-9]{4}[A-Z]{1}$", ErrorMessage = "Invalid PAN format.")]
        public string PANNumber { get; set; }




        // Contact Details
        [Required]
        //[RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Invalid mobile number")]
        public string MobileNumber { get; set; }

        [Required]
        [EmailAddress]
        public string EmailId { get; set; }

        [Required]
        [StringLength(200)]
        public string Address { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        //[RegularExpression(@"^\d{6}$", ErrorMessage = "Pincode must be exactly 6 digits")]
        public string Pincode { get; set; }




        // Account Details
        [Required]
        //[RegularExpression("^(Savings|Current)$", ErrorMessage = "Invalid account type")]
        public string AccountType { get; set; }

        public bool IsJointAccount { get; set; }

        public int? JointClientId { get; set; } = 0;

        //public int? AccountId { get; set; }

        //public decimal? InitialDeposit { get; set; }




        // Login Details
        [Required]
        //[StringLength(50, MinimumLength = 6, ErrorMessage = "Username must be between 6 and 50 characters.")]
        public string Username { get; set; }

        [Required]
        //[StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters.")]
        public string Password { get; set; }

        [Required]
        //[Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }

        // Approval Flags
        public bool CoHolderApproved { get; set; } = false;

        public bool AdminApproved { get; set; } = false;

        // Extra (optional)
        //public string Status { get; set; } // e.g., Active / Inactive 
    }
}

