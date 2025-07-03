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
using System.Threading.Tasks;
using System.Web.Razor.Tokenizer;
using System.Globalization;

namespace BankingManagementSystem.DAL
{
    public class RequestDAL
    {
        private static readonly String CS = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;

        public async static Task<int> SendJointAccountPendingRequests(int? clientId, string requestType, int targetClientId, string payload)
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


                    //con.Open();
                    //cmd.ExecuteNonQuery();

                    //return (Task<int>)outputParam.Value;

                    await con.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    return (int)(outputParam.Value ?? 0);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Database error during Create Joint Account Pending Requests.", ex);
            }
        }
        //public static Task<int> SendAdminPendingRequest(int? clientId, string requestType, string payload)
        //{
        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(CS))
        //        {
        //            SqlCommand cmd = new SqlCommand("sp_CreateAdminPendingRequest", con);
        //            cmd.CommandType = CommandType.StoredProcedure;

        //            cmd.Parameters.AddWithValue("@ClientId", clientId ?? (object)DBNull.Value);
        //            cmd.Parameters.AddWithValue("@RequestType", requestType);
        //            cmd.Parameters.AddWithValue("@Payload", payload);

        //            // Output parameter for AdminRequestId as RegistrationRequestId
        //            SqlParameter outputParam = new SqlParameter("@AdminRequestId", SqlDbType.Int)
        //            {
        //                Direction = ParameterDirection.Output
        //            };
        //            cmd.Parameters.Add(outputParam);


        //            con.Open();
        //            cmd.ExecuteNonQueryAsync();

        //            return (Task<int>)outputParam.Value;
        //        }
        //    }
        //    catch (SqlException ex)
        //    {
        //        throw new Exception("Database error during Create Admin Pending Request.", ex);
        //    }
        //}

        public static async Task<int> SendAdminPendingRequest(int? clientId, string requestType, string payload)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(CS))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_CreateAdminPendingRequest", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@ClientId", clientId ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@RequestType", requestType);
                        cmd.Parameters.AddWithValue("@Payload", payload);

                        // Output parameter
                        SqlParameter outputParam = new SqlParameter("@AdminRequestId", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputParam);

                        await con.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();

                        return (int)(outputParam.Value ?? 0);
                    }
                }
            }
            catch (SqlException ex) 
            {
                throw new Exception("Database error during Create Admin Pending Request.", ex);
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
                throw new Exception("Database error during Check Duplicate New Registration Request.", ex);
            }

        }



        public static List<RequestDTO> GetAllRequestsByStatus(string status, string sortColumn, string sortDirection)
        {
            try
            {
                var requests = new List<RequestDTO>();
                using (SqlConnection con = new SqlConnection(CS))
                {
                    SqlCommand cmd = new SqlCommand("sp_GetRequestsByStatusForAdmin", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Status", string.IsNullOrEmpty(status) ? DBNull.Value : (object)status);
                    cmd.Parameters.AddWithValue("@SortBy", sortColumn);
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
                            int idxRepliedOn = reader.GetOrdinal(DbColumns.RepliedOn);



                            requests.Add(new RequestDTO
                            {
                                RequestId = reader.GetInt32(idxRequestId),
                                RequestType = reader.IsDBNull(idxRequestType) ? null : reader.GetString(idxRequestType),
                                Payload = reader.IsDBNull(idxPayload) ? null : reader.GetString(idxPayload),
                                RequestedOn = reader.GetDateTime(idxRequestedOn),
                                RepliedOn = reader.IsDBNull(idxRepliedOn) ? (DateTime?)null : reader.GetDateTime(idxRepliedOn)

                            });
                        }
                    }
                    return requests;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Database error during Get Request By Status For Admin.", ex);
            }

        }

        public static RequestDTO GetPendingRequestById(int requestId)
        {
            try
            {
                var requests = new List<RequestDTO>();
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

                        return new RequestDTO
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
            catch (SqlException ex)
            {
                throw new Exception("Database error during Get Pending Requests By Id.", ex);
            }
        }
        public static RequestDTO GetAllRequestById(int requestId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(CS))
                {
                    SqlCommand cmd = new SqlCommand("sp_GetAllRequestById", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RequestId", requestId);

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        int idxRequestType = reader.GetOrdinal(DbColumns.RequestType);
                        int idxPayload = reader.GetOrdinal(DbColumns.Payload);
                        int idxRequestedOn = reader.GetOrdinal(DbColumns.CreatedOn);
                        int idxStatus = reader.GetOrdinal(DbColumns.Status);

                        return new RequestDTO
                        {
                            RequestId = requestId,
                            RequestType = reader.IsDBNull(idxRequestType) ? null : reader.GetString(idxRequestType),
                            Payload = reader.IsDBNull(idxPayload) ? null : reader.GetString(idxPayload),
                            RequestedOn = reader.IsDBNull(idxRequestedOn) ? (DateTime?)null : reader.GetDateTime(idxRequestedOn),
                            Status = reader.IsDBNull(idxStatus) ? null : reader.GetString(idxStatus)
                        };
                    }
                    return null;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Database error during Get All Requests By Id.", ex);
            }

        }

        public static bool UpdateRequestStatus(int requestId, string newStatus, int repliedBy)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(CS))
                {
                    SqlCommand cmd = new SqlCommand("sp_UpdateRequestStatus", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RequestId", requestId);
                    cmd.Parameters.AddWithValue("@Status", newStatus);
                    cmd.Parameters.AddWithValue("@RepliedBy", repliedBy);

                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Database error during Update Request Status.", ex);
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
            catch (SqlException ex)
            {
                throw new Exception("Database error during Update Request Payload.", ex);
            }
        }

        public static bool DeleteRequestByStatus(int requestId, string status)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(CS))
                {
                    SqlCommand cmd = new SqlCommand("sp_DeleteRequestByStatus", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RequestId", requestId);
                    cmd.Parameters.AddWithValue("@Status", status);

                    con.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Database error during Delete Request By Status.", ex);
            }
        }

















        // Client Pending Requests


        public static async Task<List<RequestDTO>> GetReceivedRequestsForClient(int clientId, string sortColumn, string sortDirection)
        {
            try
            {
                var requests = new List<RequestDTO>();
                using (SqlConnection con = new SqlConnection(CS))
                {
                    SqlCommand cmd = new SqlCommand("sp_GetReceivedPendingRequestsForClient", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.AddWithValue("@Status", string.IsNullOrEmpty(status) ? DBNull.Value : (object)status);
                    cmd.Parameters.AddWithValue("@ClientId", clientId);
                    cmd.Parameters.AddWithValue("@SortBy", sortColumn);
                    cmd.Parameters.AddWithValue("@SortDirection", sortDirection);

                    con.Open();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            int idxRequestId = reader.GetOrdinal(DbColumns.RequestId);
                            int idxRequestType = reader.GetOrdinal(DbColumns.RequestType);
                            int idxPayload = reader.GetOrdinal(DbColumns.Payload);
                            int idxRequestedOn = reader.GetOrdinal(DbColumns.CreatedOn);
                            //int idxRepliedOn = reader.GetOrdinal(DbColumns.RepliedOn);



                            requests.Add(new RequestDTO
                            {
                                RequestId = reader.GetInt32(idxRequestId),
                                RequestType = reader.IsDBNull(idxRequestType) ? null : reader.GetString(idxRequestType),
                                Payload = reader.IsDBNull(idxPayload) ? null : reader.GetString(idxPayload),
                                RequestedOn = reader.GetDateTime(idxRequestedOn),
                                //RepliedOn = reader.IsDBNull(idxRepliedOn) ? (DateTime?)null : reader.GetDateTime(idxRepliedOn)

                            });
                        }
                    }
                    return requests;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Database error during Get Received Requests For Client.", ex);
            }
        }
         public static async Task<List<RequestDTO>> GetSentRequestsByClient(int clientId, string sortColumn, string sortDirection)
        {
            try
            {
                var requests = new List<RequestDTO>();
                using (SqlConnection con = new SqlConnection(CS))
                {
                    SqlCommand cmd = new SqlCommand("sp_GetSentRequestsByClient", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.AddWithValue("@Status", string.IsNullOrEmpty(status) ? DBNull.Value : (object)status);
                    cmd.Parameters.AddWithValue("@ClientId", clientId);
                    cmd.Parameters.AddWithValue("@SortBy", sortColumn);
                    cmd.Parameters.AddWithValue("@SortDirection", sortDirection);

                    con.Open();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            int idxRequestId = reader.GetOrdinal(DbColumns.RequestId);
                            int idxRequestType = reader.GetOrdinal(DbColumns.RequestType);
                            int idxPayload = reader.GetOrdinal(DbColumns.Payload);
                            int idxRequestedOn = reader.GetOrdinal(DbColumns.CreatedOn);
                            int idxRepliedOn = reader.GetOrdinal(DbColumns.RepliedOn);
                            int idxStatus = reader.GetOrdinal(DbColumns.Status);



                            requests.Add(new RequestDTO
                            {
                                RequestId = reader.GetInt32(idxRequestId),
                                RequestType = reader.IsDBNull(idxRequestType) ? null : reader.GetString(idxRequestType),
                                Payload = reader.IsDBNull(idxPayload) ? null : reader.GetString(idxPayload),
                                RequestedOn = reader.GetDateTime(idxRequestedOn),
                                RepliedOn = reader.IsDBNull(idxRepliedOn) ? (DateTime?)null : reader.GetDateTime(idxRepliedOn),
                                Status = reader.IsDBNull(idxStatus) ? null : reader.GetString(idxStatus)

                            });
                        }
                    }
                    return requests;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Database error during Get Received Requests For Client.", ex);
            }
        }

    }
}