using Savio.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace User.Contract
{
    [ServiceContract]
    public interface IUserService
    {
        [OperationContract]
        Tuple<int, List<UserModel>> GetAllUsers();
        [OperationContract]
        int InsertUser(UserModel user);
    }
}
