using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankingManagementSystem.Models.DTOs
{
    public class PendingRequestDTO
    {
        public int RequestId { get; set; }
        public string RequestType { get; set; }
        public string Payload { get; set; }
        public DateTime? RequestedOn { get; set; }
        public string Status { get; set; }
        public DateTime? RepliedOn { get; set; }
    }

}