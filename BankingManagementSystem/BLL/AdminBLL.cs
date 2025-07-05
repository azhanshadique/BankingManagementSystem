using BankingManagementSystem.DAL;
using BankingManagementSystem.Models;
using BankingManagementSystem.Models.API;
using BankingManagementSystem.Models.DTOs;
using System.Threading.Tasks;

namespace BankingManagementSystem.BLL
{
    public static class AdminBLL
    {
        public static async Task<User> ValidateAdminLoginAsync(AuthRequestDTO admin)
        {
            return await AdminDAL.CheckAdminCredentialsAsync(admin);
        }

        public static async Task<bool> IsAdminExistsByAdminIdAsync(int adminId)
        {
            return await AdminDAL.IsAdminExistsByAdminIdAsync(adminId);
        }

        public static async Task<(bool IsSuccess, string Message)> CreateNewClientAsync(ClientDTO client)
        {
            return await AdminDAL.CreateClientAsync(client);
        }
    }
}
