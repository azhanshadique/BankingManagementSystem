using BankingManagementSystem.Models;
using BankingManagementSystem.Models.API;
using BankingManagementSystem.Models.Constants;
using BankingManagementSystem.Models.ConstraintTypes;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;

namespace BankingManagementSystem.DAL
{
	public class ClientDAL
	{
        private readonly String CS = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
      
        public User CheckClientCredentials(AuthRequestDTO client)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(CS))
                {
                    SqlCommand cmd = new SqlCommand("sp_ValidateClient", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Username", client.Username);
                    cmd.Parameters.AddWithValue("@Password", client.Password);

                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int idxClientId = reader.GetOrdinal(DbColumns.ClientId);
                            int idxFullName = reader.GetOrdinal(DbColumns.FullName);
                            int idxUsername = reader.GetOrdinal(DbColumns.Username);

                            int clientId = reader.GetInt32(idxClientId);
                            string fullName = reader.IsDBNull(idxFullName) ? null : reader.GetString(idxFullName);
                            string username = reader.IsDBNull(idxUsername) ? null : reader.GetString(idxUsername);

                            return new User
                            {
                                UserID = clientId,
                                Username = username,
                                Role = UserRoles.CLIENT,
                                FullName = fullName

                            };
                        }
                    }
                    return null; // Not found or invalid
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Database error during client validation.", ex);
            }

           
        }
        public bool CheckIfClientExists(string aadhaar, string pan)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(CS))
                {
                    SqlCommand cmd = new SqlCommand("sp_CheckClientExists", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@AadhaarNumber", aadhaar);
                    cmd.Parameters.AddWithValue("@PANNumber", pan);

                    con.Open();
                    object result = cmd.ExecuteScalar();

                    return Convert.ToInt32(result) > 0;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Database error during client exists validation.", ex);
            }
        }

    }
}