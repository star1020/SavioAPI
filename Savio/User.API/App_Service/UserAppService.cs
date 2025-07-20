using Abp.Application.Services;
using Savio.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
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
        readonly IUserService _userService;

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
            var r = _userService.InsertUser(request);
            return r;
        }


    }
}