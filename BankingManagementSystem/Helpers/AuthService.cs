using BankingManagementSystem.Models.API;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BankingManagementSystem.Helpers
{
    public static class AuthService
    {
        private static readonly HttpClient httpClient = new HttpClient();

        static AuthService()
        {
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static async Task<AuthTokenResponse> AuthenticateUserAsync(AuthRequestDTO request, string apiUrl)
        {
            //var json = JsonConvert.SerializeObject(request);
            //var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsJsonAsync(apiUrl, request);

            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<AuthTokenResponse>(responseContent);
            }

            return null;
        }
    }
}
