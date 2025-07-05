using BankingManagementSystem.BLL;
using BankingManagementSystem.DAL;
using BankingManagementSystem.Models.Constants;
using BankingManagementSystem.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Services.Description;

namespace BankingManagementSystem.Controllers.API
{
    [Authorize(Roles = "ADMIN")]
    [RoutePrefix("api/admin/requests")]
    public class AdminRequestsController : ApiController
    {
        // Admin-only endpoints for managing client requests

        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetRequestsByStatusAsync([FromUri] string status = "Pending", [FromUri] string sortColumn = DbColumns.CreatedOn, [FromUri] string sortDirection = "DESC")
        {
            var requests = await RequestBLL.GetRequestsByStatusAsync(status, sortColumn, sortDirection);
            return Ok(requests);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> DeleteRequestAsync([FromUri] int id, [FromUri] string status = "Pending")
        {
            bool result = await RequestBLL.DeleteRequestAsync(id, status);
            return Ok(new { success = result });
        }

        //[HttpGet]
        //[Route("{id:int}")]
        //public async Task<IHttpActionResult> GetRequestsById([FromUri] int id)
        //{
        //    var request = await RequestBLL.GetRequestByIdAsync(id);
        //    return request != null ? Ok(request) : (IHttpActionResult)NotFound();
        //}

        //[HttpPut]
        //[Route("{id:int}/update/payload")]
        //public async Task<IHttpActionResult> UpdatePayloadById([FromUri] int id, [FromBody] ClientDTO client)
        //{
        //    var (result, message) = await RequestBLL.UpdatePayloadAsync(id, client);
        //    if (result)
        //        return Ok(new { success = result });
        //    else
        //        return BadRequest(message);

        //    //return result ? Ok(new { success = result }) : BadRequest(message);
        //}

        //[HttpPut]
        //[Route("{id:int}/update/status")]
        //public async Task<IHttpActionResult> UpdateStatusById([FromUri] int id, [FromUri] string status, [FromUri] int repliedBy = -1)
        //{
        //    bool result = await RequestBLL.UpdateStatusAsync(id, status, repliedBy);
        //    return Ok(new { success = result });
        //}

        //[HttpDelete]
        //[Route("{id:int}")]
        //public async Task<IHttpActionResult> DeleteRequest([FromUri] int id, [FromUri] string status = "Pending")
        //{
        //    bool result = await RequestBLL.DeleteRequestAsync(id, status);
        //    return Ok(new { success = result });
        //}
    }


    //[RoutePrefix("api/requests")]
    //public class AdminRequestsController : ApiController
    //{
    //    [HttpGet]
    //    [Route("")]
    //    public async Task<IHttpActionResult> GetRequestsByStatus(string status = "Pending", string sortBy = DbColumns.CreatedOn, string sortDirection = "DESC")
    //    {
    //        var requests = await RequestBLL.GetRequestsByStatusAsync(status, sortBy, sortDirection);
    //        return Ok(requests);
    //    }

    //    [HttpGet]
    //    [Route("{id:int}")]
    //    public async Task<IHttpActionResult> GetRequestsById(int id)
    //    {
    //        var request = await RequestBLL.GetRequestByIdAsync(id);
    //        return request != null ? Ok(request) : (IHttpActionResult)NotFound();
    //    }

    //    [HttpPut]
    //    [Route("{id:int}/update/payload")]
    //    public async Task<IHttpActionResult> UpdatePayloadById(int id, [FromBody] ClientDTO client)
    //    {
    //        var (result, message) = await RequestBLL.UpdatePayloadAsync(id, client);
    //        if (result)
    //            return Ok(new { success = result });
    //        else
    //            return BadRequest(message);
    //    }

    //    [HttpPut]
    //    [Route("{id:int}/update/status")]
    //    public async Task<IHttpActionResult> UpdateStatusById(int id, [FromBody] string status, int repliedBy = -1)
    //    {
    //        bool result = await RequestBLL.UpdateStatusAsync(id, status, repliedBy);
    //        return Ok(new { success = result });
    //    }



    //    [HttpDelete]
    //    [Route("{id:int}")]
    //    public async Task<IHttpActionResult> DeleteRequest(int id, string status = "Pending")
    //    {
    //        bool result = await RequestBLL.DeleteRequestAsync(id, status);
    //        return Ok(new { success = result });
    //    }
    //}

}