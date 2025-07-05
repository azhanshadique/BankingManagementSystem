using BankingManagementSystem.BLL;
using BankingManagementSystem.Models.Constants;
using BankingManagementSystem.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace BankingManagementSystem.Controllers.API
{
    [Authorize(Roles = "ADMIN,CLIENT")]
    [RoutePrefix("api/requests")]
    public class RequestsController : ApiController
    {
        // common endpoints for managing client requests

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> GetRequestsByIdAsync([FromUri] int id)
        {
            var request = await RequestBLL.GetRequestByIdAsync(id);
            return request != null ? Ok(request) : (IHttpActionResult)NotFound();
        }

        [HttpPut]
        [Route("{id:int}/update/request")]
        public async Task<IHttpActionResult> UpdateRequestByIdAsync([FromUri] int id, [FromBody] ClientDTO client)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid client data.");

            var (result, message) = await RequestBLL.UpdateRequestAsync(id, client);
            if (result)
                return Ok(new { success = result });
            else
                return BadRequest(message);
        }

        [HttpPut]
        [Route("{id:int}/approve")]
        public async Task<IHttpActionResult> ApproveRequestAsync(int id, [FromUri] int repliedBy)
        {
            bool result = await RequestBLL.UpdateStatusAsync(id, "Approved", repliedBy);
            return Ok(new { success = result });
        }

        [HttpPut]
        [Route("{id:int}/reject")]
        public async Task<IHttpActionResult> RejectRequestAsync(int id, [FromUri] int repliedBy)
        {
            bool result = await RequestBLL.UpdateStatusAsync(id, "Rejected", repliedBy);
            return Ok(new { success = result });
        }



    }
}