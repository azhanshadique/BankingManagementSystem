using BankingManagementSystem.Helpers;
using BankingManagementSystem.Models.API;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BankingManagementSystem.BLL
{
	public class AuthBLL
	{
        public async Task<AuthTokenResponse> LoginClientAsync(AuthRequestDTO request)
        {
            string apiUrl = ConfigurationManager.AppSettings["AuthClientApiUrl"];
            return await AuthService.AuthenticateUserAsync(request, apiUrl);
        }

        public async Task<AuthTokenResponse> LoginAdminAsync(AuthRequestDTO request)
        {
            string apiUrl = ConfigurationManager.AppSettings["AuthAdminApiUrl"];
            return await AuthService.AuthenticateUserAsync(request, apiUrl);
        }
    }
}