using BankingManagementSystem.Helpers;
using BankingManagementSystem.Models.Constants;
using BankingManagementSystem.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace BankingManagementSystem.DAL
{
    public class RequestDAL
    {
        public async static Task<int> SendJointAccountPendingRequestsAsync(int? clientId, string requestType, int? targetClientId, string payload)
        {
            try
            {
                using (SqlConnection con = DBConnectionManager.GetConnection())
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

        public static async Task<int> SendAdminPendingRequestAsync(int? clientId, string requestType, string payload)
        {
            try
            {
                using (SqlConnection con = DBConnectionManager.GetConnection())
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

        public static async Task<bool> IsDuplicateNewRegistrationPendingRequestAsync(string aadhaar, string pan)
        {
            try
            {
                using (SqlConnection con = DBConnectionManager.GetConnection())
                {
                    SqlCommand cmd = new SqlCommand("sp_CheckDuplicateNewRegistrationRequest", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@AadhaarNumber", aadhaar);
                    cmd.Parameters.AddWithValue("@PANNumber", pan);

                    await con.OpenAsync();
                    object result = await cmd.ExecuteScalarAsync();

                    return Convert.ToInt32(result) > 0;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Database error during Check Duplicate New Registration Request.", ex);
            }
        }

        public static async Task<bool> IsDuplicateUpdateProfileDetailsPendingRequestAsync(string aadhaar, string pan)
        {
            try
            {
                using (SqlConnection con = DBConnectionManager.GetConnection())
                {
                    SqlCommand cmd = new SqlCommand("sp_CheckDuplicateUpdateProfileDetailsRequest", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@AadhaarNumber", aadhaar);
                    cmd.Parameters.AddWithValue("@PANNumber", pan);

                    await con.OpenAsync();
                    object result = await cmd.ExecuteScalarAsync();

                    return Convert.ToInt32(result) > 0;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Database error during Check Duplicate Update Profile Details Request.", ex);
            }
        }

        public static async Task<List<RequestDTO>> GetAllRequestsByStatusAsync(string status, string sortColumn, string sortDirection)
        {
            try
            {
                var requests = new List<RequestDTO>();
                using (SqlConnection con = DBConnectionManager.GetConnection())
                using (SqlCommand cmd = new SqlCommand("sp_GetRequestsByStatusForAdmin", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Status", string.IsNullOrEmpty(status) ? DBNull.Value : (object)status);
                    cmd.Parameters.AddWithValue("@SortBy", sortColumn);
                    cmd.Parameters.AddWithValue("@SortDirection", sortDirection);

                    await con.OpenAsync();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
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
                }
                return requests;
            }
            catch (SqlException ex)
            {
                throw new Exception("Database error during Get Request By Status For Admin.", ex);
            }
        }

        public static async Task<RequestDTO> GetPendingRequestByIdAsync(int requestId)
        {
            try
            {
                using (SqlConnection con = DBConnectionManager.GetConnection())
                {
                    SqlCommand cmd = new SqlCommand("sp_GetPendingRequestById", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RequestId", requestId);

                    await con.OpenAsync();
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    if (await reader.ReadAsync())
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

        public static async Task<RequestDTO> GetAllRequestByIdAsync(int requestId)
        {
            try
            {
                using (SqlConnection con = DBConnectionManager.GetConnection())
                {
                    SqlCommand cmd = new SqlCommand("sp_GetAllRequestById", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RequestId", requestId);

                    await con.OpenAsync();
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    if (await reader.ReadAsync())
                    {
                        int idxRequestType = reader.GetOrdinal(DbColumns.RequestType);
                        int idxPayload = reader.GetOrdinal(DbColumns.Payload);
                        int idxRequestedOn = reader.GetOrdinal(DbColumns.CreatedOn);
                        int idxStatus = reader.GetOrdinal(DbColumns.Status);
                        int idxRepliedBy = reader.GetOrdinal(DbColumns.RepliedBy);

                        return new RequestDTO
                        {
                            RequestId = requestId,
                            RequestType = reader.IsDBNull(idxRequestType) ? null : reader.GetString(idxRequestType),
                            Payload = reader.IsDBNull(idxPayload) ? null : reader.GetString(idxPayload),
                            RequestedOn = reader.IsDBNull(idxRequestedOn) ? (DateTime?)null : reader.GetDateTime(idxRequestedOn),
                            Status = reader.IsDBNull(idxStatus) ? null : reader.GetString(idxStatus),
                            RepliedBy = reader.IsDBNull(idxRepliedBy) ? (int?)null : reader.GetInt32(idxRepliedBy)
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

        public static async Task<bool> UpdateRequestStatusAsync(int requestId, string newStatus, int repliedBy)
        {
            try
            {
                using (SqlConnection con = DBConnectionManager.GetConnection())
                {
                    SqlCommand cmd = new SqlCommand("sp_UpdateRequestStatus", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RequestId", requestId);
                    cmd.Parameters.AddWithValue("@Status", newStatus);
                    cmd.Parameters.AddWithValue("@RepliedBy", repliedBy);

                    await con.OpenAsync();
                    int rowsAffected = await cmd.ExecuteNonQueryAsync();

                    return rowsAffected > 0;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Database error during Update Request Status.", ex);
            }
        }

        public static async Task<bool> UpdateRequestPayloadAsync(int requestId, string newPayload)
        {
            try
            {
                using (SqlConnection con = DBConnectionManager.GetConnection())
                {
                    SqlCommand cmd = new SqlCommand("sp_UpdateRequestPayload", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RequestId", requestId);
                    cmd.Parameters.AddWithValue("@Payload", newPayload);

                    await con.OpenAsync();
                    int rows = await cmd.ExecuteNonQueryAsync();
                    return rows > 0;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Database error during Update Request Payload.", ex);
            }
        }

        public static async Task<bool> DeleteRequestByStatusAsync(int requestId, string status)
        {
            try
            {
                using (SqlConnection con = DBConnectionManager.GetConnection())
                {
                    SqlCommand cmd = new SqlCommand("sp_DeleteRequestByStatus", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RequestId", requestId);
                    cmd.Parameters.AddWithValue("@Status", status);

                    await con.OpenAsync();
                    int rows = await cmd.ExecuteNonQueryAsync();
                    return rows > 0;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Database error during Delete Request By Status.", ex);
            }
        }


        // Client Pending Requests
        public static async Task<List<RequestDTO>> GetReceivedRequestsForClientAsync(int clientId, string sortColumn, string sortDirection)
        {
            try
            {
                var requests = new List<RequestDTO>();
                using (SqlConnection con = DBConnectionManager.GetConnection())
                {
                    SqlCommand cmd = new SqlCommand("sp_GetReceivedRequestsForClient", con);
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

                            requests.Add(new RequestDTO
                            {
                                RequestId = reader.GetInt32(idxRequestId) - 1, // -1 to maintain the request id numbering across platform
                                RequestType = reader.IsDBNull(idxRequestType) ? null : reader.GetString(idxRequestType),
                                Payload = reader.IsDBNull(idxPayload) ? null : reader.GetString(idxPayload),
                                RequestedOn = reader.GetDateTime(idxRequestedOn),
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

        public static async Task<List<RequestDTO>> GetSentRequestsByClientAsync(int clientId, string sortColumn, string sortDirection)
        {
            try
            {
                var requests = new List<RequestDTO>();
                using (SqlConnection con = DBConnectionManager.GetConnection())
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