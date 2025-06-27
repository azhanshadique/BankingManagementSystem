using BankingManagementSystem.Helpers;
using BankingManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BankingManagementSystem.WebForms
{
	public partial class Dashboard : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

            if (!IsPostBack)
            {
                try
                {

                    if (Session["AlertMessage"] != null && Session["AlertType"] != null)
                    {
                        string message = Session["AlertMessage"]?.ToString();
                        string type = Session["AlertType"]?.ToString();

                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"showAlert('{message}', '{type}');", true);

                        // Clear after showing once
                        //Session.Remove("AlertMessage");
                        //Session.Remove("AlertType");
                        Session["AlertMessage"] = null;
                        Session["AlertType"] = null;
                    }
                    string token = Request.Cookies["auth_token"]?.Value;
                    if (string.IsNullOrEmpty(token))
                    {
                        DefaultDashboard.Visible = true;
                        ClientDashboard.Visible = false;
                        AdminDashboard.Visible = false;
                        return;
                    }

                    var principal = JwtTokenManager.ValidateToken(token);
                    string role = principal.FindFirst(ClaimTypes.Role)?.Value;
                    string fullName = principal.FindFirst("FullName")?.Value;

                    string firstName = fullName.Split(' ')[0];
                    
                    // Show based on role
                    if (role == Models.ConstraintTypes.UserRoles.CLIENT.ToString())
                    {
                        ClientDashboard.Visible = true;
                        DefaultDashboard.Visible = false;
                        AdminDashboard.Visible = false;

                        lblClientName.Text = $" {firstName}";
                    }
                    else if (role == Models.ConstraintTypes.UserRoles.ADMIN.ToString())
                    {
                        AdminDashboard.Visible = true;
                        ClientDashboard.Visible = false;
                        DefaultDashboard.Visible = false;

                        lblAdminName.Text = $" {firstName}";
                    }
                }
                catch
                {
                    // Token invalid or missing
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showAlert('Token Invalid!', 'danger');", true);

                }
            }
        }
	}
}