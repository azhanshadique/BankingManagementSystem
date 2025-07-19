using BankingManagementSystem.BLL;
using BankingManagementSystem.Helpers;
using BankingManagementSystem.Models;
using BankingManagementSystem.Models.API;
using BankingManagementSystem.Models.ConstraintTypes;
using System;
using System.Configuration;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;

namespace BankingManagementSystem.WebForms.Login.Client
{
	public partial class ClientLogin : System.Web.UI.Page
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
            //AuthTokenResponse result = await AuthService.AuthenticateClientAsync(request);
            AuthTokenResponse result = await AuthBLL.LoginClientAsync(request);


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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showAlert('Invalid Username or Password!', 'danger');", true);

            }


            //User user = new ClientBLL().ValidateClientLogin(client);

            //if (user != null)
            //{

            //             // Generate JWT token
            //             string token = JwtTokenManager.GenerateToken(user);

            //             // Store token in cookie
            //             HttpCookie tokenCookie = new HttpCookie("auth_token", token);
            //             tokenCookie.HttpOnly = true;
            //             tokenCookie.Expires = DateTime.Now.AddHours(1);
            //             Response.Cookies.Add(tokenCookie);

            //             Response.Redirect("~/WebForms/Home/Dashboard.aspx");
            //         }
            //else
            //{
            //             //lblMessage.Text = "Invalid email or password!";
            //             //throw new Exception("Invalid Login Credentials.");
            //         }



        }

    }
}