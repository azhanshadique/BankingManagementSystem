using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankingManagementSystem.Models.DTOs
{
    public class TransferDTO
    {
        public long FromAccountNumber { get; set; }
        public long ToAccountNumber { get; set; }
        public decimal Amount { get; set; }
        public string Remarks { get; set; }
        public string Password { get; set; }
        public int ClientId { get; set; }
    }

}