<%@ Page Title="" Language="C#" Async="true" MasterPageFile="~/WebForms/MasterPage.Master" AutoEventWireup="true" CodeBehind="ClientProfile.aspx.cs" Inherits="BankingManagementSystem.WebForms.Client.ClientProfile" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

        <%--Profile Details--%>
    <asp:Panel ID="pnlRequestDetails" runat="server" Visible="true">
        <div class="container py-5">

            <div class="card shadow-sm rounded-4">
                <div class="card-body px-5">


                    <div class="d-flex align-items-center justify-content-between mb-4 pb-2 border-bottom">
                        <a href='<%= Page.GetRouteUrl("DashboardRoute", null) %>' class="btn btn-sm btn-light border"><i class="fas fa-angle-left"></i> Back</a>
                        <%--<asp:Button ID="Button1" runat="server" CssClass="btn btn-sm btn-light border" Style="padding: 3px 10px;" Text="Back" OnClick="BtnReject_Click" />--%>

                        <h2 class="mb-0 mx-auto text-center flex-grow-1">Profile Details</h2>

                        <!-- Invisible spacer to balance layout -->
                        <%--<span style="width: 80px;"></span>--%>
                      <%--  <div class="d-flex justify-content-center align-items-center">
                            <h6 class="text-muted mt-2 fs-6 fw-medium">Request ID: &nbsp; </h6>
                            <asp:Label ID="lblRequestId" runat="server" CssClass="text-dark  d-block fs-6 fw-semibold"></asp:Label>
                        </div>--%>
                    </div>


                 

                    <!-- PERSONAL DETAILS -->
                    <h4 class="text-primary mb-3"><i class="fas fa-user-tag me-2 fs-5"></i>Personal Details</h4>
                    <div class="row g-3">
                        <div class="col-md-3">
                            <label for="txtFullName" class="form-label">Full Name<span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" placeholder="Full Name" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            <label for="txtParentName" class="form-label">Parent's Name<span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtParentName" runat="server" CssClass="form-control" placeholder="Parent's Name" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            <label for="txtDOB" class="form-label">Date of Birth<span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtDOB" runat="server" CssClass="form-control" TextMode="Date" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            <label for="ddlGender" class="form-label">Gender<span class="text-danger">*</span></label>
                            <asp:DropDownList ID="ddlGender" runat="server" CssClass="form-select" Enabled="false">
                                <asp:ListItem Text="Select" Value="" Disabled="true" Selected="True" />
                                <asp:ListItem Text="Male" Value="Male" />
                                <asp:ListItem Text="Female" Value="Female" />
                                <asp:ListItem Text="Other" Value="Other" />
                                <asp:ListItem Text="Prefer not to say" Value="PreferNotToSay" />
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-3">
                            <label for="txtNationality" class="form-label">Nationality<span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtNationality" runat="server" CssClass="form-control" placeholder="e.g., Indian" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            <label for="txtOccupation" class="form-label">Occupation<span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtOccupation" runat="server" CssClass="form-control" placeholder="Your Occupation" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            <label for="txtAadhaar" class="form-label">Aadhaar Number<span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtAadhaar" runat="server" CssClass="form-control" placeholder="0000 0000 0000" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            <label for="txtPan" class="form-label">PAN Number<span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtPan" runat="server" CssClass="form-control" placeholder="ABCDE1234F" ReadOnly="true"></asp:TextBox>
                        </div>

                    </div>

                    <hr class="my-4" />



                    <!-- CONTACT DETAILS -->
                    <h4 class="text-primary mb-3"><i class="fas fa-address-book me-2 fs-5"></i>Contact Details</h4>
                    <div class="row g-3">
                        <div class="col-md-3">
                            <label for="txtMobile" class="form-label">Mobile Number<span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtMobile" runat="server" CssClass="form-control" placeholder="+91 9876543210" TextMode="Phone" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            <label for="txtEmail" class="form-label">Email ID<span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="you@example.com" TextMode="Email" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="row g-3">
                            <div class="col-3">
                                <label for="txtAddress" class="form-label">Address<span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="1" placeholder="Your complete address..." ReadOnly="true"></asp:TextBox>
                            </div>
                            <div class="col-md-3">
                                <label for="txtState" class="form-label">State<span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtState" runat="server" CssClass="form-control" placeholder="State" ReadOnly="true"></asp:TextBox>
                            </div>
                            <div class="col-md-3">
                                <label for="txtCity" class="form-label">City<span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtCity" runat="server" CssClass="form-control" placeholder="City" ReadOnly="true"></asp:TextBox>
                            </div>
                            <div class="col-md-3">
                                <label for="txtPincode" class="form-label">Pincode<span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtPincode" runat="server" CssClass="form-control" placeholder="Pincode" ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                    <hr class="my-4" />


                    <!-- ACCOUNT DETAILS -->
                  <%--  <h4 class="text-primary mb-3"><i class="fas fa-id-card me-2 fs-5"></i>Account Details</h4>
                    <div class="row g-3">
                        <div class="col-md-3">
                            <label for="ddlAccountType" class="form-label">Account Type<span class="text-danger">*</span></label>
                            <asp:DropDownList ID="ddlAccountType" runat="server" CssClass="form-select" Enabled="false">
                                <asp:ListItem Text="Select" Value="" Disabled="true" Selected="True" />
                                <asp:ListItem Text="Savings" Value="Savings" />
                                <asp:ListItem Text="Current" Value="Current" />
                            </asp:DropDownList>
                        </div>

                        <div class="col-md-3">
                            <label for="ddlIsJointAccount" class="form-label">Is Joint Account?<span class="text-danger">*</span></label>
                            <asp:DropDownList ID="ddlIsJointAccount" runat="server" CssClass="form-select" AutoPostBack="true" Enabled="false" OnSelectedIndexChanged="DdlIsJoint_SelectedIndexChanged">
                                <asp:ListItem Text="Select" Value="" Disabled="true" Selected="True" />
                                <asp:ListItem Text="Yes" Value="Yes" />
                                <asp:ListItem Text="No" Value="No" />
                            </asp:DropDownList>
                        </div>
                        <asp:Panel ID="fsJointAccount" runat="server" CssClass="col-md-6" Visible="false">
                            <div class="row g-3">
                                <div class="col-md-6">
                                    <label for="txtJointClientId" class="form-label">Your Client ID<span class="text-danger">*</span></label>
                                    <asp:TextBox ID="txtJointClientId" runat="server" CssClass="form-control" placeholder="Client ID" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                        </asp:Panel>
                    </div>

                    <hr class="my-4" />--%>

                    <!-- LOGIN CREDENTIALS -->
                    <h4 class="text-primary mb-3"><i class="fas fa-lock me-2 fs-5"></i>Other Details</h4>
                    <div class="row g-3">
                        <div class="col-md-3">
                            <label for="txtUsername" class="form-label">Username<span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" placeholder="Choose a username" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-md-3" style="display: none">
                            <label for="txtPassword" class="form-label">Password<span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Create a password"></asp:TextBox>
                        </div>
                        <div class="col-md-3" style="display: none">
                            <label for="txtConfirmpassword" class="form-label">Confirm Password<span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Repeat password"></asp:TextBox>
                        </div>
                        <div class="col-md-3" style="display: block">
                            <label for="txtClientId" class="form-label">Client ID<span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtClientId" runat="server" CssClass="form-control" placeholder="xxxxxx" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>



                    <hr class="my-4" />

                    <div class="d-flex justify-content-end gap-3">
                       <%-- <asp:Panel ID="pnlBtnsAproveReject" runat="server" Visible="true">
                            <asp:Button ID="btnApprove" runat="server" CssClass="btn btn-success btn-sm shadow-sm  fs-6 me-2" Style="padding: 2px 10px;" Text="Approve" OnClick="BtnApprove_Click" />
                            <button type="button" class="btn btn-danger btn-sm shadow-sm  fs-6 me-2" style="padding: 2px 10px; margin-top: 2px;" data-bs-toggle="modal" data-bs-target="#rejectConfirmModal">Reject</button>
                        </asp:Panel>--%>
                        <asp:Panel ID="pnlBntsEditUpdateDlt" runat="server" Visible="true">
                            <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-secondary btn-sm shadow-sm  fs-6 me-2" Style="padding: 2px 10px; margin-top: 3px;" Text=" Cancel " OnClick="BtnCancel_Click" Visible="false" />

                            <asp:Button ID="btnEdit" runat="server" CssClass="btn btn-primary btn-sm shadow-sm  fs-6 me-2" Style="padding: 2px 10px; margin-top: 3px;" Text=" Edit " OnClick="BtnEdit_Click" />
                            <asp:Button ID="btnUpdate" runat="server" CssClass="btn btn-warning btn-sm shadow-sm  fs-6 me-2" Style="padding: 2px 10px; margin-top: 3px;" Text="Update" OnClick="BtnUpdate_Click" Visible="false" />
                            <%--<button type="button" class="btn btn-danger btn-sm shadow-sm  fs-6" style="padding: 2px 10px; margin-top: 2px;" data-bs-toggle="modal" data-bs-target="#deleteConfirmModal">Delete</button>--%>
                        </asp:Panel>
                    </div>

                </div>
            </div>
        </div>

    </asp:Panel>

</asp:Content>
