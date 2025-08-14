using Abp.Application.Services;
using Newtonsoft.Json;
using NLog;
using Savio.API;
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

        public UserAppService()
        {
            _userService = GlobalContext.UserService;
        }

        public Tuple<int, List<UserModel>> GetAllUsers()
        {
            var r = _userService.GetAllUsers();
            return new Tuple<int, List<UserModel>>(r.Item1, r.Item2);

        }

        public int AddEditUsers(UserModel request)
        {
            return _userService.InsertUser(request);
        }

    }
}