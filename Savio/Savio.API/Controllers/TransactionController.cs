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
        public readonly string _controllerName;
        public TransactionController()
        {
            _transactionAppService = new TransactionAppService();
            _controllerName = GetType().Name;
        }

        [HttpGet]
        [HttpPost]
        [Route("GetAllTransactionsWithData")]
        public IActionResult GetAllTransactionsWithData(TransactionModel request)
        {
            var w = Stopwatch.StartNew();
            string method = ControllerContext.ActionDescriptor.ActionName;

            LogHelper.Logging(_controllerName, method, request, true);

            var r = _transactionAppService.GetAllTransactionsWithData(request);

            var response = new TransactionResponse
            {
                Code = r.Item1,
                Message = r.Item1.ToErrorMsg(),
                Transaction = r.Item2
            };

            LogHelper.Logging(_controllerName, method, response, false);
            return Ok(response);
        }

        [HttpPost]
        [Route("AddEditTransaction")]
        public IActionResult AddEditTransaction(TransactionModel request)
        {
            var w = Stopwatch.StartNew();
            string method = ControllerContext.ActionDescriptor.ActionName;

            LogHelper.Logging(_controllerName, method, request, true);

            var r = _transactionAppService.AddEditTransaction(request);

            var response = new TransactionResponse
            {
                Code = r,
                Message = r.ToErrorMsg(),
            };

            LogHelper.Logging(_controllerName, method, response, false);
            return Ok(response);
        }

        [HttpPost]
        [Route("DeleteTransactionById/{id:int}")]
        public IActionResult DeleteTransactionById(int id)
        {
            var w = Stopwatch.StartNew();
            string method = ControllerContext.ActionDescriptor.ActionName;

            LogHelper.Logging(_controllerName, method, id, true);

            var r = _transactionAppService.DeleteTransactionById(id);

            var response = new TransactionResponse
            {
                Code = r,
                Message = r.ToErrorMsg(),
            };

            LogHelper.Logging(_controllerName, method, response, false);
            return Ok(response);
        }
    }
}
