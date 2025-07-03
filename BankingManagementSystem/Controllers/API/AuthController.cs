using BankingManagementSystem.BLL;
using BankingManagementSystem.Helpers;
using BankingManagementSystem.Models.API;
using BankingManagementSystem.Models.ConstraintTypes;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Http;

namespace BankingManagementSystem.Controllers.API
{
    [RoutePrefix("api/auth")]
    public class AuthController : ApiController
    {

        [HttpPost]
        [Route("client")]
        public IHttpActionResult ClientLogin(AuthRequestDTO request)
        {
            var user = ClientBLL.ValidateClientLogin(request);

            if (user != null)
            {
                string token = JwtTokenManager.GenerateToken(user);

                return Ok(new
                {
                    token
                });
            }

            return Unauthorized();
        }

        [HttpPost]
        [Route("admin")]
        public IHttpActionResult AdminLogin(AuthRequestDTO request)
        {
            var user = AdminBLL.ValidateAdminLogin(request);

            if (user != null)
            {
                string token = JwtTokenManager.GenerateToken(user);

                return Ok(new
                {
                    token
                });
            }

            return Unauthorized();
        }



        [HttpGet]
        [Route("test")]
        public IHttpActionResult Test()
        {
            return Ok("API is working!");
        }
    }
}
