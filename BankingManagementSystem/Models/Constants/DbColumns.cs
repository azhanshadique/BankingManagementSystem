using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankingManagementSystem.Models.Constants
{
    public static class DbColumns
    {
        public const string AdminId = "admin_id";

        public const string ClientId = "client_id";
        public const string FullName = "full_name";
        public const string ParentName = "parent_name";
        public const string DOB = "dob";
        public const string Gender = "gender";
        public const string Nationality = "nationality";
        public const string Occupation = "occupation";
        public const string AadhaarNumber = "aadhaar_number";
        public const string PANNumber = "pan_number";

        public const string MobileNumber = "mobile_number";
        public const string EmailId = "email_id";
        public const string Address = "address";
        public const string City = "city";
        public const string State = "state";
        public const string Pincode = "pincode";

        public const string Username = "username";
        public const string Password = "password";

        public const string RequestId = "request_id";
        public const string RequestType = "request_type";
        public const string Payload = "payload";
        public const string CreatedOn = "created_on";
        public const string RepliedOn = "replied_on";
        public const string Status = "status";
        public const string RepliedBy = "replied_by";

        public const string AccountNumber = "account_number";
        public const string AccountType = "account_type";
        public const string Balance = "balance";
        public const string AccountStatus = "account_status";
        public const string IsPrimary = "is_primary_account";
        public const string CoHolderClientId = "coholder_client_id";
        public const string CoHolderName = "coholder_name";

        public const string TransactionId = "transaction_id";
        public const string TransactionDate = "transaction_date";
        public const string TransactionType = "transaction_type";
        public const string TransactionAmount = "amount";
        public const string BalanceAfter = "balance_after";
        public const string Remarks = "remarks";
        public const string PerformedBy = "performed_by";
        public const string CounterPartyAccountNo = "counterparty_account_no";
        public const string IsSuccessful = "is_successful";




    }
}