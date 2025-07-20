using NLog;
using Npgsql;
using Savio.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Contract;

namespace User.Client
{
    public class Program
    {
        static void Main(string[] args)
        {
            var ip = ConfigurationManager.AppSettings["service-ip"];
            var port = int.Parse(ConfigurationManager.AppSettings["service-port"]);
            var maxConcurrentCalls = int.Parse(ConfigurationManager.AppSettings["service-max-concurrentcalls"]);

            Console.Title = "User Service";

            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

            using (var host = new UserServiceHost(
                name: "UserService",
                localEndpoint: $"net.tcp://localhost:{port}/UserService",
                remoteEndpoint: $"net.tcp://{ip}:{port}/UserService",
                maxConcurrentCalls: maxConcurrentCalls))
            {
                var w = Stopwatch.StartNew();
                host.Open();
                w.Stop();

                Console.WriteLine("[{0}] service started in {1} seconds...", DateTime.Now, w.ElapsedMilliseconds / 1000d);

                while (true)
                {
                    Console.WriteLine($"[{DateTime.Now}] press 'E' to exit");

                    var c = Console.ReadLine();
                    if (c != null && c.ToUpper() == "E") break;
                }
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();

            Environment.Exit(0);
        }

        static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception exp)
            {
                Console.WriteLine(exp.Message);
                Console.WriteLine(exp.StackTrace);
                LogManager.GetCurrentClassLogger().Error(exp);
            }
        }

        public class UserServiceHost : ServiceHost.AbstractServiceHost<IUserService>
        {
            readonly IUserRepository _repo;
            public UserServiceHost(string name, string localEndpoint, string remoteEndpoint, int maxConcurrentCalls)
                : base(name, localEndpoint, remoteEndpoint, maxConcurrentCalls)
            {
                _repo = new UserRepository(
                ConfigurationManager.ConnectionStrings["Postgres"].ConnectionString);

            }

            protected override IUserService InitServiceInstance()
            {
                return new UserService(_repo);
            }
        }
    }
}
