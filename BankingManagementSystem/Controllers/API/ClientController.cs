using BankingManagementSystem.BLL;
using BankingManagementSystem.Models.DTOs;
using System.Web.Http;
using System.Web.Services.Description;

namespace BankingManagementSystem.Controllers.API
{
    [RoutePrefix("api/client")]
    public class ClientController : ApiController
	{
        [HttpPost]
        [Route("register")]
        public IHttpActionResult RegisterClient(ClientDTO client)
        {
            bool result = new ClientBLL().RegisterNewClient(client, out string message);

            if (result)
                return Ok(message);
            else
                return BadRequest(message);
        }

        [HttpPost]
        [Route("create")]
        public IHttpActionResult CreateClient(ClientDTO client)
        {
            bool result = AdminBLL.CreateNewClient(client, out string message);

            if (result)
                return Ok(message);
            else
                return BadRequest(message);
        }

    }
}