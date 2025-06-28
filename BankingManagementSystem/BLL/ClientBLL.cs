using BankingManagementSystem.DAL;
using BankingManagementSystem.Models;
using BankingManagementSystem.Models.API;
using BankingManagementSystem.Models.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BankingManagementSystem.BLL
{
	public class ClientBLL
	{
        public User ValidateClientLogin(AuthRequestDTO client)
        {
            return new ClientDAL().CheckClientCredentials(client);

        }

        public bool RegisterNewClient(ClientDTO client, out string message)
        {
            message = "";
           
            // Validate DOB
            if (!ValidationServiceBLL.IsValidDOB(dobInput: client.DOB, out message))
            {
                return false;
            }

            // Validate if online account already exists 
            if (ClientDAL.IsClientExistsByPersonalDetails(aadhaar: client.AadhaarNumber, pan: client.PANNumber))
            {
                message = "Client already exists. Link your existing account."; // more validations to be added
                return false;
            }

            // Validate joint account client exists
            if (client.IsJointAccount && ClientDAL.IsClientExistsByClientId(clientId: client.JointClientId))
            {
                message = "Invalid Co-holder details.";
                return false;
            }

            // Validate username already exists
            if (ClientDAL.IsClientExistsByUsername(username: client.Username))
            {
                message = "Username already taken, try another username.";
                return false;
            }

            // Validate for Strong Password
            if (!ValidationServiceBLL.IsStrongPassword(password: client.Password, confirmPassword: client.ConfirmPassword, out message))
            {
                return false;
            }

            // If joint account then create and send request to Co-Holder and Admin
            if (client.IsJointAccount && !client.CoHolderApproved)
            {
                try
                {
                    string payloadJson = JsonConvert.SerializeObject(client);

                    int requestIdCreated = RequestDAL.SendJointAccountPendingRequests(
                        clientId: null,
                        targetClientId: client.JointClientId,
                        payload: payloadJson
                    );
                    message = $"Client registration successful. Awaiting administration and joint account co-holder's approval.\nYour Request ID is: {requestIdCreated}";
                    return true;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error in Json Payload.", ex);
                }
            }

            // If not joint account then create and send request only to Admin
            if (!client.AdminApproved)
            {
                try
                {
                    string payloadJson = JsonConvert.SerializeObject(client);

                    int requestIdCreated = RequestDAL.SendAdminPendingRequest(
                        clientId: null,
                        payload: payloadJson
                    );
                    message = $"Client registration successful. Awaiting administration approval.\nYour Request ID is: {requestIdCreated}";
                    return true;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error in Json Payload.", ex);
                }
            }

            // If admin approval is given directly or through Json
            if (client.AdminApproved)
            {
                //return AdminDAL.CreateClient();
            }
            return true;
        }


    }
}