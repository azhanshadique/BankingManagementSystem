using System;
using System.Web.UI;
using Newtonsoft.Json;
using BankingManagementSystem.Models.DTOs;
using BankingManagementSystem.DAL;
using BankingManagementSystem.Models.ConstraintTypes;
using BankingManagementSystem.Helpers;
using BankingManagementSystem.Models.API;
using System.Threading.Tasks;
using BankingManagementSystem.BLL;
using System.Web.UI.WebControls;
using System.Web;

namespace BankingManagementSystem.WebForms.Home.Requests
{
    public partial class PublicRequest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        protected void BtnSearch_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtRequestId.Text.Trim(), out int requestId))
            {
                ViewState["SelectedRequestId"] = requestId;
                ShowRequestDetails(requestId);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"showAlert('Invalid Request Id.', 'danger');", true);
            }
        }
        protected async void ShowRequestDetails(int requestId)
        {
            var (request, errorMessage) = await RequestsService.GetPublicRegisterRequestByIdAsync(requestId);

            // Case: API error or request not found
            if (request == null)
            {
                pnlRequestDetails.Visible = false;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"showAlert('{errorMessage}', 'danger');", true);
                return;
            }

            // Case: Payload is null or invalid
            var client = JsonConvert.DeserializeObject<ClientDTO>(request.Payload);
            if (client == null)
            {
                pnlRequestDetails.Visible = false;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showAlert('Invalid client data.', 'danger');", true);
                return;
            }

            // Case: Wrong request type (shouldn’t happen if API filters properly)
            //if (request.RequestType != RequestType.CreateNewRegistration.ToString())
            //{
            //    pnlRequestDetails.Visible = false;
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showAlert('Request is not of type Create New Registration.', 'danger');", true);
            //    return;
            //}


            // Valid Request
            pnlRequestDetails.Visible = true;
            lblRequestType.Text = "Create New Registration";
            //lblRequestStatus.Text = request.Status;
            SetStatusBadge(lblRequestStatus, request.Status?.Trim());


            lblRequestId.Text = $"#{requestId}";

            bool isPending = request.Status == RequestStatus.Pending.ToString(); 
            SetButtonState(client, isPending);
            SetApprovalLabels(client, isPending);

            if (!isPending)
            {
                switch (request.Status)
                {
                    case nameof(RequestStatus.Approved):
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showAlert('Request is approved. Please login to view more details.', 'success');", true);
                        break;

                    case nameof(RequestStatus.Rejected):
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showAlert('Request is rejected by the admin.', 'danger');", true);
                        break;

                    default:
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showAlert('Request is no longer pending. Please login to view more details.', 'info');", true);
                        break;
                }
            }

            PopulateClientForm(client);
        }

        private void SetApprovalLabels(ClientDTO client, bool isPending)
        {
            // Co-holder
            lblCoHolderApprovalHeading.Visible = client.IsJointAccount && isPending;
            lblCoHolderApproval.Visible = client.IsJointAccount && isPending;

            if (client.IsJointAccount)
            {
                string coHolderStatus = (client.CoHolderApproved ?? "").Trim();
                SetStatusBadge(lblCoHolderApproval, coHolderStatus);
            }

            


            // Admin
            lblAdminApprovalHeading.Visible = isPending;
            lblAdminApproval.Visible = isPending;

            string adminStatus = (client.AdminApproved ?? "").Trim();
            SetStatusBadge(lblAdminApproval, adminStatus);
          
        }


        private void SetStatusBadge(Label label, string status)
        {
            label.Text = status;

            switch (status)
            {
                case "Approved":
                    label.CssClass = "badge bg-success fw-semibold text-white fs-6 px-6 py-2";
                    break;
                case "Rejected":
                    label.CssClass = "badge bg-danger fw-semibold text-white fs-6 px-6 py-2";
                    break;
                case "Pending":
                    label.CssClass = "badge bg-primary fw-semibold text-white fs-6 px-6 py-2";
                    break;
                case "Awaiting":
                    label.CssClass = "badge bg-warning text-dark fw-semibold fs-6 px-6 py-2";
                    break;
                default:
                    label.CssClass = "badge bg-secondary text-white fw-semibold fs-6 px-6 py-2";
                    break;
            }
        }


        private void SetButtonState(ClientDTO client, bool isPending)
        {
            //btnUpdate.Enabled = enable;
            //btnEdit.Enabled = enable;
            //btnDelete.Enabled = enable;
            bool coApproved = client.CoHolderApproved == RequestStatus.Awaiting.ToString();
            pnlButtons.Visible = coApproved && isPending;    
        }
        private void PopulateClientForm(ClientDTO client)
        {
            txtFullName.Text = client.FullName;
            txtParentName.Text = client.ParentName;
            //txtDOB.Text = client.DOB.ToString();
            txtDOB.Text = client.DOB?.ToString("yyyy-MM-dd");
            ddlGender.SelectedValue = client.Gender;
            txtNationality.Text = client.Nationality;
            txtOccupation.Text = client.Occupation;
            txtAadhaar.Text = client.AadhaarNumber;
            txtPan.Text = client.PANNumber;
            txtEmail.Text = client.EmailId;
            txtMobile.Text = client.MobileNumber;
            txtAddress.Text = client.Address;
            txtCity.Text = client.City;
            txtState.Text = client.State;
            txtPincode.Text = client.Pincode;
            ddlAccountType.SelectedValue = client.AccountType;
            ddlIsJointAccount.SelectedValue = client.IsJointAccount ? "Yes" : "No";
            fsJointAccount.Visible = client.IsJointAccount;
            txtJointClientId.Text = client.JointClientId != 0 ? client.JointClientId.ToString() : "";
            txtUsername.Text = client.Username;
            txtPassword.Attributes["value"] = client.Password;
            txtConfirmPassword.Attributes["value"] = client.Password;
        }


        protected void ToggleButtons(bool toggle)
        {
            btnEdit.Visible = !toggle;
            btnUpdate.Visible = toggle;
            btnCancel.Visible = toggle;
        }
        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            int requestId = (int)ViewState["SelectedRequestId"];
            SetClientFormReadOnly(true);
            ToggleButtons(false);
            ShowRequestDetails(requestId);
        }
        protected void BtnEdit_Click(object sender, EventArgs e)
        {
            SetClientFormReadOnly(false);
            ToggleButtons(true);
     
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
            //ddlAccountType.Enabled = !isReadOnly;
            //ddlIsJointAccount.Enabled = !isReadOnly;
            //txtJointClientId.ReadOnly = isReadOnly;
            txtUsername.ReadOnly = isReadOnly;
            //txtPassword.ReadOnly = isReadOnly;
            //txtConfirmPassword.ReadOnly = isReadOnly;
        }
        protected ClientDTO GetClient()
        {
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
                AccountType = ddlAccountType.SelectedValue,
                IsJointAccount = ddlIsJointAccount.SelectedValue == "Yes",
                JointClientId = string.IsNullOrWhiteSpace(txtJointClientId.Text.Trim()) ? 0 : Convert.ToInt32(txtJointClientId.Text.Trim()),
                Username = txtUsername.Text.Trim(),
                Password = txtPassword.Text,
                ConfirmPassword = txtConfirmPassword.Text,
                CoHolderApproved = RequestStatus.Awaiting.ToString(),
                AdminApproved = RequestStatus.Awaiting.ToString()
            };
        }

        protected async void BtnUpdate_Click(object sender, EventArgs e)
        {
            int requestId = (int)ViewState["SelectedRequestId"];

            ClientDTO updatedClient = GetClient();

            try
            {
                ApiResponseMessage result = await RequestsService.UpdatePublicRegisterRequestAsync(requestId, updatedClient);

                string message = result.MessageContent;


                if (result.MessageContent.StartsWith("{") && result.MessageContent.Contains("Message"))
                {
                    var parsed = JsonConvert.DeserializeObject<ApiErrorMessageWrapper>(result.MessageContent);
                    message = parsed?.Message;
                }
                if (message.StartsWith("\"") && message.EndsWith("\""))
                {
                    message = message.Trim('"');
                }
                string messageContent = result.MessageType == "success" ? "Request updated successfully." : message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "customAlert", $"showAlert('{messageContent}', '{result.MessageType}');", true);
                SetClientFormReadOnly(true);
                ToggleButtons(false);
                await ReloadUI(true);

            }
            catch
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "customAlert", "showAlert('Update failed due to a technical error.', 'danger');", true);
            }
        }

        //protected async void BtnReject_Click(object sender, EventArgs e)
        //{
        //    int requestId = (int)ViewState["SelectedRequestId"];
        //    bool result = await RequestsService.RejectRequestAsync(requestId, requestId);
        //    if (result)
        //    {
        //        string message = $"Client Request ID: #{requestId} Deleted Successfully.";

        //        string redirectUrl = ResolveClientUrl(Page.GetRouteUrl("PublicRequestRoute", null));
        //        string script = $@"
        //        setTimeout(function() {{
        //            showDynamicModal({{
        //                titleText: 'Request Deleted',
        //                messageText: '{HttpUtility.JavaScriptStringEncode(message)}',
        //                type: 'danger',                      
        //                redirectUrl:'{redirectUrl}'
        //            }});
        //        }}, 300);";

        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", script, true);
        //        //ScriptManager.RegisterStartupScript(this, this.GetType(), "showSuccess", $"setTimeout(function(){{ showDeleteMessageByClientOnDashboard('Client Request ID: #{requestId} Deleted Successfully.', 'danger'); }}, 200);", true);
        //    }
        //    await ReloadUI(result);
        //}

        private async Task ReloadUI(bool success)
        {
            if (success)
            {
                int requestId = (int)ViewState["SelectedRequestId"];
                ShowRequestDetails(requestId);
            }
        }

        protected async void BtnDelete_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtRequestId.Text.Trim(), out int requestId))
            {
                ApiResponseMessage response = await RequestsService.DeletePublicRegisterRequestAsync(requestId);

                if (response.MessageType == "success")
                {
                    pnlRequestDetails.Visible = false;
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "showSuccess", $"setTimeout(function(){{ showDeleteConfirmModal('Client Request ID: #{requestId} Deleted Successfully.', 'danger'); }}, 200);", true);

                    string message = $"Your request with Request ID: #{requestId}  has been deleted successfully!";

                    string redirectUrl = ResolveClientUrl(Page.GetRouteUrl("PublicRequestRoute", null));
                    string script = $@"
                    setTimeout(function() {{
                        showDynamicModal({{
                            titleText: 'Request Deleted',
                            messageText: '{HttpUtility.JavaScriptStringEncode(message)}',
                            type: 'danger',                      
                            redirectUrl:'{redirectUrl}'
                        }});
                    }}, 300);";

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", script, true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "showDanger", $"showAlert('{response.MessageContent}', 'danger');", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"showAlert('Invalid Request ID.', 'danger');", true);
            }
        }

        private void ClearForm()
        {
            txtFullName.Text = "";
            txtParentName.Text = "";
            txtDOB.Text = "";
            ddlGender.SelectedValue = "";
            txtNationality.Text = "";
            txtOccupation.Text = "";
            txtEmail.Text = "";
            txtMobile.Text = "";
            txtAddress.Text = "";
            txtCity.Text = "";
            txtState.Text = "";
            txtPincode.Text = "";
            ddlAccountType.SelectedValue = "";
            ddlIsJointAccount.SelectedValue = "";
            txtJointClientId.Text = "";
            txtUsername.Text = "";
            txtPassword.Text = "";
            pnlRequestDetails.Visible = false;
        }
        protected void DdlIsJoint_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlIsJointAccount.SelectedValue == "Yes")
            {
                fsJointAccount.Visible = true;
            }
            else
            {
                txtJointClientId.Text = string.Empty;
                fsJointAccount.Visible = false;

            }
        }
    }
}
