using BankingManagementSystem.Helpers;
using BankingManagementSystem.Models.API;
using System.Configuration;
using System.Threading.Tasks;

namespace BankingManagementSystem.BLL
{
    public static class AuthBLL
    {
        public async static Task<AuthTokenResponse> LoginClientAsync(AuthRequestDTO request)
        {
            string apiUrl = ConfigurationManager.AppSettings["AuthClientApiUrl"];
            return await AuthService.AuthenticateUserAsync(request, apiUrl);
        }

        public async static Task<AuthTokenResponse> LoginAdminAsync(AuthRequestDTO request)
        {
            string apiUrl = ConfigurationManager.AppSettings["AuthAdminApiUrl"];
            return await AuthService.AuthenticateUserAsync(request, apiUrl);
        }
    }
}