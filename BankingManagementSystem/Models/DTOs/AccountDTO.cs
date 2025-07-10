using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankingManagementSystem.Models.DTOs
{
    public class AccountDTO
    {
        public long AccountNumber { get; set; }
        public string AccountType { get; set; }
        public decimal Balance { get; set; }
        public string Status { get; set; }
        public bool IsPrimary { get; set; }
        public bool IsJoint { get; set; }
        public int CoHolderClientId { get; set; }
        public string CoHolderName { get; set; }
    }
}
