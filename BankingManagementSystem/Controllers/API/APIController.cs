using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace BankingManagementSystem.Controllers.API
{
	public class APIController : ApiController
	{
        [HttpGet]
        [Route("api/login/demo")]
        public IHttpActionResult GetClientLoginHtml()
        {
            try
            {
                using (StringWriter stringWriter = new StringWriter())
                {
                    // Render the .aspx page to a string
                    HttpContext.Current.Server.Execute("~/WebForms/Login/ClientLogin.aspx", stringWriter, false);
                    string renderedHtml = stringWriter.ToString();
                    return Ok(renderedHtml);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("api/login/test")]
        public IHttpActionResult Test()
        {
            return Ok("Login API is working!");
        }
    }
}

