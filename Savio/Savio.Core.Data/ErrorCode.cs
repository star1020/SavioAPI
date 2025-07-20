using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Savio.Core.Data
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class ErrorMessageAttribute : Attribute
    {
        public string Message { get; }

        public ErrorMessageAttribute(string message)
        {
            Message = message;
        }
    }

    public static class ErrorCode
    {
        [ErrorMessage("Operation Error.")]
        public const int OperationError = -1;

        [ErrorMessage("Success.")]
        public const int Success = 0;

        public static string ToErrorMsg(this int code)
        {
            // Get all fields in ErrorCode class
            var fields = typeof(ErrorCode)
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

            foreach (var field in fields)
            {
                if (field.IsLiteral && !field.IsInitOnly && (int)field.GetRawConstantValue() == code)
                {
                    var attr = field.GetCustomAttribute<ErrorMessageAttribute>();
                    return attr?.Message ?? "Unknown error.";
                }
            }

            return "Unknown error.";
        }
    }
    
}
