using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Configuration;

namespace BankingManagementSystem.DAL
{
	public class RequestDAL
	{
        private static readonly String CS = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;

        public static int SendJointAccountPendingRequests(int? clientId, int targetClientId, string payload)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(CS))
                {
                    SqlCommand cmd = new SqlCommand("sp_CreateJointAccountPendingRequests", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ClientId", clientId ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@TargetClientId", targetClientId);
                    cmd.Parameters.AddWithValue("@Payload", payload);

                    // Output parameter for AdminRequestId as RegistrationRequestId
                    SqlParameter outputParam = new SqlParameter("@AdminRequestId", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outputParam);


                    con.Open();
                    cmd.ExecuteNonQuery();

                    return (int)outputParam.Value;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Database error during creating joint account pending requests.", ex);
            }
        }
        public static int SendAdminPendingRequest(int? clientId, string payload)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(CS))
                {
                    SqlCommand cmd = new SqlCommand("sp_CreateAdminPendingRequest", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ClientId", clientId ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@RequestType", "CreateAccount_AdminApproval");
                    cmd.Parameters.AddWithValue("@Payload", payload);

                    // Output parameter for AdminRequestId as RegistrationRequestId
                    SqlParameter outputParam = new SqlParameter("@AdminRequestId", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outputParam);


                    con.Open();
                    cmd.ExecuteNonQuery();

                    return (int)outputParam.Value;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Database error during creating admin pending request.", ex);
            }
        }
    }
}