//namespace BankingManagementSystem.Models.API
//{
//    public class AuthRequestDTO
//    {
//        public string Username { get; set; }
//        public string Password { get; set; }
//    }

//}

using System.ComponentModel.DataAnnotations;

namespace BankingManagementSystem.Models.API
{
    public class AuthRequestDTO
    {
        [Required]
        [StringLength(50, MinimumLength = 6)]
        public string Username { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 8)]
        public string Password { get; set; }
    }
}
