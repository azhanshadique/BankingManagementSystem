using BankingManagementSystem.DAL;
using BankingManagementSystem.Models;
using BankingManagementSystem.Models.API;
using BankingManagementSystem.Models.ConstraintTypes;
using BankingManagementSystem.Models.DTOs;
using Newtonsoft.Json;
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
        public static async Task<(bool IsSuccess, string Message)> CreateNewOfflineClientAsync(ClientDTO client)
        {
            return await AdminDAL.CreateOfflineClientAsync(client);
        }
        public static async Task<(bool IsSuccess, string Message)> UpdateClientDetailsAsync(int clientId, ClientDTO client)
        {
            if (!await ClientDAL.IsClientExistsByClientIdAsync(clientId))
                return (false, "Client does not exists.");

            var (IsValid, Message) = ClientBLL.ValidateClientProfileDetails(client);
            if (IsValid)
            {
                return await AdminDAL.UpdateClientProfileDetailsAsync(clientId, client);
            }
            return (false, Message);
        }

        public static async Task<(bool IsSuccess, string Message)> DeleteClientAccountAsync(long accountNumber)
        {
            decimal currentBalance = await TransactionDAL.GetBalanceAsync(accountNumber);
            if (currentBalance < 0)
                return (false, "Invalid account number.");
            return await AdminDAL.DeleteClientAccountAsync(accountNumber);
        }
    }
}
