using BankingManagementSystem.BLL;
using BankingManagementSystem.Models.Constants;
using BankingManagementSystem.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Services.Description;

namespace BankingManagementSystem.Controllers.API
{
    [RoutePrefix("api")]
    public class ClientRequestController : ApiController
	{
        [HttpGet]
        [Route("client/requests/received")]
        public async Task<IHttpActionResult> GetReceivedRequests(int clientId, string sortColumn = DbColumns.CreatedOn, string sortDirection = "DESC")
        {
            var requests = await RequestBLL.GetReceivedRequestsForClient(clientId, sortColumn, sortDirection);
            return Ok(requests);
        }

        [HttpGet]
        [Route("client/requests/sent")]
        public async Task<IHttpActionResult> GetSentRequests(int clientId, string sortColumn = DbColumns.CreatedOn, string sortDirection = "DESC")
        {
            var requests = await RequestBLL.GetSentRequestsByClient(clientId, sortColumn, sortDirection);
            return Ok(requests);
        }

        [HttpPost]
        [Route("client/send-request")]
        public async Task<IHttpActionResult> SendRequest(int clientId, string requestType, [FromBody] ClientDTO client)
        {
            var requests = await RequestBLL.SendRequestsByClient(clientId, requestType, client);
            if (requests != 0)
                return Ok(new { RequestID = requests });
            else
                return BadRequest("Request not sent, Invalid given details.");
            
        }

        //[HttpGet]
        //[Route("api/request/{id}")]
        //public async Task<IHttpActionResult> GetRequestById(int id)
        //{
        //    var request = await RequestBLL.GetRequestByIdAsync(id);
        //    return Ok(request);
        //}

        //[HttpPost]
        //[Route("api/request/approve-client")]
        //public async Task<IHttpActionResult> ApproveClientSideRequest([FromBody] ApproveRequestDTO dto)
        //{
        //    bool result = await RequestBLL.ApproveClientSideRequestAsync(dto.RequestId, dto.ClientId);
        //    return Ok(result);
        //}

        //[HttpPost]
        //[Route("api/request/reject-client")]
        //public async Task<IHttpActionResult> RejectClientSideRequest([FromBody] ApproveRequestDTO dto)
        //{
        //    bool result = await RequestBLL.RejectClientSideRequestAsync(dto.RequestId, dto.ClientId);
        //    return Ok(result);
        //}

        //[HttpDelete]
        //[Route("api/request/{id}")]
        //public async Task<IHttpActionResult> DeleteRequest(int id)
        //{
        //    bool result = await RequestBLL.DeleteRequestAsync(id);
        //    return Ok(result);
        //}

        //[HttpPost]
        //[Route("api/request/update")]
        //public async Task<IHttpActionResult> UpdatePayload(UpdatePayloadDTO dto)
        //{
        //    bool result = await RequestBLL.UpdatePayloadAsync(dto.RequestId, dto.Client);
        //    return Ok(result);
        //}

    }

}