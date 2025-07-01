using BankingManagementSystem.DAL;
using BankingManagementSystem.Models.API;
using BankingManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BankingManagementSystem.Models.DTOs;
using Newtonsoft.Json;
using System.Web.Services.Description;
using Microsoft.Owin.Security.Provider;

namespace BankingManagementSystem.BLL
{
    public class AdminBLL
    {
        public User ValidateAdminLogin(AuthRequestDTO admin)
        {
            return new AdminDAL().CheckAdminCredentials(admin);

        }
        public static List<PendingRequestDTO> GetRequestsByStatus(string status, string sortBy, string sortDirection)
        {
            return RequestDAL.GetAllRequestsByStatus(status, sortBy, sortDirection);
        }

        public static PendingRequestDTO GetRequestById(int requestId)
        {
            return RequestDAL.GetPendingRequestById(requestId);
        }

        public static bool UpdatePayload(int requestId, ClientDTO client, out string message)
        {
            var isClient = GetRequestById(requestId);
            if (isClient == null) {
                message = "Invalid Request ID";
                return false;
            }
            bool result = new ClientBLL().ValidateClientDetails(client, out string newMessage);
            message = newMessage;
            if (result)
            {
                string updatedPayload = JsonConvert.SerializeObject(client);
                return RequestDAL.UpdateRequestPayload(requestId, updatedPayload);
            }
            return false;
        }

        public static bool UpdateStatus(int requestId, string status, int repliedBy)
        {
            return RequestDAL.UpdateRequestStatus(requestId, status, repliedBy);
        }

        public static bool DeleteRequest(int requestId)
        {
            return RequestDAL.DeletePendingRequest(requestId);
        }
    }

}