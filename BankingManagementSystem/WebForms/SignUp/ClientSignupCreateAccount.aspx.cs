using BankingManagementSystem.BLL;
using BankingManagementSystem.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        protected void BtnSubmit_Click(object sender, EventArgs e)
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
            string jointId = TextBox_jointaccclient.Text.Trim();
            string relation = TextBox_relationship.Text.Trim();
            string username = TextBox_username.Text.Trim();
            string password = TextBox_password.Text.Trim();
            string confirmPassword = TextBox_confirmpassword.Text.Trim();
            string termsCondition = CheckBox_terms.Checked ? "Yes" : "";

            // Store all required fields in a Dictionary
            Dictionary<string, string> requiredFields = new Dictionary<string, string>
            {
                { "Full Name", fullName },
                { "Parent's Name", parentName },
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

           


            // Validate Required Fields
            if (!ValidateRequiredFields(requiredFields))
            {
                return;
            }

            // Validate if Online Account already exists or not
            if (ValidateClientExists(aadhaar, pan))
            {
                return;
            }

            // Check if joint account, validate extra fields
            //if (!ValidateJointAccount())
            //{
            //    return;
            //}

            //// Check if the username already exists
            //if (ValidateUsernameExists())
            //{
            //    Response.Write("<script>alert('Username Already Exists, Try Another Username.');</script>");
            //    return;
            //}

            // Validate for Strong Password
            if (ValidatePassword(password, confirmPassword))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"showAlert('Signed Up Successfully', 'danger');", true);

                // If all validations pass, redirect to next step
                //SignUpNewClient();
            }

            //ClientDTO client = new ClientDTO
            //{
            //    FullName = TextBox_fullname.Text.Trim(),
            //    ParentName = TextBox_parentname.Text.Trim(),
            //    DOB = Convert.ToDateTime(TextBox_dob.Text),
            //    Gender = ddl_gender.SelectedValue,
            //    Nationality = TextBox_nationality.Text.Trim(),
            //    Occupation = TextBox_occupation.Text.Trim(),
            //    AadhaarNumber = TextBox_aadhaar.Text.Trim(),
            //    PANNumber = TextBox_pan.Text.Trim(),
            //    MobileNumber = TextBox_mobilenumber.Text.Trim(),
            //    EmailId = TextBox_emailid.Text.Trim(),
            //    Address = TextBox_fulladdress.Text.Trim(),
            //    State = TextBox_state.Text.Trim(),
            //    City = TextBox_city.Text.Trim(),
            //    Pincode = TextBox_pincode.Text.Trim(),
            //    AccountType = ddl_accounttype.SelectedValue,
            //    IsJointAccount = ddlIsJointAccount.SelectedValue == "Yes",
            //    JointClientId = TextBox_jointaccclient.Text.Trim(),
            //    Relationship = TextBox_relationship.Text.Trim(),
            //    Username = TextBox_username.Text.Trim(),
            //    Password = TextBox_password.Text.Trim()
            //};

            //bool isRegistered = new ClientBLL().RegisterClient(client);

            //if (isRegistered)
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showAlert('Account created successfully!', 'success');", true);

            //}
            //else
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showAlert('Registration failed. Try again!', 'danger');", true);
            //}
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
        protected bool ValidateClientExists(string aadhaar, string pan)
        {
            if (ClientBLL.DoesClientExists(aadhaar, pan))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showAlert('Client already exists with these credentials!', 'danger');", true);
                return true;
            }
            return false;
        }
        protected bool ValidatePassword(string password, string confirmPassword)
        {
            string errorMessage;
            if (!PasswordValidatorBLL.IsStrongPassword(password, confirmPassword, out errorMessage))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"showAlert('{errorMessage}', 'danger');", true);
                return false;
            }
            return true;

        }
       
    }
}