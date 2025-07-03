using BankingManagementSystem.DAL;
using BankingManagementSystem.Models.ConstraintTypes;
using BankingManagementSystem.Models.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BankingManagementSystem.BLL
{
	public static class RequestBLL
	{
        public async static Task<List<RequestDTO>> GetReceivedRequestsForClient(int clientId, string sortBy, string sortDirection)
        {
            return await RequestDAL.GetReceivedRequestsForClient(clientId, sortBy, sortDirection);
        }
        public static async Task<List<RequestDTO>> GetSentRequestsByClient(int clientId, string sortColumn, string sortDirection)
        {
            return await RequestDAL.GetSentRequestsByClient(clientId, sortColumn, sortDirection);
        }
         public static async Task<int> SendRequestsByClient(int? clientId, string requestType, ClientDTO client)
        {
            if (!ClientDAL.IsClientExistsByClientId(clientId))
                return 0;

            string payloadJson = JsonConvert.SerializeObject(client);

            if (requestType.ToLower() == RequestType.UpdateDetails.ToString().ToLower())
                return await RequestDAL.SendAdminPendingRequest(clientId, requestType, payloadJson);
            else if (requestType.ToLower() == RequestType.CreateNewAccount.ToString().ToLower()) 
            {
                if (client.IsJointAccount)
                {
                    if(clientId == client.JointClientId)
                    {
                        return 0;
                    }
                    return await RequestDAL.SendJointAccountPendingRequests(clientId, requestType, client.JointClientId, payloadJson);

                }
                else
                    return await RequestDAL.SendAdminPendingRequest(clientId, requestType, payloadJson);
            }
            return 0;

        }
        public static async Task<List<RequestDTO>> SendCreateJointAccountRequestsByClient(int clientId, string sortColumn, string sortDirection)
        {
            return await RequestDAL.GetSentRequestsByClient(clientId, sortColumn, sortDirection);
        }

    }
}