using BankingManagementSystem.BLL;
using BankingManagementSystem.Models.ConstraintTypes;
using BankingManagementSystem.Models.DTOs;
using System.Threading.Tasks;
using System.Web.Http;

namespace BankingManagementSystem.Controllers.API
{
    [AllowAnonymous]
    [RoutePrefix("api/public")]
    public class PublicController : ApiController
    {
        [HttpPost]
        [Route("register-client")]
        public async Task<IHttpActionResult> Register([FromBody] ClientDTO client)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid client data.");

            var (IsSuccess, Message) = await ClientBLL.RegisterNewClient(client);

            return IsSuccess
                ? Ok(Message)
                : (IHttpActionResult)BadRequest(Message);
        }


        [HttpGet]
        [Route("register/request/{id:int}")]
        public async Task<IHttpActionResult> GetPublicRegistrationRequestByIdAsync([FromUri] int id)
        {
            var (isValid, message, request) = await RequestBLL.GetPublicRequestByIdAsync(id);

            if (!isValid)
                return BadRequest(message);

            if (request == null)
                return NotFound();

            return Ok(request);
        }

        
        [HttpPut]
        [Route("update/register-request/{id:int}")]
        public async Task<IHttpActionResult> UpdatePublicRegisterRequest([FromUri] int id, [FromBody] ClientDTO client)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid client data.");
            var (success, message) = await RequestBLL.UpdateRegisterRequestPublicAsync(id, client);
            return success ? Ok(new { success }) : (IHttpActionResult)BadRequest(message);
        }

        [HttpPut]
        [Route("delete/request/{id:int}")]
        public async Task<IHttpActionResult> DeletePublicRegisterRequest([FromUri] int id)
        {
            var (success, message) = await RequestBLL.DeleteRegisterRequestPublicAsync(id, id);

            if (!success)
                return BadRequest(message);

            return Ok(new { success = true, message = "Request deleted successfully." });
        }

    }
}


//using BankingManagementSystem.BLL;
//using BankingManagementSystem.DAL;
//using BankingManagementSystem.Models.DTOs;
//using System.Threading.Tasks;
//using System.Web.Http;
//using System.Web.Services.Description;

//namespace BankingManagementSystem.Controllers.API
//{
//    [RoutePrefix("api/client")]
//    public class ClientController : ApiController
//    {
//        [HttpPost]
//        [Route("register")]
//        public async Task<IHttpActionResult> RegisterClientAsync(ClientDTO client)
//        {
//            var result = await ClientBLL.RegisterNewClient(client);

//            if (result.IsSuccess)
//                return Ok(result.Message);
//            else
//                return BadRequest(result.Message);
//        }



//    }
//}

