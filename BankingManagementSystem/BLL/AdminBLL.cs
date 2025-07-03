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
    public static class AdminBLL
    {
        public static User ValidateAdminLogin(AuthRequestDTO admin)
        {
            return AdminDAL.CheckAdminCredentials(admin);
        }
        public static bool IsAdminExistsByAdminId(int adminId)
        {
            return AdminDAL.IsAdminExistsByAdminId(adminId);
        }

        public static List<RequestDTO> GetRequestsByStatus(string status, string sortBy, string sortDirection)
        {
            return RequestDAL.GetAllRequestsByStatus(status, sortBy, sortDirection);
        }

        public static RequestDTO GetRequestById(int requestId)
        {
            return RequestDAL.GetAllRequestById(requestId);
        }

        public static bool UpdatePayload(int requestId, ClientDTO client, out string message)
        {
            var isClientRequest = GetRequestById(requestId);
            if (isClientRequest == null) {
                message = "Invalid Request ID";
                return false;
            }
            bool result = ClientBLL.ValidateClientDetails(client, out message);
            if (result)
            {
                string updatedPayload = JsonConvert.SerializeObject(client);
                return RequestDAL.UpdateRequestPayload(requestId, updatedPayload);
            }
            return false;
        }

        public static bool UpdateStatus(int requestId, string status, int repliedBy)
        {
            var isClientRequest = RequestDAL.GetPendingRequestById(requestId);
            if (isClientRequest == null)
                return false;
            if (repliedBy == -1 || AdminDAL.IsAdminExistsByAdminId(repliedBy) || ClientDAL.IsClientExistsByClientId(repliedBy))
                return RequestDAL.UpdateRequestStatus(requestId, status, repliedBy);
            return false;
            
        }

        public static bool DeleteRequest(int requestId, string status)
        {
            return RequestDAL.DeleteRequestByStatus(requestId, status);
        }
         public static bool CreateNewClient(ClientDTO client, out string message)
        {
            return AdminDAL.CreateClient(client, out message);
        }


    }

}