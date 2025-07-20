using Savio.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using User.Contract;
using User.Proxy;

namespace User.API
{
    public class GlobalContext
    {
        public static IUserService UserService { get; }
        public static IUserRepository UserRepository { get; }

        static GlobalContext()
        {
            try
            {
                var configuration = GetConfig.Configuration;
                var env = configuration["Env"];

                if (!string.IsNullOrWhiteSpace(env) && env.Equals("PROD"))
                {
                    UserRepository = new UserRepository(configuration);
                }
                else
                {
                    UserService = new UserServiceProxy(configuration);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[GlobalContext] Failed to initialize UserServiceProxy: " + ex.ToString());
                throw;
            }
        }
    }

}