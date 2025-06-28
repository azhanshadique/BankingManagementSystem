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




            routes.MapRoute(
               name: "Default",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
           );
        }

  
    }
}
