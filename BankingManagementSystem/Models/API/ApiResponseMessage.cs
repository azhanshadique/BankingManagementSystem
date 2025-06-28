using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankingManagementSystem.Models.API
{
    public class ApiResponseMessage
    {
        public string MessageType { get; set; }
        public string MessageContent { get; set; }
    }

}