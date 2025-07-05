using Microsoft.Owin.Security.OAuth;
using System.Configuration;
using System.Text;
using System.Web.Http;

namespace BankingManagementSystem.App_Start
{
    public static class WebApiConfig
    {
        //public static void Register(HttpConfiguration config)
        //{
        //    //config.SuppressDefaultHostAuthentication();
        //    //config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

        //    config.MapHttpAttributeRoutes();

        //    config.Routes.MapHttpRoute(
        //        name: "DefaultApi",
        //        routeTemplate: "api/{controller}/{action}/{id}",
        //        defaults: new { id = RouteParameter.Optional }
        //    );

        //    config.Formatters.Remove(config.Formatters.XmlFormatter); // JSON only
        //}


        public static void Register(HttpConfiguration config)
        {

            // Allow CORS
            //var cors = new System.Web.Http.Cors.EnableCorsAttribute("*", "*", "*");
            //config.EnableCors(cors);

            // Enable attribute routing
            config.MapHttpAttributeRoutes();

            // Other Web API configuration...

            // Enable JWT authentication
            var key = Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings["JwtSecretKey"]);

            config.SuppressDefaultHostAuthentication(); // Prevent other auth types (like Forms)
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            config.Filters.Add(new AuthorizeAttribute());

            // Enable Bearer Token authentication
            config.MessageHandlers.Add(new JwtAuthenticationHandler(key));

            // Default route 
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }

}