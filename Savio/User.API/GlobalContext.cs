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

        static GlobalContext()
        {
            try
            {
                UserService = new UserServiceProxy();
            }
            catch (Exception ex)
            {
                // Log to console or a file if needed
                Console.WriteLine("[GlobalContext] Failed to initialize UserServiceProxy: " + ex.ToString());
                throw;
            }
        }
    }

}