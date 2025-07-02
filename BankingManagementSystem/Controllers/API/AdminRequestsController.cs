using BankingManagementSystem.BLL;
using BankingManagementSystem.DAL;
using BankingManagementSystem.Models.Constants;
using BankingManagementSystem.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Services.Description;

namespace BankingManagementSystem.Controllers.API
{

    [RoutePrefix("api/requests")]
    public class AdminRequestsController : ApiController
    {
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetRequestsByStatus(string status = "Pending", string sortBy = DbColumns.CreatedOn, string sortDirection = "DESC")
        {
            var requests = AdminBLL.GetRequestsByStatus(status, sortBy, sortDirection);
            return Ok(requests);
        }

        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetRequestsById(int id)
        {
            //var request = RequestDAL.GetPendingRequestById(id);
            var request = AdminBLL.GetRequestById(id);
            return request != null ? Ok(request) : (IHttpActionResult)NotFound();
        }

        [HttpPut]
        [Route("{id:int}/update/payload")]
        public IHttpActionResult UpdatePayloadById(int id, [FromBody] ClientDTO client)
        {
            bool result = AdminBLL.UpdatePayload(id, client, out string message);
            if (result)
                return Ok(new { success = result });
            else
                return BadRequest(message);
            //return Ok(new { success = result });
        }

        [HttpPut]
        [Route("{id:int}/update/status")]
        public IHttpActionResult UpdateStatusById(int id, [FromBody] string status, int repliedBy = -1)
        {
            bool result = AdminBLL.UpdateStatus(id, status, repliedBy);
            return Ok(new { success = result });
        }



        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult DeleteRequest(int id, string status="Pending")
        {
            bool result = AdminBLL.DeleteRequest(id, status);
            return Ok(new { success = result });
        }
    }

}