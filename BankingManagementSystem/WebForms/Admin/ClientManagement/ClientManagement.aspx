<%@ Page Title="" Language="C#" Async="true" MasterPageFile="~/WebForms/MasterPage.Master" AutoEventWireup="true" CodeBehind="ClientManagement.aspx.cs" Inherits="BankingManagementSystem.WebForms.Admin.ClientManagement.ClientManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

    <script>
        function setAccountNumberToDelete(accountNumber) {
            document.getElementById('<%= hfAccountNumberToDelete.ClientID %>').value = accountNumber;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1 class="mt-5 mx-auto text-center ">Client Management</h1>
    <div class="container mt-5 mx-auto text-center  d-flex justify-content-center align-items-center gap-0">

        <div class="col-md-2">
            <a href='<%= Page.GetRouteUrl("DashboardRoute", null) %>' class="btn btn-sm btn-light border"><i class="fas fa-angle-left"></i>Back</a>
        </div>
        <div class="col-md-6">
            <asp:TextBox ID="txtSearchClientId" runat="server" CssClass="form-control" placeholder="Enter Client ID"></asp:TextBox>
        </div>
        <div class="col-md-2">
            <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary" Text="Search" OnClick="BtnSearch_Click" />
        </div>
    </div>







    <%--Profile Details--%>
    <asp:Panel ID="pnlClientProfileDetails" runat="server" Visible="false">
        <div class="container py-5">

            <div class="card shadow-sm rounded-4">
                <div class="card-body px-5">


                    <div class="d-flex align-items-center justify-content-between mb-4 pb-2 border-bottom">
                        <%--<a href='<%= Page.GetRouteUrl("DashboardRoute", null) %>' class="btn btn-sm btn-light border"><i class="fas fa-angle-left"></i> Back</a>--%>
                        <!-- Invisible spacer to balance layout -->
                        <span style="width: 80px;"></span>
                        <h2 class="mb-2 mx-auto text-center flex-grow-1">Client Profile Details</h2>
                        <asp:Button ID="btnShowAccount" runat="server" CssClass="btn btn-sm btn-primary" Text="Client Linked Accounts >" OnClick="BtnShowAccounts_Click" />
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

    <%--Account Details--%>
    <asp:Panel ID="pnlClientAccountDetails" runat="server" Visible="false">
        <div class="container py-5">

            <div class="card shadow-sm rounded-4">
                <div class="card-body px-5">


                    <div class="d-flex align-items-center justify-content-between mb-4 pb-2 border-bottom">
                        <asp:Button ID="btnShowProfile" runat="server" CssClass="btn btn-sm btn-primary" Text="< Client Profile Details" OnClick="BtnShowProfile_Click" />
                        <h2 class="mb-0 mx-auto text-center flex-grow-1">Client Account Details</h2>

                        <div class="mb-2">
                            <asp:DropDownList ID="ddlAccountType" runat="server" CssClass="form-select w-auto d-inline-block me-3" AutoPostBack="true" OnSelectedIndexChanged="DdlAccountType_SelectedIndexChanged">
                                <asp:ListItem Text="All Accounts" Value="All" />
                                <asp:ListItem Text="Savings" Value="Savings" />
                                <asp:ListItem Text="Current" Value="Current" />
                                <asp:ListItem Text="Joint" Value="Joint" />
                            </asp:DropDownList>

                        </div>
                    </div>







                    <asp:Panel ID="pnlAccountList" runat="server">



                        <%-- <asp:GridView ID="gvAccounts" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-hover" OnRowCreated="GvAccounts_RowCreated" ShowHeaderWhenEmpty="true" EmptyDataText="No accounts found.">--%>
                        <asp:GridView ID="gvAccounts" runat="server"
                            AutoGenerateColumns="false"
                            CssClass="table table-bordered table-hover"
                            OnRowCreated="GvAccounts_RowCreated"
                            ShowHeaderWhenEmpty="true"
                            EmptyDataText="No accounts found."
                            AllowPaging="true" PageSize="5"
                            OnPageIndexChanging="GvAccounts_PageIndexChanging"
                            PagerStyle-CssClass="grid-pager"
                            PagerSettings-PageButtonCount="5">

                            <Columns>
                                <asp:BoundField DataField="AccountNumber" HeaderText="Account Number" />
                                <asp:BoundField DataField="AccountType" HeaderText="Type" />
                                <asp:BoundField DataField="Balance" HeaderText="Balance" DataFormatString="{0:C}" />
                                <%--<asp:BoundField DataField="Balance" HeaderText="Balance" DataFormatString='Rs. {0:N2}' HtmlEncode="false" />--%>

                                <asp:BoundField DataField="Status" HeaderText="Status" />
                                <%-- <asp:TemplateField HeaderText="Primary">
                        <ItemTemplate>
                            <%# Convert.ToBoolean(Eval("IsPrimary")) ? "Yes" : "No" %>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="Joint Account">
                                    <ItemTemplate>
                                        <%# Convert.ToBoolean(Eval("IsJoint")) ? "Yes" : "No" %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="CoHolderClientId" HeaderText="Co-Holder Client Id" />
                                <asp:BoundField DataField="CoHolderName" HeaderText="Co-Holder Name" />
                                <asp:TemplateField HeaderText="Actions">
                                    <ItemTemplate>
                                        <button type="button" class="btn btn-outline-danger btn-sm shadow-sm fs-6" style="padding: 2px 10px; margin-top: 3px;" data-bs-toggle="modal" data-bs-target="#deleteConfirmModal" onclick="setAccountNumberToDelete('<%# Eval("AccountNumber") %>')">
                                            Delete
                                        </button>
                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>
                        </asp:GridView>

                        <asp:HiddenField ID="hfAccountNumberToDelete" runat="server" />

                    </asp:Panel>

                </div>

            </div>
        </div>
    </asp:Panel>


    <!-- Reject Modal -->
    <div class="modal fade" id="deleteConfirmModal" tabindex="-1" aria-labelledby="deleteConfirmModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content rounded-4 shadow">
                <div class="modal-header bg-danger text-white rounded-top-4" style="height: 60px; border-top-left-radius: 14px; border-top-right-radius: 14px;">
                    <h5 class="modal-title" id="deleteConfirmModalLabel">Delete Account</h5>
                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body text-start">
                    Are you sure you want to delete this account?
                    <br />
                    <br />
                    <i>(Note: If this is a joint account, deleting it will remove access from both account holders.)</i>
                </div>
                <div class="modal-footer justify-content-end border-0">

                    <!-- Real Delete Button -->
                    <asp:Button ID="btnDeleteAccount" runat="server" Text="Delete" CssClass="btn btn-danger border py-1 px-3" OnClick="BtnDeleteAccount_Click" />
                    <button type="button" class="btn btn-light border py-1 px-3" style="padding: 2px 10px;" data-bs-dismiss="modal">Cancel</button>

                </div>
            </div>
        </div>
    </div>

     



</asp:Content>
