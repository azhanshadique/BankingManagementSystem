using BankingManagementSystem.Helpers;
using BankingManagementSystem.Models.ConstraintTypes;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BankingManagementSystem.WebForms.Client.Transactions
{
    public partial class TransactionHistory : System.Web.UI.Page
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

                await LoadAccountNumber();

            }
            catch
            {
                RedirectToLogin();
            }
        }
        protected async Task LoadAccountNumber()
        {
            int clientId = Convert.ToInt32(ViewState["ClientId"]);
            var accounts = await ClientService.GetClientAccountsAsync(clientId, accountTypeAll);

            ddlAccountNumber.Items.Clear();

            if (accounts != null && accounts.Any())
            {
                foreach (var acc in accounts)
                {
                    ddlAccountNumber.Items.Add(new ListItem(acc.AccountNumber.ToString(), acc.AccountNumber.ToString()));
                }
                ddlAccountNumber.SelectedIndex = 0;
                await LoadTransactions(Convert.ToInt64(ddlAccountNumber.SelectedValue));
            }
            else
            {
                ddlAccountNumber.Items.Add(new ListItem("No accounts found", ""));
            }
        }

        protected async Task LoadTransactions(long accountNumber)
        {
            var transactions = await ClientService.GetTransactionHistoryAsync(accountNumber);
            gvTransactions.DataSource = transactions;
            gvTransactions.DataBind();
        }

        protected async void DdlAccountNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvTransactions.PageIndex = 0; // reset to first page

            if (long.TryParse(ddlAccountNumber.SelectedValue, out long accountNumber))
            {
                await LoadTransactions(accountNumber);
            }
        }


        protected async void GvTransactions_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvTransactions.PageIndex = e.NewPageIndex;

            if (long.TryParse(ddlAccountNumber.SelectedValue, out long accountNumber))
            {
                await LoadTransactions(accountNumber);
            }
        }


        protected void GvTransactions_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.CssClass = "table-primary";
            }
        }



        private void RedirectToLogin()
        {
            Response.Redirect(Page.GetRouteUrl("ClientLoginRoute", null));
        }






    }
}