using BankingManagementSystem.Helpers;
using BankingManagementSystem.Models.API;
using BankingManagementSystem.Models.ConstraintTypes;
using BankingManagementSystem.Models.DTOs;
using Newtonsoft.Json;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Results;
using System.Web.Services.Description;
using System.Web.UI;

namespace BankingManagementSystem.WebForms.Client
{
    public partial class ClientProfile : Page
    {
        private int clientId;

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
                    return;
                }

                var principal = JwtTokenManager.ValidateToken(token);
                string role = principal.FindFirst(ClaimTypes.Role)?.Value;
                clientId = Convert.ToInt32(principal.FindFirst("UserID")?.Value);
                ViewState["ClientId"] = clientId;


                if (role != UserRoles.CLIENT.ToString())
                {
                    RedirectToLogin();
                    return;
                }

                await LoadClientProfile(clientId);
            }
            catch
            {
                RedirectToLogin();
            }
        }

        private async Task LoadClientProfile(int clientId)
        {
            ClientDTO client = await ClientService.GetClientByIdAsync(clientId);
            if (client == null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "notFound", "showAlert('Client profile not found!', 'danger');", true);
                return;
            }

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
        protected async void BtnCancel_Click(object sender, EventArgs e)
        {
            SetClientFormReadOnly(true);
            ToggleButtons(false);
            await ReloadUI(true);
        }
        protected void ToggleButtons(bool toggle)
        {
            btnEdit.Visible = !toggle;
            btnUpdate.Visible = toggle;
            btnCancel.Visible = toggle;
        }
        private void SetClientFormReadOnly(bool isReadOnly)
        {
            //txtFullName.ReadOnly = isReadOnly;
            //txtParentName.ReadOnly = isReadOnly;
            //txtDOB.ReadOnly = isReadOnly;
            //ddlGender.Enabled = !isReadOnly;
            txtNationality.ReadOnly = isReadOnly;
            txtOccupation.ReadOnly = isReadOnly;
            //txtAadhaar.ReadOnly = isReadOnly;
            //txtPan.ReadOnly = isReadOnly;
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
            int clientId = (int)(ViewState["ClientId"] ?? 0); 

            ClientDTO updatedClient = GetClient();

            try
            {
                var (requestId, errorMessage) = await ClientService.SendUpdateProfileDetailsRequestAsync(clientId, RequestType.UpdateProfileDetails.ToString(), updatedClient);

                if (requestId != 0)
                {
                    string message = $"Update Profile Details request sent successfully. Request ID: #{requestId}.";

                    string redirectUrl = ResolveClientUrl(Page.GetRouteUrl("ClientProfileRoute", null));
                    string script = $@"
                            setTimeout(function() {{
                                showDynamicModal({{
                                    titleText: 'Update Request Sent',
                                    messageText: '{HttpUtility.JavaScriptStringEncode(message)}',
                                    type: 'success',                      
                                    redirectUrl:'{redirectUrl}'
                                }});
                            }}, 300);";

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", script, true);

                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "successMsg", $"showAlert('Update request sent successfully. Request ID: #{requestId}', 'success');", true);
                }
                else
                {
                    string message = GetParsedErrorMessage(errorMessage);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "failMsg", $"showAlert('{HttpUtility.JavaScriptStringEncode(message)}', 'danger');", true);
                }
                SetClientFormReadOnly(true);
                ToggleButtons(false);

            }
            catch (Exception)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "failMsg",
                    "showAlert('Update failed due to a technical error.', 'danger');", true);
            }

            await ReloadUI(true);


        }
        private async Task ReloadUI(bool success)
        {
            if (success)
            {
                int clientId = (int)(ViewState["ClientId"] ?? 0);
                await LoadClientProfile(clientId);
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
            int clientId = (int)(ViewState["ClientId"] ?? 0);
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
        private void RedirectToLogin()
        {
            Response.Redirect(Page.GetRouteUrl("ClientLoginRoute", null));
        }
    }
}
