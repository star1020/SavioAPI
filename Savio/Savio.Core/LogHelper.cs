using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NLog;

namespace Savio.Core
{
    public static class LogHelper
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static string GetMethodName([CallerMemberName] string methodName = "")
        {
            return methodName;
        }


        public static void Logging(string controller, string method, object data, bool isRequest)
        {
            string serialized = JsonConvert.SerializeObject(data);
            string message = $"[{controller}] {method} {(isRequest ? "Request" : "Response")} -> {serialized}";

            Logger.Info(message);
            Console.WriteLine(message);
        }
        public static void LoggingException(Exception msg)
        {
            Logger.Error(msg);
            Console.WriteLine("Error:"+msg);
        }
    }
}
