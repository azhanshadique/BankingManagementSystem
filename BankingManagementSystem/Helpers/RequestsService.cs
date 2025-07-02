using BankingManagementSystem.Models.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using BankingManagementSystem.Models.Constants;
using BankingManagementSystem.Models.API;
using System.Configuration;

namespace BankingManagementSystem.Helpers
{
    public static class RequestsService
    {
        private static readonly HttpClient httpClient = new HttpClient
        {
            BaseAddress = new Uri(ConfigurationManager.AppSettings["BaseApiUrl"])
        };

        static RequestsService()
        {
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static async Task<List<PendingRequestDTO>> GetRequestsAsync(string status = "Pending", string sortBy = DbColumns.CreatedOn, string sortDirection = "DESC")
        {
            string url = $"requests?status={status}&sortBy={sortBy}&sortDirection={sortDirection}";
            var response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<PendingRequestDTO>>(json);
            }

            return new List<PendingRequestDTO>();
        }

        public static async Task<PendingRequestDTO> GetRequestByIdAsync(int id)
        {
            string url = $"requests/{id}";
            var response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<PendingRequestDTO>(json);
            }

            return null;
        }

        public static async Task<bool> UpdateStatusAsync(int id, string status, int repliedBy)
        {
            var json = JsonConvert.SerializeObject(status);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            string url = $"requests/{id}/update/status?repliedBy={repliedBy}";

            var response = await httpClient.PutAsync(url, content);
            return response.IsSuccessStatusCode;
        }

        public static async Task<ApiResponseMessage> UpdatePayloadAsync(int id, ClientDTO clientDTO)
        {
            var json = JsonConvert.SerializeObject(clientDTO);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            string url = $"requests/{id}/update/payload";

            var response = await httpClient.PutAsync(url, content);
            //return response.IsSuccessStatusCode;

            //HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content);
            string responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return new ApiResponseMessage
                {
                    MessageType = "success",
                    MessageContent = responseContent
                };
            }
            else
            {
                return new ApiResponseMessage
                {
                    MessageType = "danger",
                    MessageContent = !string.IsNullOrEmpty(responseContent) ? responseContent : "Update failed."
                };
            }
        }

        public static async Task<bool> DeleteRequestAsync(int id, string status = "Pending")
        {
            string url = $"requests/{id}?status={status}";
            var response = await httpClient.DeleteAsync(url);
            return response.IsSuccessStatusCode;
        }
    }
}
