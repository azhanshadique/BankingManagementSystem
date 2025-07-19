using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BankingManagementSystem.Models.DTOs
{
	public class LinkAccountDTO
	{
        [Required]
        public long AccountNumber { get; set; }

        [Required]
        public string AccountType { get; set; }

        [Required]
        public string MobileNumber { get; set; }

        [Required]
        public string EmailId { get; set; }

        [Required]
        public int ClientId { get; set; }

        [Required]
        public bool IsJointAccount { get; set; }


        public int? JointClientId { get; set; } = 0;
        public string JointClientMobileNo { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}