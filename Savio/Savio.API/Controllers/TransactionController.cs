using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NLog;
using Savio.Core;
using Savio.Core.Data;
using Transaction.API.App_Service;
using Transaction.API.Models;

namespace Transaction.API.Controllers
{
    [ApiController]
    [Route("transaction")]
    public class TransactionController : ControllerBase
    {
        readonly ITransactionAppService _transactionAppService;
        public TransactionController()
        {
            _transactionAppService = new TransactionAppService();
        }

        [HttpGet]
        [HttpPost]
        [Route("GetAllTransactions")]
        public IActionResult GetAllTransactions()
        {
            var w = Stopwatch.StartNew();
            var correlationId = Guid.NewGuid().ToString();
            string method = LogHelper.GetMethodName();

            LogManager.GetCurrentClassLogger().Info($"[{correlationId}] {method} Request -> no body");

            var r = _transactionAppService.GetAllTransactions();

            var response = new TransactionResponse
            {
                Code = r.Item1,
                Message = r.Item1.ToErrorMsg(),
                Transaction = r.Item2
            };

            LogManager.GetCurrentClassLogger().Info($"[{correlationId}] {method} Response -> {JsonConvert.SerializeObject(response)} | TotalProcessedTimeMls = [{w.ElapsedMilliseconds}]");

            return Ok(response);
        }

        [HttpPost]
        [Route("AddEditTransaction")]
        public IActionResult AddEditTransaction(TransactionModel request)
        {
            var w = Stopwatch.StartNew();
            var correlationId = Guid.NewGuid().ToString();
            string method = LogHelper.GetMethodName();

            LogManager.GetCurrentClassLogger().Info($"[{correlationId}] {method} Request -> {request}");

            var r = _transactionAppService.AddEditTransaction(request);

            var response = new TransactionResponse
            {
                Code = r,
                Message = r.ToErrorMsg(),
            };

            LogManager.GetCurrentClassLogger().Info($"[{correlationId}] {method} Response -> {JsonConvert.SerializeObject(response)} | TotalProcessedTimeMls = [{w.ElapsedMilliseconds}]");

            return Ok(response);
        }

        [HttpGet]
        [HttpPost]
        [Route("GetTransactionById/{id:int}")]
        public IActionResult GetTransactionById(int id)
        {
            var w = Stopwatch.StartNew();
            var correlationId = Guid.NewGuid().ToString();
            string method = LogHelper.GetMethodName();

            LogManager.GetCurrentClassLogger().Info($"[{correlationId}] {method} Request -> no body");

            var r = _transactionAppService.GetTransactionById(id);

            var response = new GetTransactionResponse
            {
                Code = r.Item1,
                Message = r.Item1.ToErrorMsg(),
                Transaction = r.Item2
            };

            LogManager.GetCurrentClassLogger().Info($"[{correlationId}] {method} Response -> {JsonConvert.SerializeObject(response)} | TotalProcessedTimeMls = [{w.ElapsedMilliseconds}]");

            return Ok(response);
        }

        [HttpGet]
        [HttpPost]
        [Route("DeleteTransactionById/{id:int}")]
        public IActionResult DeleteTransactionById(int id)
        {
            var w = Stopwatch.StartNew();
            var correlationId = Guid.NewGuid().ToString();
            string method = LogHelper.GetMethodName();

            LogManager.GetCurrentClassLogger().Info($"[{correlationId}] {method} Request -> {id}");

            var r = _transactionAppService.DeleteTransactionById(id);

            var response = new TransactionResponse
            {
                Code = r,
                Message = r.ToErrorMsg(),
            };

            LogManager.GetCurrentClassLogger().Info($"[{correlationId}] {method} Response -> {JsonConvert.SerializeObject(response)} | TotalProcessedTimeMls = [{w.ElapsedMilliseconds}]");

            return Ok(response);
        }
    }
}
