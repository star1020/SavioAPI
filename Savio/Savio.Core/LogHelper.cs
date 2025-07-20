using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Savio.Core
{
    public static class LogHelper
    {
        public static string GetMethodName([CallerMemberName] string methodName = "")
        {
            return methodName;
        }
    }
}
