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
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BankingManagementSystem.WebForms.Client
{
	public partial class PendingRequest : System.Web.UI.Page
	{
        private static string sortColumn = DbColumns.CreatedOn;
        private static string sortColumnDemo = "RepliedOn";
        private static string sortDirection = "DESC";
        private static int clientId = -1;
        private static readonly string typeReceived = "Received";
        private static readonly string typeSent = "Sent";

        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    string token = Request.Cookies["auth_token"]?.Value;
                    if (string.IsNullOrEmpty(token))
                        Response.Redirect(Page.GetRouteUrl("ClientLoginRoute", null));

                    var principal = JwtTokenManager.ValidateToken(token);
                    string role = principal.FindFirst(ClaimTypes.Role)?.Value;
                    clientId = Convert.ToInt32(principal.FindFirst("UserID")?.Value);

                    if (role != UserRoles.CLIENT.ToString())
                        Response.Redirect(Page.GetRouteUrl("ClientLoginRoute", null));

                    string type = Session["RequestType"]?.ToString() ?? typeReceived;
                    ddlRequestType.SelectedValue = type;
                    await LoadRequests(typeReceived, sortColumn, sortDirection);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showAlert('Invalid Token!', 'danger');", true);
                }
            }
        }

        protected async Task LoadRequests(string type, string sortColumn, string sortDirection)
        {
            var requests = type == typeReceived
                ? await RequestsService.GetReceivedRequestsForClientAsync(clientId, sortColumn, sortDirection)
                : await RequestsService.GetSentRequestsByClientAsync(clientId, sortColumn, sortDirection);

            if (gvRequests.Columns.Count >= 4)
            {
                var statusCol = gvRequests.Columns[3] as BoundField;
                var requestedOnCol = gvRequests.Columns[2] as BoundField;
                if (statusCol != null)
                {
                    if (type == typeReceived)
                    {
                        statusCol.Visible = false;
                        requestedOnCol.HeaderText = "Received On";
                    }
                    else
                    {
                        statusCol.Visible = true;
                        requestedOnCol.HeaderText = "Requested On";
                    }
                }
            }

            gvRequests.DataSource = requests;
            gvRequests.DataBind();
        }
     
        protected async void DdlRequestType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedType = ddlRequestType.SelectedValue;
            Session["RequestType"] = selectedType;
            //sortColumn = selectedType == RequestStatus.Pending.ToString() ? DbColumns.CreatedOn : DbColumns.RepliedOn;
            //sortDirection = "DESC";
            await LoadRequests(selectedType, sortColumn, sortDirection);
        }


        protected void GvRequests_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Show")
            {
                int requestId = Convert.ToInt32(e.CommandArgument);
                ViewState["SelectedRequestId"] = requestId;
                ShowRequestDetails(requestId);
            }
        }

        protected void GvRequests_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                string type = Session["RequestType"]?.ToString()?.ToLower() ?? typeReceived.ToLower();
                switch (type)
                {
                    case "received":
                        e.Row.CssClass = "table-warning";
                        break;
                    case "sent":
                        e.Row.CssClass = "table-primary";
                        break;                   
                    default:
                        e.Row.CssClass = "table-secondary";
                        break;
                }

                e.Row.Style["color"] = "black";
            }
        }
        protected async void GvRequests_Sorting(object sender, GridViewSortEventArgs e)
        {
            var columnMap = new Dictionary<string, string>
            {
                { "RequestId", DbColumns.RequestId },
                { "RequestType", DbColumns.RequestType },
                { "RequestedOn", DbColumns.CreatedOn },
                { "Status", DbColumns.Status }
            };

            if (columnMap.TryGetValue(e.SortExpression, out var mappedColumn))
            {
                sortColumn = mappedColumn;
                sortColumnDemo = e.SortExpression;
            }

            sortDirection = e.SortExpression == sortColumnDemo && sortDirection == "ASC" ? "DESC" : "ASC";

            await LoadRequests(ddlRequestType.SelectedValue, sortColumn, sortDirection);
        }
        protected async void ShowRequestDetails2(int requestId)
        {
            var request = await RequestsService.GetRequestByIdAsync(requestId);
            if (request != null)
            {
                var client = JsonConvert.DeserializeObject<ClientDTO>(request.Payload);
                PopulateClientForm(client);

                //pnlClientRequestList.Visible = false;
                //pnlClientRequestDetails.Visible = true;

                btnApprove.Visible = request.Status == RequestStatus.Pending.ToString();
                btnReject.Visible = request.Status == RequestStatus.Pending.ToString();
                //btnDelete.Visible = request.Status == RequestStatus.Pending.ToString();
                btnUpdate.Visible = request.Status == RequestStatus.Pending.ToString();
            }
        }

       

        

        

        protected async void ShowRequestDetails(int requestId)
        {
            var request = await RequestsService.GetRequestByIdAsync(requestId);
            if (request != null)
            {
                var client = JsonConvert.DeserializeObject<ClientDTO>(request.Payload);
                if (client == null)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"showAlert('Error in Get Request By Id.', 'danger');", true);
                    return;
                }

                if (request.RequestType == RequestType.JointAccountApproval.ToString())
                {
                    pnlRequestTable.Visible = false;
                    pnlRequestDetails.Visible = true;

                    lblRequestType.Text = "Joint Account Approval";
                    lblRequestStatus.Text = request.Status;
                    lblRequestId.Text = $"#{requestId.ToString()}";


                    lblCoHolderApprovalHeading.Visible = true;
                    lblCoHolderApproval.Text = client.CoHolderApproved ? RequestStatus.Approved.ToString() : RequestStatus.Awaiting.ToString();
                    lblAdminApprovalHeading.Visible = true;
                    lblAdminApproval.Text = client.AdminApproved ? RequestStatus.Approved.ToString() : RequestStatus.Awaiting.ToString();

                    SetButtonState(true);
                    btnEdit.Visible = false;
                    btnUpdate.Visible = false;

                    //if (request.Status == RequestStatus.Pending.ToString())
                    //{
                    //    if (client.IsJointAccount)
                    //    {
                    //        lblCoHolderApprovalHeading.Visible = true;
                    //        lblCoHolderApproval.Text = client.CoHolderApproved ? RequestStatus.Approved.ToString() : RequestStatus.Awaiting.ToString();

                    //    }
                    //    lblAdminApprovalHeading.Visible = true;
                    //    lblAdminApproval.Text = client.AdminApproved ? RequestStatus.Approved.ToString() : RequestStatus.Awaiting.ToString();
                    //    SetButtonState(true);
                        
                    //}
                    //else
                    //{
                    //    SetButtonState(false);

                    //}

                    PopulateClientForm(client);
                    //txtPrimaryAccHolderClientId.Text = 
                }
                else if (request.RequestType == RequestType.UpdateDetails.ToString())
                {

                }
                else if (request.RequestType == RequestType.CreateNewAccount.ToString())
                {

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
            txtDOB.Text = client.DOB?.ToString();
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
            btnApprove.Enabled = enable;
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
            return new ClientDTO
            {
                FullName = txtFullName.Text.Trim(),
                ParentName = txtParentName.Text.Trim(),
                DOB = txtDOB.Text,
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
                ApiResponseMessage result = await RequestsService.UpdatePayloadAsync(requestId, updatedClient);

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
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "customAlert", "showAlert('Update failed due to a technical error.', 'danger');", true);
            }
        }


        protected async void btnDelete_Click(object sender, EventArgs e)
        {
            int requestId = (int)ViewState["SelectedRequestId"];
            bool success = await RequestsService.DeleteRequestAsync(requestId);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", success ? "showAlert('Deleted!', 'success');" : "showAlert('Delete Failed!', 'danger');", true);
            await LoadRequests(ddlRequestType.SelectedValue, sortColumn, sortDirection);
        }



        protected async void BtnApprove_Click(object sender, EventArgs e)
        {
            int requestId = (int)ViewState["SelectedRequestId"];
            bool statusUpdated = await RequestsService.UpdateStatusAsync(requestId, RequestStatus.Approved.ToString(), clientId);
            if (statusUpdated)
            {
                //ClientDTO client = GetClient();

                //bool result = await RegistrationService.CreateClientAsync(client);
                //if (result)
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "showSuccess", $"setTimeout(function(){{ showApproveRejectMessageByClient('Client Request ID: #{requestId} for Joint Account Approved Successfully.', 'success'); }}, 200);", true);
                await ReloadUI(statusUpdated);

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "failAlert", "showAlert('Approval failed.', 'danger');", true);
            }

        }


        protected async void BtnReject_Click(object sender, EventArgs e)
        {
            int requestId = (int)ViewState["SelectedRequestId"];
            bool result = await RequestsService.UpdateStatusAsync(requestId, RequestStatus.Rejected.ToString(), clientId);
            if (result)
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showSuccess", $"setTimeout(function(){{ showApproveRejectMessageByClient('Client Request ID: #{requestId} for Joint Account Rejected Successfully.', 'danger'); }}, 200);", true);

            await ReloadUI(result);
        }

        private async Task ReloadUI(bool success)
        {
            if (success)
            {
                await LoadRequests(ddlRequestType.SelectedValue, sortColumn, sortDirection);
            }
        }

        protected void DdlIsJoint_SelectedIndexChanged(object sender, EventArgs e)
        {
            fsJointAccount.Visible = ddlIsJointAccount.SelectedValue == "Yes";
            if (!fsJointAccount.Visible) txtJointClientId.Text = string.Empty;
        }
    }
}
