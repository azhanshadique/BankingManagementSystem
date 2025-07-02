using BankingManagementSystem.BLL;
using BankingManagementSystem.DAL;
using BankingManagementSystem.Helpers;
using BankingManagementSystem.Models.API;
using BankingManagementSystem.Models.Constants;
using BankingManagementSystem.Models.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Claims;
using System.Web;
using System.Web.Helpers;
using System.Web.Http.Results;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BankingManagementSystem.WebForms.Admin
{
    public partial class PendingRequest : System.Web.UI.Page
    {
        //private static string sortExpression = DbColumns.RequestId;
        private static string sortColumn = DbColumns.CreatedOn; 
        private static string sortDirection = "DESC";  


        protected void Page_Load(object sender, EventArgs e)
        {
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            //Response.Cache.SetNoStore();
            if (!IsPostBack)
            {
                try
                {
                    string token = Request.Cookies["auth_token"]?.Value;
                    if (string.IsNullOrEmpty(token))
                    {
                        Response.Redirect(Page.GetRouteUrl("DashboardRoute", null));
                    }

                    var principal = JwtTokenManager.ValidateToken(token);
                    string role = principal.FindFirst(ClaimTypes.Role)?.Value;

                    // Show based on role
                    if (role == Models.ConstraintTypes.UserRoles.CLIENT.ToString())
                    {
                        Response.Redirect(Page.GetRouteUrl("DashboardRoute", null));
                    }
               
                    if (ViewState["RequestType"] != null)
                        LoadRequests("Approved", sortColumn, sortDirection);
                    else
                        LoadRequests("Pending", sortColumn, sortDirection);
                }
                catch
                {
                    // Token invalid or missing
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showAlert('Token Invalid!', 'danger');", true);

                }
            }

        }
        //protected async void LoadRequests(string status, string sortColumn, string sortDirection)
        //{
        //    var requests = await RequestsService.GetRequestsAsync(status, sortColumn, sortDirection);
        //    gvRequests.DataSource = requests;
        //    gvRequests.DataBind();
        //}
        protected async void LoadRequests(string status, string sortColumn, string sortDirection)
        {
            ViewState["RequestType"] = status;
            var requests = await RequestsService.GetRequestsAsync(status, sortColumn, sortDirection);
            if (gvRequests.Columns.Count >= 4)
            {
                var repliedCol = gvRequests.Columns[3] as BoundField;
                if (repliedCol != null)
                {
                    if (status == "Pending")
                    {
                        repliedCol.Visible = false;
                    }
                    else
                    {
                        repliedCol.Visible = true;
                        repliedCol.HeaderText = status == "Approved" ? "Approved On" :
                                                 status == "Rejected" ? "Rejected On" :
                                                 "Replied On";
                    }
                }
            }

      

            gvRequests.DataSource = requests;
            gvRequests.DataBind();
            

            if (gvRequests.HeaderRow != null)
            {
                string headerClass = "";

                switch (status.ToLower())
                {
                    case "approved":
                        headerClass = "table-success";
                        break;
                    case "rejected":
                        headerClass = "table-danger";
                        break;
                    case "pending":
                        headerClass = "table-primary";
                        break;
                    default:
                        headerClass = "table-secondary"; 
                        break;
                }

                gvRequests.HeaderRow.CssClass = headerClass;
            }
        }

        protected void DdlFilterStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["RequestType"] = ddlFilterStatus.SelectedValue;
            LoadRequests(ddlFilterStatus.SelectedValue, sortColumn, sortDirection);
        }
        protected void GvRequests_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Show")
            {
                int requestId = Convert.ToInt32(e.CommandArgument);
                ViewState["SelectedRequestId"] = requestId;
                BtnShow_Click(requestId);
            }
        }
        protected async void BtnShow_Click(int requestId)
        {
            var request = await RequestsService.GetRequestByIdAsync(requestId);
            if (request != null)
            {
                //var client = JsonConvert.DeserializeObject<ClientDTO>(request.Payload);
                //try
                //{
                var client = JsonConvert.DeserializeObject<ClientDTO>(request.Payload);
                if (client == null)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"showAlert('Error in Get Request By Id.', 'danger');", true);
                    return;
                }
                if (request.RequestType == "CreateNewRegistration")
                {

                    pnlRequestTable.Visible = false;
                    pnlRequestDetails.Visible = true;

                    lblRequestType.Text = request.RequestType;
                    lblRequestStatus.Text = request.Status;

                    if (request.Status == "Pending")
                    {
                        lblAdminApprovalHeading.Visible = true;
                        lblCoHolderApprovalHeading.Visible = true;
                        lblAdminApproval.Text = client.AdminApproved ? "Approved" : "Awaiting";
                        lblCoHolderApproval.Text = client.CoHolderApproved ? "Approved" : "Awaiting";
                    
                    }

                    lblRequestId.Text = $"#{requestId.ToString()}";
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
                    if (client.IsJointAccount)
                    {
                        fsJointAccount.Visible = true;
                    }
                    else
                    {
                        fsJointAccount.Visible = false;
                    }
                    txtJointClientId.Text = client.JointClientId != 0 ? client.JointClientId.ToString() : "";
                    txtUsername.Text = client.Username;
                    txtPassword.Attributes["value"] = client.Password;
                    txtConfirmPassword.Attributes["value"] = client.Password;

                    btnUpdate.Enabled = true;
                    //btnDelete.Enabled = true;
                }
                //else
                //{
                //    //lblMessage.Text = "Failed to parse client request payload.";
                //}
                //}
                //catch
                //{
                //    //lblMessage.Text = "Invalid payload format.";
                //}
            }
            else
            {
                //lblMessage.Text = "No request found with this ID.";
                pnlRequestDetails.Visible = false;
            }
        }

        protected void BtnEdit_Click(object sender, EventArgs e)
        {
            txtFullName.ReadOnly = false;
            txtParentName.ReadOnly = false;
            txtDOB.ReadOnly = false;
            ddlGender.Enabled = true;
            txtNationality.ReadOnly = false;
            txtOccupation.ReadOnly = false;
            txtAadhaar.ReadOnly = false;
            txtPan.ReadOnly = false;
            txtEmail.ReadOnly = false;
            txtMobile.ReadOnly = false;
            txtAddress.ReadOnly = false;
            txtCity.ReadOnly = false;
            txtState.ReadOnly = false;
            txtPincode.ReadOnly = false;
            ddlAccountType.Enabled = true;
            ddlIsJointAccount.Enabled = true;
            txtJointClientId.ReadOnly = false;
            txtUsername.ReadOnly = false;
            txtPassword.ReadOnly = false;
            txtConfirmPassword.ReadOnly = false;
            btnEdit.Visible = false;
            btnUpdate.Visible = true;
        }





        protected async void BtnUpdate_Click(object sender, EventArgs e)
        {
            int requestId = (int)ViewState["SelectedRequestId"];

            ClientDTO updatedClient = new ClientDTO
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

            //bool result = await RequestsService.UpdatePayloadAsync(requestId, updatedClient);
            //bool result = AdminBLL.UpdatePayload(requestId, updatedClient, out string message);

            //string messageContent = result ? "Request updated successfully." : "Update failed.";
            //string messageType = result ? "success" : "danger";

            //ScriptManager.RegisterStartupScript(this, this.GetType(), "customAlert", $"showAlert('{messageContent}', '{messageType}');", true);
            try
            {
                ApiResponseMessage result = await RequestsService.UpdatePayloadAsync(requestId, updatedClient);

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

                if (result.MessageType == "success")
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "showSuccess", $"setTimeout(function(){{ showRegisterSuccessMessage('{message}', '{result.MessageType}'); }}, 200);", true);
                else
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "customAlert", $"showAlert('{message}', '{result.MessageType}');", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "customAlert", $"showAlert('Update failed due to a technical error.', 'danger');", true);
            }

        }





        //protected void LoadRequests(string status, string sortColumn, string sortDirection)
        //{
        //    var requests = AdminBLL.GetRequestsByStatus(status, sortColumn, sortDirection);
        //    gvRequests.DataSource = requests;
        //    gvRequests.DataBind();
        //}

        protected void GvRequests_Sorting(object sender, GridViewSortEventArgs e)
        {

            if (e.SortExpression == "RequestId")
                sortColumn = DbColumns.RequestId;
            else if (e.SortExpression == "RequestType")
                sortColumn = DbColumns.RequestType;
            else if (e.SortExpression == "RequestedOn")
                sortColumn = DbColumns.CreatedOn;

            // If same column clicked again → toggle direction
            if (e.SortExpression == sortColumn)
            {
                sortDirection = sortDirection == "ASC" ? "DESC" : "ASC";
            }
            else
            {
                //sortColumn = e.SortExpression;
                sortDirection = "DESC"; // default to DESC when new column clicked
            }
            LoadRequests(ddlFilterStatus.SelectedValue, sortColumn, sortDirection);
        }



        protected void BtnApprove_Click(object sender, EventArgs e)
        {
            int requestId = (int)ViewState["SelectedRequestId"];
            bool result = RequestDAL.UpdateRequestStatus(requestId, "Approved", -1);

            //pnlDetails.Visible = false;
            LoadRequests(ddlFilterStatus.SelectedValue, sortColumn, sortDirection);
        }

        protected void BtnReject_Click(object sender, EventArgs e)
        {
            int requestId = (int)ViewState["SelectedRequestId"];
            bool result = RequestDAL.UpdateRequestStatus(requestId, "Rejected", -1);

            //pnlDetails.Visible = false;
            LoadRequests(ddlFilterStatus.SelectedValue, sortColumn, sortDirection);
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            // Optional: Redirect to a detailed editor page with ID in querystring
            Response.Redirect($"~/WebForms/Admin/EditRequest.aspx?RequestId={ViewState["SelectedRequestId"]}");
        }

        protected void BtnShow2_Click(int requestId)
        {
            //if (int.TryParse(txtRequestId.Text.Trim(), out int requestId))
            //{
            var request = RequestDAL.GetAllRequestById(requestId);
            if (request != null)
            {
                try
                {
                    var client = JsonConvert.DeserializeObject<ClientDTO>(request.Payload);
                    if (client != null)
                    {

                        pnlRequestTable.Visible = false;
                        pnlRequestDetails.Visible = true;

                        lblRequestType.Text = request.RequestType;
                        lblRequestStatus.Text = request.Status;
                        if (request.Status == "Pending")
                        {
                            lblAdminApproval.Text = client.AdminApproved ? "Approved" : "Awaiting";
                            lblCoHolderApproval.Text = client.CoHolderApproved ? "Approved" : "Awaiting";
                        }

                        //pnlSummary.Visible = true;
                        lblRequestId.Text = $"#{requestId.ToString()}";
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
                        if (client.IsJointAccount)
                        {
                            fsJointAccount.Visible = true;
                        }
                        else
                        {
                            fsJointAccount.Visible = false;
                        }
                        txtJointClientId.Text = client.JointClientId != 0 ? client.JointClientId.ToString() : "";
                        txtUsername.Text = client.Username;
                        //txtPassword.Attributes["value"] = client.Password;

                        btnUpdate.Enabled = true;
                        //btnDelete.Enabled = true;
                    }
                    else
                    {
                        lblMessage.Text = "Failed to parse client request payload.";
                    }
                }
                catch
                {
                    lblMessage.Text = "Invalid payload format.";
                }
            }
            else
            {
                lblMessage.Text = "No request found with this ID.";
                pnlRequestDetails.Visible = false;
            }
            //}
            //else
            //{
            //    lblMessage.Text = "Enter a valid Request ID.";
            //}
        }

        protected void BtnUpdate2_Click(object sender, EventArgs e)
        {
            //if (int.TryParse(txtRequestId.Text.Trim(), out int requestId))
            //{
            int requestId = (int)ViewState["SelectedRequestId"];
            ClientDTO updatedClient = new ClientDTO
            {
                FullName = txtFullName.Text.Trim(),
                ParentName = txtParentName.Text.Trim(),
                DOB = txtDOB.Text,
                Gender = ddlGender.SelectedValue,
                Nationality = txtNationality.Text.Trim(),
                Occupation = txtOccupation.Text.Trim(),
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
                //Password = txtPassword.Text.Trim()
            };

            string updatedPayload = JsonConvert.SerializeObject(updatedClient);
            bool success = RequestDAL.UpdateRequestPayload(requestId, updatedPayload);
            string messageContent = success ? "Request updated successfully." : "Update failed.";
            string messageType = success ? "success" : "danger";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "customAlert", $"showAlert('{messageContent}', '{messageType}');", true);
            //}
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            //if (int.TryParse(txtRequestId.Text.Trim(), out int requestId))
            //{
            int requestId = (int)ViewState["SelectedRequestId"];
            bool success = RequestDAL.DeleteRequestByStatus(requestId, "Pending");
            if (success)
            {
                ClearForm();
                lblMessage.Text = "Request deleted successfully.";
            }
            else
            {
                lblMessage.Text = "Delete failed.";
            }
            //}
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
            //txtPassword.Text = "";
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