using BankingManagementSystem.BLL;
using BankingManagementSystem.Helpers;
using BankingManagementSystem.Models.API;
using BankingManagementSystem.Models.ConstraintTypes;
using BankingManagementSystem.Models.DTOs;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace BankingManagementSystem.Controllers.API
{
    [RoutePrefix("api/auth")]
    public class AuthController : ApiController
    {

        [HttpPost]
        [Route("client")]
        public async Task<IHttpActionResult> ClientLoginAsync(AuthRequestDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid login request.");

            var clientUser = await ClientBLL.ValidateClientLoginAsync(request);

            if (clientUser != null)
            {
                string token = JwtTokenManager.GenerateToken(clientUser);
                return Ok(new { token });
            }

            return Content(HttpStatusCode.Unauthorized, "Invalid username or password.");
        }


        [HttpPost]
        [Route("admin")]
        public async Task<IHttpActionResult> AdminLoginAsync(AuthRequestDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid login request.");

            var adminUser = await AdminBLL.ValidateAdminLoginAsync(request);

            if (adminUser != null)
            {
                string token = JwtTokenManager.GenerateToken(adminUser);
                return Ok(new { token });
            }

            return Content(HttpStatusCode.Unauthorized, "Invalid username or password.");
        }



        [HttpGet]
        [Route("test")]
        public async Task<IHttpActionResult> TestAsync()
        {
            return Ok("API is working!");
        }
    }
}
