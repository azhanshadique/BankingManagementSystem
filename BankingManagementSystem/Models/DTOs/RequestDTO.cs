//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;

//namespace BankingManagementSystem.Models.DTOs
//{
//    public class RequestDTO
//    {
//        public int RequestId { get; set; }
//        public string RequestType { get; set; }
//        public string Payload { get; set; }
//        public DateTime? RequestedOn { get; set; }
//        public string Status { get; set; }
//        public DateTime? RepliedOn { get; set; }
//    }

//}

using System;
using System.ComponentModel.DataAnnotations;

namespace BankingManagementSystem.Models.DTOs
{
    public class RequestDTO
    {
        public int RequestId { get; set; } // Optional in POST, required in PUT

        [Required(ErrorMessage = "RequestType is required")]
        [StringLength(50)]
        public string RequestType { get; set; }

        [Required(ErrorMessage = "Payload is required")]
        public string Payload { get; set; }

        public DateTime? RequestedOn { get; set; }

        [StringLength(20)]
        public string Status { get; set; }

        public DateTime? RepliedOn { get; set; }
        public int? RepliedBy { get; set; }
    }
}
