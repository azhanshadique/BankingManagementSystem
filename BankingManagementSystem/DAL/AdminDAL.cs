using BankingManagementSystem.Models.API;
using BankingManagementSystem.Models.ConstraintTypes;
using BankingManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using BankingManagementSystem.Models.Constants;
using BankingManagementSystem.Models.DTOs;

namespace BankingManagementSystem.DAL
{
	public class AdminDAL
	{
        private static readonly String CS = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;

        public User CheckAdminCredentials(AuthRequestDTO admin)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(CS))
                {
                    SqlCommand cmd = new SqlCommand("sp_ValidateAdmin", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Username", admin.Username);
                    cmd.Parameters.AddWithValue("@Password", admin.Password);

                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int idxAdminId = reader.GetOrdinal(DbColumns.AdminId);
                            int idxFullName = reader.GetOrdinal(DbColumns.FullName);
                            int idxUsername = reader.GetOrdinal(DbColumns.Username);

                            int adminId = reader.GetInt32(idxAdminId);
                            string fullName = reader.IsDBNull(idxFullName) ? null : reader.GetString(idxFullName);
                            string username = reader.IsDBNull(idxUsername) ? null : reader.GetString(idxUsername);

                            return new User
                            {
                                UserID = adminId,
                                Username = username,
                                Role = UserRoles.ADMIN,
                                FullName = fullName

                            };
                        }
                    }
                    return null; // Not found or invalid
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Database error during admin validation.", ex);
            }
        }

        public static bool CreateClient(ClientDTO client, out string message)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(CS))
                {
                    SqlCommand cmd = new SqlCommand("sp_CreateClient", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    


                    // Add all client parameters from ClientDTO
                    cmd.Parameters.AddWithValue("@FullName", client.FullName);
                    cmd.Parameters.AddWithValue("@ParentName", client.ParentName);

                    DateTime.TryParse(client.DOB, out DateTime parsedDOB);
                    cmd.Parameters.AddWithValue("@DOB", parsedDOB);

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
                    //cmd.Parameters.AddWithValue("@JointRelationship", string.IsNullOrEmpty(client.JointRelationship) ? (object)DBNull.Value : client.JointRelationship);
                    cmd.Parameters.AddWithValue("@Username", client.Username);
                    cmd.Parameters.AddWithValue("@Password", client.Password);

                    con.Open();
                    cmd.ExecuteNonQuery();

                    message = "Client created successfully.";
                    return true;
                }
            }
            catch (SqlException ex)
            {
                message = $"Database error during client creation. {ex.Message}";
                return false;
                //throw new Exception("Database error during client creation.", ex);
            }
        }

    }
}