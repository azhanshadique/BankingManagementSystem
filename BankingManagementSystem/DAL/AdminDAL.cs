using BankingManagementSystem.Models.API;
using BankingManagementSystem.Models.ConstraintTypes;
using BankingManagementSystem.Models;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using BankingManagementSystem.Models.Constants;
using BankingManagementSystem.Models.DTOs;
using BankingManagementSystem.Helpers;

namespace BankingManagementSystem.DAL
{
    public static class AdminDAL
    {
        public static async Task<User> CheckAdminCredentialsAsync(AuthRequestDTO admin)
        {
            try
            {
                using (SqlConnection con = DBConnectionManager.GetConnection())
                using (SqlCommand cmd = new SqlCommand("sp_ValidateAdminPassword", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Username", admin.Username);

                    await con.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {

                            int idxPassword = reader.GetOrdinal(DbColumns.Password);

                            string storedHash = reader.IsDBNull(idxPassword) ? null : reader.GetString(idxPassword);

                            bool isValid = PasswordHasher.VerifyPassword(admin.Password, storedHash);

                            if (!isValid) return null;

                            int idxAdminId = reader.GetOrdinal(DbColumns.AdminId);
                            int idxFullName = reader.GetOrdinal(DbColumns.FullName);
                            int idxUsername = reader.GetOrdinal(DbColumns.Username);

                            return new User
                            {

                                UserID = reader.GetInt32(idxAdminId),
                                FullName = reader.IsDBNull(idxFullName) ? null : reader.GetString(idxFullName),
                                Username = reader.IsDBNull(idxUsername) ? null : reader.GetString(idxUsername),
                                Role = UserRoles.ADMIN
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

        //public static async Task<User> CheckAdminCredentialsAsync2(AuthRequestDTO admin)
        //{
        //    try
        //    {
        //        using (SqlConnection con = DBConnectionManager.GetConnection())
        //        using (SqlCommand cmd = new SqlCommand("sp_ValidateAdmin", con))
        //        {
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.AddWithValue("@Username", admin.Username);
        //            cmd.Parameters.AddWithValue("@Password", admin.Password);

        //            await con.OpenAsync();
        //            using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
        //            {
        //                if (await reader.ReadAsync())
        //                {
        //                    int idxAdminId = reader.GetOrdinal(DbColumns.AdminId);
        //                    int idxFullName = reader.GetOrdinal(DbColumns.FullName);
        //                    int idxUsername = reader.GetOrdinal(DbColumns.Username);

        //                    int adminId = reader.GetInt32(idxAdminId);
        //                    string fullName = reader.IsDBNull(idxFullName) ? null : reader.GetString(idxFullName);
        //                    string username = reader.IsDBNull(idxUsername) ? null : reader.GetString(idxUsername);

        //                    return new User
        //                    {
        //                        UserID = adminId,
        //                        Username = username,
        //                        Role = UserRoles.ADMIN,
        //                        FullName = fullName
        //                    };
        //                }
        //            }
        //            return null;
        //        }
        //    }
        //    catch (SqlException ex)
        //    {
        //        throw new Exception("Database error during admin validation.", ex);
        //    }
        //}

        public static async Task<bool> IsAdminExistsByAdminIdAsync(int adminId)
        {
            try
            {
                using (SqlConnection con = DBConnectionManager.GetConnection())
                using (SqlCommand cmd = new SqlCommand("sp_CheckAdminExistsByAdminId", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@AdminId", adminId);

                    await con.OpenAsync();
                    object result = await cmd.ExecuteScalarAsync();

                    return Convert.ToInt32(result) > 0;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Database error during Check Admin Exists By AdminId.", ex);
            }
        }
        public static async Task<(bool IsSuccess, string Message)> CreateOfflineClientAsync(ClientDTO client)
        {
            try
            {
                using (SqlConnection con = DBConnectionManager.GetConnection())
                using (SqlCommand cmd = new SqlCommand("sp_CreateClient", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@FullName", client.FullName);
                    cmd.Parameters.AddWithValue("@ParentName", client.ParentName);
                    cmd.Parameters.AddWithValue("@DOB", client.DOB);
                    cmd.Parameters.AddWithValue("@Gender", client.Gender);
                    cmd.Parameters.AddWithValue("@Nationality", client.Nationality);
                    cmd.Parameters.AddWithValue("@Occupation", client.Occupation);
                    cmd.Parameters.AddWithValue("@AadhaarNumber", client.AadhaarNumber);
                    cmd.Parameters.AddWithValue("@PANNumber", client.PANNumber);
                    cmd.Parameters.AddWithValue("@MobileNumber", client.MobileNumber);
                    cmd.Parameters.AddWithValue("@EmailId", client.EmailId);
                    cmd.Parameters.AddWithValue("@Address", client.Address);
                    cmd.Parameters.AddWithValue("@State", client.State);
                    cmd.Parameters.AddWithValue("@City", client.City);
                    cmd.Parameters.AddWithValue("@Pincode", client.Pincode);
                    cmd.Parameters.AddWithValue("@AccountType", client.AccountType);
                    cmd.Parameters.AddWithValue("@AccountStatus", "Active");
                    cmd.Parameters.AddWithValue("@IsJointAccount", client.IsJointAccount ? "Yes" : "No");
                    cmd.Parameters.AddWithValue("@JointClientId", client.JointClientId == 0 ? (object)DBNull.Value : client.JointClientId);
                    

                    await con.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    return (true, "Offline client created successfully.");
                }
            }
            catch (SqlException ex)
            {
                return (false, $"Database error during offline client creation. {ex.Message}");
            }
        }
        public static async Task<(bool IsSuccess, string Message)> CreateClientAsync(ClientDTO client)
        {
            try
            {
                using (SqlConnection con = DBConnectionManager.GetConnection())
                using (SqlCommand cmd = new SqlCommand("sp_CreateClient", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@FullName", client.FullName);
                    cmd.Parameters.AddWithValue("@ParentName", client.ParentName);

                    //DateTime.TryParse(client.DOB, out DateTime parsedDOB);
                    cmd.Parameters.AddWithValue("@DOB", client.DOB);

                    cmd.Parameters.AddWithValue("@Gender", client.Gender);
                    cmd.Parameters.AddWithValue("@Nationality", client.Nationality);
                    cmd.Parameters.AddWithValue("@Occupation", client.Occupation);
                    cmd.Parameters.AddWithValue("@AadhaarNumber", client.AadhaarNumber);
                    cmd.Parameters.AddWithValue("@PANNumber", client.PANNumber);
                    cmd.Parameters.AddWithValue("@MobileNumber", client.MobileNumber);
                    cmd.Parameters.AddWithValue("@EmailId", client.EmailId);
                    cmd.Parameters.AddWithValue("@Address", client.Address);
                    cmd.Parameters.AddWithValue("@State", client.State);
                    cmd.Parameters.AddWithValue("@City", client.City);
                    cmd.Parameters.AddWithValue("@Pincode", client.Pincode);
                    cmd.Parameters.AddWithValue("@AccountType", client.AccountType);
                    cmd.Parameters.AddWithValue("@AccountStatus", "Active");
                    cmd.Parameters.AddWithValue("@IsJointAccount", client.IsJointAccount ? "Yes" : "No");
                    cmd.Parameters.AddWithValue("@JointClientId", client.JointClientId == 0 ? (object)DBNull.Value : client.JointClientId);
                    cmd.Parameters.AddWithValue("@Username", client.Username);
                    cmd.Parameters.AddWithValue("@Password", client.Password);

                    await con.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    return (true, "Client created successfully.");
                }
            }
            catch (SqlException ex)
            {
                return (false, $"Database error during client creation. {ex.Message}");
            }
        } 

        public static async Task<(bool IsSuccess, string Message)> UpdateClientProfileDetailsAsync(int clientId, ClientDTO client)
        {
            try
            {
                using (SqlConnection con = DBConnectionManager.GetConnection())
                using (SqlCommand cmd = new SqlCommand("sp_UpdateClientProfileDetails", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ClientID", clientId);
                    cmd.Parameters.AddWithValue("@FullName", client.FullName);
                    cmd.Parameters.AddWithValue("@ParentName", client.ParentName);

                    //DateTime.TryParse(client.DOB, out DateTime parsedDOB);
                    cmd.Parameters.AddWithValue("@DOB", client.DOB);

                    cmd.Parameters.AddWithValue("@Gender", client.Gender);
                    cmd.Parameters.AddWithValue("@Nationality", client.Nationality);
                    cmd.Parameters.AddWithValue("@Occupation", client.Occupation);
                    cmd.Parameters.AddWithValue("@AadhaarNumber", client.AadhaarNumber);
                    cmd.Parameters.AddWithValue("@PANNumber", client.PANNumber);
                    cmd.Parameters.AddWithValue("@MobileNumber", client.MobileNumber);
                    cmd.Parameters.AddWithValue("@EmailId", client.EmailId);
                    cmd.Parameters.AddWithValue("@Address", client.Address);
                    cmd.Parameters.AddWithValue("@State", client.State);
                    cmd.Parameters.AddWithValue("@City", client.City);
                    cmd.Parameters.AddWithValue("@Pincode", client.Pincode);
                   
                    cmd.Parameters.AddWithValue("@Username", client.Username);

                    await con.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    return (true, "Client Updated successfully.");
                }
            }
            catch (SqlException ex)
            {
                return (false, $"Database error during client data update. {ex.Message}");
            }
        }
        
        public static async Task<(bool IsSuccess, string Message)> DeleteClientAccountAsync(long accountNumber)
        {
            try
            {
                using (SqlConnection con = DBConnectionManager.GetConnection())
                using (SqlCommand cmd = new SqlCommand("sp_DeleteClientAccount", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@AccountNumber", accountNumber);

                    await con.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return (true, "Account deleted successfully.");
                }
            }
            catch (SqlException ex)
            {
                return (false, $"Database error during client account delete. {ex.Message}");
            }
        }
    }
}
