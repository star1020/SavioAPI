using Savio.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using User.Contract;
using User;

namespace User.API
{
    public class GlobalContext
    {
        public static IUserService UserService { get; }
        private static IUserRepository UserRepository { get; }

        static GlobalContext()
        {
            try
            {
                var configuration = GetConfig.Configuration;

                UserRepository = new UserRepository(configuration);
                UserService = new UserService(UserRepository);
            }
            catch (Exception ex)
            {
                Console.WriteLine("[GlobalContext] Failed to initialize: " + ex.ToString());
                throw;
            }
        }
    }

}