using BankingManagementSystem.BLL;
using BankingManagementSystem.Models.ConstraintTypes;
using BankingManagementSystem.Models.DTOs;
using System.Threading.Tasks;
using System.Web.Http;

namespace BankingManagementSystem.Controllers.API
{
    [RoutePrefix("api/client")]
    public class ClientController : ApiController
    {
        [HttpPost]
        [AllowAnonymous]
        [Route("register")]
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
        [AllowAnonymous]
        [Route("register/request/{id:int}")]
        public async Task<IHttpActionResult> GetPublicRegistrationRequestByIdAsync(int id)
        {
            var (isValid, message, request) = await RequestBLL.GetPublicRequestByIdAsync(id);

            if (!isValid)
                return BadRequest(message);

            if (request == null)
                return NotFound();

            return Ok(request);
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

