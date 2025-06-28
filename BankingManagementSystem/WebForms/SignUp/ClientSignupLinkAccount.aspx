<%@ Page Title="" Language="C#" MasterPageFile="~/WebForms/MasterPage.Master" AutoEventWireup="true" CodeBehind="ClientSignupCreateAccountLinkAccount.aspx.cs" Inherits="BankingManagementSystem.WebForms.SignUp.ClientSignupCreateAccountLinkAccount" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container py-5">
        <div class="card shadow-lg rounded-4">
            <div class="card-body px-5">
                <%--<h2 class="mb-4 fw-bold border-bottom pb-2 text-center">Link to Existing Account</h2>--%>
                <div class="d-flex align-items-center justify-content-between mb-4 pb-2 border-bottom">
                    <a href='<%= Page.GetRouteUrl("DashboardRoute", null) %>' class="btn btn-sm btn-light border"><i class="fas fa-angle-left"></i> Back to Home</a>

                    <h2 class="mb-0 mx-auto fw-bold text-center flex-grow-1">Link to Existing Account</h2>

                    <!-- Invisible spacer to balance layout -->
                    <span style="width: 80px;"></span>
                </div>
                <!-- ACCOUNT DETAILS -->
                <h4 class="text-primary mb-3"><i class="fas fa-id-card me-2 fs-5"></i>Account Details</h4>
                <div class="row g-3">
                    <div class="col-md-3">
                        <label for="TextBox_account_no" class="form-label">Account Number<span class="text-danger">*</span></label>
                        <asp:TextBox ID="TextBox_account_no" runat="server" CssClass="form-control" placeholder="XXXXX XXXXX"></asp:TextBox>
                    </div>
                    <div class="col-md-3">
                        <label for="ddl_accounttype" class="form-label">Account Type<span class="text-danger">*</span></label>
                        <asp:DropDownList ID="ddl_accounttype" runat="server" CssClass="form-select">
                            <asp:ListItem Text="Select" Value="" Disabled="true" Selected="True" />
                            <asp:ListItem Text="Savings" Value="Savings" />
                            <asp:ListItem Text="Current" Value="Current" />
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-3">
                        <label for="TextBox_mobile_no" class="form-label">Registered Mobile Number<span class="text-danger">*</span></label>
                        <asp:TextBox ID="TextBox_mobile_no" runat="server" CssClass="form-control" placeholder="+91 9876543210" TextMode="Phone"></asp:TextBox>
                    </div>
                    <div class="col-md-3">
                        <label for="TextBox_email_id" class="form-label">Email ID<span class="text-danger">*</span></label>
                        <asp:TextBox ID="TextBox_email_id" runat="server" CssClass="form-control" placeholder="you@example.com" TextMode="Email"></asp:TextBox>
                    </div>
                    <div class="col-md-3">
                        <label for="TextBox_client_id" class="form-label">Client ID<span class="text-danger">*</span></label>
                        <asp:TextBox ID="TextBox_client_id" runat="server" CssClass="form-control" placeholder="XXXXXX"></asp:TextBox>
                    </div>

                    <div class="col-md-3">
                        <label for="CheckBox_is_joint_acc" class="form-label">Is Joint Account?</label>
                        <asp:DropDownList ID="CheckBox_is_joint_acc" runat="server" CssClass="form-select" AutoPostBack="True">
                            <asp:ListItem Text="Select" Value="" Disabled="true" Selected="True" />
                            <asp:ListItem Text="Yes" Value="Yes" />
                            <asp:ListItem Text="No" Value="No" />
                        </asp:DropDownList>
                    </div>

                    <asp:Panel ID="fsJointAccount" runat="server" CssClass="col-md-6" Visible="true">
                        <div class="row g-3">
                            <div class="col-md-6">
                                <label for="TextBox_coholder_clientid" class="form-label">Co-holder's Client ID<span class="text-danger">*</span></label>
                                <asp:TextBox ID="TextBox_coholder_clientid" runat="server" CssClass="form-control" placeholder="XXXXXX"></asp:TextBox>
                            </div>
                            <div class="col-md-6">
                                <label for="TextBox_coholder_mobile" class="form-label">Co-holder's Registered Mobile No<span class="text-danger">*</span></label>
                                <asp:TextBox ID="TextBox_coholder_mobile" runat="server" CssClass="form-control" placeholder="+91 9876543210"></asp:TextBox>
                            </div>
                        </div>

                    </asp:Panel>

                </div>

                <hr class="my-4" />

                <!-- LOGIN CREDENTIALS -->
                <h4 class="text-primary mb-3"><i class="fas fa-lock me-2 fs-5"></i>Login Credentials</h4>
                <div class="row g-3">
                    <div class="col-md-3">
                        <label for="TextBox_username" class="form-label">Create Username<span class="text-danger">*</span></label>
                        <asp:TextBox ID="TextBox_username" runat="server" CssClass="form-control" placeholder="Username"></asp:TextBox>
                    </div>

                    <div class="col-md-3">
                        <label for="TextBox_password" class="form-label">Create Password<span class="text-danger">*</span></label>
                        <asp:TextBox ID="TextBox_password" runat="server" CssClass="form-control" placeholder="New Password" TextMode="Password"></asp:TextBox>
                    </div>
                    <div class="col-md-3">
                        <label for="TextBox_confirm_password" class="form-label">Confirm Password<span class="text-danger">*</span></label>
                        <asp:TextBox ID="TextBox_confirm_password" runat="server" CssClass="form-control" placeholder="Confirm Password" TextMode="Password"></asp:TextBox>
                    </div>
                </div>

                <div class="form-check mt-4 d-flex align-items-center" style="margin-left: -1.5rem;">
                    <asp:CheckBox ID="CheckBox_terms" runat="server" CssClass="me-2" />
                    <label class="form-check-label" for="CheckBox_terms" style="font-size: 0.9rem;">
                        I agree to the <a href="#">Terms & Conditions</a> and <a href="#">Privacy Policy</a><span class="text-danger">*</span>
                    </label>
                </div>

                <hr class="my-4" />

                <div class="d-flex justify-content-between">
                    <%--<span style="width: 80px;"></span>--%>
                    <asp:Button ID="Button_clear" runat="server" CssClass="btn btn-primary btn-sm shadow-sm" Text="Clear Form" />
                    <asp:Button ID="Button_submit" runat="server" CssClass="btn btn-primary btn-sm shadow-sm" Text="Submit" />
                </div>
            </div>
        </div>
    </div>

</asp:Content>
