using BankingManagementSystem.BLL;
using BankingManagementSystem.DAL;
using BankingManagementSystem.Helpers;
using BankingManagementSystem.Models.API;
using BankingManagementSystem.Models.DTOs;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Helpers;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BankingManagementSystem.WebForms.SignUp
{
    public partial class ClientSignupCreateAccount : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected async void BtnSubmit_Click(object sender, EventArgs e)
        {
            // Extract all input values into variables
            string fullName = TextBox_fullname.Text.Trim();
            string parentName = TextBox_parentname.Text.Trim();
            string dob = TextBox_dob.Text.Trim();
            string gender = ddl_gender.SelectedValue;
            string nationality = TextBox_nationality.Text.Trim();
            string occupation = TextBox_occupation.Text.Trim();
            string aadhaar = TextBox_aadhaar.Text.Trim();
            string pan = TextBox_pan.Text.Trim();
            string mobile = TextBox_mobilenumber.Text.Trim();
            string email = TextBox_emailid.Text.Trim();
            string address = TextBox_fulladdress.Text.Trim();
            string state = TextBox_state.Text.Trim();
            string city = TextBox_city.Text.Trim();
            string pincode = TextBox_pincode.Text.Trim();
            string accountType = ddl_accounttype.SelectedValue;
            string isJoint = ddlIsJointAccount.SelectedValue;
            string jointClientId = TextBox_jointaccclient.Text.Trim();
            string username = TextBox_username.Text.Trim();
            string password = TextBox_password.Text.Trim();
            string confirmPassword = TextBox_confirmpassword.Text.Trim();
            string termsCondition = CheckBox_terms.Checked ? "Yes" : "";

            // Store all required fields in a Dictionary
            Dictionary<string, string> requiredFields = new Dictionary<string, string>
            {
                { "Full Name", fullName },
                { "Parent\\'s Name", parentName },
                { "DOB", dob },
                { "Gender", gender },
                { "Nationality", nationality },
                { "Occupation", occupation },
                { "Aadhaar", aadhaar },
                { "PAN", pan },
                { "Mobile", mobile },
                { "Email", email },
                { "State", state },
                { "City", city },
                { "Pincode", pincode },
                { "Account Type", accountType },
                { "Is Joint Account", isJoint },
                { "Username", username },
                { "Password", password },
                { "Confirm Password", confirmPassword },
                { "Agree to Terms & Condition and Privacy Policy", termsCondition }
            };


            // Validate required fields
            if (!ValidateRequiredFields(requiredFields))
                return;

            // Validate joint account required fields (if joint account)
            if (isJoint == "Yes")
            {

                Dictionary<string, string> requiredJointFields = new Dictionary<string, string>
                {
                    { "Co-holder\\'s Client ID", jointClientId },
                };

                if (!ValidateRequiredFields(requiredJointFields))
                {
                    return;

                }
            }

            // Create DTO
            ClientDTO client = new ClientDTO
            {
                FullName = fullName,
                ParentName = parentName,
                DOB = dob,
                Gender = gender,
                Nationality = nationality,
                Occupation = occupation,
                AadhaarNumber = aadhaar,
                PANNumber = pan,
                MobileNumber = mobile,
                EmailId = email,
                Address = address,
                State = state,
                City = city,
                Pincode = pincode,
                AccountType = accountType,
                IsJointAccount = isJoint == "Yes",
                JointClientId = string.IsNullOrWhiteSpace(jointClientId) ? 0 : Convert.ToInt32(jointClientId),
                Username = username,
                Password = password,
                ConfirmPassword = confirmPassword,
                CoHolderApproved = false,
                AdminApproved = false
            };

            try
            {
                ApiResponseMessage result = await RegistrationService.RegisterClientAsync(client);

                string message;

                if (result.MessageContent.StartsWith("{") && result.MessageContent.Contains("Message"))
                {
                    var parsed = JsonConvert.DeserializeObject<ApiErrorMessageWrapper>(result.MessageContent);
                    message = parsed?.Message;
                }
                else
                {
                    message = result.MessageContent;
                }
                // Trim quotes if they exist
                if (message.StartsWith("\"") && message.EndsWith("\""))
                {
                    message = message.Trim('"');
                }

                //var parsed = JsonConvert.DeserializeObject<dynamic>(result.MessageContent);
                //string message = parsed?.Message;


                if (result.MessageType == "success")
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "showSuccess", $"setTimeout(function(){{ showRegisterSuccessMessage('{message}', '{result.MessageType}'); }}, 200);", true);
                else
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "customAlert", $"showAlert('{message}', '{result.MessageType}');", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "customAlert", $"showAlert('Registration failed due to a technical error.', 'danger');", true);
            }
        }
        protected bool ValidateRequiredFields(Dictionary<string, string> requiredFields)
        {

            foreach (var field in requiredFields)
            {
                if (string.IsNullOrWhiteSpace(field.Value))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"showAlert('{field.Key} is required!', 'danger');", true);
                    return false;
                }
            }
            return true;
        }

        protected void DdlIsJoint_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlIsJointAccount.SelectedValue == "Yes")
            {
                fsJointAccount.Visible = true;
            }
            else
            {
                TextBox_jointaccclient.Text = string.Empty;
                fsJointAccount.Visible = false;

            }
        }
        protected void BtnClear_Click(object sender, EventArgs e)
        {
            // Clear textboxes
            TextBox_fullname.Text = "";
            TextBox_parentname.Text = "";
            TextBox_dob.Text = "";
            TextBox_nationality.Text = "";
            TextBox_occupation.Text = "";
            TextBox_aadhaar.Text = "";
            TextBox_pan.Text = "";
            TextBox_mobilenumber.Text = "";
            TextBox_emailid.Text = "";
            TextBox_fulladdress.Text = "";
            TextBox_state.Text = "";
            TextBox_city.Text = "";
            TextBox_pincode.Text = "";
            TextBox_username.Text = "";
            TextBox_password.Text = "";
            TextBox_confirmpassword.Text = "";
            TextBox_jointaccclient.Text = "";

            // Reset dropdowns
            ddl_gender.SelectedIndex = 0;
            ddl_accounttype.SelectedIndex = 0;
            ddlIsJointAccount.SelectedIndex = 0;

            // Hide Joint Account Section
            fsJointAccount.Visible = false;

            // Uncheck checkbox
            CheckBox_terms.Checked = false;
        }


       
    }
}