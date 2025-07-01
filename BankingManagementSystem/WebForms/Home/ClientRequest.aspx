<%@ Page Title="" Language="C#" MasterPageFile="~/WebForms/MasterPage.Master" AutoEventWireup="true" CodeBehind="ClientRequest.aspx.cs" Inherits="BankingManagementSystem.WebForms.Home.ClientRequest" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-5 ">
        <h3 class="mb-4">Client Request</h3>
        <div class="row mb-3">
            <div class="col-md-6">
                <asp:TextBox ID="txtRequestId" runat="server" CssClass="form-control" placeholder="Enter Request ID"></asp:TextBox>
            </div>
            <div class="col-md-2">
                <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary" Text="Search" OnClick="btnSearch_Click" />
            </div>
        </div>

        <!-- Summary Panel -->
<asp:Panel ID="pnlSummary" runat="server" Visible="false" CssClass="border rounded-3 p-3 bg-light mb-3 shadow-sm">
    <h5 class="mb-3">Request Summary</h5>
    <div class="row mb-2">
        <div class="col-md-4">
            <strong>Request ID:</strong>
            <asp:Label ID="lblSummaryRequestId" runat="server" CssClass="ms-2 text-dark fw-bold"></asp:Label>
        </div>
        <div class="col-md-4">
            <strong>Request Type:</strong>
            <asp:Label ID="lblSummaryRequestType" runat="server" CssClass="ms-2 text-dark"></asp:Label>
        </div>
        <div class="col-md-4">
            <strong>Created On:</strong>
            <asp:Label ID="lblSummaryCreatedOn" runat="server" CssClass="ms-2 text-dark"></asp:Label>
        </div>
    </div>
    <div class="row mb-2">
        <div class="col-md-4">
            <strong>Status:</strong>
            <asp:Label ID="lblSummaryStatus" runat="server" CssClass="ms-2 text-dark"></asp:Label>
        </div>
        <div class="col-md-4">
            <strong>Admin Approval:</strong>
            <asp:Label ID="lblSummaryAdminApproval" runat="server" CssClass="ms-2 text-dark"></asp:Label>
        </div>
        <div class="col-md-4">
            <strong>Co-holder Approval:</strong>
            <asp:Label ID="lblSummaryJointApproval" runat="server" CssClass="ms-2 text-dark"></asp:Label>
        </div>
    </div>
</asp:Panel>


        <asp:Panel ID="pnlRequestDetails" runat="server" Visible="false">
                <div class="container py-5">

        <div class="card shadow-sm rounded-4">
            <div class="card-body px-5">


                <%--<h2 class="mb-4 fw-bold border-bottom pb-2 text-center">Create Your Bank Account</h2>--%>
              <%--  <div class="d-flex align-items-center justify-content-between mb-4 pb-2 border-bottom">
                    <a href='<%= Page.GetRouteUrl("DashboardRoute", null) %>' class="btn btn-sm btn-light border"><i class="fas fa-angle-left"></i> Back to Home</a>

                    <h2 class="mb-0 mx-auto fw-bold text-center flex-grow-1">Create Your Bank Account</h2>

                    <!-- Invisible spacer to balance layout -->
                    <span style="width: 80px;"></span>
                </div>--%>


                <!-- PERSONAL DETAILS -->
                <h4 class="text-primary mb-3"><i class="fas fa-user-tag me-2 fs-5"></i>Personal Details</h4>
                <div class="row g-3">
                    <div class="col-md-3">
                        <label for="txtFullName" class="form-label">Full Name<span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" placeholder="Full Name"></asp:TextBox>
                    </div>
                    <div class="col-md-3">
                        <label for="txtParentName" class="form-label">Parent's Name<span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtParentName" runat="server" CssClass="form-control" placeholder="Parent's Name"></asp:TextBox>
                    </div>
                    <div class="col-md-3">
                        <label for="txtDOB" class="form-label">Date of Birth<span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtDOB" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                    </div>
                    <div class="col-md-3">
                        <label for="ddlGender" class="form-label">Gender<span class="text-danger">*</span></label>
                         <asp:DropDownList ID="ddlGender" runat="server" CssClass="form-select">
                            <asp:ListItem Text="Select" Value="" Disabled="true" Selected="True" />
                            <asp:ListItem Text="Male" Value="Male" />
                            <asp:ListItem Text="Female" Value="Female" />
                            <asp:ListItem Text="Other" Value="Other" />
                            <asp:ListItem Text="Prefer not to say" Value="PreferNotToSay" />
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-3">
                        <label for="txtNationality" class="form-label">Nationality<span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtNationality" runat="server" CssClass="form-control" placeholder="e.g., Indian"></asp:TextBox>
                    </div>
                    <div class="col-md-3">
                        <label for="txtOccupation" class="form-label">Occupation<span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtOccupation" runat="server" CssClass="form-control" placeholder="Your Occupation"></asp:TextBox>
                    </div>
                    <div class="col-md-3">
                        <label for="txtAadhaar" class="form-label">Aadhaar Number<span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtAadhaar" runat="server" CssClass="form-control" placeholder="0000 0000 0000"></asp:TextBox>
                    </div>
                    <div class="col-md-3">
                        <label for="txtPan" class="form-label">PAN Number<span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtPan" runat="server" CssClass="form-control" placeholder="ABCDE1234F"></asp:TextBox>
                    </div>

                </div>

                <hr class="my-4" />

              

                <!-- CONTACT DETAILS -->
                <h4 class="text-primary mb-3"><i class="fas fa-address-book me-2 fs-5"></i>Contact Details</h4>
                <div class="row g-3">
                    <div class="col-md-3">
                        <label for="txtMobile" class="form-label">Mobile Number<span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtMobile" runat="server" CssClass="form-control" placeholder="+91 9876543210" TextMode="Phone"></asp:TextBox>
                    </div>
                    <div class="col-md-3">
                        <label for="txtEmail" class="form-label">Email ID<span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="you@example.com" TextMode="Email"></asp:TextBox>
                    </div>
                    <div class="row g-3">
                        <div class="col-3">
                            <label for="txtAddress" class="form-label">Address</label>
                            <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="1" placeholder="Your complete address..."></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            <label for="txtState" class="form-label">State<span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtState" runat="server" CssClass="form-control" placeholder="State"></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            <label for="txtCity" class="form-label">City<span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtCity" runat="server" CssClass="form-control" placeholder="City"></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            <label for="txtPincode" class="form-label">Pincode<span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtPincode" runat="server" CssClass="form-control" placeholder="Pincode"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <hr class="my-4" />


                <!-- ACCOUNT DETAILS -->
                <h4 class="text-primary mb-3"><i class="fas fa-id-card me-2 fs-5"></i>Account Details</h4>
                <div class="row g-3">
                    <div class="col-md-3">
                        <label for="ddlAccountType" class="form-label">Account Type<span class="text-danger">*</span></label>
                        <asp:DropDownList ID="ddlAccountType" runat="server" CssClass="form-select">
                            <asp:ListItem Text="Select" Value="" Disabled="true" Selected="True" />
                            <asp:ListItem Text="Savings" Value="Savings" />
                            <asp:ListItem Text="Current" Value="Current" />
                        </asp:DropDownList>
                    </div>

                    <div class="col-md-3">
                        <label for="ddlIsJointAccount" class="form-label">Is Joint Account?<span class="text-danger">*</span></label>
                        <asp:DropDownList ID="ddlIsJointAccount" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="DdlIsJoint_SelectedIndexChanged">
                            <asp:ListItem Text="Select" Value="" Disabled="true" Selected="True" />
                            <asp:ListItem Text="Yes" Value="Yes" />
                            <asp:ListItem Text="No" Value="No" />
                        </asp:DropDownList>
                    </div>
                    <asp:Panel ID="fsJointAccount" runat="server" CssClass="col-md-6" Visible="false">
                        <div class="row g-3">
                            <div class="col-md-6">
                                <label for="txtJointClientId" class="form-label">Co-holder's Client ID<span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtJointClientId" runat="server" CssClass="form-control" placeholder="Client ID"></asp:TextBox>
                            </div>
                        </div>
                    </asp:Panel>
                </div>

                <hr class="my-4" />

                <!-- LOGIN CREDENTIALS -->
                <h4 class="text-primary mb-3"><i class="fas fa-lock me-2 fs-5"></i>Login Credentials</h4>
                <div class="row g-3">
                    <div class="col-md-3">
                        <label for="txtUsername" class="form-label">Username<span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" placeholder="Choose a username"></asp:TextBox>
                    </div>
                    <div class="col-md-3">
                        <label for="txtPassword" class="form-label">Password<span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Create a password"></asp:TextBox>
                    </div>
<%--                    <div class="col-md-3">
                        <label for="txtConfirmpassword" class="form-label">Confirm Password<span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtConfirmpassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Repeat password"></asp:TextBox>
                    </div>--%>
                </div>


<%--                <div class="form-check mt-4 d-flex align-items-center" style="margin-left: -1.5rem;">
                    <asp:CheckBox ID="CheckBox_terms" runat="server" CssClass="me-2" />
                    <label class="form-check-label" for="CheckBox_terms" style="font-size: 0.9rem;">
                        I agree to the <a href="#">Terms & Conditions</a> and <a href="#">Privacy Policy</a><span class="text-danger">*</span>
                    </label>
                </div>--%>
                <hr class="my-4" />

                <div class="d-flex justify-content-between">
                    <%--<span style="width: 80px;"></span>--%>
                    <%--                    <asp:Button ID="Button_clear" runat="server" CssClass="btn btn-sm btn-light border" Text="Clear Form" OnClick="BtnClear_Click" />--%>
                    <button type="button" class="btn btn-sm btn-light border" style="padding: 2px 10px; margin-top: 3px;" data-bs-toggle="modal" data-bs-target="#clearConfirmModal">Clear Form</button>

                    <%--<asp:Button ID="Button_submit" runat="server" CssClass="btn btn-primary btn-sm shadow-sm" Text="Submit" OnClick="BtnSubmit_Click" />--%>
                    <button type="button" class="btn btn-primary btn-sm shadow-sm" style="padding: 2px 10px; margin-top: 3px;" data-bs-toggle="modal" data-bs-target="#submitConfirmModal">Submit</button>
                </div>

            </div>
        </div>
    </div>
           


           

            <div class="mt-4">
                <asp:Button ID="btnUpdate" runat="server" CssClass="btn btn-success me-2" Text="Update" OnClick="btnUpdate_Click" />
                <asp:Button ID="btnDelete" runat="server" CssClass="btn btn-danger" Text="Delete Request" OnClick="btnDelete_Click" />
            </div>
        </asp:Panel>

        <asp:Label ID="lblMessage" runat="server" CssClass="text-danger mt-3 d-block"></asp:Label>
    </div>
</asp:Content>
