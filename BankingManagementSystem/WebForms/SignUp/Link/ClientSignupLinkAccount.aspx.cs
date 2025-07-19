using BankingManagementSystem.Helpers;
using BankingManagementSystem.Models.API;
using BankingManagementSystem.Models.ConstraintTypes;
using BankingManagementSystem.Models.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BankingManagementSystem.WebForms.SignUp.Link
{
	public partial class ClientSignupCreateAccountLinkAccount : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}
        protected async void BtnSubmit_Click(object sender, EventArgs e)
        {
            // Extract all input values from the form
            Dictionary<string, string> fieldValues = ExtractFormFields();

            // Validate required fields (including joint client ID if applicable)
            if (!ValidateRequiredFields(fieldValues)) return;

            // Map extracted fields into a ClientDTO with hashed password
            LinkAccountDTO client = GetLinkAccountFromForm(fieldValues);

            try
            {
                // Call the registration service to create client
                ApiResponseMessage result = await RegistrationService.LinkClientAsync(client);
                string message = HttpUtility.JavaScriptStringEncode(GetParsedErrorMessage(result.MessageContent));

                // Show success/failure feedback with modal or alert
                if (result.MessageType == "success")
                {
                    string redirectUrl = ResolveClientUrl(Page.GetRouteUrl("DashboardRoute", null));
                    string script = $@"
                        setTimeout(function() {{
                            showDynamicModal({{
                                titleText: 'Client Registration Successful',
                                messageText: '{message}',
                                type: '{result.MessageType}',
                                redirectUrl: '{redirectUrl}'
                            }});
                        }}, 300);";

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", script, true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "customAlert", $"showAlert('{message}', '{result.MessageType}');", true);
                }
            }
            catch
            {
                // Handle unexpected server-side failure
                ScriptManager.RegisterStartupScript(this, this.GetType(), "customAlert", "showAlert('Registration failed due to a technical error.', 'danger');", true);
            }
        }

        // Extracts all values from the form controls into a dictionary 
        private Dictionary<string, string> ExtractFormFields()
        {
            return new Dictionary<string, string>
            {
                { "Account Number", txtAccountNumber.Text.Trim() },
                { "Account Type", ddlAccountType.SelectedValue },
                { "Mobile Number", txtMobileNumber.Text.Trim() },
                { "Email ID", txtEmailId.Text.Trim() },
                { "Client ID", txtClientId.Text.Trim() },
                { "Is Joint Account", ddlIsJointAccount.SelectedValue },
                { "Co-holder Client ID", txtCoholderClientId.Text.Trim() },
                { "Co-holder Mobile Number", txtCoholderMobile.Text.Trim() },
                { "Username", txtUsername.Text.Trim().ToLower() },
                { "Password", txtPassword.Text.Trim() },
                { "Confirm Password", txtConfirmPassword.Text.Trim() },
                { "Agree to Terms", chkBoxTerms.Checked ? "Yes" : "No" }
            };
        }

        // Converts extracted values to a LinkAccountDTO and hashes the password
        private LinkAccountDTO GetLinkAccountFromForm(Dictionary<string, string> values)
        {
            return new LinkAccountDTO
            {
                AccountNumber = Convert.ToInt64(values["Account Number"]),
                AccountType = values["Account Type"],
                MobileNumber = values["Mobile Number"],
                EmailId = values["Email ID"],
                ClientId = Convert.ToInt32(values["Client ID"]),
                IsJointAccount = values["Is Joint Account"] == "Yes",
                JointClientId = string.IsNullOrWhiteSpace(values["Co-holder Client ID"]) ? 0 : Convert.ToInt32(values["Co-holder Client ID"]),
                JointClientMobileNo = values["Co-holder Mobile Number"],
                Username = values["Username"],
                Password = values["Password"],
                ConfirmPassword = values["Confirm Password"]
           
            };
        }


        // Attempts to parse and clean the response message
        protected string GetParsedErrorMessage(string message)
        {
            if (message.StartsWith("{") && message.Contains("Message"))
            {
                var parsed = JsonConvert.DeserializeObject<ApiErrorMessageWrapper>(message);
                message = parsed?.Message;
            }

            if (message.StartsWith("\"") && message.EndsWith("\""))
            {
                message = message.Trim('"');
            }

            return message;
        }

        // Checks that all required fields have non-empty values
        protected bool ValidateRequiredFields(Dictionary<string, string> fields)
        {
            foreach (var field in fields)
            {
                if (string.IsNullOrWhiteSpace(field.Value))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"showAlert('{field.Key} is required!', 'danger');", true);
                    return false;
                }
            }
            return true;
        }

        // Toggles joint account field visibility based on dropdown selection
        protected void DdlIsJoint_SelectedIndexChanged(object sender, EventArgs e)
        {
            fsJointAccount.Visible = ddlIsJointAccount.SelectedValue == "Yes";
            if (!fsJointAccount.Visible)
            {
                txtCoholderClientId.Text = string.Empty;
                txtCoholderMobile.Text = string.Empty;
            }
        }

        // Resets all fields to default state
        protected void BtnClear_Click(object sender, EventArgs e)
        {
            txtAccountNumber.Text = string.Empty;
            ddlAccountType.SelectedIndex = 0;
            txtMobileNumber.Text = string.Empty;
            txtEmailId.Text = string.Empty;
            txtClientId.Text = string.Empty;

            ddlIsJointAccount.SelectedIndex = 0;

            txtCoholderClientId.Text = string.Empty;
            txtCoholderMobile.Text = string.Empty;

            txtUsername.Text = string.Empty;
            txtPassword.Text = string.Empty;
            txtConfirmPassword.Text = string.Empty;

            chkBoxTerms.Checked = false;

            fsJointAccount.Visible = false;
        }

    }
}