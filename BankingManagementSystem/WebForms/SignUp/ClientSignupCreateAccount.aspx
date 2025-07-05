<%@ Page Title="" Language="C#" MasterPageFile="~/WebForms/MasterPage.Master" AutoEventWireup="true" CodeBehind="ClientSignupCreateAccount.aspx.cs" Inherits="BankingManagementSystem.WebForms.SignUp.ClientSignupCreateAccount" Async="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


    <div class="container py-5">

        <div class="card shadow-lg rounded-4">
            <div class="card-body px-5">


                <%--<h2 class="mb-4 fw-bold border-bottom pb-2 text-center">Create Your Bank Account</h2>--%>
                <div class="d-flex align-items-center justify-content-between mb-4 pb-2 border-bottom">
                    <a href='<%= Page.GetRouteUrl("DashboardRoute", null) %>' class="btn btn-sm btn-light border"><i class="fas fa-angle-left"></i> Back to Home</a>

                    <h2 class="mb-0 mx-auto text-center flex-grow-1">Create Your Bank Account</h2>

                    <!-- Invisible spacer to balance layout -->
                    <span style="width: 80px;"></span>
                </div>


                <!-- PERSONAL DETAILS -->
                <h4 class="text-primary mb-3"><i class="fas fa-user-tag me-2 fs-5"></i>Personal Details</h4>
                <div class="row g-3">
                    <div class="col-md-3">
                        <label for="TextBox_fullname" class="form-label">Full Name<span class="text-danger">*</span></label>
                        <asp:TextBox ID="TextBox_fullname" runat="server" CssClass="form-control" placeholder="Full Name"></asp:TextBox>
                    </div>
                    <div class="col-md-3">
                        <label for="TextBox_parentname" class="form-label">Parent's Name<span class="text-danger">*</span></label>
                        <asp:TextBox ID="TextBox_parentname" runat="server" CssClass="form-control" placeholder="Parent's Name"></asp:TextBox>
                    </div>
                    <div class="col-md-3">
                        <label for="TextBox_dob" class="form-label">Date of Birth<span class="text-danger">*</span></label>
                        <asp:TextBox ID="TextBox_dob" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                    </div>
                    <div class="col-md-3">
                        <label for="ddl_gender" class="form-label">Gender<span class="text-danger">*</span></label>
                        <asp:DropDownList ID="ddl_gender" runat="server" CssClass="form-select">
                            <asp:ListItem Text="Select" Value="" Disabled="true" Selected="True" />
                            <asp:ListItem Text="Male" Value="Male" />
                            <asp:ListItem Text="Female" Value="Female" />
                            <asp:ListItem Text="Other" Value="Other" />
                            <asp:ListItem Text="Prefer not to say" Value="PreferNotToSay" />
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-3">
                        <label for="TextBox_nationality" class="form-label">Nationality<span class="text-danger">*</span></label>
                        <asp:TextBox ID="TextBox_nationality" runat="server" CssClass="form-control" placeholder="e.g., Indian"></asp:TextBox>
                    </div>
                    <div class="col-md-3">
                        <label for="TextBox_occupation" class="form-label">Occupation<span class="text-danger">*</span></label>
                        <asp:TextBox ID="TextBox_occupation" runat="server" CssClass="form-control" placeholder="Your Occupation"></asp:TextBox>
                    </div>
                    <div class="col-md-3">
                        <label for="TextBox_aadhaar" class="form-label">Aadhaar Number<span class="text-danger">*</span></label>
                        <asp:TextBox ID="TextBox_aadhaar" runat="server" CssClass="form-control" placeholder="0000 0000 0000"></asp:TextBox>
                    </div>
                    <div class="col-md-3">
                        <label for="TextBox_pan" class="form-label">PAN Number<span class="text-danger">*</span></label>
                        <asp:TextBox ID="TextBox_pan" runat="server" CssClass="form-control" placeholder="ABCDE1234F"></asp:TextBox>
                    </div>

                </div>

                <hr class="my-4" />

                <!-- CONTACT DETAILS -->
                <h4 class="text-primary mb-3"><i class="fas fa-address-book me-2 fs-5"></i>Contact Details</h4>
                <div class="row g-3">
                    <div class="col-md-3">
                        <label for="TextBox_mobilenumber" class="form-label">Mobile Number<span class="text-danger">*</span></label>
                        <asp:TextBox ID="TextBox_mobilenumber" runat="server" CssClass="form-control" placeholder="9876543210" TextMode="Phone"></asp:TextBox>
                    </div>
                    <div class="col-md-3">
                        <label for="TextBox_emailid" class="form-label">Email ID<span class="text-danger">*</span></label>
                        <asp:TextBox ID="TextBox_emailid" runat="server" CssClass="form-control" placeholder="you@example.com" TextMode="Email"></asp:TextBox>
                    </div>
                    <div class="row g-3">
                        <div class="col-3">
                            <label for="TextBox_fulladdress" class="form-label">Address</label>
                            <asp:TextBox ID="TextBox_fulladdress" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="1" placeholder="Your complete address..."></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            <label for="TextBox_state" class="form-label">State<span class="text-danger">*</span></label>
                            <asp:TextBox ID="TextBox_state" runat="server" CssClass="form-control" placeholder="State"></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            <label for="TextBox_city" class="form-label">City<span class="text-danger">*</span></label>
                            <asp:TextBox ID="TextBox_city" runat="server" CssClass="form-control" placeholder="City"></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            <label for="TextBox_pincode" class="form-label">Pincode<span class="text-danger">*</span></label>
                            <asp:TextBox ID="TextBox_pincode" runat="server" CssClass="form-control" placeholder="700000"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <hr class="my-4" />

                <!-- ACCOUNT DETAILS -->
                <h4 class="text-primary mb-3"><i class="fas fa-id-card me-2 fs-5"></i>Account Details</h4>
                <div class="row g-3">
                    <div class="col-md-3">
                        <label for="ddl_accounttype" class="form-label">Account Type<span class="text-danger">*</span></label>
                        <asp:DropDownList ID="ddl_accounttype" runat="server" CssClass="form-select">
                            <asp:ListItem Text="Select" Value="" Disabled="true" Selected="True" />
                            <asp:ListItem Text="Savings" Value="Savings" />
                            <asp:ListItem Text="Current" Value="Current" />
                        </asp:DropDownList>
                    </div>
                    <%-- <div class="col-md-3 d-flex align-items-end gap-3">
                        <asp:CheckBox ID="CheckBox_isjointacc" runat="server" AutoPostBack="True" />
                        <label for="CheckBox_isjointacc" class="form-label mb-0">Is Joint Account?</label>
                    </div>--%>
                    <div class="col-md-3">
                        <label for="CheckBox_isjointacc" class="form-label">Is Joint Account?<span class="text-danger">*</span></label>
                        <asp:DropDownList ID="ddlIsJointAccount" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="DdlIsJoint_SelectedIndexChanged">
                            <asp:ListItem Text="Select" Value="" Disabled="true" Selected="True" />
                            <asp:ListItem Text="Yes" Value="Yes" />
                            <asp:ListItem Text="No" Value="No" />
                        </asp:DropDownList>
                    </div>
                    <asp:Panel ID="fsJointAccount" runat="server" CssClass="col-md-6" Visible="false">
                        <div class="row g-3">
                            <div class="col-md-6">
                                <label for="TextBox_jointaccclient" class="form-label">Co-holder's Client ID<span class="text-danger">*</span></label>
                                <asp:TextBox ID="TextBox_jointaccclient" runat="server" CssClass="form-control" placeholder="Client ID"></asp:TextBox>
                            </div>
                            <%-- <div class="col-md-6">
                                <label for="TextBox_relationship" class="form-label">Relationship<span class="text-danger">*</span></label>
                                <asp:TextBox ID="TextBox_relationship" runat="server" CssClass="form-control" placeholder="Relationship"></asp:TextBox>
                            </div>--%>
                        </div>
                    </asp:Panel>
                </div>

                <hr class="my-4" />

                <!-- LOGIN CREDENTIALS -->
                <h4 class="text-primary mb-3"><i class="fas fa-lock me-2 fs-5"></i>Login Credentials</h4>
                <div class="row g-3">
                    <div class="col-md-3">
                        <label for="TextBox_username" class="form-label">Username<span class="text-danger">*</span></label>
                        <asp:TextBox ID="TextBox_username" runat="server" CssClass="form-control" placeholder="Choose a username"></asp:TextBox>
                    </div>
                    <div class="col-md-3">
                        <label for="TextBox_password" class="form-label">Password<span class="text-danger">*</span></label>
                        <asp:TextBox ID="TextBox_password" runat="server" CssClass="form-control" TextMode="Password" placeholder="Create a password"></asp:TextBox>
                    </div>
                    <div class="col-md-3">
                        <label for="TextBox_confirmpassword" class="form-label">Confirm Password<span class="text-danger">*</span></label>
                        <asp:TextBox ID="TextBox_confirmpassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Repeat password"></asp:TextBox>
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
                    <%--                    <asp:Button ID="Button_clear" runat="server" CssClass="btn btn-sm btn-light border" Text="Clear Form" OnClick="BtnClear_Click" />--%>
                    <button type="button" class="btn btn-sm btn-light border" style="padding: 2px 10px; margin-top: 3px;" data-bs-toggle="modal" data-bs-target="#clearConfirmModal">Clear Form</button>

                    <%--<asp:Button ID="Button_submit" runat="server" CssClass="btn btn-primary btn-sm shadow-sm" Text="Submit" OnClick="BtnSubmit_Click" />--%>
                    <button type="button" class="btn btn-primary btn-sm shadow-sm" style="padding: 2px 10px; margin-top: 3px;" data-bs-toggle="modal" data-bs-target="#submitConfirmModal">Submit</button>
                </div>

            </div>
        </div>
    </div>

    <!-- Submit Confirmation Modal -->
    <div class="modal fade" id="submitConfirmModal" tabindex="-1" aria-labelledby="submitConfirmModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content rounded-4 shadow">
                <div class="modal-header bg-primary text-white rounded-top-4" style="height: 60px; border-top-left-radius: 14px; border-top-right-radius: 14px;">
                    <h5 class="modal-title" id="submitConfirmModalLabel">Confirm Submit</h5>
                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body text-start">
                    Are you sure you want to submit?
                </div>
                <div class="modal-footer justify-content-end border-0">

                    <!-- Real Submit Button -->
                    <asp:Button ID="Button_submit" runat="server" CssClass="btn btn-primary py-1 px-3" Style="padding: 2px 10px;" Text="Submit" OnClick="BtnSubmit_Click" />
                    <button type="button" class="btn btn-light border py-1 px-3" style="padding: 2px 10px;" data-bs-dismiss="modal">Cancel</button>

                </div>
            </div>
        </div>
    </div>

    <!-- Clear Form Confirmation Modal -->
    <div class="modal fade" id="clearConfirmModal" tabindex="-1" aria-labelledby="clearConfirmModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content rounded-4 shadow">
                <div class="modal-header bg-secondary text-white rounded-top-4" style="height: 60px; border-top-left-radius: 14px; border-top-right-radius: 14px;">
                    <h5 class="modal-title" id="clearConfirmModalLabel">Confirm Clear Form</h5>
                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body text-start">
                    Are you sure you want to clear the form?
                </div>
                <div class="modal-footer justify-content-end border-0">

                    <!-- Real Clear Form Button -->
                    <asp:Button ID="Button_clear" runat="server" CssClass="btn btn-secondary py-1 px-3" tyle="padding: 2px 10px;" Text="Clear Form" OnClick="BtnClear_Click" />

                    <button type="button" class="btn btn-light border py-1 px-3" style="padding: 2px 10px;" data-bs-dismiss="modal">Cancel</button>

                </div>
            </div>
        </div>
    </div>
   

</asp:Content>
