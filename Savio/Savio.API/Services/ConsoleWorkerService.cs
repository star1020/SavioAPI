using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Savio.Core;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Savio.API.Services
{
    public class UserServiceBackground : BackgroundService
    {
        private readonly ILogger<UserServiceBackground> _logger;
        private UserServiceHost _host;

        public UserServiceBackground(ILogger<UserServiceBackground> logger)
        {
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(() =>
            {
                try
                {
                    var w = Stopwatch.StartNew();

                    _host = new UserServiceHost(
                        name: "UserService",
                        localEndpoint: $"net.tcp://localhost:1001/UserService",
                        remoteEndpoint: $"net.tcp://localhost:1001/UserService",
                        maxConcurrentCalls: 100);

                    _host.Open();

                    w.Stop();
                    _logger.LogInformation($"[UserService] Started in {w.Elapsed.TotalSeconds:F2} seconds");

                    stoppingToken.Register(() =>
                    {
                        _logger.LogInformation("Stopping UserService...");
                        _host.Close();
                    });

                    while (!stoppingToken.IsCancellationRequested)
                    {
                        Thread.Sleep(5000);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unhandled error in UserServiceBackground");
                }
            });
        }

        public override void Dispose()
        {
            base.Dispose();
            _host?.Close();
        }
    }
}
