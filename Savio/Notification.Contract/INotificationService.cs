using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Savio.Core.Data;

namespace Notification.Contract
{
    [ServiceContract]
    public interface INotificationService
    {
        [OperationContract]
        Tuple<int, List<NotificationModel>> GetAllNotificationsWithData(NotificationModel notification);
        [OperationContract]
        int InsertNotification(NotificationModel user);
        [OperationContract]
        int DeleteNotificationById(int id);
    }
}
