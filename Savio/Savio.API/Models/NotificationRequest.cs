using Savio.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Notification.API.Models
{
    public class NotificationResponse
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public List<NotificationModel> Notification { get; set; }
    }

}