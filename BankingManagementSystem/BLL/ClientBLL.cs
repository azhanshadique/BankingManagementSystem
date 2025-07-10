using BankingManagementSystem.DAL;
using BankingManagementSystem.Models;
using BankingManagementSystem.Models.API;
using BankingManagementSystem.Models.ConstraintTypes;
using BankingManagementSystem.Models.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankingManagementSystem.BLL
{
    public static class ClientBLL
    {
        public static async Task<User> ValidateClientLoginAsync(AuthRequestDTO client)
        {
            return await ClientDAL.CheckClientCredentialsAsync(client);
        }

        public static async Task<(bool IsSuccess, string Message)> RegisterNewClient(ClientDTO client)
        {
            string message = "";

            var (isValid, validationMessage) = await ValidateClientDetailsAsync(client);
            if (!isValid)
                return (false, validationMessage);


            // If admin approval is given directly or through Json
            bool adminApproved = client.AdminApproved == RequestStatus.Approved.ToString();
            if (adminApproved)
            {
                var (isSuccess, msg) = await AdminDAL.CreateClientAsync(client);
                return (isSuccess, msg);
            }

            // Prevent Duplicate Requests
            var IsDuplicate = await RequestDAL.IsDuplicateNewRegistrationPendingRequestAsync(aadhaar: client.AadhaarNumber, pan: client.PANNumber);
            if (IsDuplicate)
            {
                message = "A pending registration request already exists. \nPlease wait for admin approval or delete the request before submitting a new one.";
                return (false, message);
            }

            string newRequestType = RequestType.CreateNewRegistration.ToString();

            // If joint account then create and send request to Co-Holder and Admin
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
                    message = $"Client registration successful. Awaiting administration and joint account co-holder approval. \nYour Request ID is: #{requestIdCreated}";
                    return (true, message);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error in Json Payload.", ex);
                }
            }

            // If not joint account then create and send request only to Admin
            if (!adminApproved)
            {
                try
                {
                    client.Username = client.Username.ToLower();
                    //client.JointClientId = client.JointClientId ? 0 : client.JointClientId;
                    string payloadJson = JsonConvert.SerializeObject(client);

                    int requestIdCreated = await RequestDAL.SendAdminPendingRequestAsync(
                        clientId: null,
                        requestType: newRequestType,
                        payload: payloadJson
                    );
                    message = $"Client registration successful. Awaiting administration approval. \nYour Request ID is: #{requestIdCreated}";
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

            // Validate DOB
            if (!ValidationServiceBLL.IsValidDOB(dob: client.DOB, out message))
            {
                return (false, message);
            }

            // Validate Gender
            if (!ValidationServiceBLL.IsValidGender(client.Gender, out message))
            {
                return (false, message);
            }

            // Validate aadhaar number 
            if (!ValidationServiceBLL.IsValidAadhaar(client.AadhaarNumber, out message))
            {
                return (false, message);
            }

            // Validate pan number
            if (!ValidationServiceBLL.IsValidPAN(client.PANNumber, out message))
            {
                return (false, message);
            }

            // Validate mobile number
            if (!ValidationServiceBLL.IsValidMobileNumber(client.MobileNumber, out message))
            {
                return (false, message);
            }

            // Validate pincode
            if (!ValidationServiceBLL.IsValidPincode(client.Pincode, out message))
            {
                return (false, message);
            }

            // Validate if online account already exists 
            int accountType = await ClientDAL.IsClientExistsByPersonalDetailsAsync(client.AadhaarNumber, client.PANNumber);

            switch (accountType)
            {
                case -1:
                    message = "Mismatch found: Aadhaar and PAN belong to different clients. Please enter valid details.";
                    return (false, message);
                case 1:
                    message = "Client online account already exists. Log in with your username and password.";
                    return (false, message);
                case 2:
                    message = "Client offline account already exists. Link your existing account.";
                    return (false, message);
                case 0:
                    message = ""; // continue registration
                    break;
            }



            // Validate joint account client exists
            if (client.IsJointAccount && !await ClientDAL.IsClientExistsByClientIdAsync(clientId: client.JointClientId))
            {
                message = "Invalid Co-holder details.";
                return (false, message);
            }

            // Validate username already exists
            if (!ValidationServiceBLL.IsValidUsername(client.Username, out message))
            {
                return (false, message);
            }
            if (await ClientDAL.IsClientExistsByUsernameAsync(client.Username))
            {
                message = "Username already taken, try another username.";
                return (false, message);
            }


            // Validate for Strong Password
            if (!ValidationServiceBLL.IsStrongPassword(client.Password, client.ConfirmPassword, out message))
            {
                return (false, message);
            }

            return (true, message);
        }

        public static (bool IsValid, string Message) ValidateClientProfileDetails(ClientDTO client)
        {
            string message = "";

            // Validate DOB
            if (!ValidationServiceBLL.IsValidDOB(dob: client.DOB, out message))
            {
                return (false, message);
            }

            // Validate Gender
            if (!ValidationServiceBLL.IsValidGender(client.Gender, out message))
            {
                return (false, message);
            }

            // Validate aadhaar number 
            if (!ValidationServiceBLL.IsValidAadhaar(client.AadhaarNumber, out message))
            {
                return (false, message);
            }

            // Validate pan number
            if (!ValidationServiceBLL.IsValidPAN(client.PANNumber, out message))
            {
                return (false, message);
            }

            // Validate mobile number
            if (!ValidationServiceBLL.IsValidMobileNumber(client.MobileNumber, out message))
            {
                return (false, message);
            }

            // Validate pincode
            if (!ValidationServiceBLL.IsValidPincode(client.Pincode, out message))
            {
                return (false, message);
            }



            // Validate username 
            if (!ValidationServiceBLL.IsValidUsername(client.Username, out message))
            {
                return (false, message);
            }


            return (true, message);
        }

        //public static async Task<(bool IsValid, string Message)> ValidateClientDetailsAsync(ClientDTO client)
        //{
        //    string message = "";

        //    // Validate Client Personal Details
        //    var (isValid, validationMessage) = await ValidateClientPersonalDetailsAsync(client);
        //    if (!isValid)
        //        return (false, validationMessage);


        //    // Validate if online account already exists 
        //    int accountType = await ClientDAL.IsClientExistsByPersonalDetailsAsync(client.AadhaarNumber, client.PANNumber);

        //    switch (accountType)
        //    {
        //        case -1:
        //            message = "Mismatch found: Aadhaar and PAN belong to different clients. Please enter valid details.";
        //            return (false, message);
        //        case 1:
        //            message = "Client online account already exists. Log in with your username and password.";
        //            return (false, message);
        //        case 2:
        //            message = "Client offline account already exists. Link your existing account.";
        //            return (false, message);
        //        case 0:
        //            message = ""; // continue registration
        //            break;
        //    }



        //    // Validate joint account client exists
        //    if (client.IsJointAccount && !await ClientDAL.IsClientExistsByClientIdAsync(clientId: client.JointClientId))
        //    {
        //        message = "Invalid Co-holder details.";
        //        return (false, message);
        //    }

        //    // Validate username already exists
        //    if (!ValidationServiceBLL.IsValidUsername(client.Username, out message))
        //    {
        //        return (false, message);
        //    }
        //    if (await ClientDAL.IsClientExistsByUsernameAsync(client.Username))
        //    {
        //        message = "Username already taken, try another username.";
        //        return (false, message);
        //    }

        //    // Validate for Strong Password
        //    if (!ValidationServiceBLL.IsStrongPassword(client.Password, client.ConfirmPassword, out message))
        //    {
        //        return (false, message);
        //    }

        //    return (true, message);
        //}


        //public static async Task<(bool IsValid, string Message)> ValidateClientPersonalDetailsAsync(ClientDTO client)
        //{
        //    string message = "";

        //    // Validate DOB
        //    if (!ValidationServiceBLL.IsValidDOB(dob: client.DOB, out message))
        //    {
        //        return (false, message);
        //    }

        //    // Validate Gender
        //    if (!ValidationServiceBLL.IsValidGender(client.Gender, out message))
        //    {
        //        return (false, message);
        //    }

        //    // Validate aadhaar number 
        //    if (!ValidationServiceBLL.IsValidAadhaar(client.AadhaarNumber, out message))
        //    {
        //        return (false, message);
        //    }

        //    // Validate pan number
        //    if (!ValidationServiceBLL.IsValidPAN(client.PANNumber, out message))
        //    {
        //        return (false, message);
        //    }

        //    // Validate mobile number
        //    if (!ValidationServiceBLL.IsValidMobileNumber(client.MobileNumber, out message))
        //    {
        //        return (false, message);
        //    }

        //    // Validate pincode
        //    if (!ValidationServiceBLL.IsValidPincode(client.Pincode, out message))
        //    {
        //        return (false, message);
        //    }


        //    return (true, message);
        //}



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