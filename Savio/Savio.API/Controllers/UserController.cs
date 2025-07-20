using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NLog;
using Savio.Core;
using Savio.Core.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Web;
using User.API.App_Service;
using User.API.Models;

namespace User.API.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController : ControllerBase
    {
        readonly IUserAppService _userAppService;
        public UserController()
        {
            _userAppService = new UserAppService();
        }

        [HttpGet]
        [HttpPost]
        [Route("GetAllUsers")]
        public IActionResult GetAllUsers()
        {
            var w = Stopwatch.StartNew();
            var correlationId = Guid.NewGuid().ToString();
            string method = LogHelper.GetMethodName();

            LogManager.GetCurrentClassLogger().Info($"[{correlationId}] {method} Request -> no body");

            var r = _userAppService.GetAllUsers();

            var response = new UserResponse
            {
                Code = r.Item1,
                Message = r.Item1.ToErrorMsg(),
                User = r.Item2
            };

            LogManager.GetCurrentClassLogger().Info($"[{correlationId}] {method} Response -> {JsonConvert.SerializeObject(response)} | TotalProcessedTimeMls = [{w.ElapsedMilliseconds}]");

            return Ok(response);
        }


        [HttpPost]
        [Route("AddEditUsers")]
        public IActionResult AddEditUsers(UserModel request)
        {
            var w = Stopwatch.StartNew();
            var correlationId = Guid.NewGuid().ToString();
            string method = LogHelper.GetMethodName();

            LogManager.GetCurrentClassLogger().Info($"[{correlationId}] {method} Request -> {request}");

            var r = _userAppService.AddEditUsers(request);

            var response = new UserResponse
            {
                Code = r,
                Message = r.ToErrorMsg(),
            };

            LogManager.GetCurrentClassLogger().Info($"[{correlationId}] {method} Response -> {JsonConvert.SerializeObject(response)} | TotalProcessedTimeMls = [{w.ElapsedMilliseconds}]");

            return Ok(response);
        }

    }
}