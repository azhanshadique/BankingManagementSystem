
using BankingManagementSystem.Models.API;
using BankingManagementSystem.Models.DTOs;
using Newtonsoft.Json;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BankingManagementSystem.Helpers
{
    public static class RegistrationService
    {
        private static readonly HttpClient httpClient = HttpClientProvider.GetClient();

        public static async Task<ApiResponseMessage> RegisterClientAsync(ClientDTO client)
        {
            var json = JsonConvert.SerializeObject(client);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            string apiUrl = ConfigurationManager.AppSettings["RegisterClientApiUrl"];

            HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content);
            string responseContent = await response.Content.ReadAsStringAsync();

            return new ApiResponseMessage
            {
                MessageType = response.IsSuccessStatusCode ? "success" : "danger",
                MessageContent = !string.IsNullOrEmpty(responseContent)
                                    ? responseContent
                                    : "Registration failed."
            };
        }

        public static async Task<bool> CreateClientAsync(ClientDTO client)
        {
            var json = JsonConvert.SerializeObject(client);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            string apiUrl = ConfigurationManager.AppSettings["CreateClientApiUrl"];

            HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content);
            return response.IsSuccessStatusCode;
        }
    }
}
