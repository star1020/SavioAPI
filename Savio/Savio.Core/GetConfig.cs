using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savio.Core
{
    public static class GetConfig
    {
        private static readonly Lazy<IConfiguration> _config = new Lazy<IConfiguration>(BuildConfiguration);

        public static IConfiguration Configuration => _config.Value;

        private static IConfiguration BuildConfiguration()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            return config;
        }

    }

}
