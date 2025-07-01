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
            if (!ValidateClientDetails(client, out message))
            {
                return false;
            }
           

            // If admin approval is given directly or through Json
            if (client.AdminApproved)
            {
                return AdminDAL.CreateClient(client, out message);
            }

            // Prevent Duplicate Requests
            if (RequestDAL.IsDuplicateNewRegistrationPendingRequest(aadhaar: client.AadhaarNumber, pan: client.PANNumber))
            {
                message = "A pending registration request already exists. \\nPlease wait for admin approval or delete the request before submitting a new one.";
                return false;
            }

            string newRequestType = "CreateNewRegistration";

            // If joint account then create and send request to Co-Holder and Admin
            if (client.IsJointAccount && !client.CoHolderApproved)
            {
                try
                {
                    string payloadJson = JsonConvert.SerializeObject(client);
                    
                    int requestIdCreated = RequestDAL.SendJointAccountPendingRequests(
                        clientId: null,
                        requestType: newRequestType,
                        targetClientId: client.JointClientId,
                        payload: payloadJson
                    );
                    message = $"Client registration successful. Awaiting administration and joint account co-holder approval. \nYour Request ID is: #{requestIdCreated}";
                    return true;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error in Json Payload.", ex);
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
                        requestType: newRequestType,
                        payload: payloadJson
                    );
                    message = $"Client registration successful. Awaiting administration approval. \nYour Request ID is: #{requestIdCreated}";
                    return true;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error in Json Payload.", ex);
                }
            }

            
            return true;
        }

        public bool ValidateClientDetails(ClientDTO client, out string message)
        {
            // Validate DOB
            if (!ValidationServiceBLL.IsValidDOB(dobInput: client.DOB, out message))
            {
                return false;
            }

            // Validate if online account already exists 
            int accountType = ClientDAL.IsClientExistsByPersonalDetails(aadhaar: client.AadhaarNumber, pan: client.PANNumber);

            switch (accountType)
            {
                case -1:
                    message = "Mismatch found: Aadhaar and PAN belong to different clients. Please enter valid details.";
                    return false;
                case 1:
                    message = "Client online account already exists. Log in with your username and password.";
                    return false;
                case 2:
                    message = "Client offline account already exists. Link your existing account.";
                    return false;
                case 0:
                    message = ""; // continue registration
                    break;
            }


            // Validate joint account client exists
            if (client.IsJointAccount && !ClientDAL.IsClientExistsByClientId(clientId: client.JointClientId))
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

            return true;
        }
    }
}