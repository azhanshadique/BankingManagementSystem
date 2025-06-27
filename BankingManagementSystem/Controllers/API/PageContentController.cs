using BankingManagementSystem.Helpers;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.UI;

namespace BankingManagementSystem.Controllers.API
{
    public class PageContentController : ApiController
    {
        [HttpGet]
        [Route("api/client/login")]
        public HttpResponseMessage GetClientLoginHtml()
        {
            try
            {
                string html = PageRenderer.RenderPage(ConfigurationManager.AppSettings["ClientLoginRedirect"]);

                return new HttpResponseMessage
                {
                    Content = new StringContent(html, Encoding.UTF8, "text/html"),
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (HttpException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message);
            }
        }
    }
}
