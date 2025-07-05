using System;
using System.Web.UI;
using Newtonsoft.Json;
using BankingManagementSystem.Models.DTOs;
using BankingManagementSystem.DAL;
using BankingManagementSystem.Models.ConstraintTypes;
using BankingManagementSystem.Helpers;
using BankingManagementSystem.Models.API;
using System.Threading.Tasks;

namespace BankingManagementSystem.WebForms.Home
{
    public partial class ClientRequest : System.Web.UI.Page
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
            var request = await RequestsService.GetRegisterRequestByIdAsync(requestId);
            if (request != null)
            {
                var client = JsonConvert.DeserializeObject<ClientDTO>(request.Payload);
                if (client == null)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"showAlert('Error in Get Request By Id.', 'danger');", true);
                    return;
                }

                if (request.RequestType == RequestType.CreateNewRegistration.ToString())
                {
                    //pnlRequestTable.Visible = false;
                    pnlRequestDetails.Visible = true;

                    lblRequestType.Text = "Create New Registration";
                    lblRequestStatus.Text = request.Status;
                    lblRequestId.Text = $"#{requestId.ToString()}";

                    if (request.Status == RequestStatus.Pending.ToString())
                    {
                        if (client.IsJointAccount)
                        {
                            lblCoHolderApprovalHeading.Visible = true;
                            lblCoHolderApproval.Text = client.CoHolderApproved ? RequestStatus.Approved.ToString() : RequestStatus.Awaiting.ToString();

                        }
                        lblAdminApprovalHeading.Visible = true;
                        lblAdminApproval.Text = client.AdminApproved ? RequestStatus.Approved.ToString() : RequestStatus.Awaiting.ToString();
                        SetButtonState(true);
                    }
                    else
                    {
                        SetButtonState(false);

                    }

                    PopulateClientForm(client);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"showAlert('Request is not of type Create New Registration.', 'danger');", true);
                    //Response.Redirect(Page.GetRouteUrl("ClientRequestRoute", null));
                }
            }
            else
            {
                pnlRequestDetails.Visible = false;
            }
        }

        private void PopulateClientForm(ClientDTO client)
        {
            txtFullName.Text = client.FullName;
            txtParentName.Text = client.ParentName;
            txtDOB.Text = client.DOB.ToString();
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

        private void SetButtonState(bool enable)
        {
            btnUpdate.Enabled = enable;
            btnEdit.Enabled = enable;
            //btnApprove.Enabled = enable;
            btnReject.Enabled = enable;
        }

        protected void BtnEdit_Click(object sender, EventArgs e)
        {
            SetClientFormReadOnly(false);
            btnEdit.Visible = false;
            btnUpdate.Visible = true;
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
            ddlAccountType.Enabled = !isReadOnly;
            ddlIsJointAccount.Enabled = !isReadOnly;
            txtJointClientId.ReadOnly = isReadOnly;
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
                ConfirmPassword = txtConfirmPassword.Text
            };
        }

        protected async void BtnUpdate_Click(object sender, EventArgs e)
        {
            int requestId = (int)ViewState["SelectedRequestId"];

            ClientDTO updatedClient = GetClient();

            try
            {
                ApiResponseMessage result = await RequestsService.UpdateRequestAsync(requestId, updatedClient);

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
                btnEdit.Visible = true;
                btnUpdate.Visible = false;
                await ReloadUI(true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "customAlert", "showAlert('Update failed due to a technical error.', 'danger');", true);
            }
        }

        protected async void BtnReject_Click(object sender, EventArgs e)
        {
            int requestId = (int)ViewState["SelectedRequestId"];
            bool result = await RequestsService.RejectRequestAsync(requestId, requestId);
            if (result)
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showSuccess", $"setTimeout(function(){{ showDeleteMessageByClientOnDashboard('Client Request ID: #{requestId} Deleted Successfully.', 'danger'); }}, 200);", true);

            await ReloadUI(result);
        }

        private async Task ReloadUI(bool success)
        {
            if (success)
            {
                int requestId = (int)ViewState["SelectedRequestId"];
                ShowRequestDetails(requestId);
            }
        }

        protected void BtnDelete_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showSuccess", $"setTimeout(function(){{ showDeleteConfirmModal(); }}, 200);", true);

            //if (int.TryParse(txtRequestId.Text.Trim(), out int requestId))
            //{
            //    bool success = RequestDAL.DeleteRequestByStatus(requestId, "Pending");
            //    if (success)
            //    {
            //        ClearForm();
            //        //lblMessage.Text = "Request deleted successfully.";
            //    }
            //    else
            //    {
            //        //lblMessage.Text = "Delete failed.";
            //    }
            //}
        }
        //protected void BtnEdit_Click(object sender, EventArgs e)
        //{
        //    //SetClientFormReadOnly(false);
        //    //btnEdit.Visible = false;
        //    //btnUpdate.Visible = true;
        //}
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
