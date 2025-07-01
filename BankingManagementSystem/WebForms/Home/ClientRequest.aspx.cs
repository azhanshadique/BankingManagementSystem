using System;
using System.Web.UI;
using Newtonsoft.Json;
using BankingManagementSystem.Models.DTOs;
using BankingManagementSystem.DAL;

namespace BankingManagementSystem.WebForms.Home
{
    public partial class ClientRequest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Text = "";
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtRequestId.Text.Trim(), out int requestId))
            {
                var request = RequestDAL.GetPendingRequestById(requestId);
                if (request != null)
                {
                    try
                    {
                        var client = JsonConvert.DeserializeObject<ClientDTO>(request.Payload);
                        if (client != null)
                        {
                            //pnlRequestDetails.Visible = true;
                            pnlSummary.Visible = true;

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
                            btnDelete.Enabled = true;
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
            }
            else
            {
                lblMessage.Text = "Enter a valid Request ID.";
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtRequestId.Text.Trim(), out int requestId))
            {
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
                lblMessage.Text = success ? "Request updated successfully." : "Update failed.";
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtRequestId.Text.Trim(), out int requestId))
            {
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
