using BankingManagementSystem.Models;
using BankingManagementSystem.Models.API;
using BankingManagementSystem.Models.Constants;
using BankingManagementSystem.Models.ConstraintTypes;
using BankingManagementSystem.Models.DTOs;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace BankingManagementSystem.DAL
{
    public static class ClientDAL
    {
        private static readonly string CS = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;

        public static async Task<User> CheckClientCredentialsAsync(AuthRequestDTO client)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(CS))
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
                using (SqlConnection con = new SqlConnection(CS))
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
                using (SqlConnection con = new SqlConnection(CS))
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
                using (SqlConnection con = new SqlConnection(CS))
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
    }
}
