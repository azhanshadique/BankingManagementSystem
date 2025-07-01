using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Configuration;
using BankingManagementSystem.Models.DTOs;
using BankingManagementSystem.Models.Constants;
using System.Runtime.Remoting.Messaging;
using BankingManagementSystem.Models.ConstraintTypes;
using BankingManagementSystem.Models;

namespace BankingManagementSystem.DAL
{
	public class RequestDAL
	{
        private static readonly String CS = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;

        public static int SendJointAccountPendingRequests(int? clientId, string requestType, int targetClientId, string payload)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(CS))
                {
                    SqlCommand cmd = new SqlCommand("sp_CreateJointAccountPendingRequests", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ClientId", clientId ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@RequestType", requestType);
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
        public static int SendAdminPendingRequest(int? clientId, string requestType, string payload)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(CS))
                {
                    SqlCommand cmd = new SqlCommand("sp_CreateAdminPendingRequest", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ClientId", clientId ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@RequestType", requestType);
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
        public static bool IsDuplicateNewRegistrationPendingRequest(string aadhaar, string pan)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(CS))
                {
                    SqlCommand cmd = new SqlCommand("sp_CheckDuplicateNewRegistrationRequest", con);
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
                throw new Exception("Database error during checking duplicate pending request.", ex);
            }
            
        }



        public static List<PendingRequestDTO> GetAllRequestsByStatus(string status, string sortBy, string sortDirection)
        {
            try
            {
                var requests = new List<PendingRequestDTO>();
                using (SqlConnection con = new SqlConnection(CS))
                {
                    SqlCommand cmd = new SqlCommand("sp_GetAllRequestsByStatus", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Status", string.IsNullOrEmpty(status) ? DBNull.Value : (object)status);
                    cmd.Parameters.AddWithValue("@SortBy", sortBy);
                    cmd.Parameters.AddWithValue("@SortDirection", sortDirection);

                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int idxRequestId = reader.GetOrdinal(DbColumns.RequestId);
                            int idxRequestType = reader.GetOrdinal(DbColumns.RequestType);
                            int idxPayload = reader.GetOrdinal(DbColumns.Payload);
                            int idxRequestedOn = reader.GetOrdinal(DbColumns.CreatedOn);



                            requests.Add(new PendingRequestDTO
                            {
                                RequestId = reader.GetInt32(idxRequestId),
                                RequestType = reader.IsDBNull(idxRequestType) ? null : reader.GetString(idxRequestType),
                                Payload = reader.IsDBNull(idxPayload) ? null : reader.GetString(idxPayload),
                                RequestedOn = reader.GetDateTime(idxRequestedOn)
                            });
                        }
                    }
                    return requests;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Database error during getting all request by status.", ex);
            }
           
        }

        public static PendingRequestDTO GetPendingRequestById(int requestId)
        {
            try
            {
                var requests = new List<PendingRequestDTO>();
                using (SqlConnection con = new SqlConnection(CS))
                {
                    SqlCommand cmd = new SqlCommand("sp_GetPendingRequestById", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RequestId", requestId);

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        int idxRequestType = reader.GetOrdinal(DbColumns.RequestType);
                        int idxPayload = reader.GetOrdinal(DbColumns.Payload);
                        int idxRequestedOn = reader.GetOrdinal(DbColumns.CreatedOn);

                        return new PendingRequestDTO
                        {

                            RequestId = requestId,
                            RequestType = reader.IsDBNull(idxRequestType) ? null : reader.GetString(idxRequestType),
                            Payload = reader.IsDBNull(idxPayload) ? null : reader.GetString(idxPayload),
                            RequestedOn = reader.IsDBNull(idxRequestedOn) ? (DateTime?)null : reader.GetDateTime(idxRequestedOn),

                        };
                    }

                    return null;
                }
            }
            catch
            {
                return null;
            }
        }
        public static PendingRequestDTO GetRequestDetailsForAdmin(int requestId)
        {
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("sp_GetRequestDetailsForAdmin", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RequestId", requestId);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    return new PendingRequestDTO
                    {
                        RequestId = requestId,
                        RequestType = rdr["request_type"].ToString(),
                        Payload = rdr["payload"].ToString(),
                        //CreatedOn = Convert.ToDateTime(rdr["created_on"]),
                        //Status = rdr["status"].ToString(),
                        //AdminApproved = rdr["admin_approved"].ToString(),
                        //CoHolderApproved = rdr["co_holder_approved"]?.ToString()
                    };
                }
            }
            return null;
        }

        public static bool UpdateRequestStatus(int requestId, string newStatus, int repliedBy)
        {
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("sp_UpdateRequestStatus", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RequestId", requestId);
                cmd.Parameters.AddWithValue("@Status", newStatus);
                cmd.Parameters.AddWithValue("@RepliedBy", repliedBy);

                con.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }







        public static bool UpdateRequestPayload(int requestId, string newPayload)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(CS))
                {
                    SqlCommand cmd = new SqlCommand("sp_UpdateRequestPayload", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RequestId", requestId);
                    cmd.Parameters.AddWithValue("@Payload", newPayload);

                    con.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool DeletePendingRequest(int requestId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(CS))
                {
                    SqlCommand cmd = new SqlCommand("sp_DeletePendingRequest", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RequestId", requestId);

                    con.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}