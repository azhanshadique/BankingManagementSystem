using BankingManagementSystem.BLL;
using BankingManagementSystem.Helpers;
using BankingManagementSystem.Models.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BankingManagementSystem.WebForms.Login
{
	public partial class AdminLogin : System.Web.UI.Page
	{
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    string token = Request.Cookies["auth_token"]?.Value;
                    if (!string.IsNullOrEmpty(token))
                    {
                        Response.Redirect(Page.GetRouteUrl("DashboardRoute", null));

                    }
                }
                catch
                {
                    // token invalid
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showAlert('Token Invalid!', 'danger');", true);

                }

            }
        }
        protected async void BtnLogin_Click(object sender, EventArgs e)
        {

            AuthRequestDTO request = new AuthRequestDTO()
            {
                Username = txtUsername.Text.Trim(),
                Password = txtPassword.Text.Trim(),
            };

            AuthTokenResponse result = await new AuthBLL().LoginAdminAsync(request);

            if (result != null && !string.IsNullOrEmpty(result.Token))
            {
                HttpCookie tokenCookie = new HttpCookie("auth_token", result.Token);
                tokenCookie.HttpOnly = true;
                tokenCookie.Expires = DateTime.Now.AddHours(1);
                Response.Cookies.Add(tokenCookie);

                Session["AlertMessage"] = "Logged In Successfully!";
                Session["AlertType"] = "success";
                Response.Redirect(Page.GetRouteUrl("DashboardRoute", null));
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showAlert('Invalid username or password!', 'danger');", true);

            }

        }
    }
}