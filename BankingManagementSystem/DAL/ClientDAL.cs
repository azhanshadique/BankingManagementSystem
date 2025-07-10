using BankingManagementSystem.Helpers;
using BankingManagementSystem.Models;
using BankingManagementSystem.Models.API;
using BankingManagementSystem.Models.Constants;
using BankingManagementSystem.Models.ConstraintTypes;
using BankingManagementSystem.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace BankingManagementSystem.DAL
{
    public static class ClientDAL
    {

        public static async Task<User> CheckClientCredentialsAsync(AuthRequestDTO client)
        {
            try
            {
                using (SqlConnection con = DBConnectionManager.GetConnection())
                using (SqlCommand cmd = new SqlCommand("sp_ValidateClient", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Username", client.Username);
                    cmd.Parameters.AddWithValue("@Password", client.Password);

                    await con.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            int idxClientId = reader.GetOrdinal(DbColumns.ClientId);
                            int idxFullName = reader.GetOrdinal(DbColumns.FullName);
                            int idxUsername = reader.GetOrdinal(DbColumns.Username);

                            return new User
                            {
                                UserID = reader.GetInt32(idxClientId),
                                FullName = reader.IsDBNull(idxFullName) ? null : reader.GetString(idxFullName),
                                Username = reader.IsDBNull(idxUsername) ? null : reader.GetString(idxUsername),
                                Role = UserRoles.CLIENT
                            };
                        }
                    }
                    return null;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Database error during client login credentials validation.", ex);
            }
        }

        public static async Task<int> IsClientExistsByPersonalDetailsAsync(string aadhaar, string pan)
        {
            try
            {
                using (SqlConnection con = DBConnectionManager.GetConnection())
                using (SqlCommand cmd = new SqlCommand("sp_CheckClientExistsByPersonalDetails", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@AadhaarNumber", aadhaar);
                    cmd.Parameters.AddWithValue("@PANNumber", pan);

                    await con.OpenAsync();
                    object result = await cmd.ExecuteScalarAsync();
                    return Convert.ToInt32(result);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Database error during client exists by Aadhaar or PAN validation.", ex);
            }
        }

        public static async Task<bool> IsClientExistsByClientIdAsync(int? clientId)
        {
            try
            {
                using (SqlConnection con = DBConnectionManager.GetConnection())
                using (SqlCommand cmd = new SqlCommand("sp_CheckClientExistsByClientId", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ClientId", clientId);

                    await con.OpenAsync();
                    object result = await cmd.ExecuteScalarAsync();
                    return Convert.ToInt32(result) > 0;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Database error during client exists by client-id validation.", ex);
            }
        }

        public static async Task<bool> IsClientExistsByUsernameAsync(string username)
        {
            try
            {
                using (SqlConnection con = DBConnectionManager.GetConnection())
                using (SqlCommand cmd = new SqlCommand("sp_CheckClientExistsByUsername", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Username", username);

                    await con.OpenAsync();
                    object result = await cmd.ExecuteScalarAsync();
                    return Convert.ToInt32(result) > 0;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Database error during client exists by username validation.", ex);
            }
        }

        public static async Task<ClientDTO> GetClientByClientIdAsync(int clientId)
        {
            try
            {
                using (SqlConnection con = DBConnectionManager.GetConnection())
                using (SqlCommand cmd = new SqlCommand("sp_GetClientByClientId", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ClientId", clientId);

                    await con.OpenAsync();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            int idxClientId = reader.GetOrdinal(DbColumns.ClientId);
                            int idxFullName = reader.GetOrdinal(DbColumns.FullName);
                            int idxParentName = reader.GetOrdinal(DbColumns.ParentName);
                            int idxDOB = reader.GetOrdinal(DbColumns.DOB);
                            int idxGender = reader.GetOrdinal(DbColumns.Gender);
                            int idxNationality = reader.GetOrdinal(DbColumns.Nationality);
                            int idxOccupation = reader.GetOrdinal(DbColumns.Occupation);
                            int idxAadhaar = reader.GetOrdinal(DbColumns.AadhaarNumber);
                            int idxPAN = reader.GetOrdinal(DbColumns.PANNumber);

                            int idxMobile = reader.GetOrdinal(DbColumns.MobileNumber);
                            int idxEmail = reader.GetOrdinal(DbColumns.EmailId);
                            int idxAddress = reader.GetOrdinal(DbColumns.Address);
                            int idxCity = reader.GetOrdinal(DbColumns.City);
                            int idxState = reader.GetOrdinal(DbColumns.State);
                            int idxPincode = reader.GetOrdinal(DbColumns.Pincode);
 
                            int idxUsername = reader.GetOrdinal(DbColumns.Username);


                            return new ClientDTO
                            {
                                ClientId = reader.GetInt32(idxClientId),
                                FullName = reader.IsDBNull(idxFullName) ? null : reader.GetString(idxFullName),
                                ParentName = reader.IsDBNull(idxParentName) ? null : reader.GetString(idxParentName),
                                DOB = reader.IsDBNull(idxDOB) ? DateTime.MinValue : reader.GetDateTime(idxDOB),
                                Gender = reader.IsDBNull(idxGender) ? null : reader.GetString(idxGender),
                                Nationality = reader.IsDBNull(idxNationality) ? null : reader.GetString(idxNationality),
                                Occupation = reader.IsDBNull(idxOccupation) ? null : reader.GetString(idxOccupation),
                                AadhaarNumber = reader.IsDBNull(idxAadhaar) ? null : reader.GetString(idxAadhaar),
                                PANNumber = reader.IsDBNull(idxPAN) ? null : reader.GetString(idxPAN),

                                MobileNumber = reader.IsDBNull(idxMobile) ? null : reader.GetString(idxMobile),
                                EmailId = reader.IsDBNull(idxEmail) ? null : reader.GetString(idxEmail),
                                Address = reader.IsDBNull(idxAddress) ? null : reader.GetString(idxAddress),
                                City = reader.IsDBNull(idxCity) ? null : reader.GetString(idxCity),
                                State = reader.IsDBNull(idxState) ? null : reader.GetString(idxState),
                                Pincode = reader.IsDBNull(idxPincode) ? null : reader.GetString(idxPincode),

                                Username = reader.IsDBNull(idxUsername) ? null : reader.GetString(idxUsername)
                            };
                        }
                    }
                    return null;

                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Database error during get client by Client Id.", ex);
            }
        }

        public static async Task<List<AccountDTO>> GetAccountsByClientIdAsync(int clientId, string type)
        {
            var accounts = new List<AccountDTO>();

            using (SqlConnection con = DBConnectionManager.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_GetClientAccountsByType", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ClientId", clientId);
                cmd.Parameters.AddWithValue("@Type", type);

                await con.OpenAsync();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    int idxAccountNo = reader.GetOrdinal(DbColumns.AccountNumber);
                    int idxAccountType = reader.GetOrdinal(DbColumns.AccountType);
                    int idxBalance = reader.GetOrdinal(DbColumns.Balance);
                    int idxStatus = reader.GetOrdinal(DbColumns.AccountStatus);
                    int idxIsPrimary = reader.GetOrdinal(DbColumns.IsPrimary);
                    int idxCoHolderClientId = reader.GetOrdinal(DbColumns.CoHolderClientId);
                    int idxCoHolderName = reader.GetOrdinal(DbColumns.CoHolderName);

                    while (await reader.ReadAsync())
                    {
                        accounts.Add(new AccountDTO
                        {
                            AccountNumber = reader.IsDBNull(idxAccountNo) ? 0 : reader.GetInt64(idxAccountNo),
                            AccountType = reader.IsDBNull(idxAccountType) ? null : reader.GetString(idxAccountType),
                            Balance = reader.IsDBNull(idxBalance) ? 0 : reader.GetDecimal(idxBalance),
                            Status = reader.IsDBNull(idxStatus) ? null : reader.GetString(idxStatus),
                            IsPrimary = !reader.IsDBNull(idxIsPrimary) && reader.GetString(idxIsPrimary) == "Yes",
                            CoHolderClientId = reader.IsDBNull(idxCoHolderClientId) ? 0 : reader.GetInt32(idxCoHolderClientId),
                            CoHolderName = reader.IsDBNull(idxCoHolderName) ? null : reader.GetString(idxCoHolderName),
                            IsJoint = !reader.IsDBNull(idxCoHolderClientId),
                        });
                    }
                }
            }

            return accounts;
        }

        public static async Task<bool> CheckClientPassword(int clientId, string password)
        {
            try
            {
                using (SqlConnection con = DBConnectionManager.GetConnection())
                using (SqlCommand cmd = new SqlCommand("sp_CheckClientPassword", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ClientId", clientId);
                    cmd.Parameters.AddWithValue("@Password", password);

                    await con.OpenAsync();
                    object result = await cmd.ExecuteScalarAsync();
                    return Convert.ToInt32(result) > 0;
                }          
            }
            catch (SqlException ex)
            {
                throw new Exception("Database error during check client password.", ex);
            }
        }
    }
}
