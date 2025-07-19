<%@ Page Title="" Language="C#" Async="true" MasterPageFile="~/WebForms/MasterPage.Master" AutoEventWireup="true" CodeBehind="ClientSignupLinkAccount.aspx.cs" Inherits="BankingManagementSystem.WebForms.SignUp.Link.ClientSignupCreateAccountLinkAccount" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container py-5">
        <div class="card shadow-lg rounded-4">
            <div class="card-body px-5">
                <%--<h2 class="mb-4 fw-bold border-bottom pb-2 text-center">Link to Existing Account</h2>--%>
                <div class="d-flex align-items-center justify-content-between mb-4 pb-2 border-bottom">
                    <a href='<%= Page.GetRouteUrl("DashboardRoute", null) %>' class="btn btn-sm btn-light border"><i class="fas fa-angle-left"></i> Back to Home</a>

                    <h2 class="mb-0 mx-auto text-center flex-grow-1">Link to Existing Account</h2>

                    <!-- Invisible spacer to balance layout -->
                    <span style="width: 80px;"></span>
                </div>
                <!-- ACCOUNT DETAILS -->
                <h4 class="text-primary mb-3"><i class="fas fa-id-card me-2 fs-5"></i>Account Details</h4>
                <div class="row g-3">
                    <div class="col-md-3">
                        <label for="txtAccountNumber" class="form-label">Account Number<span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtAccountNumber" runat="server" CssClass="form-control" placeholder="XXXXX XXXXX"></asp:TextBox>
                    </div>
                    <div class="col-md-3">
                        <label for="ddlAccountType" class="form-label">Account Type<span class="text-danger">*</span></label>
                        <asp:DropDownList ID="ddlAccountType" runat="server" CssClass="form-select">
                            <asp:ListItem Text="Select" Value="" Disabled="true" Selected="True" />
                            <asp:ListItem Text="Savings" Value="Savings" />
                            <asp:ListItem Text="Current" Value="Current" />
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-3">
                        <label for="txtMobileNumber" class="form-label">Registered Mobile Number<span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtMobileNumber" runat="server" CssClass="form-control" placeholder="9876543210" TextMode="Phone"></asp:TextBox>
                    </div>
                    <div class="col-md-3">
                        <label for="txtEmailId" class="form-label">Email ID<span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtEmailId" runat="server" CssClass="form-control" placeholder="you@example.com" TextMode="Email"></asp:TextBox>
                    </div>
                    <div class="col-md-3">
                        <label for="txtClientId" class="form-label">Client ID<span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtClientId" runat="server" CssClass="form-control" placeholder="XXXXXX"></asp:TextBox>
                    </div>

                    <div class="col-md-3">
                        <label for="ddlIsJointAccount" class="form-label">Is Joint Account?</label>
                        <asp:DropDownList ID="ddlIsJointAccount" runat="server" CssClass="form-select" AutoPostBack="True" OnSelectedIndexChanged="DdlIsJoint_SelectedIndexChanged">
                            <asp:ListItem Text="Select" Value="" Disabled="true" Selected="True" />
                            <asp:ListItem Text="Yes" Value="Yes" />
                            <asp:ListItem Text="No" Value="No" />
                        </asp:DropDownList>
                    </div>

                    <asp:Panel ID="fsJointAccount" runat="server" CssClass="col-md-6" Visible="false">
                        <div class="row g-3">
                            <div class="col-md-6">
                                <label for="txtCoholderClientId" class="form-label">Co-holder's Client ID<span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtCoholderClientId" runat="server" CssClass="form-control" placeholder="XXXXXX"></asp:TextBox>
                            </div>
                            <div class="col-md-6">
                                <label for="txtCoholderMobile" class="form-label">Co-holder's Registered Mobile No<span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtCoholderMobile" runat="server" CssClass="form-control" placeholder="9876543210"></asp:TextBox>
                            </div>
                        </div>

                    </asp:Panel>

                </div>

                <hr class="my-4" />

                <!-- LOGIN CREDENTIALS -->
                <h4 class="text-primary mb-3"><i class="fas fa-lock me-2 fs-5"></i>Login Credentials</h4>
                <div class="row g-3">
                    <div class="col-md-3">
                        <label for="txtUsername" class="form-label">Create Username<span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" placeholder="Username"></asp:TextBox>
                    </div>

                    <div class="col-md-3">
                        <label for="txtPassword" class="form-label">Create Password<span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" placeholder="New Password" TextMode="Password"></asp:TextBox>
                    </div>
                    <div class="col-md-3">
                        <label for="txtConfirmPassword" class="form-label">Confirm Password<span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" placeholder="Confirm Password" TextMode="Password"></asp:TextBox>
                    </div>
                </div>

                <div class="form-check mt-4 d-flex align-items-center" style="margin-left: -1.5rem;">
                    <asp:CheckBox ID="chkBoxTerms" runat="server" CssClass="me-2" />
                    <label for="chkBoxTerms" class="form-check-label" style="font-size: 0.9rem;">
                        I agree to the <a href="#">Terms & Conditions</a> and <a href="#">Privacy Policy</a><span class="text-danger">*</span>
                    </label>
                </div>

                <hr class="my-4" />

                <div class="d-flex justify-content-between">
                    <%--<span style="width: 80px;"></span>--%>
                     <button type="button" class="btn btn-sm btn-light border" style="padding: 2px 10px; margin-top: 3px;" data-bs-toggle="modal" data-bs-target="#clearConfirmModal">Clear Form</button>
                    <button type="button" class="btn btn-primary btn-sm shadow-sm" style="padding: 2px 10px; margin-top: 3px;" data-bs-toggle="modal" data-bs-target="#submitConfirmModal">Submit</button>
                </div>
            </div>
        </div>
    </div>
     <!-- Submit Confirmation Modal -->
    <div class="modal fade" id="submitConfirmModal" tabindex="-1" aria-labelledby="submitConfirmModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content rounded-4 shadow">
                <div class="modal-header bg-primary text-white rounded-top-4" style="height: 60px;">
                    <h5 class="modal-title" id="submitConfirmModalLabel">Confirm Submit</h5>
                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body text-start">
                    Are you sure you want to submit?
                </div>
                <div class="modal-footer justify-content-end border-0">
                    <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-primary py-1 px-3" Text="Submit" OnClick="BtnSubmit_Click" />
                    <button type="button" class="btn btn-light border py-1 px-3" data-bs-dismiss="modal">Cancel</button>
                </div>
            </div>
        </div>
    </div>

    <!-- Clear Form Confirmation Modal -->
    <div class="modal fade" id="clearConfirmModal" tabindex="-1" aria-labelledby="clearConfirmModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content rounded-4 shadow">
                <div class="modal-header bg-secondary text-white rounded-top-4" style="height: 60px;">
                    <h5 class="modal-title" id="clearConfirmModalLabel">Confirm Clear Form</h5>
                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body text-start">
                    Are you sure you want to clear the form?
                </div>
                <div class="modal-footer justify-content-end border-0">
                    <asp:Button ID="btnClear" runat="server" CssClass="btn btn-secondary py-1 px-3" Text="Clear Form" OnClick="BtnClear_Click" />
                    <button type="button" class="btn btn-light border py-1 px-3" data-bs-dismiss="modal">Cancel</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
