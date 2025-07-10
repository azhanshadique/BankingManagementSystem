using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BankingManagementSystem.Models.DTOs
{
    public class DepositDTO
    {
        [Required]
        //[RegularExpression(@"[0-9]$")]
        public long AccountNumber { get; set; }

        [Required]
        //[RegularExpression(@"[0-9]$")]
        public decimal Amount { get; set; }
        public string Remarks { get; set; }

        [Required]
        //[StringLength(100, MinimumLength = 8)]
        public string Password { get; set; }

        [Required]
        //[RegularExpression(@"[0-9]$")]
        public int ClientId { get; set; }
    }

}