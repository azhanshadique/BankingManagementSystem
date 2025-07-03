using BankingManagementSystem.BLL;
using BankingManagementSystem.Models.DTOs;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Services.Description;

namespace BankingManagementSystem.Controllers.API
{
    [RoutePrefix("api/client")]
    public class ClientController : ApiController
	{
        [HttpPost]
        [Route("register")]
        public async Task<IHttpActionResult> RegisterClient(ClientDTO client)
        {
            var result = await ClientBLL.RegisterNewClient(client);

            if (result.IsSuccess)
                return Ok(result.Message);
            else
                return BadRequest(result.Message);
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