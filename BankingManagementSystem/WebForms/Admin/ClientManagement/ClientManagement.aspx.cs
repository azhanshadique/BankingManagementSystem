using BankingManagementSystem.Helpers;
using BankingManagementSystem.Models.API;
using BankingManagementSystem.Models.Constants;
using BankingManagementSystem.Models.ConstraintTypes;
using BankingManagementSystem.Models.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BankingManagementSystem.WebForms.Admin.ClientManagement
{
    public partial class ClientManagement : Page
    {
        private static int adminId = -1;
        private static readonly string accountTypeAll = "All";
        private static readonly string accountTypeJoint = "Joint";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    string token = Request.Cookies["auth_token"]?.Value;
                    if (string.IsNullOrEmpty(token))
                        RedirectToLogin();

                    var principal = JwtTokenManager.ValidateToken(token);
                    string role = principal.FindFirst(ClaimTypes.Role)?.Value;
                    //adminId = Convert.ToInt32(principal.FindFirst("UserID")?.Value);
                    //ViewState["AdminId"] = adminId;

                    if (role != UserRoles.ADMIN.ToString())
                        RedirectToLogin();

                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showAlert('Token Invalid!', 'danger');", true);
                    RedirectToLogin();
                }
            }
        }

        protected void BtnSearch_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtSearchClientId.Text.Trim(), out int clientId))
            {
                ViewState["SelectedClientId"] = clientId;
                LoadClientProfileDetails(clientId);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"showAlert('Invalid Client Id.', 'danger');", true);
            }
        }
        private async void LoadClientProfileDetails(int clientId)
        {

            ClientDTO client = await ClientService.GetClientByIdAsync(clientId);
            if (client == null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "notFound", $"showAlert('Client not found!', 'danger');", true);
                pnlClientProfileDetails.Visible = false;
                return;
            }
            pnlClientProfileDetails.Visible = true;

            // Personal 
            txtFullName.Text = client.FullName;
            txtParentName.Text = client.ParentName;
            txtDOB.Text = client.DOB?.ToString("yyyy-MM-dd");
            ddlGender.SelectedValue = client.Gender;
            txtNationality.Text = client.Nationality;
            txtOccupation.Text = client.Occupation;
            txtAadhaar.Text = client.AadhaarNumber;
            txtPan.Text = client.PANNumber;

            // Contact
            txtMobile.Text = client.MobileNumber;
            txtEmail.Text = client.EmailId;
            txtAddress.Text = client.Address;
            txtCity.Text = client.City;
            txtState.Text = client.State;
            txtPincode.Text = client.Pincode;

            // Others
            txtUsername.Text = client.Username;
            txtClientId.Text = client.ClientId.ToString();
        }
        protected void BtnEdit_Click(object sender, EventArgs e)
        {
            SetClientFormReadOnly(false);
            ToggleButtons(true);

        }
        protected void ToggleButtons(bool toggle)
        {
            btnEdit.Visible = !toggle;
            btnUpdate.Visible = toggle;
            btnCancel.Visible = toggle;
        }
        private void SetClientFormReadOnly(bool isReadOnly)
        {
            txtFullName.ReadOnly = isReadOnly;
            txtParentName.ReadOnly = isReadOnly;
            txtDOB.ReadOnly = isReadOnly;
            ddlGender.Enabled = !isReadOnly;
            txtNationality.ReadOnly = isReadOnly;
            txtOccupation.ReadOnly = isReadOnly;
            txtAadhaar.ReadOnly = isReadOnly;
            txtPan.ReadOnly = isReadOnly;
            txtEmail.ReadOnly = isReadOnly;
            txtMobile.ReadOnly = isReadOnly;
            txtAddress.ReadOnly = isReadOnly;
            txtCity.ReadOnly = isReadOnly;
            txtState.ReadOnly = isReadOnly;
            txtPincode.ReadOnly = isReadOnly;
            //txtUsername.ReadOnly = isReadOnly;
            //txtClientId.ReadOnly = isReadOnly;
        }
        
       
        protected async void BtnUpdate_Click(object sender, EventArgs e)
        {
            int clientId = (int)(ViewState["SelectedClientId"] ?? 0);

            ClientDTO updatedClient = GetClient();

            try
            {
                bool result = await AdminService.UpdateClientDetailsAsync(updatedClient);
                if (result)
                {
                    string message = $"Client {clientId} profile details updated successfully.";

                    //string redirectUrl = ResolveClientUrl(Page.GetRouteUrl("ClientManagementRoute", null));
                    //string script = $@"
                    //        setTimeout(function() {{
                    //            showDynamicModal({{
                    //                titleText: 'Client Profile Updated',
                    //                messageText: '{HttpUtility.JavaScriptStringEncode(message)}',
                    //                type: 'success',                      
                    //                redirectUrl:'#'
                    //            }});
                    //        }}, 300);";

                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", script, true);

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "showSuccess", $"showAlert('{message}', 'success');", true);
                }
                else
                {
              
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "failMsg", $"showAlert('Profile Update Failed', 'danger');", true);
                }
                SetClientFormReadOnly(true);
                ToggleButtons(false);

            }
            catch (Exception)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "failMsg",
                    "showAlert('Update failed due to a technical error.', 'danger');", true);
            }

            ReloadUI(true);


        }
        private void ReloadUI(bool success)
        {
            if (success)
            {
                int clientId = (int)(ViewState["SelectedClientId"] ?? 0);
                LoadClientProfileDetails(clientId);
            }
        }
        protected string GetParsedErrorMessage(string errorMessage)
        {
            string message;
            if (errorMessage.StartsWith("{") && errorMessage.Contains("Message"))
            {
                var parsed = JsonConvert.DeserializeObject<ApiErrorMessageWrapper>(errorMessage);
                message = parsed?.Message;
            }
            else
            {
                message = errorMessage;
            }
            // Trim quotes if they exist
            if (message.StartsWith("\"") && message.EndsWith("\""))
            {
                message = message.Trim('"');
            }
            return message;
        }
        protected ClientDTO GetClient()
        {
            int clientId = (int)(ViewState["SelectedClientId"] ?? 0);
            DateTime dob = DateTime.MinValue;
            bool isDobValid = DateTime.TryParse(txtDOB.Text, out dob);

            return new ClientDTO
            {
                FullName = txtFullName.Text.Trim(),
                ParentName = txtParentName.Text.Trim(),
                DOB = isDobValid ? dob : (DateTime?)null,
                Gender = ddlGender.SelectedValue,
                Nationality = txtNationality.Text.Trim(),
                Occupation = txtOccupation.Text.Trim(),
                AadhaarNumber = txtAadhaar.Text.Trim(),
                PANNumber = txtPan.Text.Trim(),

                EmailId = txtEmail.Text.Trim(),
                MobileNumber = txtMobile.Text.Trim(),
                Address = txtAddress.Text.Trim(),
                City = txtCity.Text.Trim(),
                State = txtState.Text.Trim(),
                Pincode = txtPincode.Text.Trim(),

                Username = txtUsername.Text.Trim(),
                AdminApproved = RequestStatus.Awaiting.ToString(),
                ClientId = clientId,
            };
        }
        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            SetClientFormReadOnly(true);
            ToggleButtons(false);
            ReloadUI(true);
        }


        protected async void BtnShowAccounts_Click(object sender, EventArgs e)
        {
            pnlClientAccountDetails.Visible = true;
            pnlClientProfileDetails.Visible = false;
            //LoadAccountTypes();
            string type = Session["AccountType"]?.ToString() ?? accountTypeAll;
            ddlAccountType.SelectedValue = type;
            Session["AccountType"] = type;
            await LoadAccounts(type);
        } 
        
        protected void BtnShowProfile_Click(object sender, EventArgs e)
        {
            pnlClientAccountDetails.Visible = false;
            pnlClientProfileDetails.Visible = true;

        }
       
        protected async Task LoadAccounts(string type)
        {
            int clientId = Convert.ToInt32(ViewState["SelectedClientId"]);
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
            string selectedType = Session["AccountType"]?.ToString() ?? accountTypeAll;
            await LoadAccounts(selectedType);

        }

        protected async void BtnDeleteAccount_Click(object sender, EventArgs e)
        {
            
            long accountNumber;
            if (long.TryParse(hfAccountNumberToDelete.Value, out accountNumber))
            {
                bool isDeleted = await AdminService.DeleteAccountAsync(accountNumber);

                if (isDeleted)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "deleteSuccess", "showAlert('Account deleted successfully.', 'success');", true);
                    await LoadAccounts(Session["AccountType"]?.ToString() ?? "All");
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "deleteError", "showAlert('Failed to delete account.', 'danger');", true);
                }
            }
        }



        private void RedirectToLogin()
        {
            Response.Redirect(Page.GetRouteUrl("AdminLoginRoute", null));
        }
    }
}