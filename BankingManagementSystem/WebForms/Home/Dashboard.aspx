<%@ Page Title="" Language="C#" MasterPageFile="~/WebForms/MasterPage.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="BankingManagementSystem.WebForms.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <!-- Default Dashboard -->
    <main id="DefaultDashboard" runat="server" visible="true" class="d-flex align-items-center justify-content-center">
        <div class="container text-center d-flex flex-column gap-lg-5">

            <section class="mb-5">
                <h1 id="title">Welcome to NRIFT Online Banking</h1>
                <p class="lead">
                    Experience hassle-free digital banking — manage your accounts, access services,
                    <br />
                    and stay connected with your bank from anywhere.
                </p>
                <a href='<%# Page.GetRouteUrl("ClientLoginRoute", null) %>' class="btn btn-primary btn-md">Login Now <i class="fas fa-angle-right"></i> </a>
            </section>

            <div class="row mt-lg-2">
                <section class="col-md-4 mb-4 border-end border-dark px-4">
                    <h2>Create New Account</h2>
                    <p>Open a new online bank account with just a few steps. Secure, fast, and paperless.</p>
                    <a class="btn btn-outline-dark" href='<%# Page.GetRouteUrl("ClientSignupCreateAccountRoute", null) %>'>Create Account <i class="fas fa-angle-right"></i></a>
                </section>

                <section class="col-md-4 mb-4 border-end border-dark px-4">
                    <h2>Link Existing Account</h2>
                    <p>Already have an account at a branch? Link it to your online profile and manage it digitally.</p>
                    <a class="btn btn-outline-dark" href='<%# Page.GetRouteUrl("ClientSignupLinkAccountRoute", null) %>'>Link Account <i class="fas fa-angle-right"></i></a>
                </section>

               <%-- <section class="col-md-4 mb-4 px-4">
                    <h2>Login</h2>
                    <p>Access your dashboard to view balance, transaction history, and manage your accounts.</p>
                    <a class="btn btn-outline-dark" href='<%# Page.GetRouteUrl("ClientLoginRoute", null) %>'>Login Now <i class="fas fa-angle-right"></i></a>
                </section>--%>
                <section class="col-md-4 mb-4 px-4">
                    <h2>Client Requests </h2>
                    <p>Manage your requests by your request id. View, Update or Delete your requests.</p>
                    <a class="btn btn-outline-dark" href='<%# Page.GetRouteUrl("ClientRequestRoute", null) %>'>Client Requests <i class="fas fa-angle-right"></i></a>
                </section>
            </div>

        </div>
    </main>

    <!-- Client Dashboard -->
    <main id="ClientDashboard" runat="server" visible="true" class="d-flex align-items-center justify-content-center py-5">
        <div class="container text-center d-flex flex-column gap-5">

            <section class="mb-5">
                <h1 class="display-5">Welcome,
                    <asp:Label ID="lblClientName" runat="server" Text="Client" CssClass="text-black" /></h1>
                <p class="lead">Manage your banking needs quickly and securely from your personalized dashboard.</p>
            </section>

            <div class="row g-4">
                <!-- View Profile -->
                <section class="col-md-4">
                    <div class="card h-100 shadow-sm">
                        <div class="card-body">
                            <h5 class="card-title">View Profile</h5>
                            <p class="card-text">Check and update your profile information, contact details, and more.</p>
                            <a class="btn btn-outline-dark" href="<%= ResolveUrl("~/WebForms/Client/ViewProfile.aspx") %>">Go to Profile <i class="fas fa-angle-right"></i></a>
                        </div>
                    </div>
                </section>

                <!-- Manage Accounts -->
                <section class="col-md-4">
                    <div class="card h-100 shadow-sm">
                        <div class="card-body">
                            <h5 class="card-title">Manage Accounts</h5>
                            <p class="card-text">View account summary, open new accounts, and manage linked accounts.</p>
                            <a class="btn btn-outline-dark" href="<%= ResolveUrl("~/WebForms/Client/ManageAccounts.aspx") %>">Manage Accounts <i class="fas fa-angle-right"></i></a>
                        </div>
                    </div>
                </section>

                <!-- Deposit -->
                <section class="col-md-4">
                    <div class="card h-100 shadow-sm">
                        <div class="card-body">
                            <h5 class="card-title">Deposit</h5>
                            <p class="card-text">
                                Add money to your account securely - anytime,
                                <br />
                                from anywhere.
                            </p>
                            <a class="btn btn-outline-dark" href="<%= ResolveUrl("~/WebForms/Client/Deposit.aspx") %>">Deposit Now <i class="fas fa-angle-right"></i></a>
                        </div>
                    </div>
                </section>

                <!-- Withdraw -->
                <section class="col-md-4">
                    <div class="card h-100 shadow-sm">
                        <div class="card-body">
                            <h5 class="card-title">Withdraw</h5>
                            <p class="card-text">Withdraw funds from your registered bank account - get quick access to your money anytime.</p>
                            <a class="btn btn-outline-dark" href="<%= ResolveUrl("~/WebForms/Client/Withdraw.aspx") %>">Withdraw <i class="fas fa-angle-right"></i></a>
                        </div>
                    </div>
                </section>

                <!-- Transfer Money -->
                <section class="col-md-4">
                    <div class="card h-100 shadow-sm">
                        <div class="card-body">
                            <h5 class="card-title">Transfer Money</h5>
                            <p class="card-text">Send money to another client quickly — instant and secure transfers 24/7.</p>
                            <a class="btn btn-outline-dark" href="<%= ResolveUrl("~/WebForms/Client/MoneyTransfer.aspx") %>">Transfer <i class="fas fa-angle-right"></i></a>
                        </div>
                    </div>
                </section>

                <!-- Transaction History -->
                <section class="col-md-4">
                    <div class="card h-100 shadow-sm">
                        <div class="card-body">
                            <h5 class="card-title">Transaction History</h5>
                            <p class="card-text">Track all your past transactions with details to ensure complete transparency and control.</p>
                            <a class="btn btn-outline-dark" href="<%= ResolveUrl("~/WebForms/Client/TransactionHistory.aspx") %>">View History <i class="fas fa-angle-right"></i></a>
                        </div>
                    </div>
                </section>
            </div>

        </div>
    </main>


    <!-- Admin Dashboard -->
    <main id="AdminDashboard" runat="server" visible="true" class="d-flex align-items-center justify-content-center py-5">
        <div class="container text-center d-flex flex-column gap-5">

            <!-- Welcome Message -->
            <section class="mb-5">
                <h1 class="display-5">Welcome,
                <asp:Label ID="lblAdminName" runat="server" Text="Admin" CssClass="text-black" />
                </h1>
                <p class="lead">Efficiently manage client and accounts directly from your admin dashboard.</p>
            </section>

            <div class="row g-4 justify-content-center">

                <!-- Client Management -->
                <section class="col-md-4">
                    <div class="card h-100 shadow-sm">
                        <div class="card-body">
                            <h5 class="card-title">Client Management</h5>
                            <p class="card-text">View, edit, or deactivate client and monitor their linked accounts.</p>
                            <a class="btn btn-outline-dark" href='<%= ResolveUrl("~/WebForms/Admin/ClientManagement.aspx") %>'>Manage Clients <i class="fas fa-angle-right"></i></a>
                        </div>
                    </div>
                </section>

                <!-- View All Accounts -->
                <section class="col-md-4">
                    <div class="card h-100 shadow-sm">
                        <div class="card-body">
                            <h5 class="card-title">View All Accounts</h5>
                            <p class="card-text">Browse all accounts across clients and access account summaries.</p>
                            <a class="btn btn-outline-dark" href='<%= ResolveUrl("~/WebForms/Admin/ViewAllAccounts.aspx") %>'>View Accounts <i class="fas fa-angle-right"></i></a>
                        </div>
                    </div>
                </section> 

                 <!-- Manage Client Requests -->
                <section class="col-md-4">
                    <div class="card h-100 shadow-sm">
                        <div class="card-body">
                            <h5 class="card-title">Manage Client Requests</h5>
                            <p class="card-text">Manage client requests. View, Update, Approve or Reject requests.</p>
                            <a class="btn btn-outline-dark" href='<%# Page.GetRouteUrl("AdminPendingRequestRoute", null) %>'>Client Requests <i class="fas fa-angle-right"></i></a>
                        </div>
                    </div>
                </section>
                
            </div>
        </div>
    </main>



</asp:Content>
