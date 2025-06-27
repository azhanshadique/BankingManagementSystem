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

namespace BankingManagementSystem.DAL
{
	public class AdminDAL
	{
        private readonly String CS = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;

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
    }
}