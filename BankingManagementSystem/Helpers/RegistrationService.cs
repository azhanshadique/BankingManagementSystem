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
using System.Web.UI;

namespace BankingManagementSystem.Helpers
{
    public static class RegistrationService
    {
        public static async Task<ApiResponseMessage> RegisterClientAsync(ClientDTO client)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(client);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                string apiUrl = ConfigurationManager.AppSettings["RegisterClientApiUrl"];

                HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content);
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
                        MessageContent = !string.IsNullOrEmpty(responseContent) ? responseContent : "Registration failed."
                    };
                }
            }
        }
        public static async Task<bool> CreateClientAsync(ClientDTO client)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(client);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                string apiUrl = ConfigurationManager.AppSettings["CreateClientApiUrl"];

                HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content);
                string responseContent = await response.Content.ReadAsStringAsync();
                return response.IsSuccessStatusCode;

                //if (response.IsSuccessStatusCode)
                //{
                //    return new ApiResponseMessage
                //    {
                //        MessageType = "success",
                //        MessageContent = responseContent
                //    };
                //}
                //else
                //{
                //    return new ApiResponseMessage
                //    {
                //        MessageType = "danger",
                //        MessageContent = !string.IsNullOrEmpty(responseContent) ? responseContent : "Client creation failed."
                //    };
                //}
            }
        }
    }

}