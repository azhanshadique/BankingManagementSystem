using BankingManagementSystem.Models.API;
using BankingManagementSystem.Models.Constants;
using BankingManagementSystem.Models.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BankingManagementSystem.Helpers
{
    public static class RequestsService
    {

        private static readonly HttpClient httpClient = HttpClientProvider.GetClient();

        public static async Task<List<RequestDTO>> GetRequestsForAdminAsync(string status = "Pending", string sortColumn = DbColumns.CreatedOn, string sortDirection = "DESC")
        {
            string url = $"api/admin/requests?status={status}&sortBy={sortColumn}&sortDirection={sortDirection}";
            var response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<RequestDTO>>(json);
            }

            return new List<RequestDTO>();
        }

        public static async Task<RequestDTO> GetRequestByIdAsync(int id)
        {
            string url = $"api/requests/{id}";
            var response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<RequestDTO>(json);
            }

            return null;
        }
        public static async Task<(RequestDTO Request, string ErrorMessage)> GetPublicRegisterRequestByIdAsync(int id)
        {
            string url = $"api/public/register/request/{id}";
            var response = await httpClient.GetAsync(url);
            string content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var request = JsonConvert.DeserializeObject<RequestDTO>(content);
                return (request, null);
            }
            else
            {
                try
                {
                    var errorObj = JsonConvert.DeserializeObject<ApiErrorMessageWrapper>(content);
                    return (null, errorObj?.Message ?? "Something went wrong.");
                }

                catch
                {
                    return (null, "Something went wrong.");
                }
            }
        }

        public static async Task<bool> ApproveRequestAsync(int id, int repliedBy)
        {
            string url = $"api/requests/{id}/approve?repliedBy={repliedBy}";
            var response = await httpClient.PutAsync(url, null);
            return response.IsSuccessStatusCode;
        }
        public static async Task<bool> RejectRequestAsync(int id, int repliedBy)
        {
            string url = $"api/requests/{id}/reject?repliedBy={repliedBy}";
            var response = await httpClient.PutAsync(url, null);
            return response.IsSuccessStatusCode;
        }

        public static async Task<ApiResponseMessage> UpdatePublicRegisterRequestAsync(int id, ClientDTO clientDTO)
        {
            //var json = JsonConvert.SerializeObject(clientDTO);
            //var content = new StringContent(json, Encoding.UTF8, "application/json");
            string url = $"api/public/update/register-request/{id}";

            var response = await httpClient.PutAsJsonAsync(url, clientDTO);
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
        public static async Task<ApiResponseMessage> DeletePublicRegisterRequestAsync(int id)
        {
            string url = $"api/public/delete/request/{id}";

            var response = await httpClient.PutAsync(url, null); 
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
                    MessageContent = !string.IsNullOrEmpty(responseContent) ? responseContent : "Delete failed."
                };
            }
        }
         public static async Task<ApiResponseMessage> DeleteClientRequestByClientAsync(int id, int clientid)
        {
            string url = $"api/client/requests/{id}/delete?repliedBy={clientid}";

            var response = await httpClient.PutAsync(url, null); 
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
                    MessageContent = !string.IsNullOrEmpty(responseContent) ? responseContent : "Delete failed."
                };
            }
        }

        public static async Task<ApiResponseMessage> UpdateRequestAsync(int id, ClientDTO clientDTO)
        {
            //var json = JsonConvert.SerializeObject(clientDTO);
            //var content = new StringContent(json, Encoding.UTF8, "application/json");
            string url = $"api/requests/{id}/update/request";

            var response = await httpClient.PutAsJsonAsync(url, clientDTO);
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
            string url = $"api/admin/requests/{id}?status={status}";
            var response = await httpClient.DeleteAsync(url);
            return response.IsSuccessStatusCode;
        }

        public static async Task<List<RequestDTO>> GetReceivedRequestsForClientAsync(int clientId, string sortColumn = DbColumns.CreatedOn, string sortDirection = "DESC")
        {
            string url = $"api/client/requests/received?clientId={clientId}&sortColumn={sortColumn}&sortDirection={sortDirection}";
            var response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<RequestDTO>>(json);
            }

            return new List<RequestDTO>();
        }

        public static async Task<List<RequestDTO>> GetSentRequestsByClientAsync(int clientId, string sortColumn = DbColumns.CreatedOn, string sortDirection = "DESC")
        {
            string url = $"api/client/requests/sent?clientId={clientId}&sortColumn={sortColumn}&sortDirection={sortDirection}";
            var response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<RequestDTO>>(json);
            }

            return new List<RequestDTO>();
        }
    }
}

