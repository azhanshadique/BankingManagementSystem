using BankingManagementSystem.Models.ConstraintTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankingManagementSystem.Models.DTOs
{
    public class ClientDTO
    {
        public string FullName { get; set; }
        public string ParentName { get; set; }
        public string DOB { get; set; }
        public string Gender { get; set; }
        public string Nationality { get; set; }
        public string Occupation { get; set; }
        public string AadhaarNumber { get; set; }
        public string PANNumber { get; set; }
        public string MobileNumber { get; set; }
        public string EmailId { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Pincode { get; set; }
        public string AccountType { get; set; }
        public bool IsJointAccount { get; set; }
        public int JointClientId { get; set; }
        public string JointRelationship { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public bool CoHolderApproved { get; set; }
        public bool AdminApproved { get; set; }
    }

}