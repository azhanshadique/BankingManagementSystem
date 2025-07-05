using BankingManagementSystem.BLL;
using BankingManagementSystem.Models.Constants;
using BankingManagementSystem.Models.DTOs;
using System.Threading.Tasks;
using System.Web.Http;

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
                return BadRequest("Invalid client data.");

            var requestId = await RequestBLL.SendRequestsByClientAsync(clientId, requestType, client);
            if (requestId != 0)
                return Ok(new { RequestID = requestId });
            else 
                return BadRequest("Request not sent. Invalid details.");
            //return requestId != 0 ? Ok(new { RequestID = requestId }) : BadRequest("Request not sent. Invalid details.");
        }
    }



    //[RoutePrefix("api")]
    //public class ClientRequestController : ApiController
    //{
    //    [HttpGet]
    //    [Route("client/requests/received")]
    //    public async Task<IHttpActionResult> GetReceivedRequests(int clientId, string sortColumn = DbColumns.CreatedOn, string sortDirection = "DESC")
    //    {
    //        var requests = await RequestBLL.GetReceivedRequestsForClient(clientId, sortColumn, sortDirection);
    //        return Ok(requests);
    //    }

    //    [HttpGet]
    //    [Route("client/requests/sent")]
    //    public async Task<IHttpActionResult> GetSentRequests(int clientId, string sortColumn = DbColumns.CreatedOn, string sortDirection = "DESC")
    //    {
    //        var requests = await RequestBLL.GetSentRequestsByClient(clientId, sortColumn, sortDirection);
    //        return Ok(requests);
    //    }

    //    [HttpPost]
    //    [Route("client/send-request")]
    //    public async Task<IHttpActionResult> SendRequest(int clientId, string requestType, [FromBody] ClientDTO client)
    //    {
    //        var requests = await RequestBLL.SendRequestsByClient(clientId, requestType, client);
    //        if (requests != 0)
    //            return Ok(new { RequestID = requests });
    //        else
    //            return BadRequest("Request not sent, Invalid given details.");

    //    }

    //    //[HttpGet]
    //    //[Route("api/request/{id}")]
    //    //public async Task<IHttpActionResult> GetRequestById(int id)
    //    //{
    //    //    var request = await RequestBLL.GetRequestByIdAsync(id);
    //    //    return Ok(request);
    //    //}

    //    //[HttpPost]
    //    //[Route("api/request/approve-client")]
    //    //public async Task<IHttpActionResult> ApproveClientSideRequest([FromBody] ApproveRequestDTO dto)
    //    //{
    //    //    bool result = await RequestBLL.ApproveClientSideRequestAsync(dto.RequestId, dto.ClientId);
    //    //    return Ok(result);
    //    //}

    //    //[HttpPost]
    //    //[Route("api/request/reject-client")]
    //    //public async Task<IHttpActionResult> RejectClientSideRequest([FromBody] ApproveRequestDTO dto)
    //    //{
    //    //    bool result = await RequestBLL.RejectClientSideRequestAsync(dto.RequestId, dto.ClientId);
    //    //    return Ok(result);
    //    //}

    //    //[HttpDelete]
    //    //[Route("api/request/{id}")]
    //    //public async Task<IHttpActionResult> DeleteRequest(int id)
    //    //{
    //    //    bool result = await RequestBLL.DeleteRequestAsync(id);
    //    //    return Ok(result);
    //    //}

    //    //[HttpPost]
    //    //[Route("api/request/update")]
    //    //public async Task<IHttpActionResult> UpdatePayload(UpdatePayloadDTO dto)
    //    //{
    //    //    bool result = await RequestBLL.UpdatePayloadAsync(dto.RequestId, dto.Client);
    //    //    return Ok(result);
    //    //}

    //}

}