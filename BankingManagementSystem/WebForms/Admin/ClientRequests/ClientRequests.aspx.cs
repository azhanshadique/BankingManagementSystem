using BankingManagementSystem.BLL;
using BankingManagementSystem.Helpers;
using BankingManagementSystem.Models.API;
using BankingManagementSystem.Models.Constants;
using BankingManagementSystem.Models.ConstraintTypes;
using BankingManagementSystem.Models.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Results;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BankingManagementSystem.WebForms.Admin.ClientRequests
{
    public partial class ClientRequests : System.Web.UI.Page
    {
        private static string sortColumn = DbColumns.CreatedOn;
        private static string sortColumnDemo = "RepliedOn";
        private static string sortDirection = "DESC";
        private static int adminId = -1;

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
                    RedirectToLogin();

                var principal = JwtTokenManager.ValidateToken(token);
                string role = principal.FindFirst(ClaimTypes.Role)?.Value;
                adminId = Convert.ToInt32(principal.FindFirst("UserID")?.Value);
                ViewState["AdminId"] = adminId;

                if (role != UserRoles.ADMIN.ToString())
                    RedirectToLogin();

                string status = Session["RequestStatus"]?.ToString() ?? RequestStatus.Pending.ToString();
                ddlFilterStatus.SelectedValue = status;
                await LoadRequests(status, sortColumn, sortDirection);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showAlert('Token Invalid!', 'danger');", true);
                RedirectToLogin();
            }
        }
        protected async Task LoadRequests(string status, string sortColumn, string sortDirection)
        {
            var requests = await RequestsService.GetRequestsForAdminAsync(status, sortColumn, sortDirection);

            if (gvRequests.Columns.Count >= 4)
            {
                var repliedCol = gvRequests.Columns[3] as BoundField;
                if (repliedCol != null)
                {
                    if (status == RequestStatus.Pending.ToString())
                        repliedCol.Visible = false;
                    else
                    {
                        repliedCol.Visible = true;
                        repliedCol.HeaderText = status == RequestStatus.Approved.ToString() ? "Approved On" :
                                                status == RequestStatus.Rejected.ToString() ? "Rejected On" :
                                                "Replied On";
                    }
                }
            }

            gvRequests.DataSource = requests;
            gvRequests.DataBind();
        }

        protected async void DdlFilterStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedStatus = ddlFilterStatus.SelectedValue;
            Session["RequestStatus"] = selectedStatus;
            sortColumn = selectedStatus == RequestStatus.Pending.ToString() ? DbColumns.CreatedOn : DbColumns.RepliedOn;
            sortDirection = "DESC";
            gvRequests.PageIndex = 0;
            await LoadRequests(selectedStatus, sortColumn, sortDirection);
        }
        protected async void GvRequests_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvRequests.PageIndex = e.NewPageIndex;

            string selectedType = Session["RequestStatus"]?.ToString() ?? RequestStatus.Pending.ToString();

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
                string status = Session["RequestStatus"]?.ToString()?.ToLower() ?? RequestStatus.Pending.ToString().ToLower();
                switch (status)
                {
                    case "approved":
                        e.Row.CssClass = "table-success";
                        break;
                    case "rejected":
                        e.Row.CssClass = "table-danger";
                        break;
                    case "pending":
                        e.Row.CssClass = "table-warning";
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
                { "RepliedOn", DbColumns.RepliedOn }
            };

            if (columnMap.TryGetValue(e.SortExpression, out var mappedColumn))
            {
                sortColumn = mappedColumn;
                sortColumnDemo = e.SortExpression;
            }

            sortDirection = e.SortExpression == sortColumnDemo && sortDirection == "ASC" ? "DESC" : "ASC";

            await LoadRequests(ddlFilterStatus.SelectedValue, sortColumn, sortDirection);
        }
        protected async void ShowRequestDetails(int requestId)
        {
            var request = await RequestsService.GetRequestByIdAsync(requestId);

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
            pnlRequestTable.Visible = false;
            pnlRequestDetails.Visible = true;
            PopulateClientForm(client);
            lblRequestId.Text = $"#{requestId}";
            bool isPending = request.Status == RequestStatus.Pending.ToString();
            SetButtonState(isPending);

            ViewState["RequestType"] = request.RequestType;
            switch (request.RequestType)
            {
                case nameof(RequestType.CreateNewRegistration):
                    lblRequestType.Text = "Create New Registration";
                    SetStatusBadge(lblRequestStatus, request.Status?.Trim());

                    SetApprovalLabels(client, isPending);
                    pnlClientId.Visible = false;
                    break;

                case nameof(RequestType.UpdateProfileDetails):
                    lblRequestType.Text = "Update Profile Details";
                    SetStatusBadge(lblRequestStatus, request.Status?.Trim());
                    SetApprovalLabels(client, isPending);
                    pnlAccountDetails.Visible = false;
                    pnlPasswords.Visible = false;
                    pnlClientId.Visible = true;
                    break;

                case nameof(RequestType.CreateNewAccount):
                    lblRequestType.Text = "Create New Account";
                    pnlClientId.Visible = true;
                    break;

                default:
                    pnlRequestDetails.Visible = false;
                    break;
            }

           
        }

        private void SetApprovalLabels(ClientDTO client, bool isPending)
        {
            // Co-holder Approval
            lblCoHolderApprovalHeading.Visible = client.IsJointAccount && isPending;
            lblCoHolderApproval.Visible = client.IsJointAccount && isPending;

            if (client.IsJointAccount)
            {
                string coHolderStatus = (client.CoHolderApproved ?? "").Trim();
                SetStatusBadge(lblCoHolderApproval, coHolderStatus);
            }

            // Admin Approval
            lblAdminApprovalHeading.Visible = isPending;
            lblAdminApproval.Visible = isPending;

            string adminStatus = (client.AdminApproved ?? "").Trim();
            SetStatusBadge(lblAdminApproval, adminStatus);
        }

        private void SetButtonState(bool isPending)
        {
            //btnEdit.Visible = enable;
            //btnUpdate.Visible = false;
            //btnEdit.Enabled = enable;
            //btnUpdate.Enabled = enable;
            //btnApprove.Enabled = enable;
            //btnReject.Enabled = enable;

            //bool coApproved = client.CoHolderApproved == RequestStatus.Awaiting.ToString();
            pnlButtons.Visible = isPending;
            btnDelete.Visible = !isPending;
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
            txtClientId.Text = client.ClientId.ToString();
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
        protected void BtnDelete_Click(object sender, EventArgs e)
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
                ConfirmPassword = txtConfirmPassword.Text,
                CoHolderApproved = lblCoHolderApproval.Text,
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
                ToggleButtons(false);
                ShowRequestDetails(requestId);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "customAlert", "showAlert('Update failed due to a technical error.', 'danger');", true);
            }
        }

        

        protected async void BtnApprove_Click(object sender, EventArgs e)
        {
            int requestId = (int)(ViewState["SelectedRequestId"] ?? 0);
            int adminId = (int)(ViewState["AdminId"] ?? 0);

            //bool statusUpdated = true;
            bool statusUpdated = await RequestsService.ApproveRequestAsync(requestId, adminId);
            if (!statusUpdated)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "failAlert", "showAlert('Approval failed.', 'danger');", true);
            }
            bool result = false;
            ClientDTO client = GetClient();
            switch (ViewState["RequestType"].ToString())
            {
                case nameof(RequestType.CreateNewRegistration):
                    result = await AdminService.CreateClientAsync(client);
                    break;

                case nameof(RequestType.UpdateProfileDetails):
                    result = await AdminService.UpdateClientDetailsAsync(client);
                    break;

                case nameof(RequestType.CreateNewAccount):
                    break;

                default:
                    //pnlRequestDetails.Visible = false;
                    break;
            }

            if (statusUpdated && result)
            {
                string message = $"Client Request ID: #{requestId} Approved Successfully.";

                string redirectUrl = ResolveClientUrl(Page.GetRouteUrl("AdminClientRequestRedirect", null));
                string script = $@"
                            setTimeout(function() {{
                                showDynamicModal({{
                                    titleText: 'Client Request Approved',
                                    messageText: '{HttpUtility.JavaScriptStringEncode(message)}',
                                    type: 'success',                      
                                    redirectUrl:'{redirectUrl}'
                                }});
                            }}, 300);";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", script, true);
                await ReloadUI(result);

                //ScriptManager.RegisterStartupScript(this, this.GetType(), "showSuccess", $"setTimeout(function(){{ showApproveRejectMessage('Client Request ID: #{requestId} Approved Successfully.', 'success'); }}, 200);", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "failAlert", $"showAlert('Approval failed.', 'danger');", true);
            }

        }



   
        protected async void BtnReject_Click(object sender, EventArgs e)
        {
            int requestId = (int)ViewState["SelectedRequestId"];
            int adminId = (int)(ViewState["AdminId"] ?? 0);

            bool result = await RequestsService.RejectRequestAsync(requestId, adminId);
            if (result)
            {
                string message = $"Client Request ID: #{requestId} Rejected Successfully.";

                string redirectUrl = ResolveClientUrl(Page.GetRouteUrl("AdminClientRequestRedirect", null));
                string script = $@"
                setTimeout(function() {{
                    showDynamicModal({{
                        titleText: 'Client Request Rejected',
                        messageText: '{HttpUtility.JavaScriptStringEncode(message)}',
                        type: 'danger',                      
                        redirectUrl:'{redirectUrl}'
                    }});
                }}, 300);";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", script, true);
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "showSuccess", $"setTimeout(function(){{ showApproveRejectMessage('Client Request ID: #{requestId} Rejected Successfully.', 'danger'); }}, 200);", true);
            }
            await ReloadUI(result);
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
        private async Task ReloadUI(bool success)
        {
            if (success)
            {
                await LoadRequests(ddlFilterStatus.SelectedValue, sortColumn, sortDirection);
            }
        }

        private void ClearForm()
        {
            txtFullName.Text = txtParentName.Text = txtDOB.Text = txtNationality.Text = txtOccupation.Text = txtAadhaar.Text = txtPan.Text = txtEmail.Text = txtMobile.Text = txtAddress.Text = txtCity.Text = txtState.Text = txtPincode.Text = txtJointClientId.Text = txtUsername.Text = "";
            ddlGender.SelectedValue = ddlAccountType.SelectedValue = ddlIsJointAccount.SelectedValue = "";
            pnlRequestDetails.Visible = false;
        }

        protected void DdlIsJoint_SelectedIndexChanged(object sender, EventArgs e)
        {
            fsJointAccount.Visible = ddlIsJointAccount.SelectedValue == "Yes";
            if (!fsJointAccount.Visible) txtJointClientId.Text = string.Empty;
        }


        private void RedirectToLogin()
        {
            Response.Redirect(Page.GetRouteUrl("AdminLoginRoute", null));
        }
    }
}
