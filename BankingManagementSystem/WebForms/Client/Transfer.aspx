<%@ Page Title="" Language="C#" Async="true" MasterPageFile="~/WebForms/MasterPage.Master" AutoEventWireup="true" CodeBehind="Transfer.aspx.cs" Inherits="BankingManagementSystem.WebForms.Client.Transfer" %>

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
                    <h2 class="text-center flex-grow-1 mb-0">Transfer Money</h2>
                    <span style="width: 80px;"></span>
                </div>

                <!-- Account Details -->
                <h4 class="text-primary mb-3"><i class="fas fa-university me-2"></i>Account Details</h4>
                <div class="row g-3">
                    <div class="col-md-4">
                        <label for="ddlFromAccountNumber" class="form-label">From Account<span class="text-danger">*</span></label>
                        <asp:DropDownList ID="ddlFromAccountNumber" runat="server" CssClass="form-select" />
                    </div>
                    <div class="col-md-4">
                        <label for="txtToAccount" class="form-label">To Account Number<span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtToAccount" runat="server" CssClass="form-control" placeholder="Enter receiver's account number" />
                    </div>
                    <div class="col-md-4">
                        <label for="txtAmount" class="form-label">Amount to Transfer<span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control" TextMode="Number" placeholder="Enter amount" />
                    </div>
                    <div class="col-md-4">
                        <label for="txtRemarks" class="form-label">Remarks (Optional)</label>
                        <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" placeholder="Add a note" />
                    </div>
                    <div class="col-md-4">
                        <label for="txtPassword" class="form-label">Password<span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Enter your password" />
                    </div>
                </div>

                <!-- Buttons -->
                <div class="d-flex justify-content-end gap-3 mt-5">
                    <asp:Button ID="btnTransfer" runat="server" Text="Transfer" CssClass="btn btn-primary" OnClick="BtnTransfer_Click" />
                </div>

            </div>
        </div>
    </div>
</asp:Content>
