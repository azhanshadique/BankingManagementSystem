<%@ Page Title="" Language="C#" Async="true" MasterPageFile="~/WebForms/MasterPage.Master" AutoEventWireup="true" CodeBehind="TransactionHistory.aspx.cs" Inherits="BankingManagementSystem.WebForms.Client.TransactionHistory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container py-5">
        <h1 class="text-center mb-4">Transaction History</h1>

        <asp:Panel ID="pnlAccountList" runat="server">

            <div class="d-flex justify-content-between align-items-center mb-3">
                <a href='<%= Page.GetRouteUrl("DashboardRoute", null) %>' class="btn btn-sm btn-light border"><i class="fas fa-angle-left"></i>Back</a>



                <div class="d-flex flex-column align-items-end justify-content-center mb-2" style="width: 200px;">
                    <h6 class="text-muted me-2 text-nowrap">Account Number</h6>
                   <asp:DropDownList ID="ddlAccountNumber" runat="server" CssClass="form-select w-auto d-inline-block " AutoPostBack="true" OnSelectedIndexChanged="DdlAccountNumber_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>
            </div>

            <asp:GridView ID="gvTransactions" runat="server"
                AutoGenerateColumns="false"
                CssClass="table table-bordered table-hover"
                OnRowCreated="GvTransactions_RowCreated"
                ShowHeaderWhenEmpty="true"
                EmptyDataText="No records found."
                AllowPaging="true" PageSize="5"
                OnPageIndexChanging="GvTransactions_PageIndexChanging"
                PagerStyle-CssClass="grid-pager"
                PagerSettings-Mode="NumericFirstLast"
                PagerSettings-Position="Bottom"
                PagerSettings-FirstPageText="«"
                PagerSettings-LastPageText="»"
                PagerSettings-PageButtonCount="5">
                <Columns>
                    <asp:BoundField DataField="TransactionId" HeaderText="Transaction ID" />
                    <asp:BoundField DataField="TransactionDate" HeaderText="Date & Time" DataFormatString="{0:dd-MM-yyyy hh:mm tt}" />
                    <asp:BoundField DataField="TransactionType" HeaderText="Type" />
                    <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="{0:C}" />
                    <asp:BoundField DataField="BalanceAfter" HeaderText="Balance After" DataFormatString="{0:C}" />
                    <asp:BoundField DataField="Remarks" HeaderText="Remarks" />
                   <%-- <asp:BoundField DataField="PerformedBy" HeaderText="Performed By<br/>(Client ID)" />--%>

                    <asp:TemplateField HeaderText="Performed By<br/>(Client ID)">
                        <ItemTemplate>
                            <%# Eval("PerformedBy") %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Counterparty<br/>(Account No.)">
                        <ItemTemplate>
                            <%# Eval("CounterPartyAccountNo") %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <%--<asp:BoundField DataField="CounterPartyAccountNo" HeaderText="Counterparty (Acc. No.)" />--%>
                    <asp:TemplateField HeaderText="Successful">
                        <ItemTemplate>
                            <%# Convert.ToBoolean(Eval("IsSuccessful")) ? "Yes" : "No" %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--<asp:BoundField DataField="Balance" HeaderText="Balance" DataFormatString='Rs. {0:N2}' HtmlEncode="false" />--%>

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
