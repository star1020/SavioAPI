using Abp.Application.Services;
using Newtonsoft.Json;
using NLog;
using Savio.Core;
using Savio.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using User.API.Models;
using User.Contract;

namespace User.API.App_Service
{
    public interface IUserAppService : IApplicationService
    {
        Tuple<int, List<UserModel>> GetAllUsers();
        int AddEditUsers(UserModel request);
    }
    public class UserAppService : IUserAppService
    {
        private readonly IUserService _userService;
        private readonly IUserRepository _userDb;
        private readonly bool _isProd;

        public UserAppService()
        {
            _userService = GlobalContext.UserService;
            _userDb = GlobalContext.UserRepository;

            var env = GetConfig.Configuration["Env"];
            _isProd = !string.IsNullOrWhiteSpace(env) && env.Equals("PROD", StringComparison.OrdinalIgnoreCase);
        }

        public Tuple<int, List<UserModel>> GetAllUsers()
        {
            if (_isProd)
            {
                var method = MethodBase.GetCurrentMethod().Name;
                LogManager.GetCurrentClassLogger().Info($"[{method}] RequestInfo -> no data");
                var p = _userDb.GetAllUsers();
                LogManager.GetCurrentClassLogger().Info($"[{method}] UserInfo -> {JsonConvert.SerializeObject(p)}");

                if (p == null) return new Tuple<int, List<UserModel>>(ErrorCode.OperationError, new List<UserModel>());
                if (p.Count == 0) return new Tuple<int, List<UserModel>>(ErrorCode.OperationError, new List<UserModel>());

                return new Tuple<int, List<UserModel>>(ErrorCode.Success, p);
            }
            var r = _userService.GetAllUsers();
            return new Tuple<int, List<UserModel>>(r.Item1, r.Item2);

        }

        public int AddEditUsers(UserModel request)
        {
            if (_isProd)
            {
                var method = MethodBase.GetCurrentMethod().Name;
                LogManager.GetCurrentClassLogger().Info($"[{method}] RequestInfo -> {JsonConvert.SerializeObject(request)}");
                var r = _userDb?.InsertUser(request) ?? -1;
                LogManager.GetCurrentClassLogger().Info($"[{method}] Result -> {JsonConvert.SerializeObject(r)}");
                return r;
            }
            return _userService.InsertUser(request);
        }


    }
}