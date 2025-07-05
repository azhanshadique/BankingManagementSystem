using BankingManagementSystem.DAL;
using BankingManagementSystem.Models.ConstraintTypes;
using BankingManagementSystem.Models.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankingManagementSystem.BLL
{
    public static class RequestBLL
    {
        public static async Task<List<RequestDTO>> GetRequestsByStatusAsync(string status, string sortBy, string sortDirection)
        {
            return await RequestDAL.GetAllRequestsByStatusAsync(status, sortBy, sortDirection);
        }

        public static async Task<RequestDTO> GetRequestByIdAsync(int requestId)
        {
            return await RequestDAL.GetAllRequestByIdAsync(requestId);
        }
        //public static async Task<RequestDTO> GetValidCreateRegistrationRequestByIdAsync(int id)
        //{
        //    var request = await RequestDAL.GetAllRequestByIdAsync(id);

        //    if (request == null || request.RequestType != RequestType.CreateNewRegistration.ToString())
        //        return null;

        //    return request;
        //}
        public static async Task<(bool IsValid, string Message, RequestDTO Request)> GetPublicRequestByIdAsync(int id)
        {
            var request = await GetRequestByIdAsync(id);

            if (request == null)
                return (false, "Request not found.", null);

            if (request.RequestType != RequestType.CreateNewRegistration.ToString())
                return (false, "Request is not of type 'Create New Registration'. Please login to view the request.", null);

            if (request.Status != RequestStatus.Pending.ToString())
                return (false, "Request is no longer pending. Please login to view more details.", null);

            return (true, null, request);
        }

        public static async Task<(bool success, string message)> UpdateRequestAsync(int requestId, ClientDTO client)
        {
            var IsClientRequest = await GetRequestByIdAsync(requestId);
            if (IsClientRequest == null)
            {
                return (false, "Invalid Request ID");
            }

            var (IsValid, Message) = await ClientBLL.ValidateClientDetailsAsync(client);
            if (IsValid)
            {
                string updatedPayload = JsonConvert.SerializeObject(client);
                bool IsUpdated = await RequestDAL.UpdateRequestPayloadAsync(requestId, updatedPayload);
                return (IsUpdated, IsUpdated ? "Payload updated successfully." : "Failed to update payload.");
            }

            return (false, Message);
        }

        public static async Task<bool> UpdateStatusAsync(int requestId, string status, int repliedBy)
        {
            var IsClientRequest = await RequestDAL.GetPendingRequestByIdAsync(requestId);
            if (IsClientRequest == null)
                return false;

            if (repliedBy == -1 || await AdminDAL.IsAdminExistsByAdminIdAsync(repliedBy) || await ClientDAL.IsClientExistsByClientIdAsync(repliedBy) || repliedBy == requestId)
                return await RequestDAL.UpdateRequestStatusAsync(requestId, status, repliedBy);

            return false;
        }

        public static async Task<bool> DeleteRequestAsync(int requestId, string status)
        {
            return await RequestDAL.DeleteRequestByStatusAsync(requestId, status);
        }

        public static async Task<List<RequestDTO>> GetReceivedRequestsForClientAsync(int clientId, string sortBy, string sortDirection)
        {
            return await RequestDAL.GetReceivedRequestsForClientAsync(clientId, sortBy, sortDirection);
        }

        public static async Task<List<RequestDTO>> GetSentRequestsByClientAsync(int clientId, string sortColumn, string sortDirection)
        {
            return await RequestDAL.GetSentRequestsByClientAsync(clientId, sortColumn, sortDirection);
        }

        public static async Task<int> SendRequestsByClientAsync(int? clientId, string requestType, ClientDTO client)
        {
            if (!await ClientDAL.IsClientExistsByClientIdAsync(clientId))
                return 0;

            string payloadJson = JsonConvert.SerializeObject(client);

            if (requestType.Equals(RequestType.UpdateDetails.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return await RequestDAL.SendAdminPendingRequestAsync(clientId, requestType, payloadJson);
            }
            else if (requestType.Equals(RequestType.CreateNewAccount.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                if (client.IsJointAccount)
                {
                    if (clientId == client.JointClientId)
                        return 0;

                    return await RequestDAL.SendJointAccountPendingRequestsAsync(clientId, requestType, client.JointClientId, payloadJson);
                }

                return await RequestDAL.SendAdminPendingRequestAsync(clientId, requestType, payloadJson);
            }

            return 0;
        }

        public static async Task<List<RequestDTO>> SendCreateJointAccountRequestsByClientAsync(int clientId, string sortColumn, string sortDirection)
        {
            return await RequestDAL.GetSentRequestsByClientAsync(clientId, sortColumn, sortDirection);
        }
    }
}
