using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BankingManagementSystem.Models.DTOs
{
    public class TransactionDTO
    {
        public long AccountNumber { get; set; }
        public int TransactionId { get; set; }

        public string TransactionType { get; set; } // Deposit, Withdraw, Transfer

        public decimal Amount { get; set; }
        public decimal BalanceAfter { get; set; }
        public string Remarks { get; set; }

        public int PerformedBy { get; set; } // client_id
        public long CounterPartyAccountNo { get; set; } 
        public DateTime TransactionDate { get; set; } = DateTime.Now;
        public bool IsSuccessful { get; set; } = true;
    }

}