using BankingManagementSystem.Helpers;
using BankingManagementSystem.Models;
using BankingManagementSystem.Models.ConstraintTypes;
using System;
using System.Configuration;
using System.Security.Claims;
using System.Web;
using System.Web.UI;

namespace BankingManagementSystem.WebForms
{
	public partial class MasterPage : System.Web.UI.MasterPage
	{
        //public PageRoute Routes { get; set; }
        protected void Page_Load(object sender, EventArgs e)
		{
            // clear cache - always re-fetch from server.
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();

            if (!IsPostBack)
            {
                this.DataBind();
                User user = null;

                try
                {
                    //Routes = new PageRoute();

                    string token = Request.Cookies["auth_token"]?.Value;
                    if (!string.IsNullOrEmpty(token))
                    {
                        var principal = JwtTokenManager.ValidateToken(token);
                        user = new User()
                        {
                            UserID = Convert.ToInt32(principal.FindFirst("UserID")?.Value),
                            Username = principal.FindFirst(ClaimTypes.Name)?.Value,
                            FullName = principal.FindFirst("FullName")?.Value,
                            Role = principal.FindFirst(ClaimTypes.Role)?.Value == UserRoles.CLIENT.ToString() ? UserRoles.CLIENT : UserRoles.ADMIN

                        }; 
                    }
                }
                catch
                {
                    // token invalid
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showAlert('Token Invalid!', 'danger');", true);

                }

                ActivateDashboard(user); // role may be null
                Session["AlertMessage"] = null;
                Session["AlertType"] = null;
            }
        }
        private void ActivateDashboard(User user)
        {
            SetCommonVisibility(user);

            // Hide all optional labels first
            lblClientDetails.Visible = false;
            lblClientId.Visible = false;
            lblAdminDetails.Visible = false;
            lblAdminId.Visible = false;
            lbtnClientManagement.Visible = false;
            lbtnClientRequests.Visible = false;
            lbtnMyRequests.Visible = false;
            lbtnViewProfile.Visible = false;
            lbtnManageAccounts.Visible = false;
            lbtnDeposit.Visible = false;
            lbtnWithdraw.Visible = false;
            lbtnTransferMoney.Visible = false;
            lbtnTransactionHistory.Visible = false;

            if(user == null)
            {
                return;
            }
            else if (user.Role == UserRoles.CLIENT)
            {
                lblClientDetails.Visible = true;
                lblClientId.Visible = true;

                lbtnMyRequests.Visible = true;
                lbtnViewProfile.Visible = true;
                lbtnManageAccounts.Visible = true;
                lbtnDeposit.Visible = true;
                lbtnWithdraw.Visible = true;
                lbtnTransferMoney.Visible = true;
                lbtnTransactionHistory.Visible = true;

                lblClientId.Text = $"Client ID: {user.UserID}";
               
            }
            else if (user.Role == UserRoles.ADMIN)
            {
                lblAdminDetails.Visible = true;
                lblAdminId.Visible = true;

                lbtnClientManagement.Visible = true;
                lbtnClientRequests.Visible = true;

                lblAdminId.Text = $"Admin ID: {user.UserID}";
              
            }
            string firstName = user.FullName.Split(' ')[0];
            lblUserName.Text = $"{firstName} ";
        }
        private void SetCommonVisibility(User user)
        {
            bool loggedIn = false;
            if(user != null) { loggedIn = true; }

            hlAdminLogin.Visible = !loggedIn;
            hlClientLogin.Visible = !loggedIn;
            ddlSignup.Visible = !loggedIn;

            iconUser.Visible = loggedIn;
            lblUserName.Visible = loggedIn;
            lbtnLogout.Visible = loggedIn;

            sidebar.Visible = loggedIn;
            toggleSidebar.Visible = loggedIn;

           
        }

        protected void BtnLogout_Click(object sender, EventArgs e)
        {
            if (Request.Cookies["auth_token"] != null)
            {
                HttpCookie cookie = new HttpCookie("auth_token");
                cookie.Expires = DateTime.Now.AddDays(-1);
                cookie.Value = null;
                Response.Cookies.Add(cookie);
            }
            Session["AlertMessage"] = "Logged Out Successfully!";
            Session["AlertType"] = "danger";
            Response.Redirect(Page.GetRouteUrl("DashboardRoute", null));
        }
        protected void BtnHomeDashboard_Click(object sender, EventArgs e)
        {
            Response.Redirect(Page.GetRouteUrl("DashboardRoute", null));
        }
        protected void BtnAbout_Click(object sender, EventArgs e)
        {
            Response.Redirect(Page.GetRouteUrl("AboutRoute", null));
        }
        protected void BtnAdminLogin_Click(object sender, EventArgs e)
        {

            Response.Redirect(Page.GetRouteUrl("AdminLoginRoute", null));
        }
        protected void BtnClientLogin_Click(object sender, EventArgs e)
        {
            Response.Redirect(Page.GetRouteUrl("ClientLoginRoute", null));
        }
        protected void BtnMyRequests_Click(object sender, EventArgs e)
        {
            Response.Redirect(Page.GetRouteUrl("ClientPendingRequestRoute", null));
        }
        protected void BtnViewProfile_Click(object sender, EventArgs e)
        {
            Response.Redirect(Page.GetRouteUrl("ClientProfileRoute", null));
        }
        protected void BtnManageAccounts_Click(object sender, EventArgs e)
        {
            Response.Redirect(Page.GetRouteUrl("ClientAccountsRoute", null));
        }
        protected void BtnDeposit_Click(object sender, EventArgs e)
        {
            Response.Redirect(Page.GetRouteUrl("DepositAmountRoute", null));
        }
        protected void BtnWithdraw_Click(object sender, EventArgs e)
        {
            Response.Redirect(Page.GetRouteUrl("WithdrawAmountRoute", null));
        }
         protected void BtnTransferMoney_Click(object sender, EventArgs e)
        {
            Response.Redirect(Page.GetRouteUrl("TransferAmountRoute", null));
        }
         protected void BtnTransactionHistory_Click(object sender, EventArgs e)
        {
            Response.Redirect(Page.GetRouteUrl("TransactionHistoryRoute", null));
        }


        protected void BtnClientManagement_Click(object sender, EventArgs e)
        {
            Response.Redirect(Page.GetRouteUrl("ClientLoginRoute", null));
        }
        protected void BtnClientRequests_Click(object sender, EventArgs e)
        {
            Response.Redirect(Page.GetRouteUrl("AdminClientRequestRedirect", null));
        }
        //protected void BtnOk_Click(object sender, EventArgs e)
        //{
        //    Response.Redirect(Page.GetRouteUrl("DashboardRoute", null));
        //}
        //protected void BtnOkApproveReject_Click(object sender, EventArgs e)
        //{
        //    Response.Redirect(Page.GetRouteUrl("AdminClientRequestRedirect", null));
        //}
        //protected void BtnOkApproveRejectByClient_Click(object sender, EventArgs e)
        //{
        //    Response.Redirect(Page.GetRouteUrl("ClientPendingRequestRoute", null));
        //}
        //protected void BtnOkDeleteByClient_Click(object sender, EventArgs e)
        //{
        //    Response.Redirect(Page.GetRouteUrl("PublicRequestRoute", null));
        //}
    }
}