using BankingManagementSystem.Models.API;
using BankingManagementSystem.Models.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace BankingManagementSystem.Helpers
{
	public class ClientService
    {
        private static readonly HttpClient httpClient = HttpClientProvider.GetClient();
        public static async Task<ClientDTO> GetClientByIdAsync(int id)
        {
            string url = $"api/client/{id}";
            var response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ClientDTO>(json);
            }

            return null;
        }
        //public static async Task<int> SendUpdateProfileDetailsRequestAsync(int clientId, string requestType, ClientDTO client)
        //{
        //    string url = $"api/client/requests/send-request?clientId={clientId}&requestType={requestType}";
        //    StringContent content = new StringContent(JsonConvert.SerializeObject(client), System.Text.Encoding.UTF8, "application/json");

        //    var response = await httpClient.PostAsync(url, content);

        //    if (response.IsSuccessStatusCode)
        //    {
        //        string json = await response.Content.ReadAsStringAsync();
        //        dynamic result = JsonConvert.DeserializeObject<dynamic>(json);
        //        return (int)result.RequestID;
        //    }

        //    return 0;
        //}

        //public static async Task<int> SendUpdateProfileDetailsRequestAsync(int clientId, string requestType, ClientDTO client)
        //{
        //    string url = $"api/client/requests/send-request?clientId={clientId}&requestType={requestType}";
        //    StringContent content = new StringContent(JsonConvert.SerializeObject(client), System.Text.Encoding.UTF8, "application/json");

        //    var response = await httpClient.PostAsync(url, content);

        //    string responseText = await response.Content.ReadAsStringAsync();

        //    if (response.IsSuccessStatusCode)
        //    {
        //        var result = JsonConvert.DeserializeObject<dynamic>(responseText);
        //        Console.WriteLine($"API call not failed. Status: {response.StatusCode}, Response: {responseText}");
        //        return (int)result.RequestID;
        //    }
        //    else
        //    {
        //        Console.WriteLine($"API call failed. Status: {response.StatusCode}, Response: {responseText}");
        //        return 0;
        //    }
        //}

        public static async Task<(int RequestId, string Message)> SendUpdateProfileDetailsRequestAsync(int clientId, string requestType, ClientDTO client)
        {
            string url = $"api/client/requests/send-request?clientId={clientId}&requestType={requestType}";
            //StringContent content = new StringContent(JsonConvert.SerializeObject(client), System.Text.Encoding.UTF8, "application/json");
            //var response = await httpClient.PostAsync(url, content);
 
            var response = await httpClient.PostAsJsonAsync(url, client);
            string responseText = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var result = JsonConvert.DeserializeObject<dynamic>(responseText);
                return ((int)result.RequestID, null);
            }
            else
            {
                return (0, responseText);
            }
        }

        public static async Task<List<AccountDTO>> GetClientAccountsAsync(int clientId, string accountType)
        {
            string url = $"api/client/{clientId}/accounts?type={accountType}";
            var response = await httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return new List<AccountDTO>();

            string json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<AccountDTO>>(json);

            //List<AccountDTO> accounts = JsonConvert.DeserializeObject<List<AccountDTO>>(response);
            //return accounts;
        }

        public static async Task<(bool IsSuccess, string Message)> DepositAmountAsync(DepositDTO dto)
        {
            string url = "api/client/transaction/deposit";
            var response = await httpClient.PostAsJsonAsync(url, dto);
            string responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                //return new ApiResponseMessage
                //{
                //    MessageType = "success",
                //    MessageContent = responseContent
                //};
                return (true, responseContent);
            }
            else
            {
                return (false, responseContent);
                //return new ApiResponseMessage
                //{
                //    MessageType = "danger",
                //    MessageContent = !string.IsNullOrEmpty(responseContent) ? responseContent : "Deposit Amount Failed."
                //};
            }

        } 
        public static async Task<(bool IsSuccess, string Message)> WithdrawAmountAsync(WithdrawDTO dto)
        {
            string url = "api/client/transaction/withdraw";
            var response = await httpClient.PostAsJsonAsync(url, dto);
            string responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
                return (true, responseContent);
            else
                return (false, responseContent);

        }
        public static async Task<(bool IsSuccess, string Message)> TransferAmountAsync(TransferDTO dto)
        {
            string url = "api/client/transaction/transfer";
            var response = await httpClient.PostAsJsonAsync(url, dto);
            string responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
                return (true, responseContent);
            else
                return (false, responseContent);

        }
        

        public static async Task<List<TransactionDTO>> GetTransactionHistoryAsync(long accountNumber)
        {
            string url = $"api/client/transaction/history?accountNumber={accountNumber}";
            var response = await httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return new List<TransactionDTO>();

            string json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<TransactionDTO>>(json);

            //List<TransactionDTO> transactions = JsonConvert.DeserializeObject<List<TransactionDTO>>(response);
            //return transactions;
        }
    }
}