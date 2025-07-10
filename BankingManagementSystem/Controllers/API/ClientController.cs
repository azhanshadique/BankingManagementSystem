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

    //[Authorize(Roles = "CLIENT")]
    [RoutePrefix("api/client")]
    public class ClientController : ApiController
    {
        [Authorize(Roles = "ADMIN, CLIENT")]
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> GetClientByIdAsync([FromUri] int id)
        {
            var client = await ClientBLL.GetClientByIdAsync(id);
            return client != null ? Ok(client) : (IHttpActionResult)NotFound();
        }

        [Authorize(Roles = "ADMIN, CLIENT")]
        [HttpGet]
        [Route("{clientId:int}/accounts")]
        public async Task<IHttpActionResult> GetClientAccountsByTypeAsync([FromUri] int clientId, [FromUri] string type = "All")
        {
            var accounts = await ClientBLL.GetAccountsByClientIdAndTypeAsync(clientId, type);

            if (accounts == null || !accounts.Any())
                return NotFound();

            return Ok(accounts);
        }

        [Authorize(Roles = "CLIENT")]
        [HttpPost]
        [Route("transaction/deposit")]
        public async Task<IHttpActionResult> DepositAsync([FromBody] DepositDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await TransactionBLL.DepositAmountAsync(request);
            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }

        [Authorize(Roles = "CLIENT")]
        [HttpPost]
        [Route("transaction/withdraw")]
        public async Task<IHttpActionResult> WithdrawAsync([FromBody] WithdrawDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await TransactionBLL.WithdrawAmountAsync(request);
            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }

        [Authorize(Roles = "CLIENT")]
        [HttpPost]
        [Route("transaction/transfer")]
        public async Task<IHttpActionResult> TransferAsync([FromBody] TransferDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await TransactionBLL.TransferAmountAsync(request);
            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }

        [Authorize(Roles = "CLIENT")]
        [HttpGet]
        [Route("transaction/history")]
        public async Task<IHttpActionResult> GetTransactionHistoryAsync([FromUri] long accountNumber)
        {
            var transactions = await TransactionBLL.GetTransactionHistoryAsync(accountNumber);
            if (transactions == null || !transactions.Any())
                return BadRequest("Invalid Account Number.");

            return Ok(transactions);
        }
    }
}