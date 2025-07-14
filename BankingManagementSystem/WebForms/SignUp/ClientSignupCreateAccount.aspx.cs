using BankingManagementSystem.Helpers;
using BankingManagementSystem.Models.API;
using BankingManagementSystem.Models.ConstraintTypes;
using BankingManagementSystem.Models.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;

namespace BankingManagementSystem.WebForms.SignUp
{
    public partial class ClientSignupCreateAccount : System.Web.UI.Page
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
            ClientDTO client = GetClientFromForm(fieldValues);

            try
            {
                // Call the registration service to create client
                ApiResponseMessage result = await RegistrationService.RegisterClientAsync(client);
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
                { "Full Name", TextBox_fullname.Text.Trim() },
                { "Parent's Name", TextBox_parentname.Text.Trim() },
                { "DOB", TextBox_dob.Text.Trim() },
                { "Gender", ddl_gender.SelectedValue },
                { "Nationality", TextBox_nationality.Text.Trim() },
                { "Occupation", TextBox_occupation.Text.Trim() },
                { "Aadhaar", TextBox_aadhaar.Text.Trim() },
                { "PAN", TextBox_pan.Text.Trim() },
                { "Mobile", TextBox_mobilenumber.Text.Trim() },
                { "Email", TextBox_emailid.Text.Trim() },
                { "Address", TextBox_fulladdress.Text.Trim() },
                { "State", TextBox_state.Text.Trim() },
                { "City", TextBox_city.Text.Trim() },
                { "Pincode", TextBox_pincode.Text.Trim() },
                { "Account Type", ddl_accounttype.SelectedValue },
                { "Is Joint Account", ddlIsJointAccount.SelectedValue },
                { "Joint Client ID", TextBox_jointaccclient.Text.Trim() },
                { "Username", TextBox_username.Text.Trim().ToLower() },
                { "Password", TextBox_password.Text.Trim() },
                { "Confirm Password", TextBox_confirmpassword.Text.Trim() },
                { "Agree to Terms", CheckBox_terms.Checked ? "Yes" : "" }
            };
        }

        // Converts extracted values to a ClientDTO and hashes the password
        private ClientDTO GetClientFromForm(Dictionary<string, string> values)
        {
            DateTime.TryParse(values["DOB"], out DateTime dob);
            //string hashedPassword = PasswordHasher.HashPassword(values["Password"]);

            return new ClientDTO
            {
                FullName = values["Full Name"],
                ParentName = values["Parent's Name"],
                DOB = dob.Date,
                Gender = values["Gender"],
                Nationality = values["Nationality"],
                Occupation = values["Occupation"],
                AadhaarNumber = values["Aadhaar"],
                PANNumber = values["PAN"],
                MobileNumber = values["Mobile"],
                EmailId = values["Email"],
                Address = values["Address"],
                State = values["State"],
                City = values["City"],
                Pincode = values["Pincode"],
                AccountType = values["Account Type"],
                IsJointAccount = values["Is Joint Account"] == "Yes",
                JointClientId = string.IsNullOrWhiteSpace(values["Joint Client ID"]) ? 0 : Convert.ToInt32(values["Joint Client ID"]),
                Username = values["Username"],
                Password = values["Password"],
                ConfirmPassword = values["Confirm Password"],
                CoHolderApproved = RequestStatus.Awaiting.ToString(),
                AdminApproved = RequestStatus.Awaiting.ToString()
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
                TextBox_jointaccclient.Text = string.Empty;
        }

        // Resets all fields to default state
        protected void BtnClear_Click(object sender, EventArgs e)
        {
            TextBox_fullname.Text = string.Empty;
            TextBox_parentname.Text = string.Empty;
            TextBox_dob.Text = string.Empty;
            TextBox_nationality.Text = string.Empty;
            TextBox_occupation.Text = string.Empty;
            TextBox_aadhaar.Text = string.Empty;
            TextBox_pan.Text = string.Empty;
            TextBox_mobilenumber.Text = string.Empty;
            TextBox_emailid.Text = string.Empty;
            TextBox_fulladdress.Text = string.Empty;
            TextBox_state.Text = string.Empty;
            TextBox_city.Text = string.Empty;
            TextBox_pincode.Text = string.Empty;
            TextBox_username.Text = string.Empty;
            TextBox_password.Text = string.Empty;
            TextBox_confirmpassword.Text = string.Empty;
            TextBox_jointaccclient.Text = string.Empty;

            ddl_gender.SelectedIndex = 0;
            ddl_accounttype.SelectedIndex = 0;
            ddlIsJointAccount.SelectedIndex = 0;

            fsJointAccount.Visible = false;
            CheckBox_terms.Checked = false;
        }
    }
}
