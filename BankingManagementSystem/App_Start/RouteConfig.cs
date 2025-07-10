using System.Configuration;
using System.Web.Mvc;
using System.Web.Routing;

namespace BankingManagementSystem
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            routes.MapPageRoute(
               "DashboardRoute",             
               "dashboard",
               ConfigurationManager.AppSettings["DashboardRedirect"]       
           );
             routes.MapPageRoute(
               "AboutRoute",             
               "about",
               ConfigurationManager.AppSettings["AboutRedirect"]       
           );
             routes.MapPageRoute(
               "ClientLoginRoute",
               "client/login",
               ConfigurationManager.AppSettings["ClientLoginRedirect"]       
           );
             routes.MapPageRoute(
               "AdminLoginRoute",
               "admin/login",
               ConfigurationManager.AppSettings["AdminLoginRedirect"]       
           );
             routes.MapPageRoute(
               "ClientSignupCreateAccountRoute",
               "client/signup/register",
               ConfigurationManager.AppSettings["ClientSignupCreateAccountRedirect"]       
           );
             routes.MapPageRoute(
               "ClientSignupLinkAccountRoute",
               "client/signup/link",
               ConfigurationManager.AppSettings["ClientSignupLinkAccountRedirect"]       
           );
             routes.MapPageRoute(
               "PublicRequestRoute",
               "dashboard/requests",
               ConfigurationManager.AppSettings["PublicRequestRedirect"]       
           );
            routes.MapPageRoute(
                "AdminClientRequestRedirect",
               "admin/requests",
               ConfigurationManager.AppSettings["AdminClientRequestRedirect"]       
           );
              routes.MapPageRoute(
                "ClientPendingRequestRoute",
               "client/requests",
               ConfigurationManager.AppSettings["ClientPendingRequestRedirect"]       
           );
              routes.MapPageRoute(
                "ClientProfileRoute",
               "client/profile",
               ConfigurationManager.AppSettings["ClientProfileRedirect"]       
           );
            routes.MapPageRoute(
                "ClientAccountsRoute",
               "client/accounts",
               ConfigurationManager.AppSettings["ClientAccountsRedirect"]       
           );
            routes.MapPageRoute(
                "DepositAmountRoute",
               "client/deposit",
               ConfigurationManager.AppSettings["DepositAmountRedirect"]       
           );
             routes.MapPageRoute(
                "WithdrawAmountRoute",
               "client/withdraw",
               ConfigurationManager.AppSettings["WithdrawAmountRedirect"]       
           );
             routes.MapPageRoute(
                "TransferAmountRoute",
               "client/transfer",
               ConfigurationManager.AppSettings["TransferAmountRedirect"]       
           );
             routes.MapPageRoute(
                "TransactionHistoryRoute",
               "client/transactions",
               ConfigurationManager.AppSettings["TransactionHistoryRedirect"]       
           );
            routes.MapPageRoute(
                "ClientManagementRoute",
               "admin/client-management",
               ConfigurationManager.AppSettings["ClientManagementRedirect"]       
           );




            routes.MapRoute(
               name: "Default",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
           );
        }

  
    }
}
