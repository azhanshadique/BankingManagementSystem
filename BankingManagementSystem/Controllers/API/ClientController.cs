using BankingManagementSystem.BLL;
using BankingManagementSystem.Models.DTOs;
using System.Web.Http;
using System.Web.Services.Description;

namespace BankingManagementSystem.Controllers.API
{
	public class ClientController : ApiController
	{
        [HttpPost]
        [Route("api/client/register")]
        public IHttpActionResult RegisterClient(ClientDTO client)
        {
            bool result = new ClientBLL().RegisterNewClient(client, out string message);

            if (result)
                return Ok(message);
            else
                return BadRequest(message);
        }

    }
}