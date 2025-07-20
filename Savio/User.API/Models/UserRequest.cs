using Savio.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace User.API.Models
{
    public class UserResponse
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public List<UserModel> User { get; set; }
    }

}