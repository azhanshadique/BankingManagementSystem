﻿<%@ Page Title="" Language="C#" MasterPageFile="~/WebForms/MasterPage.Master" AutoEventWireup="true" CodeBehind="ClientLogin.aspx.cs" Inherits="BankingManagementSystem.WebForms.Login.Client.ClientLogin" Async="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


    <main class="container d-flex justify-content-center align-items-center">
        <div class="row w-100">
            <div class="col-md-5 col-lg-4 mx-auto">
                <div class="card shadow-lg p-2">
                    <div class="card-body">
                        <!-- Heading -->
                        <div class="text-center mb-4">
                            <h2 class="h3">Client Login</h2>
                        </div>
                        <hr />
                        <!-- Username -->
                        <div class="mb-3">
                            <label class="form-label">Username<span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" />
                        </div>

                        <!-- Password -->
                        <div class="mb-4">
                            <label class="form-label">Password<span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" />
                        </div>

                        <!-- Login Button -->
                        <div class="d-grid gap-2 mb-3">
                            <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn btn-primary" OnClick="BtnLogin_Click" />
                        </div>

                         <div class="d-grid gap-2 mb-3 text-center" style="color:grey">
                           or 
                        </div>

                        <!-- Create + Link Account -->
                        <div class="d-grid gap-2 mb-3">
                            <a href='<%= Page.GetRouteUrl("ClientSignupCreateAccountRoute", null) %>' class="btn btn-outline-secondary">Create Account</a>
                            <a href='<%= Page.GetRouteUrl("ClientSignupLinkAccountRoute", null) %>' class="btn btn-outline-secondary">Link Existing Account</a>
                        </div>
                        
                        <!-- Back & Forgot -->
                        <div class="d-flex justify-content-between align-items-center mt-0">
                            <a href='<%= Page.GetRouteUrl("DashboardRoute", null) %>' class="btn btn-sm btn-light border"><i class="fas fa-angle-left"></i> Back to Home</a>
                            <%--<a href="#" class="text-decoration-none">Forgot Password?</a>--%>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </main>


</asp:Content>
