using BankingManagementSystem.Helpers;
using BankingManagementSystem.Models.Constants;
using BankingManagementSystem.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Razor.Tokenizer;

namespace BankingManagementSystem.DAL
{
    public class TransactionDAL
    {
        public static async Task<decimal> GetBalanceAsync(long accountNumber)
        {
            try
            {
                decimal balance = -1;
                using (SqlConnection con = DBConnectionManager.GetConnection())
                using (SqlCommand cmd = new SqlCommand("sp_GetAccountBalance", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@AccountNumber", accountNumber);

                    await con.OpenAsync();
                    object result = await cmd.ExecuteScalarAsync();
                    if (result != null && decimal.TryParse(result.ToString(), out decimal b))
                    {
                        balance = b;
                    }
                }
                return balance;
            }
            catch (SqlException ex)
            {
                throw new Exception("Database error during get account balance.", ex);
            }
        }

        public static async Task<bool> UpdateBalanceAsync(long accountNumber, decimal newBalance)
        {
            try
            {
                using (SqlConnection con = DBConnectionManager.GetConnection())
                using (SqlCommand cmd = new SqlCommand("sp_UpdateAccountBalance", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@AccountNumber", accountNumber);
                    cmd.Parameters.AddWithValue("@Balance", newBalance);

                    await con.OpenAsync();
                    int rows = await cmd.ExecuteNonQueryAsync();
                    return rows > 0;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Database error during update account balance.", ex);
            }
        }

        public static async Task<bool> InsertTransactionAsync(TransactionDTO dto)
        {
            try
            {
                using (SqlConnection con = DBConnectionManager.GetConnection())
                using (SqlCommand cmd = new SqlCommand("sp_InsertTransaction", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@AccountNumber", dto.AccountNumber);
                    cmd.Parameters.AddWithValue("@TransactionType", dto.TransactionType);
                    cmd.Parameters.AddWithValue("@Amount", dto.Amount);
                    cmd.Parameters.AddWithValue("@BalanceAfter", dto.BalanceAfter);
                    cmd.Parameters.AddWithValue("@Remarks", dto.Remarks);
                    cmd.Parameters.AddWithValue("@PerformedBy", dto.PerformedBy);
                    cmd.Parameters.AddWithValue("@IsSuccessful", dto.IsSuccessful);
                    cmd.Parameters.AddWithValue("@CounterPartyAccountNo", (object)dto.CounterPartyAccountNo ?? DBNull.Value);

                    await con.OpenAsync();
                    int rows = await cmd.ExecuteNonQueryAsync();
                    return rows > 0;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Database error during Insert Transaction Async.", ex);
            }
        }


        public static async Task<List<TransactionDTO>> GetTransactionHistoryAsync(long accountNumber)
        {
            List<TransactionDTO> transactions = new List<TransactionDTO>();

            using (SqlConnection conn = DBConnectionManager.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_GetTransactionHistory", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AccountNumber", accountNumber);

                await conn.OpenAsync();

                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    int idxTransactionId = reader.GetOrdinal(DbColumns.TransactionId);
                    int idxTransactionDate = reader.GetOrdinal(DbColumns.TransactionDate);
                    int idxType = reader.GetOrdinal(DbColumns.TransactionType);
                    int idxAmount = reader.GetOrdinal(DbColumns.TransactionAmount);
                    int idxBalanceAfter = reader.GetOrdinal(DbColumns.BalanceAfter);
                    int idxRemarks = reader.GetOrdinal(DbColumns.Remarks);
                    int idxPerformedBy = reader.GetOrdinal(DbColumns.PerformedBy);
                    int idxCounterparty = reader.GetOrdinal(DbColumns.CounterPartyAccountNo);
                    int idxIsSuccessful = reader.GetOrdinal(DbColumns.IsSuccessful);

                    while (await reader.ReadAsync())
                    {
                        transactions.Add(new TransactionDTO
                        {
                            AccountNumber = accountNumber,
                            TransactionId = reader.IsDBNull(idxTransactionId) ? 0 : reader.GetInt32(idxTransactionId),
                            TransactionDate = reader.IsDBNull(idxTransactionDate) ? DateTime.MinValue : reader.GetDateTime(idxTransactionDate),
                            TransactionType = reader.IsDBNull(idxType) ? null : reader.GetString(idxType),
                            Amount = reader.IsDBNull(idxAmount) ? 0 : reader.GetDecimal(idxAmount),
                            BalanceAfter = reader.IsDBNull(idxBalanceAfter) ? 0 : reader.GetDecimal(idxBalanceAfter),
                            Remarks = reader.IsDBNull(idxRemarks) ? null : reader.GetString(idxRemarks),
                            PerformedBy = reader.IsDBNull(idxPerformedBy) ? 0 : reader.GetInt32(idxPerformedBy),
                            CounterPartyAccountNo = reader.IsDBNull(idxCounterparty) ? 0 : reader.GetInt64(idxCounterparty),
                            IsSuccessful = !reader.IsDBNull(idxIsSuccessful) && reader.GetBoolean(idxIsSuccessful)
                        });
                    }
                }
            }

            return transactions;
        }

    }

}