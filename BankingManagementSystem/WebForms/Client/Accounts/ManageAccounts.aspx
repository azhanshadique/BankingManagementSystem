<%@ Page Title="" Language="C#" Async="true" MasterPageFile="~/WebForms/MasterPage.Master" AutoEventWireup="true" CodeBehind="ManageAccounts.aspx.cs" Inherits="BankingManagementSystem.WebForms.Client.Accounts.ManageAccounts" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container py-5">
        <h1 class="text-center mb-4">Manage My Accounts</h1>

        <asp:Panel ID="pnlAccountList" runat="server">

            <div class="d-flex justify-content-between align-items-center mb-3">
                <a href='<%= Page.GetRouteUrl("DashboardRoute", null) %>' class="btn btn-sm btn-light border"><i class="fas fa-angle-left"></i> Back</a>

                <div class="mb-4">
                    <asp:DropDownList ID="ddlAccountType" runat="server" CssClass="form-select w-auto d-inline-block me-3" AutoPostBack="true" OnSelectedIndexChanged="DdlAccountType_SelectedIndexChanged">
                        <asp:ListItem Text="All Accounts" Value="All" />
                        <asp:ListItem Text="Savings" Value="Savings" />
                        <asp:ListItem Text="Current" Value="Current" />
                        <asp:ListItem Text="Joint" Value="Joint" />
                    </asp:DropDownList>
                    <asp:Button ID="btnAddAccount" runat="server" CssClass="btn btn-sm btn-success" Text="+ Add New Account" OnClick="BtnAddAccount_Click" />
                </div>
            </div>
            
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
                    <asp:TemplateField HeaderText="Joint Account" >
                        <ItemTemplate>
                            <%# Convert.ToBoolean(Eval("IsJoint")) ? "Yes" : "No" %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="CoHolderClientId" HeaderText="Co-Holder Client Id" />
                    <asp:BoundField DataField="CoHolderName" HeaderText="Co-Holder Name" />
                   <%-- <asp:TemplateField HeaderText="Actions">
                        <ItemTemplate>
                            <asp:Button ID="btnSetPrimary" runat="server" CommandName="SetPrimary" CommandArgument='<%# Eval("AccountNumber") %>' Text="Set as Primary" CssClass="btn btn-sm btn-outline-primary me-2" />
                            <asp:Button ID="btnDeleteAccount" runat="server" CommandName="DeleteAccount" CommandArgument='<%# Eval("AccountNumber") %>' Text="Delete" CssClass="btn btn-sm btn-outline-danger" />
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                </Columns>
            </asp:GridView>
        </asp:Panel>


            <%--Account Details--%>
            <asp:Panel ID="pnlAccountDetails" runat="server" Visible="false">
                <div class="container py-5">

                    <div class="card shadow-sm rounded-4">
                        <div class="card-body px-5">


                            <div class="d-flex align-items-center justify-content-between mb-4 pb-2 border-bottom">
                                <a href='<%= Page.GetRouteUrl("ClientAccountsRoute", null) %>' class="btn btn-sm btn-light border"><i class="fas fa-angle-left"></i> Back</a>
                                <%--<asp:Button ID="Button1" runat="server" CssClass="btn btn-sm btn-light border" Style="padding: 3px 10px;" Text="Back" OnClick="BtnReject_Click" />--%>

                                <h2 class="mb-0 mx-auto text-center flex-grow-1">New Account Details</h2>

                                <!-- Invisible spacer to balance layout -->
                                <%--<span style="width: 80px;"></span>--%>
                              <%--  <div class="d-flex justify-content-center align-items-center">
                                    <h6 class="text-muted mt-2 fs-6 fw-medium">Request ID: &nbsp; </h6>
                                    <asp:Label ID="lblRequestId" runat="server" CssClass="text-dark  d-block fs-6 fw-semibold"></asp:Label>
                                </div>--%>
                            </div>


               

                            <!-- ACCOUNT DETAILS -->
                            <h4 class="text-primary mb-3"><i class="fas fa-id-card me-2 fs-5"></i>Account Details</h4>
                            <div class="row g-3">
                                <div class="col-md-3">
                                    <label for="ddlAccountType" class="form-label">Account Type<span class="text-danger">*</span></label>
                                    <asp:DropDownList ID="ddlNewAccountType" runat="server" CssClass="form-select" >
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
                                            <label for="txtJointClientId" class="form-label">Co-Holder's Client ID<span class="text-danger">*</span></label>
                                            <asp:TextBox ID="txtJointClientId" runat="server" CssClass="form-control" placeholder="Client ID" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </div>

                  

                           



                            <hr class="my-4" />

                            <div class="d-flex justify-content-between">
                  
                                 <button type="button" class="btn btn-sm btn-light border" style="padding: 2px 10px; margin-top: 3px;" data-bs-toggle="modal" data-bs-target="#clearConfirmModal">Clear Form</button>
                                <button type="button" class="btn btn-primary btn-sm shadow-sm" style="padding: 2px 10px; margin-top: 3px;" data-bs-toggle="modal" data-bs-target="#submitConfirmModal">Submit</button>
                            </div>

                        </div>
                    </div>
                </div>

            </asp:Panel>

    </div>
</asp:Content>
