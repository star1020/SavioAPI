using System;
using System.ServiceModel;
using User;
using User.Contract;

namespace Savio.Core
{
    public class UserServiceHost : AbstractServiceHost<IUserService>
    {
        private readonly IUserRepository _repo;

        public UserServiceHost(string name, string localEndpoint, string remoteEndpoint, int maxConcurrentCalls)
            : base(name, localEndpoint, remoteEndpoint, maxConcurrentCalls)
        {
            var configuration = GetConfig.Configuration;
            _repo = new UserRepository(configuration);
        }

        protected override IUserService InitServiceInstance()
        {
            return new UserService(_repo);
        }

        public void Close()
        {
            Dispose(); // clean and simple
        }
    }
}
