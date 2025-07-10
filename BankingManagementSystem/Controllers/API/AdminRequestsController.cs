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
    }

}