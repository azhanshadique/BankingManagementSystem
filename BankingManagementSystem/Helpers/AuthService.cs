using BankingManagementSystem.Models.API;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BankingManagementSystem.Helpers
{
	public class AuthService
	{
        public static async Task<AuthTokenResponse> AuthenticateUserAsync(AuthRequestDTO request, string apiUrl)
        {
            using (HttpClient client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<AuthTokenResponse>(responseContent);
                }

                return null;
            }
        }

    }
}