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
using System.Web.Http.Results;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BankingManagementSystem.WebForms.Client
{
	public partial class PendingRequest : System.Web.UI.Page
	{
        private static string sortColumn = DbColumns.CreatedOn;
        private static string sortColumnDemo = "RepliedOn";
        private static string sortDirection = "DESC";
        private int clientId = -1;
        private static readonly string typeReceived = "Received";
        private static readonly string typeSent = "Sent";

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
                }

                var principal = JwtTokenManager.ValidateToken(token);
                string role = principal.FindFirst(ClaimTypes.Role)?.Value;
                if (role != UserRoles.CLIENT.ToString())
                {
                    RedirectToLogin();

                }
                clientId = Convert.ToInt32(principal.FindFirst("UserID")?.Value);
                ViewState["ClientId"] = clientId;




                string type = Session["RequestType"]?.ToString() ?? typeReceived;
                ddlRequestType.SelectedValue = type;
                Session["RequestType"] = type;
                await LoadRequests(type, sortColumn, sortDirection);
                //string type;
                //if (!IsPostBack)
                //{
                //    type = Session["RequestType"]?.ToString() ?? typeReceived;
                //    ddlRequestType.SelectedValue = type;
                //}
                //else
                //{
                //    type = ddlRequestType.SelectedValue;
                //}
                //await LoadRequests(type, sortColumn, sortDirection);

            }
            catch
            {
                //Response.Redirect(Page.GetRouteUrl("ClientLoginRoute", null));
                RedirectToLogin();

            }
        }
        protected async Task LoadRequests(string type, string sortColumn, string sortDirection)
        {
            int clientId = (int)(ViewState["ClientId"] ?? 0);

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
            gvRequests.PageIndex = 0;
            await LoadRequests(selectedType, sortColumn, sortDirection);
        }

        protected async void GvRequests_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvRequests.PageIndex = e.NewPageIndex;

            string selectedType = Session["RequestType"]?.ToString() ?? typeReceived;

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





        protected async void ShowRequestDetails(int requestId)
        {
            //if (Session["RequestType"]?.ToString() == typeReceived)
            //    requestId = requestId + 1;

            var request = Session["RequestType"]?.ToString() == typeReceived ? await RequestsService.GetRequestByIdAsync(requestId + 1) : await RequestsService.GetRequestByIdAsync(requestId);

            if (request == null)
            {
                pnlRequestDetails.Visible = false;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"showAlert('Request not found.', 'danger');", true);
                return;
            }

            var client = JsonConvert.DeserializeObject<ClientDTO>(request.Payload);
            if (client == null)
            {
                pnlRequestDetails.Visible = false;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"showAlert('Invalid client data.', 'danger');", true);
                return;
            }

            //pnlRequestTable.Visible = false;
            //pnlRequestDetails.Visible = true;
            ////lblRequestId.Text = Session["RequestType"]?.ToString() == typeReceived? $"#{requestId - 1}" : $"#{requestId}";
            ////lblRequestId.Text = request.RequestType == nameof(RequestType.JointAccountApproval) ? $"#{requestId - 1}" : $"#{requestId}";
            //lblRequestId.Text = $"#{requestId}";

            pnlRequestTable.Visible = false;
            pnlRequestDetails.Visible = true;
            PopulateClientForm(client);
            lblRequestId.Text = $"#{requestId}";



            //bool isPending = request.Status == RequestStatus.Pending.ToString();

            switch (request.RequestType)
            {
                case nameof(RequestType.JointAccountApproval):
                    lblMessage.Visible = true;
                    pnlAccountDetails.Visible = true;
                    lblRequestHeading.Text = "Joint Account Approval Request Details";
                    lblRequestType.Text = "Joint Account Approval";
                    SetStatusBadge(lblRequestStatus, request.Status?.Trim()); 

                    // Approval labels
                    lblCoHolderApprovalHeading.Visible = true;
                    lblCoHolderApproval.Visible = true;
                    SetStatusBadge(lblCoHolderApproval, (client.CoHolderApproved ?? "").Trim());

                    lblAdminApprovalHeading.Visible = true;
                    lblAdminApproval.Visible = true;
                    SetStatusBadge(lblAdminApproval, (client.AdminApproved ?? "").Trim());

                    SetButtonState(true);
                    //btnEdit.Visible = false;
                    //btnUpdate.Visible = false;

                    PopulateClientForm(client);
                    break;

                case nameof(RequestType.UpdateProfileDetails):
                    lblMessage.Visible = false;
                    pnlAccountDetails.Visible = false;

                    lblRequestHeading.Text = "Update Profile Request Details";
                    lblRequestType.Text = "Update Profile Details";
                    SetStatusBadge(lblRequestStatus, request.Status?.Trim());
                    SetButtonState(false);
                    PopulateClientForm(client);
                    if (request.Status?.Trim() == RequestStatus.Pending.ToString())
                    {
                        lblAdminApprovalHeading.Visible = true;
                        lblAdminApproval.Visible = true;
                        SetStatusBadge(lblAdminApproval, (client.AdminApproved ?? "").Trim());
                    }
                    else
                    {
                        //lblAdminApprovalHeading.Visible = false;
                        //lblAdminApproval.Visible = false;
                        pnlBntsEditUpdateDlt.Visible = false;
                    }


                        
                    break;

                case nameof(RequestType.CreateNewAccount):
                    SetButtonState(false);
                    break;

                default:
                    pnlRequestDetails.Visible = false;
                    break;
            }
        }

        private void SetButtonState(bool enable)
        {
            //btnEdit.Visible = enable;
            //btnUpdate.Visible = false;

            //btnEdit.Enabled = enable;
            //btnUpdate.Enabled = enable;
            //btnApprove.Enabled = enable;
            //btnReject.Enabled = enable;
            pnlBtnsAproveReject.Visible = enable;
            pnlBntsEditUpdateDlt.Visible = !enable;
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


        private void PopulateClientForm(ClientDTO client)
        {
            txtFullName.Text = client.FullName;
            txtParentName.Text = client.ParentName;
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
            txtClientId.Text = client.ClientId != 0 ? client.ClientId.ToString() : null;
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
            //ddlAccountType.Enabled = !isReadOnly;
            //ddlIsJointAccount.Enabled = !isReadOnly;
            //txtJointClientId.ReadOnly = isReadOnly;
            //txtUsername.ReadOnly = isReadOnly;
            //txtPassword.ReadOnly = isReadOnly;
            //txtConfirmPassword.ReadOnly = isReadOnly;
            //txtClientId.ReadOnly = isReadOnly;
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
                AdminApproved = RequestStatus.Awaiting.ToString(),
                ClientId = Convert.ToInt32(txtClientId.Text.Trim())
            };
        }


        protected async void BtnUpdate_Click(object sender, EventArgs e)
        {
            int requestId = (int)ViewState["SelectedRequestId"];

            ClientDTO updatedClient = GetClient();

            try
            {
                ApiResponseMessage result = await RequestsService.UpdateRequestAsync(requestId, updatedClient);

                //string message = result.MessageContent;
                string message = GetParsedErrorMessage(result.MessageContent);

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
            ShowRequestDetails(requestId);

        }

        protected string GetParsedErrorMessage(string Message)
        {
            string parsedMessage;
            if (Message.StartsWith("{") && Message.Contains("Message"))
            {
                var parsed = JsonConvert.DeserializeObject<ApiErrorMessageWrapper>(Message);
                parsedMessage = parsed?.Message;
            }
            else
            {
                parsedMessage = Message;
            }
            // Trim quotes if they exist
            if (parsedMessage.StartsWith("\"") && parsedMessage.EndsWith("\""))
            {
                parsedMessage = parsedMessage.Trim('"');
            }
            return parsedMessage;
        }
       
        protected async void BtnApprove_Click(object sender, EventArgs e)
        {
            if (ViewState["SelectedRequestId"] == null || !int.TryParse(ViewState["SelectedRequestId"].ToString(), out int requestId))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "invalidId", "showAlert('Invalid request ID.', 'danger');", true);
                return;
            }
            //requestId = requestId + 1;
            int clientId = (int)(ViewState["ClientId"] ?? 0);
            bool statusUpdated = await RequestsService.ApproveRequestAsync(requestId + 1, clientId);

            if (statusUpdated)
            {
                string message = $"Client Request ID: #{requestId} for Joint Account Approved Successfully. Awaiting admin approval.";

                string redirectUrl = ResolveClientUrl(Page.GetRouteUrl("ClientPendingRequestRoute", null));

                string script = $@"
                setTimeout(function() {{
                    showDynamicModal({{
                        titleText: 'Joint Account Request Approved',
                        messageText: '{HttpUtility.JavaScriptStringEncode(message)}',
                        type: 'success',
                        redirectUrl: '{redirectUrl}'
                    }});
                }}, 300);";


                ScriptManager.RegisterStartupScript(this, this.GetType(), "showSuccessModal", script, true);
                //pnlRequestTable.Visible = true;
                //pnlRequestDetails.Visible = false;
                await ReloadUI(true);

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "failAlert", "showAlert('Approval failed.', 'danger');", true);
            }
        }

        protected async void BtnReject_Click(object sender, EventArgs e)
        {
            int requestId = (int)ViewState["SelectedRequestId"];
            int clientId = (int)(ViewState["ClientId"] ?? 0);
            //requestId = requestId + 1;
            bool result = await RequestsService.RejectRequestAsync(requestId + 1,  clientId);
            if (result)
            {
                string message = $"Client Request ID: #{requestId} for Joint Account Rejected Successfully.";

                string redirectUrl = ResolveClientUrl(Page.GetRouteUrl("ClientPendingRequestRoute", null));
                string script = $@"
                setTimeout(function() {{
                    showDynamicModal({{
                        titleText: 'Joint Account Request Rejected',
                        messageText: '{HttpUtility.JavaScriptStringEncode(message)}',
                        type: 'danger',                      
                        redirectUrl:'{redirectUrl}'
                    }});
                }}, 300);";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "showSuccessModal", script, true);
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "showSuccess", $"setTimeout(function(){{ showApproveRejectMessageByClient('Client Request ID: #{requestId} for Joint Account Rejected Successfully.', 'danger'); }}, 200);", true);


            }
            await ReloadUI(result);
        }

        protected void BtnEdit_Click(object sender, EventArgs e)
        {
            SetClientFormReadOnly(false);
            btnEdit.Visible = false;
            btnUpdate.Visible = true;
        }
        protected async void BtnDelete_Click(object sender, EventArgs e)
        {
            if (ViewState["SelectedRequestId"] == null || !int.TryParse(ViewState["SelectedRequestId"].ToString(), out int requestId))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showAlert('Request ID is missing or invalid.', 'danger');", true);
                return;
            }
            int clientId = (int)(ViewState["ClientId"] ?? 0);

            ApiResponseMessage response = await RequestsService.DeleteClientRequestByClientAsync(requestId, clientId);

            if (response.MessageType == "success")
            //if(true)
            {
                //pnlRequestDetails.Visible = false;
                string message = $"Your request with Request ID: #{requestId}  has been deleted successfully!";

                string redirectUrl = ResolveClientUrl(Page.GetRouteUrl("ClientPendingRequestRoute", null));
                string script = $@"
                setTimeout(function() {{
                    showDynamicModal({{
                        titleText: 'Request Deleted',
                        messageText: '{HttpUtility.JavaScriptStringEncode(message)}',
                        type: 'danger',                      
                        redirectUrl:'{redirectUrl}'
                    }});
                }}, 300);";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "showSuccessModal", script, true);
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "showSuccessModal", $@"setTimeout(function() {{showDynamicModal({{'Request Deleted', '{HttpUtility.JavaScriptStringEncode(message)}','success','{redirectUrl}'", true);

            }
            else
            {
                string safeMsg = HttpUtility.JavaScriptStringEncode(response.MessageContent ?? "Delete failed.");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showDanger", $"showAlert('{safeMsg}', 'danger');", true);
            }

            //pnlRequestTable.Visible = true;
            //pnlRequestDetails.Visible = false;
            await ReloadUI(true);

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
        private void RedirectToLogin()
        {
            Response.Redirect(Page.GetRouteUrl("ClientLoginRoute", null));
        }
    }
}
