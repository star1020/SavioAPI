using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NLog;
using Savio.Core.Data;
using Notification.Contract;
using Savio.Core;

namespace Notification
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = true)]
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _db;
        public string className = MethodBase.GetCurrentMethod().DeclaringType.Name;
        public NotificationService(INotificationRepository db)
        {
            _db = db;
        }

        public Tuple<int, List<NotificationModel>> GetAllNotificationsWithData(NotificationModel notification)
        {
            var method = MethodBase.GetCurrentMethod().Name;
            LogHelper.Logging(className, method, notification, true);
            var r = _db.GetAllNotificationsWithData(notification);
            LogHelper.Logging(className, method, r, false);

            if (r == null) return new Tuple<int, List<NotificationModel>>(ErrorCode.OperationError, new List<NotificationModel>());
            if (r.Count == 0) return new Tuple<int, List<NotificationModel>>(ErrorCode.OperationError, new List<NotificationModel>());

            return new Tuple<int, List<NotificationModel>>(ErrorCode.Success, r);
        }

        public int InsertNotification(NotificationModel notification)
        {
            var method = MethodBase.GetCurrentMethod().Name;
            LogHelper.Logging(className, method, notification, true);
            var r = _db.InsertNotification(notification);
            LogHelper.Logging(className, method, r, false);
            return r;
        }

        public int DeleteNotificationById(int id)
        {
            var method = MethodBase.GetCurrentMethod().Name;
            LogHelper.Logging(className, method, id, true);
            var r = _db.DeleteNotificationById(id);
            LogHelper.Logging(className, method, r, false);
            return r;
        }

    }
}
