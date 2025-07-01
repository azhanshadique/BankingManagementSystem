using BankingManagementSystem.BLL;
using BankingManagementSystem.DAL;
using BankingManagementSystem.Models.Constants;
using BankingManagementSystem.Models.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
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
        private static string sortColumn = DbColumns.CreatedOn; // default column
        private static string sortDirection = "DESC";   // default direction
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadRequests("Pending", sortColumn, sortDirection);
            }
        }

        protected void DdlFilterStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadRequests(ddlFilterStatus.SelectedValue, sortColumn, sortDirection);
        }

        protected void LoadRequests(string status, string sortColumn, string sortDirection)
        {
            var requests = AdminBLL.GetRequestsByStatus(status, sortColumn, sortDirection);
            gvRequests.DataSource = requests;
            gvRequests.DataBind();
        }

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

        protected void GvRequests_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Show")
            {
                int requestId = Convert.ToInt32(e.CommandArgument);
                ViewState["SelectedRequestId"] = requestId;
                BtnShow_Click(requestId);
                //var request = RequestDAL.GetRequestDetailsForAdmin(requestId);

                //if (request != null)
                //{
                //    ViewState["SelectedRequestId"] = request.RequestId;
                //    litRequestDetails.Text = $@"
                //        <strong>Request ID:</strong> {request.RequestId}<br/>
                //        <strong>Type:</strong> {request.RequestType}<br/>
                //     </pre>";

                //    pnlDetails.Visible = true;
                //}
            }
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

        protected void BtnShow_Click(int requestId)
        {
            //if (int.TryParse(txtRequestId.Text.Trim(), out int requestId))
            //{
            var request = RequestDAL.GetPendingRequestById(requestId);
            if (request != null)
            {
                try
                {
                    var client = JsonConvert.DeserializeObject<ClientDTO>(request.Payload);
                    if (client != null)
                    {

                        pnlRequestTable.Visible = false;
                        pnlRequestDetails.Visible = true;
                        //pnlSummary.Visible = true;
                        lblRequestId.Text = "Request ID:  #" + requestId.ToString();
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

        protected void BtnUpdate_Click(object sender, EventArgs e)
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
                Password = txtPassword.Text.Trim()
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
            bool success = RequestDAL.DeletePendingRequest(requestId);
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