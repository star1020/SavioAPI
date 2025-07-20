using Newtonsoft.Json;
using NLog;
using Savio.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using User.Contract;

namespace User
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = true)]
    public class UserService : IUserService
    {
        private readonly IUserRepository _db;

        public UserService(IUserRepository db)
        {
            _db = db;
        }


        public Tuple<int, List<UserModel>> GetAllUsers()
        {
            var method = MethodBase.GetCurrentMethod().Name;
            LogManager.GetCurrentClassLogger().Info($"[{method}] RequestInfo -> no data");
            var r = _db.GetAllUsers();
            LogManager.GetCurrentClassLogger().Info($"[{method}] UserInfo -> {JsonConvert.SerializeObject(r)}");

            if (r == null) return new Tuple<int, List<UserModel>>(ErrorCode.OperationError, new List<UserModel>());
            if (r.Count == 0) return new Tuple<int, List<UserModel>>(ErrorCode.OperationError, new List<UserModel>());

            return new Tuple<int, List<UserModel>>(ErrorCode.Success, r);

        }

        public int InsertUser(UserModel user)
        {
            var method = MethodBase.GetCurrentMethod().Name;
            LogManager.GetCurrentClassLogger().Info($"[{method}] RequestInfo -> {JsonConvert.SerializeObject(user)}");
            var r = _db.InsertUser(user);
            LogManager.GetCurrentClassLogger().Info($"[{method}] Result -> {JsonConvert.SerializeObject(r)}");
            return r;
        }
    }
}
