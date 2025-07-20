using Newtonsoft.Json;
using NLog;
using Savio.Core.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Http;
using User.API.App_Service;
using User.API.Models;

namespace User.API.Controllers
{
    [RoutePrefix("user")]
    public class UserController : ApiController
    {
        readonly IUserAppService _userAppService;
        public UserController()
        {
            _userAppService = new UserAppService();
        }

        [HttpPost]
        [Route("GetAllUsers")]
        public HttpResponseMessage GetAllUsers()
        {
            var w = Stopwatch.StartNew();
            var correlationId = Guid.NewGuid().ToString();
            var method = MethodBase.GetCurrentMethod().Name;

            LogManager.GetCurrentClassLogger().Info($"[{correlationId}] {method} Request -> no body");

            var r = _userAppService.GetAllUsers();

            var response = new UserResponse
            {
                Code = r.Item1,
                Message = r.Item1.ToErrorMsg(),
                User = r.Item2
            };

            LogManager.GetCurrentClassLogger().Info($"[{correlationId}] {method} Response -> {JsonConvert.SerializeObject(response)} | TotalProcessedTimeMls = [{w.ElapsedMilliseconds}]");

            return new HttpResponseMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject(response), Encoding.UTF8, "application/json")
            };
        }


        [HttpPost]
        [Route("AddEditUsers")]
        public HttpResponseMessage AddEditUsers(UserModel request)
        {
            var w = Stopwatch.StartNew();
            var correlationId = Guid.NewGuid().ToString();
            var method = MethodBase.GetCurrentMethod().Name;

            LogManager.GetCurrentClassLogger().Info($"[{correlationId}] {method} Request -> {request}");

            var r = _userAppService.AddEditUsers(request);

            var response = new UserResponse
            {
                Code = r,
                Message = r.ToErrorMsg(),
            };

            LogManager.GetCurrentClassLogger().Info($"[{correlationId}] {method} Response -> {JsonConvert.SerializeObject(response)} | TotalProcessedTimeMls = [{w.ElapsedMilliseconds}]");

            return new HttpResponseMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject(response), Encoding.UTF8, "application/json")
            };
        }
    }
}