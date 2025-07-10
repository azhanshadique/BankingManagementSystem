using BankingManagementSystem.Helpers;
using BankingManagementSystem.Models.Constants;
using BankingManagementSystem.Models.ConstraintTypes;
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
    public partial class ManageAccounts : System.Web.UI.Page
    {

        private static int clientId = -1;
        private static readonly string accountTypeAll = "All";
        private static readonly string accountTypeSavings = "Savings";
        private static readonly string accountTypeCurrent = "Current";
        private static readonly string accountTypeJoint = "Joint";
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
                {
                    RedirectToLogin();
                }

                var principal = JwtTokenManager.ValidateToken(token);
                string role = principal.FindFirst(ClaimTypes.Role)?.Value;
                if (role != UserRoles.CLIENT.ToString())
                {
                    RedirectToLogin();
                }
                clientId = Convert.ToInt32(principal.FindFirst("UserID")?.Value);
                ViewState["ClientId"] = clientId;
               

                //LoadAccountTypes();

                string type = Session["AccountType"]?.ToString() ?? accountTypeAll;
                ddlAccountType.SelectedValue = type;
                Session["AccountType"] = type;

                //ddlAccountType.SelectedValue = accountTypeAll;
                //Session["AccountType"] = accountTypeAll;

                await LoadAccounts(type);
            }
            catch
            {
                RedirectToLogin();
            }
        }
        protected async Task LoadAccounts(string type)
        {
            int clientId = Convert.ToInt32(ViewState["ClientId"]); 
            var accounts = await ClientService.GetClientAccountsAsync(clientId, type);

            if (gvAccounts.Columns.Count >= 4)
            {
                var isJointCol = gvAccounts.Columns[4] as TemplateField;
                var coHolderClientIdCol = gvAccounts.Columns[5] as BoundField;
                var coHolderNameCol = gvAccounts.Columns[6] as BoundField;

                bool showCoHolder = type == accountTypeJoint || type == accountTypeAll;

                if (isJointCol != null) isJointCol.Visible = type == accountTypeAll;
                if (coHolderClientIdCol != null) coHolderClientIdCol.Visible = showCoHolder;
                if (coHolderNameCol != null) coHolderNameCol.Visible = showCoHolder;
            }

            gvAccounts.DataSource = accounts;
            gvAccounts.DataBind();
        }

        protected async void DdlAccountType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedType = ddlAccountType.SelectedValue;
            Session["AccountType"] = selectedType;
            gvAccounts.PageIndex = 0;
            await LoadAccounts(selectedType);
        }

        //protected void GvRequests_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    if (e.CommandName == "Show")
        //    {
        //        int requestId = Convert.ToInt32(e.CommandArgument);
        //        ViewState["SelectedRequestId"] = requestId;
        //        ShowRequestDetails(requestId);
        //    }
        //}

        protected void GvAccounts_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                string status = Session["AccountType"]?.ToString()?.ToLower() ?? accountTypeAll;
                switch (status)
                {
                    case "all":
                        e.Row.CssClass = "table-secondary";
                        break;
                    case "savings":
                        e.Row.CssClass = "table-primary";
                        break;
                    case "current":
                        e.Row.CssClass = "table-success";
                        break;
                    default:
                        e.Row.CssClass = "table-warning";
                        break;
                }

                e.Row.Style["color"] = "black";
            }
        }



        protected async void GvAccounts_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvAccounts.PageIndex = e.NewPageIndex;

            // Retrieve the account type filter from session
            string selectedType = Session["AccountType"]?.ToString() ?? accountTypeAll;

            // Refresh the data on page change
            //Page.RegisterAsyncTask(new PageAsyncTask(async () => await LoadAccounts(selectedType)));
            await LoadAccounts(selectedType);

        }

        private void RedirectToLogin()
        {
            Response.Redirect(Page.GetRouteUrl("ClientLoginRoute", null));
        }



        //private void LoadAccountTypes()
        //{
        //    ddlAccountType.Items.Clear();
        //    ddlAccountType.Items.Add(new ListItem("All", "All"));
        //    ddlAccountType.Items.Add(new ListItem("Savings", "Savings"));
        //    ddlAccountType.Items.Add(new ListItem("Current", "Current"));
        //    ddlAccountType.Items.Add(new ListItem("Joint", "Joint"));
        //}

        
        protected void BtnAddAccount_Click(object sender, EventArgs e)
        {
            //LoadAccounts();
        }

        //protected void gvAccounts_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    //int rowIndex = Convert.ToInt32(e.CommandArgument);
        //    //GridViewRow row = gvAccounts.Rows[rowIndex];
        //    //string accountNumber = row.Cells[0].Text;

        //    //switch (e.CommandName)
        //    //{
        //    //    case "SetPrimary":
        //    //        _accountService.SetPrimaryAccount(accountNumber);
        //    //        ShowMessage("Primary account updated successfully.");
        //    //        break;
        //    //    case "DeleteAccount":
        //    //        _accountService.DeleteAccount(accountNumber);
        //    //        ShowMessage("Account deleted successfully.");
        //    //        break;
        //    //}

        //    //LoadAccounts();
        //}

        //protected void gvAccounts_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    //if (e.Row.RowType == DataControlRowType.DataRow)
        //    //{
        //    //    AccountDTO account = (AccountDTO)e.Row.DataItem;
        //    //    Button btnSetPrimary = (Button)e.Row.FindControl("btnSetPrimary");
        //    //    btnSetPrimary.Visible = !account.IsPrimary;
        //    //}
        //}





    }
}
