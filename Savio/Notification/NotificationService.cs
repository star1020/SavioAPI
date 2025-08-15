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

namespace Notification
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = true)]
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _db;

        public NotificationService(INotificationRepository db)
        {
            _db = db;
        }

        public Tuple<int, List<NotificationModel>> GetAllNotificationsWithData(NotificationModel notification)
        {
            var method = MethodBase.GetCurrentMethod().Name;
            LogManager.GetCurrentClassLogger().Info($"[{method}] RequestInfo -> {JsonConvert.SerializeObject(notification)}");
            var r = _db.GetAllNotificationsWithData(notification);
            LogManager.GetCurrentClassLogger().Info($"[{method}] NotificationInfo -> {JsonConvert.SerializeObject(r)}");

            if (r == null) return new Tuple<int, List<NotificationModel>>(ErrorCode.OperationError, new List<NotificationModel>());
            if (r.Count == 0) return new Tuple<int, List<NotificationModel>>(ErrorCode.OperationError, new List<NotificationModel>());

            return new Tuple<int, List<NotificationModel>>(ErrorCode.Success, r);
        }

        public int InsertNotification(NotificationModel notification)
        {
            var method = MethodBase.GetCurrentMethod().Name;
            LogManager.GetCurrentClassLogger().Info($"[{method}] RequestInfo -> {JsonConvert.SerializeObject(notification)}");
            var r = _db.InsertNotification(notification);
            LogManager.GetCurrentClassLogger().Info($"[{method}] Result -> {JsonConvert.SerializeObject(r)}");
            return r;
        }

        public int DeleteNotificationById(int id)
        {
            var method = MethodBase.GetCurrentMethod().Name;
            LogManager.GetCurrentClassLogger().Info($"[{method}] RequestInfo -> {id}");
            var r = _db.DeleteNotificationById(id);
            LogManager.GetCurrentClassLogger().Info($"[{method}] NotificationInfo -> {JsonConvert.SerializeObject(r)}");
            return r;
        }

    }
}
