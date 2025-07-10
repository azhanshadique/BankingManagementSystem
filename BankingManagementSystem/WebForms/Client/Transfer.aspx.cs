using BankingManagementSystem.Helpers;
using BankingManagementSystem.Models.API;
using BankingManagementSystem.Models.ConstraintTypes;
using BankingManagementSystem.Models.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BankingManagementSystem.WebForms.Client
{
	public partial class Transfer : System.Web.UI.Page
	{
      
        private static int clientId = -1;
        private static readonly string accountTypeAll = "All";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page.RegisterAsyncTask(new PageAsyncTask(LoadDataAsync));
            }
        }

        private async Task LoadDataAsync()
        {
            try
            {
                string token = Request.Cookies["auth_token"]?.Value;
                if (string.IsNullOrEmpty(token))
                    RedirectToLogin();

                var principal = JwtTokenManager.ValidateToken(token);
                string role = principal.FindFirst(ClaimTypes.Role)?.Value;

                if (role != UserRoles.CLIENT.ToString())
                    RedirectToLogin();

                clientId = Convert.ToInt32(principal.FindFirst("UserID")?.Value);
                ViewState["ClientId"] = clientId;

                await LoadAccounts();
            }
            catch
            {
                RedirectToLogin();
            }
        }

        private void RedirectToLogin()
        {
            Response.Redirect(Page.GetRouteUrl("ClientLoginRoute", null));
        }

        protected async Task LoadAccounts()
        {
            int clientId = Convert.ToInt32(ViewState["ClientId"]);
            var accounts = await ClientService.GetClientAccountsAsync(clientId, accountTypeAll);

            ddlFromAccountNumber.Items.Clear();

            if (accounts != null && accounts.Any())
            {
                foreach (var acc in accounts)
                {
                    ddlFromAccountNumber.Items.Add(new ListItem(acc.AccountNumber.ToString(), acc.AccountNumber.ToString()));
                }

                ListItem defaultItem = new ListItem("-- Select Account Number --", "")
                {
                    Attributes = { ["disabled"] = "disabled" },
                    Selected = true
                };
                ddlFromAccountNumber.Items.Insert(0, defaultItem);
            }
            else
            {
                ddlFromAccountNumber.Items.Add(new ListItem("No accounts found", ""));
            }
        }

        protected async void BtnTransfer_Click(object sender, EventArgs e)
        {
            if (!ValidateRequiredFields())
                return;

            var transferDto = new TransferDTO
            {
                FromAccountNumber = Convert.ToInt64(ddlFromAccountNumber.SelectedValue),
                ToAccountNumber = Convert.ToInt64(txtToAccount.Text.Trim()),
                Amount = Convert.ToDecimal(txtAmount.Text.Trim()),
                Remarks = txtRemarks.Text.Trim(),
                Password = txtPassword.Text.Trim(),
                ClientId = Convert.ToInt32(ViewState["ClientId"])
            };

            //ApiResponseMessage response = await ClientService.DepositAmountAsync(depositDto);
            var result = await ClientService.TransferAmountAsync(transferDto);
            string message = GetParsedErrorMessage(result.Message);
            if (result.IsSuccess)
            {

                string redirectUrl = ResolveClientUrl(Page.GetRouteUrl("TransferAmountRoute", null));
                string script = $@"
                setTimeout(function() {{
                    showDynamicModal({{
                        titleText: 'Amount Transferred',
                        messageText: '{HttpUtility.JavaScriptStringEncode(message)}',
                        type: 'success',                      
                        redirectUrl:'{redirectUrl}'
                    }});
                }}, 300);";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", script, true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showDanger", $"showAlert('{message}', 'danger');", true);
            }
        }
        protected string GetParsedErrorMessage(string Message)
        {
            string parsedMessage;
            if (Message.StartsWith("{") && Message.Contains("Message"))
            {
                var parsed = JsonConvert.DeserializeObject<ApiErrorMessageWrapper>(Message);
                parsedMessage = parsed?.Message;
            }
            else
            {
                parsedMessage = Message;
            }
            // Trim quotes if they exist
            if (parsedMessage.StartsWith("\"") && parsedMessage.EndsWith("\""))
            {
                parsedMessage = parsedMessage.Trim('"');
            }
            return parsedMessage;
        }
        protected bool ValidateRequiredFields()
        {
            if (string.IsNullOrWhiteSpace(ddlFromAccountNumber.SelectedValue))
            {
                ShowError("Please select an account!");
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtToAccount.Text.Trim()))
            {
                ShowError("Receiver's account number is required!");
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtAmount.Text.Trim()))
            {
                ShowError("Amount is required!");
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtPassword.Text.Trim()))
            {
                ShowError("Password is required!");
                return false;
            }
            return true;
        }

        private void ShowError(string msg)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"showAlert('{msg}', 'danger');", true);
        }
    }
    
}