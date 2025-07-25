﻿using BankingManagementSystem.DAL;
using BankingManagementSystem.Models.ConstraintTypes;
using BankingManagementSystem.Models.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Services.Description;

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

            if (request.RequestId == request.RepliedBy)
                return (false, "Request not found.", null);

            if (request.RequestType != RequestType.CreateNewRegistration.ToString())
                return (false, "Request is not a New Registration Request. Please login to view the request.", null);

            //if (request.Status != RequestStatus.Pending.ToString())
            //    return (false, "Request is no longer pending. Please login to view more details.", null);
            //if (request.Status == RequestStatus.Rejected.ToString())
            //    return (false, "Your New Registration Request has been rejected by the admin.", null);

            return (true, null, request);
        }

        public static async Task<(bool success, string message)> UpdateRequestAsync(int requestId, ClientDTO client)
        {
            var request = await GetRequestByIdAsync(requestId);
            if (request == null)
                return (false, "Invalid Request ID");

            if (request.Status != RequestStatus.Pending.ToString())
                return (false, "Request is no longer pending.");

            var (IsValid, Message) = ClientBLL.ValidateClientProfileDetails(client);
            if (IsValid)
            {
                string updatedPayload = JsonConvert.SerializeObject(client);
                bool IsUpdated = await RequestDAL.UpdateRequestPayloadAsync(requestId, updatedPayload);
                return (IsUpdated, IsUpdated ? "Request updated successfully." : "Failed to update request.");
            }

            return (false, Message);
        }
        public static async Task<(bool success, string message)> UpdateRegisterRequestPublicAsync(int requestId, ClientDTO client)
        {
            var request = await GetRequestByIdAsync(requestId);
            if (request == null)
                return (false, "Request not found.");

            if (request.RequestType != RequestType.CreateNewRegistration.ToString())
                return (false, "Not a registration request.");

            if (request.Status != RequestStatus.Pending.ToString())
                return (false, "Request is no longer pending.");

            return await UpdateRequestAsync(requestId, client);
        }
        public static async Task<(bool success, string message)> DeleteRegisterRequestPublicAsync(int requestId, int repliedBy)
        {
            var request = await GetRequestByIdAsync(requestId);
            if (request == null)
                return (false, "Request not found.");

            if (request.RequestType != RequestType.CreateNewRegistration.ToString())
                return (false, "Not a registration request.");

            if (request.Status != RequestStatus.Pending.ToString())
                return (false, "Request is no longer pending.");

            bool isDeleted = await UpdateStatusAsync(requestId, "Rejected", repliedBy);
            return (isDeleted, isDeleted ? "" : "Failed to delete request.");
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

        public static async Task<(int requestId, string Message)> SendRequestsByClientAsync(int? clientId, string requestType, ClientDTO client)
        {
            if (!await ClientDAL.IsClientExistsByClientIdAsync(clientId))
                return (0,"Client does not exists.");


            // Prevent Duplicate Requests
            var IsDuplicate = await RequestDAL.IsDuplicateUpdateProfileDetailsPendingRequestAsync(aadhaar: client.AadhaarNumber, pan: client.PANNumber);
            if (IsDuplicate)
            {
                string message = "A pending update profile request already exists. \nPlease wait for admin approval or delete the request before submitting a new one.";
                return (0, message);
            }

            var (IsValid, Message) = ClientBLL.ValidateClientProfileDetails(client);
            if (!IsValid)
                return (0, Message);

            string payloadJson = JsonConvert.SerializeObject(client);

            if (requestType.Equals(RequestType.UpdateProfileDetails.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return (await RequestDAL.SendAdminPendingRequestAsync(clientId, requestType, payloadJson),"");
            }
            else if (requestType.Equals(RequestType.CreateNewAccount.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                if (client.IsJointAccount)
                {
                    if (clientId == client.JointClientId)
                        return (0, "Joint client id cannot be same as of your client id.");

                    return (await RequestDAL.SendJointAccountPendingRequestsAsync(clientId, requestType, client.JointClientId, payloadJson),"");
                }

                return (await RequestDAL.SendAdminPendingRequestAsync(clientId, requestType, payloadJson),"");
            }

            return (0, "Request not sent. Invalid details.");
        }

        public static async Task<List<RequestDTO>> SendCreateJointAccountRequestsByClientAsync(int clientId, string sortColumn, string sortDirection)
        {
            return await RequestDAL.GetSentRequestsByClientAsync(clientId, sortColumn, sortDirection);
        }
    }
}
