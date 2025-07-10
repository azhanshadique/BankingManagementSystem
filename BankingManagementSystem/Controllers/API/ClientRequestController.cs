using BankingManagementSystem.BLL;
using BankingManagementSystem.Models.Constants;
using BankingManagementSystem.Models.DTOs;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace BankingManagementSystem.Controllers.API
{
    [Authorize(Roles = "CLIENT")]
    [RoutePrefix("api/client/requests")]
    public class ClientRequestController : ApiController
    {
        // Client-side endpoints for viewing and sending requests

        [HttpGet]
        [Route("received")]
        public async Task<IHttpActionResult> GetReceivedRequestsAsync([FromUri] int clientId, [FromUri] string sortColumn = DbColumns.CreatedOn, [FromUri] string sortDirection = "DESC")
        {
            var requests = await RequestBLL.GetReceivedRequestsForClientAsync(clientId, sortColumn, sortDirection);
            return Ok(requests);
        }

        [HttpGet]
        [Route("sent")]
        public async Task<IHttpActionResult> GetSentRequestsAsync([FromUri] int clientId, [FromUri] string sortColumn = DbColumns.CreatedOn, [FromUri] string sortDirection = "DESC")
        {
            var requests = await RequestBLL.GetSentRequestsByClientAsync(clientId, sortColumn, sortDirection);
            return Ok(requests);
        }

        [HttpPost]
        [Route("send-request")]
        public async Task<IHttpActionResult> SendRequestAsync([FromUri] int clientId, [FromUri] string requestType, [FromBody] ClientDTO client)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid client details.");

            var (requestId, Message) = await RequestBLL.SendRequestsByClientAsync(clientId, requestType, client);
            if (requestId != 0)
                return Ok(new { RequestID = requestId });
            else 
                return BadRequest(Message);
            //return requestId != 0 ? Ok(new { RequestID = requestId }) : BadRequest("Request not sent. Invalid details.");
        }
        [HttpPut]
        [Route("{id:int}/delete")]
        public async Task<IHttpActionResult> DeleteClientRequestByClientAsync([FromUri] int id, [FromUri] int repliedBy)
        {
            //bool result = await RequestBLL.UpdateStatusAsync(id, "Rejected", repliedBy);
            //return Ok(new { success = result });
            bool result = await RequestBLL.UpdateStatusAsync(id, "Rejected", repliedBy);
            return Ok(new { success = result });
            //if (!success)
            //    return BadRequest();

            //return Ok(new { success = true, message = "Request deleted successfully." });
        }
    }


}