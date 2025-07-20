using System;
using System.ServiceModel;

namespace Savio.Core
{
    public class ServiceHost
    {
        public abstract class AbstractServiceHost<T> : IDisposable
        {
            protected string Name { get; }
            protected string LocalEndpoint { get; }
            protected string RemoteEndpoint { get; }
            protected int MaxConcurrentCalls { get; }
            private System.ServiceModel.ServiceHost _wcfHost;

            protected AbstractServiceHost(string name, string localEndpoint, string remoteEndpoint, int maxConcurrentCalls)
            {
                Name = name;
                LocalEndpoint = localEndpoint;
                RemoteEndpoint = remoteEndpoint;
                MaxConcurrentCalls = maxConcurrentCalls;
            }

            public virtual void Open()
            {
                _wcfHost = new System.ServiceModel.ServiceHost(InitServiceInstance(), new Uri(LocalEndpoint));

                var binding = new NetTcpBinding
                {
                    Security = { Mode = SecurityMode.None },
                    MaxReceivedMessageSize = int.MaxValue,
                    MaxConnections = MaxConcurrentCalls,
                    ReceiveTimeout = TimeSpan.FromMinutes(10),
                    SendTimeout = TimeSpan.FromMinutes(10)
                };

                _wcfHost.AddServiceEndpoint(typeof(T), binding, "");

                _wcfHost.Open();
                Console.WriteLine($"Service '{Name}' listening at {RemoteEndpoint}");
            }

            public void Dispose()
            {
                if (_wcfHost?.State == CommunicationState.Opened)
                {
                    _wcfHost.Close();
                }
                else
                {
                    _wcfHost?.Abort();
                }
                Console.WriteLine($"Service '{Name}' shutting down...");
            }

            protected abstract T InitServiceInstance();
        }
    }
}