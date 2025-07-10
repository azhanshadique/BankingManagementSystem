using BankingManagementSystem.DAL;
using BankingManagementSystem.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BankingManagementSystem.BLL
{
    public static class TransactionBLL
    {
        public static async Task<(bool IsSuccess, string Message)> DepositAmountAsync(DepositDTO dto)
        {
            if (dto.Amount <= 0)
                return (false, "Deposit amount must be greater than 0.");

            if (!await ClientDAL.CheckClientPassword(dto.ClientId, dto.Password))
                return (false, "Incorrect password.");

            decimal currentBalance = await TransactionDAL.GetBalanceAsync(dto.AccountNumber);
            if (currentBalance < 0)
                return (false, "Invalid account.");

            decimal newBalance = currentBalance + dto.Amount;
            bool isUpdated = await TransactionDAL.UpdateBalanceAsync(dto.AccountNumber, newBalance);

            // Handle failure scenario
            if (!isUpdated)
            {
                TransactionDTO failedTx = new TransactionDTO
                {
                    AccountNumber = dto.AccountNumber,
                    TransactionType = "Deposit",
                    Amount = dto.Amount,
                    BalanceAfter = currentBalance, // original balance
                    Remarks = "[FAILED] " + dto.Remarks,
                    PerformedBy = dto.ClientId,
                    IsSuccessful = false
                };
                await TransactionDAL.InsertTransactionAsync(failedTx);

                return (false, "Failed to update balance.");
            }

            // Handle success scenario
            TransactionDTO successTx = new TransactionDTO
            {
                AccountNumber = dto.AccountNumber,
                TransactionType = "Deposit",
                Amount = dto.Amount,
                BalanceAfter = newBalance, // new balance
                Remarks = dto.Remarks,
                PerformedBy = dto.ClientId,
                IsSuccessful = true
            };
            await TransactionDAL.InsertTransactionAsync(successTx);

            return (true, $"Rs.{dto.Amount} deposited successfully. Current Balance: Rs.{newBalance}");
        }
        public static async Task<(bool IsSuccess, string Message)> WithdrawAmountAsync(WithdrawDTO dto)
        {
            if (dto.Amount <= 0)
                return (false, "Withdraw amount must be greater than 0.");

            if (!await ClientDAL.CheckClientPassword(dto.ClientId, dto.Password))
                return (false, "Incorrect password.");

            decimal currentBalance = await TransactionDAL.GetBalanceAsync(dto.AccountNumber);
            if (currentBalance < 0)
                return (false, "Invalid account.");

            decimal newBalance = currentBalance - dto.Amount;
            if (newBalance < 0)
                return (false, "Insufficient balance.");

            bool isUpdated = await TransactionDAL.UpdateBalanceAsync(dto.AccountNumber, newBalance);

            // Handle failure scenario
            if (!isUpdated)
            {
                TransactionDTO failedTx = new TransactionDTO
                {
                    AccountNumber = dto.AccountNumber,
                    TransactionType = "Withdraw",
                    Amount = dto.Amount,
                    BalanceAfter = currentBalance, // original balance
                    Remarks = "[FAILED] " + dto.Remarks,
                    PerformedBy = dto.ClientId,
                    IsSuccessful = false
                };
                await TransactionDAL.InsertTransactionAsync(failedTx);

                return (false, "Failed to update balance.");
            }

            // Handle success scenario
            TransactionDTO successTx = new TransactionDTO
            {
                AccountNumber = dto.AccountNumber,
                TransactionType = "Withdraw",
                Amount = dto.Amount,
                BalanceAfter = newBalance, // new balance
                Remarks = dto.Remarks,
                PerformedBy = dto.ClientId,
                IsSuccessful = true
            };
            await TransactionDAL.InsertTransactionAsync(successTx);

            return (true, $"Rs.{dto.Amount} withdrawn successfully. Current Balance: Rs.{newBalance}");
        }
        public static async Task<(bool IsSuccess, string Message)> TransferAmountAsync(TransferDTO dto)
        {
            if (dto.Amount <= 0)
                return (false, "Amount to transfer must be greater than 0.");

            if (!await ClientDAL.CheckClientPassword(dto.ClientId, dto.Password))
                return (false, "Incorrect password.");

            decimal senderBalance = await TransactionDAL.GetBalanceAsync(dto.FromAccountNumber);
            if (senderBalance < 0)
                return (false, "Invalid source account.");

            decimal receiverBalance = await TransactionDAL.GetBalanceAsync(dto.ToAccountNumber);
            if (receiverBalance < 0)
                return (false, "Invalid destination account.");

            if (dto.FromAccountNumber == dto.ToAccountNumber)
                return (false, "Source account and destination account cannot be same.");

            if (senderBalance < dto.Amount)
                return (false, "Insufficient balance in source account.");

            decimal newSenderBalance = senderBalance - dto.Amount;
            decimal newReceiverBalance = receiverBalance + dto.Amount;

            // Deduct from sender
            bool senderUpdated = await TransactionDAL.UpdateBalanceAsync(dto.FromAccountNumber, newSenderBalance);
            if (!senderUpdated)
            {
                await TransactionDAL.InsertTransactionAsync(new TransactionDTO
                {
                    AccountNumber = dto.FromAccountNumber,
                    TransactionType = "Transfer (Debit)",
                    Amount = dto.Amount,
                    BalanceAfter = senderBalance,
                    Remarks = "[FAILED] " + dto.Remarks,
                    PerformedBy = dto.ClientId,
                    CounterPartyAccountNo = dto.ToAccountNumber,
                    IsSuccessful = false
                });

                return (false, "Failed to deduct from source account.");
            }

            // Add to receiver
            bool receiverUpdated = await TransactionDAL.UpdateBalanceAsync(dto.ToAccountNumber, newReceiverBalance);
            if (!receiverUpdated)
            {
                await TransactionDAL.UpdateBalanceAsync(dto.FromAccountNumber, senderBalance);

                await TransactionDAL.InsertTransactionAsync(new TransactionDTO
                {
                    AccountNumber = dto.ToAccountNumber,
                    TransactionType = "Transfer (Credit)",
                    Amount = dto.Amount,
                    BalanceAfter = receiverBalance,
                    Remarks = "[FAILED] " + dto.Remarks,
                    PerformedBy = dto.ClientId,
                    CounterPartyAccountNo = dto.FromAccountNumber,
                    IsSuccessful = false
                });

                return (false, "Failed to credit destination account. Sender balance reverted.");
            }

            // Both updates successful - Insert both transactions
            await TransactionDAL.InsertTransactionAsync(new TransactionDTO
            {
                AccountNumber = dto.FromAccountNumber,
                TransactionType = "Transfer (Debit)",
                Amount = dto.Amount,
                BalanceAfter = newSenderBalance,
                Remarks = dto.Remarks,
                PerformedBy = dto.ClientId,
                CounterPartyAccountNo = dto.ToAccountNumber,
                IsSuccessful = true
            });

            await TransactionDAL.InsertTransactionAsync(new TransactionDTO
            {
                AccountNumber = dto.ToAccountNumber,
                TransactionType = "Transfer (Credit)",
                Amount = dto.Amount,
                BalanceAfter = newReceiverBalance,
                Remarks = dto.Remarks,
                PerformedBy = dto.ClientId,
                CounterPartyAccountNo = dto.FromAccountNumber,
                IsSuccessful = true
            });

            return (true, $"Successfully transferred Rs.{dto.Amount} from Acc. {dto.FromAccountNumber} to Acc. {dto.ToAccountNumber}.");
        }


        public static async Task<List<TransactionDTO>> GetTransactionHistoryAsync(long accountNumber)
        {
            decimal balance = await TransactionDAL.GetBalanceAsync(accountNumber);
            if (balance < 0)
                return null;
            return await TransactionDAL.GetTransactionHistoryAsync(accountNumber);
        }


    }

}