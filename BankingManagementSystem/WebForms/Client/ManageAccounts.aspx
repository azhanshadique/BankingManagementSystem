<%@ Page Title="" Language="C#" Async="true" MasterPageFile="~/WebForms/MasterPage.Master" AutoEventWireup="true" CodeBehind="ManageAccounts.aspx.cs" Inherits="BankingManagementSystem.WebForms.Client.ManageAccounts" %>

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

    </div>
</asp:Content>
