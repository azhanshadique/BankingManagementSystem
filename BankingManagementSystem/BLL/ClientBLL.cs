using BankingManagementSystem.DAL;
using BankingManagementSystem.Helpers;
using BankingManagementSystem.Models;
using BankingManagementSystem.Models.API;
using BankingManagementSystem.Models.ConstraintTypes;
using BankingManagementSystem.Models.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Services.Description;

namespace BankingManagementSystem.BLL
{
    public static class ClientBLL
    {
        public static async Task<User> ValidateClientLoginAsync(AuthRequestDTO client)
        {
            return await ClientDAL.CheckClientCredentialsAsync(client);
        }
        public static async Task<(bool IsSuccess, string Message)> LinkClientAccount(LinkAccountDTO clientDto)
        {
            decimal currentBalance = await TransactionDAL.GetBalanceAsync(clientDto.AccountNumber);
            if (currentBalance < 0)
                return (false, "Invalid account.");
            // check other validations 

            // Client validation
            if (!await ClientDAL.IsClientExistsByClientIdAsync(clientDto.ClientId))
                return (false, "Invalid Client Id.");

            // Joint account validation
            if (clientDto.IsJointAccount && !await ClientDAL.IsClientExistsByClientIdAsync(clientDto.JointClientId))
                return (false, "Invalid Co-holder details.");

            string message = "";
            // Username validation
            if (!ValidationServiceBLL.IsValidUsername(clientDto.Username, out message)) return (false, message);
            if (await ClientDAL.IsClientExistsByUsernameAsync(clientDto.Username))
                return (false, "Username already taken, try another username.");

            // Password validation
            if (!ValidationServiceBLL.IsStrongPassword(clientDto.Password, clientDto.ConfirmPassword, out message))
                return (false, message);

            // Secure the password
            string hashedPassword = PasswordHasher.HashPassword(clientDto.Password);
            clientDto.Password = hashedPassword;
            clientDto.ConfirmPassword = hashedPassword;

            return await ClientDAL.LinkClientExistingAccount(clientDto);
        }
        public static async Task<(bool IsSuccess, string Message)> RegisterNewClient(ClientDTO client)
        {
            string message = "";

            var (isValid, validationMessage) = await ValidateClientDetailsAsync(client);
            if (!isValid)
                return (false, validationMessage);

            // Secure the password
            string hashedPassword = PasswordHasher.HashPassword(client.Password);
            client.Password = hashedPassword;
            client.ConfirmPassword = hashedPassword;



            // Prevent duplicate pending registration requests
            var isDuplicate = await RequestDAL.IsDuplicateNewRegistrationPendingRequestAsync(client.AadhaarNumber, client.PANNumber);
            if (isDuplicate)
            {
                message = "A pending registration request already exists. \nPlease wait for admin approval or delete the request before submitting a new one.";
                return (false, message);
            }

            // If admin directly approved registration
            bool adminApproved = client.AdminApproved == RequestStatus.Approved.ToString();
            if (adminApproved)
            {
                var (isSuccess, msg) = await AdminDAL.CreateClientAsync(client);
                return (isSuccess, msg);
            }

            string newRequestType = RequestType.CreateNewRegistration.ToString();

            // If it's a joint account and co-holder hasn't approved yet
            bool coApproved = client.CoHolderApproved == RequestStatus.Approved.ToString();
            if (client.IsJointAccount && !coApproved)
            {
                try
                {
                    client.Username = client.Username.ToLower();
                    string payloadJson = JsonConvert.SerializeObject(client);

                    int requestIdCreated = await RequestDAL.SendJointAccountPendingRequestsAsync(
                        clientId: null,
                        requestType: newRequestType,
                        targetClientId: client.JointClientId,
                        payload: payloadJson
                    );

                    message = $"Client registration successful. Awaiting administration and joint account co-holder approval. Your Request ID is: #{requestIdCreated}";
                    return (true, message);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error in Json Payload.", ex);
                }
            }

            // If not joint account, send request only to admin
            if (!adminApproved)
            {
                try
                {
                    client.Username = client.Username.ToLower();
                    string payloadJson = JsonConvert.SerializeObject(client);

                    int requestIdCreated = await RequestDAL.SendAdminPendingRequestAsync(
                        clientId: null,
                        requestType: newRequestType,
                        payload: payloadJson
                    );

                    message = $"Client registration successful. Awaiting administration approval. Your Request ID is: #{requestIdCreated}";
                    return (true, message);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error in Json Payload.", ex);
                }
            }

            return (true, message);
        }

        public static async Task<(bool IsValid, string Message)> ValidateClientDetailsAsync(ClientDTO client)
        {
            string message = "";

            // Basic validations
            if (!ValidationServiceBLL.IsValidDOB(client.DOB, out message)) return (false, message);
            if (!ValidationServiceBLL.IsValidGender(client.Gender, out message)) return (false, message);
            if (!ValidationServiceBLL.IsValidAadhaar(client.AadhaarNumber, out message)) return (false, message);
            if (!ValidationServiceBLL.IsValidPAN(client.PANNumber, out message)) return (false, message);
            if (!ValidationServiceBLL.IsValidMobileNumber(client.MobileNumber, out message)) return (false, message);
            if (!ValidationServiceBLL.IsValidPincode(client.Pincode, out message)) return (false, message);

            // Check if client already exists
            int accountType = await ClientDAL.IsClientExistsByPersonalDetailsAsync(client.AadhaarNumber, client.PANNumber);
            switch (accountType)
            {
                case -1: return (false, "Mismatch found: Aadhaar and PAN belong to different clients. Please enter valid details.");
                case 1: return (false, "Client online account already exists. Log in with your username and password.");
                case 2: return (false, "Client offline account already exists. Link your existing account.");
                case 0: break; // Continue registration
            }

            // Joint account validation
            if (client.IsJointAccount && !await ClientDAL.IsClientExistsByClientIdAsync(client.JointClientId))
                return (false, "Invalid Co-holder details.");

            // Username validation
            if (!ValidationServiceBLL.IsValidUsername(client.Username, out message)) return (false, message);
            if (await ClientDAL.IsClientExistsByUsernameAsync(client.Username))
                return (false, "Username already taken, try another username.");

            // Password validation
            if (!ValidationServiceBLL.IsStrongPassword(client.Password, client.ConfirmPassword, out message))
                return (false, message);

            return (true, message);
        }

        public static (bool IsValid, string Message) ValidateClientProfileDetails(ClientDTO client)
        {
            string message = "";

            // Basic validations for profile
            if (!ValidationServiceBLL.IsValidDOB(client.DOB, out message)) return (false, message);
            if (!ValidationServiceBLL.IsValidGender(client.Gender, out message)) return (false, message);
            if (!ValidationServiceBLL.IsValidAadhaar(client.AadhaarNumber, out message)) return (false, message);
            if (!ValidationServiceBLL.IsValidPAN(client.PANNumber, out message)) return (false, message);
            if (!ValidationServiceBLL.IsValidMobileNumber(client.MobileNumber, out message)) return (false, message);
            if (!ValidationServiceBLL.IsValidPincode(client.Pincode, out message)) return (false, message);
            if (!ValidationServiceBLL.IsValidUsername(client.Username, out message)) return (false, message);

            return (true, message);
        }

        public static async Task<ClientDTO> GetClientByIdAsync(int clientId)
        {
            return await ClientDAL.GetClientByClientIdAsync(clientId);
        }

        public static async Task<List<AccountDTO>> GetAccountsByClientIdAndTypeAsync(int clientId, string type)
        {
            return await ClientDAL.GetAccountsByClientIdAsync(clientId, type);
        }
    }
}
