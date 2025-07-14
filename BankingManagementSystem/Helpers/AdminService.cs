using BankingManagementSystem.Models.API;
using BankingManagementSystem.Models.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BankingManagementSystem.Helpers
{
	public class AdminService
	{
        private static readonly HttpClient httpClient = HttpClientProvider.GetClient();

        public static async Task<bool> CreateClientAsync(ClientDTO client)
        {
            //var json = JsonConvert.SerializeObject(client);
            //var content = new StringContent(json, Encoding.UTF8, "application/json");

            string apiUrl = ConfigurationManager.AppSettings["CreateClientApiUrl"];

            HttpResponseMessage response = await httpClient.PostAsJsonAsync(apiUrl, client);
            return response.IsSuccessStatusCode;
        } 

        public static async Task<bool> UpdateClientDetailsAsync(ClientDTO client)
        {
            //var json = JsonConvert.SerializeObject(client);
            //var content = new StringContent(json, Encoding.UTF8, "application/json");
            string apiUrl = $"api/admin/update-client?clientId={client.ClientId}";

            HttpResponseMessage response = await httpClient.PutAsJsonAsync(apiUrl, client);
            return response.IsSuccessStatusCode;
           
        }
        
        public static async Task<bool> DeleteAccountAsync(long accountNumber)
        {
            string apiUrl = $"api/admin/client-account?accountNumber={accountNumber}";

            HttpResponseMessage response = await httpClient.DeleteAsync(apiUrl);
            return response.IsSuccessStatusCode;
           
        }
    }
}