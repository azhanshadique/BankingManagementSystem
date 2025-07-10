using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BankingManagementSystem.Models.DTOs
{
	public class WithdrawDTO
	{
        [Required]
        public long AccountNumber { get; set; }

        [Required]
        public decimal Amount { get; set; }
        public string Remarks { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public int ClientId { get; set; }
    }
}