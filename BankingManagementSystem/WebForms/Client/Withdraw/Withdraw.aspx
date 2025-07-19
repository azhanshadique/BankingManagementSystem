<%@ Page Title="" Language="C#" Async="true" MasterPageFile="~/WebForms/MasterPage.Master" AutoEventWireup="true" CodeBehind="Withdraw.aspx.cs" Inherits="BankingManagementSystem.WebForms.Client.Withdraw.Withdraw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container py-5 mt-5">
        <div class="card shadow rounded-4">
            <div class="card-body px-5">

                <!-- Header -->
                <div class="d-flex justify-content-between align-items-center mb-4 pb-2 border-bottom">
                    <a href='<%= Page.GetRouteUrl("DashboardRoute", null) %>' class="btn btn-sm btn-light border">
                        <i class="fas fa-angle-left"></i> Back
                    </a>
                    <h2 class="text-center flex-grow-1 mb-0">Withdraw Money</h2>
                    <span style="width: 80px;"></span>
                </div>

                <!-- Account Details -->
                <h4 class="text-primary mb-3"><i class="fas fa-university me-2"></i>Account Details</h4>
                <div class="row g-3">
                    <div class="col-md-3">
                        <label for="ddlAccountNumber" class="form-label">Select Account<span class="text-danger">*</span></label>
                        <asp:DropDownList ID="ddlAccountNumber" runat="server" CssClass="form-select" ></asp:DropDownList>
                    </div>
                    <div class="col-md-3">
                        <label for="txtAmount" class="form-label">Amount to Withdraw<span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control" TextMode="Number" placeholder="Enter amount"></asp:TextBox>
                    </div>
                    <div class="col-md-3">
                        <label for="txtRemarks" class="form-label">Remarks (Optional)</label>
                        <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" placeholder="Add a note"></asp:TextBox>
                    </div>
                    <div class="col-md-3">
                        <label for="txtPassword" class="form-label">Password<span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Enter your password"></asp:TextBox>
                    </div>
                </div>

                <!-- Buttons -->
                <div class="d-flex justify-content-end gap-3 mt-5">
                    <asp:Button ID="btnWithdraw" runat="server" Text="Withdraw" CssClass="btn btn-danger" OnClick="BtnWithdraw_Click" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
