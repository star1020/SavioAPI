using Microsoft.Extensions.Configuration;
using NLog;
using Savio.Core.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using User.Contract;

namespace User.Proxy
{
    public class UserServiceProxy : IUserService, IDisposable
    {
        private readonly EndpointAddress _endpoint;
        private readonly ChannelFactory<IUserService> _channelFactory;

        public UserServiceProxy(IConfiguration configuration, int maxConnections = 100)
        {
            var ip = configuration["UserService:IP"];
            var port = configuration["UserService:Port"];

            var url = $"net.tcp://{ip}:{port}/UserService";

            var binding = new NetTcpBinding
            {
                Security = { Mode = SecurityMode.None },
                MaxReceivedMessageSize = int.MaxValue,
                MaxBufferSize = int.MaxValue,
                MaxBufferPoolSize = int.MaxValue,
                ReceiveTimeout = TimeSpan.FromMinutes(2),
                SendTimeout = TimeSpan.FromMinutes(2),
                MaxConnections = maxConnections
            };

            _endpoint = new EndpointAddress(url);
            _channelFactory = new ChannelFactory<IUserService>(binding);
            _channelFactory.Opened += ChannelFactory_Opened;
            _channelFactory.Faulted += ChannelFactory_Faulted;
        }

        public Tuple<int, List<UserModel>> GetAllUsers()
        {
            IUserService service = null;

            try
            {
                service = _channelFactory.CreateChannel(_endpoint);
                return service.GetAllUsers();
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
                return new Tuple<int, List<UserModel>>(-1001, new List<UserModel>());
            }
            finally
            {
                CloseOrAbortServiceChannel((ICommunicationObject)service);
            }
        }

        public int InsertUser(UserModel user)
        {
            IUserService service = null;

            try
            {
                service = _channelFactory.CreateChannel(_endpoint);
                return service.InsertUser(user);
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
                return -1;
            }
            finally
            {
                CloseOrAbortServiceChannel((ICommunicationObject)service);
            }
        }

        private void CloseOrAbortServiceChannel(ICommunicationObject commObj)
        {
            if (commObj == null || commObj.State == CommunicationState.Closed)
                return;

            try
            {
                if (commObj.State != CommunicationState.Faulted)
                    commObj.Close();
                else
                    commObj.Abort();
            }
            catch
            {
                commObj.Abort();
            }
        }

        private void ChannelFactory_Opened(object sender, EventArgs e)
        {
            Console.WriteLine("UserService ChannelFactory Opened.");
        }

        private void ChannelFactory_Faulted(object sender, EventArgs e)
        {
            Console.WriteLine("UserService ChannelFactory Faulted.");
        }

        public void Dispose()
        {
            try
            {
                _channelFactory?.Close();
            }
            catch
            {
                _channelFactory?.Abort();
            }
        }
    }
}
