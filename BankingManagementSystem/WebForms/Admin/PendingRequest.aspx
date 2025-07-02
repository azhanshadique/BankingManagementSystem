<%@ Page Title="" Language="C#" MasterPageFile="~/WebForms/MasterPage.Master" AutoEventWireup="true" CodeBehind="PendingRequest.aspx.cs" Inherits="BankingManagementSystem.WebForms.Admin.PendingRequest" Async="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="mt-4 mx-auto fw-bold text-center flex-grow-1 ">Manage Client Pending Requests</h2>
    <asp:Panel ID="pnlRequestTable" runat="server" CssClass="mt-4" class="container mt-5" Visible="true">
        <div class="container py-5">

            <%--<div class="mb-2">Request Status</div>--%>
            <h6 class="text-muted mb-2">Request Status</h6>
            <asp:DropDownList ID="ddlFilterStatus" runat="server" AutoPostBack="true" CssClass="form-select w-25 mb-3" OnSelectedIndexChanged="DdlFilterStatus_SelectedIndexChanged">
                <asp:ListItem Text="Pending" Value="Pending" />
                <asp:ListItem Text="Approved" Value="Approved" />
                <asp:ListItem Text="Rejected" Value="Rejected" />
            </asp:DropDownList>

            <asp:GridView ID="gvRequests" runat="server" CssClass="table table-bordered table-hover" AutoGenerateColumns="false" AllowSorting="true" OnSorting="GvRequests_Sorting" OnRowCommand="GvRequests_RowCommand">
                <Columns>
                    <asp:BoundField DataField="RequestId" HeaderText="Request ID" SortExpression="RequestId" />
                    <asp:BoundField DataField="RequestType" HeaderText="Request Type" SortExpression="RequestType" />
                    <asp:BoundField DataField="RequestedOn" HeaderText="Requested On" SortExpression="RequestedOn" />
                    <asp:BoundField DataField="RepliedOn" HeaderText="Replied On" SortExpression="RepliedOn" DataFormatString="{0:dd-MM-yyyy HH:mm}" HtmlEncode="false"  />

                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <asp:Button ID="btnShow" runat="server" Text="Show" CommandName="Show" CommandArgument='<%# Eval("RequestId") %>' CssClass="btn btn-sm btn-primary" />
                            <%--<asp:Button ID="btnSearch" runat="server" CssClass="btn btn-sm btn-primary" Text="Search" OnClick="btnSearch_Click" />--%>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

        </div>
    </asp:Panel>

    <asp:Panel ID="pnlRequestDetails" runat="server" Visible="false">
        <div class="container py-5">

            <div class="card shadow-sm rounded-4">
                <div class="card-body px-5">


                    <div class="d-flex align-items-center justify-content-between mb-4 pb-2 border-bottom">
                        <a href='<%= Page.GetRouteUrl("AdminPendingRequestRoute", null) %>' class="btn btn-sm btn-light border"><i class="fas fa-angle-left"></i>Back</a>
                        <%--<asp:Button ID="Button1" runat="server" CssClass="btn btn-sm btn-light border" Style="padding: 3px 10px;" Text="Back" OnClick="BtnReject_Click" />--%>

                        <h3 class="mb-0 mx-auto fw-bold text-center flex-grow-1">Client New Registration Request Details</h3>

                        <!-- Invisible spacer to balance layout -->
                        <%--<span style="width: 80px;"></span>--%>
                        <div class="d-flex justify-content-center align-items-center">
                            <h6 class="text-muted mt-2 fs-6 fw-medium">Request ID: &nbsp; </h6>
                            <asp:Label ID="lblRequestId" runat="server" CssClass="text-dark  d-block fs-6 fw-semibold"></asp:Label>
                        </div>
                    </div>


                    <div class="card shadow-sm border-1 rounded-4 mb-4">
                        <div class="card-body px-4 py-3">
                            <div class="row text-start align-items-center">


                                <div class="col-md-3 mb-3">
                                    <h6 class="text-muted mb-1">Request Type</h6>
                                    <asp:Label ID="lblRequestType" runat="server" CssClass="fw-semibold text-dark fs-6 d-block"></asp:Label>
                                </div>
                                <div class="col-md-3 mb-3">
                                    <h6 class="text-muted mb-1">Request Status</h6>
                                    <asp:Label ID="lblRequestStatus" runat="server" CssClass="badge bg-primary fw-semibold text-white fs-6 px-6 py-2"></asp:Label>
                                </div>
                                <div class="col-md-3 mb-3">
                                    <%--<h6 class="text-muted mb-1"></h6>--%>
                                    <asp:Label ID="lblAdminApprovalHeading" runat="server" CssClass="text-muted mb-1 d-block" Text="Admin Approval" Visible="false"></asp:Label>
                                    <asp:Label ID="lblAdminApproval" runat="server" CssClass="badge bg-primary fw-semibold text-white fs-6 px-6 py-2"></asp:Label>
                                </div>
                                <div class="col-md-3 mb-3">
                                    <%--<h6 class="text-muted mb-1">Co-holder Approval</h6>--%>
                                    <asp:Label ID="lblCoHolderApprovalHeading" runat="server" CssClass="text-muted mb-1 d-block" Text="Co-holder Approval" Visible="false"></asp:Label>
                                    <asp:Label ID="lblCoHolderApproval" runat="server" CssClass="badge bg-primary fw-semibold text-white  fs-6 px-6 py-2"></asp:Label>
                                </div>
                            </div>
                        </div>
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
                                <label for="txtAddress" class="form-label">Address</label>
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
                    <h4 class="text-primary mb-3"><i class="fas fa-id-card me-2 fs-5"></i>Account Details</h4>
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
                            <asp:DropDownList ID="ddlIsJointAccount" runat="server" CssClass="form-select" AutoPostBack="true" Enabled="false" OnSelectedIndexChanged="DdlIsJoint_SelectedIndexChanged" >
                                <asp:ListItem Text="Select" Value="" Disabled="true" Selected="True" />
                                <asp:ListItem Text="Yes" Value="Yes" />
                                <asp:ListItem Text="No" Value="No" />
                            </asp:DropDownList>
                        </div>
                        <asp:Panel ID="fsJointAccount" runat="server" CssClass="col-md-6" Visible="false">
                            <div class="row g-3">
                                <div class="col-md-6">
                                    <label for="txtJointClientId" class="form-label">Co-holder's Client ID<span class="text-danger">*</span></label>
                                    <asp:TextBox ID="txtJointClientId" runat="server" CssClass="form-control" placeholder="Client ID" ReadOnly="true"></asp:TextBox>
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
                            <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" placeholder="Choose a username" ReadOnly="true"></asp:TextBox>
                        </div>
                          <div class="col-md-3">
                            <label for="txtPassword" class="form-label">Password<span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Create a password" ReadOnly="true"></asp:TextBox>
                        </div>
                                            <div class="col-md-3">
                        <label for="txtConfirmpassword" class="form-label">Confirm Password<span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Repeat password" ReadOnly="true"></asp:TextBox>
                    </div>
                    </div>


                    <%--                <div class="form-check mt-4 d-flex align-items-center" style="margin-left: -1.5rem;">
                    <asp:CheckBox ID="CheckBox_terms" runat="server" CssClass="me-2" />
                    <label class="form-check-label" for="CheckBox_terms" style="font-size: 0.9rem;">
                        I agree to the <a href="#">Terms & Conditions</a> and <a href="#">Privacy Policy</a><span class="text-danger">*</span>
                    </label>
                </div>--%>
                    <hr class="my-4" />

                    <div class="d-flex justify-content-end gap-3">

                        <asp:Button ID="btnEdit" runat="server" CssClass="btn btn-primary btn-sm shadow-sm" Style="padding: 3px 10px;" Text=" Edit " OnClick="BtnEdit_Click" />
                        <asp:Button ID="btnUpdate" runat="server" CssClass="btn btn-primary btn-sm shadow-sm" Style="padding: 3px 10px;" Text="Update" OnClick="BtnUpdate_Click" Visible="false"/>
                        <asp:Button ID="btnApprove" runat="server" CssClass="btn btn-success btn-sm shadow-sm" Style="padding: 3px 10px;" Text="Approve" OnClick="BtnApprove_Click" />
                        <asp:Button ID="btnReject" runat="server" CssClass="btn btn-danger btn-sm shadow-sm" Style="padding: 3px 10px;" Text="Reject" OnClick="BtnReject_Click" />

                        <%--<span style="width: 80px;"></span>--%>
                        <%--                    <asp:Button ID="Button_clear" runat="server" CssClass="btn btn-sm btn-light border" Text="Clear Form" OnClick="BtnClear_Click" />--%>
                        <%--<button type="button" class="btn btn-sm btn-light border" style="padding: 2px 10px; margin-top: 3px;" data-bs-toggle="modal" data-bs-target="#clearConfirmModal">Clear Form</button>--%>

                        <%--<asp:Button ID="Button_submit" runat="server" CssClass="btn btn-primary btn-sm shadow-sm" Text="Submit" OnClick="BtnSubmit_Click" />--%>
                        <%--<button type="button" class="btn btn-primary btn-sm shadow-sm" style="padding: 2px 10px; margin-top: 3px;" data-bs-toggle="modal" data-bs-target="#submitConfirmModal">Submit</button>--%>
                    </div>

                </div>
            </div>
        </div>





        <%--     <div class="mt-4">
            <asp:Button ID="btnUpdate" runat="server" CssClass="btn btn-success me-2" Text="Update" OnClick="btnUpdate_Click" />
            <asp:Button ID="btnDelete" runat="server" CssClass="btn btn-danger" Text="Delete Request" OnClick="btnDelete_Click" />
        </div>--%>
    </asp:Panel>

    <asp:Label ID="lblMessage" runat="server" CssClass="text-danger mt-3 d-block"></asp:Label>
</asp:Content>
