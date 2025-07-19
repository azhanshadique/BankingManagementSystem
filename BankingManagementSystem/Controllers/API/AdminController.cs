using BankingManagementSystem.BLL;
using BankingManagementSystem.Models.DTOs;
using System.Threading.Tasks;
using System.Web.Http;


namespace BankingManagementSystem.Controllers.API
{
    [Authorize(Roles = "ADMIN")]
    [RoutePrefix("api/admin")]
    public class AdminController : ApiController
    {
        [HttpPost]
        [Route("create-client")]
        public async Task<IHttpActionResult> CreateClientAsync([FromBody] ClientDTO client)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid client data.");

            var (isSuccess, message) = await AdminBLL.CreateNewClientAsync(client);

            return isSuccess
                ? Ok(new { message })
                : (IHttpActionResult)BadRequest(message);
        }

        [HttpPut]
        [Route("update-client")]
        public async Task<IHttpActionResult> UpdateClientAsync([FromUri] int clientId, [FromBody] ClientDTO client)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid client data.");

            var (isSuccess, message) = await AdminBLL.UpdateClientDetailsAsync(clientId, client);

            return isSuccess
                ? Ok(new { message })
                : (IHttpActionResult)BadRequest(message);
        }

        [HttpDelete]
        [Route("client-account")]
        public async Task<IHttpActionResult> DeleteAccountAsync([FromUri] long accountNumber)
        {
            var (isDeleted, message) = await AdminBLL.DeleteClientAccountAsync(accountNumber);

            return isDeleted
                ? Ok(new { message })
                : (IHttpActionResult)BadRequest(message);

            //bool isDeleted = await AccountService.DeleteAccountAsync(accountNumber);
            //if (isDeleted)
            //    return Ok("Account deleted successfully.");
            //return BadRequest("Failed to delete the account.");
        }

        [HttpPost]
        [Route("create-client/offline")]
        public async Task<IHttpActionResult> CreateClientOfflineAsync([FromBody] ClientDTO client)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid client data.");

            var (isSuccess, message) = await AdminBLL.CreateNewOfflineClientAsync(client);

            return isSuccess
                ? Ok(new { message })
                : (IHttpActionResult)BadRequest(message);
        }
    }

}