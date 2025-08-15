using Abp.Application.Services;
using Savio.API;
using Savio.Core.Data;
using Notification.API;
using Notification.Contract;

namespace Notification.API.App_Service
{
    public interface INotificationAppService : IApplicationService
    {
        Tuple<int, List<NotificationModel>> GetAllNotificationsWithData(NotificationModel request);
        int AddEditNotification(NotificationModel request);
        int DeleteNotificationById(int id);
    }
    public class NotificationAppService : INotificationAppService
    {
        private readonly INotificationService _notificationService;

        public NotificationAppService()
        {
            _notificationService = GlobalContext.NotificationService;
        }

        public Tuple<int, List<NotificationModel>> GetAllNotificationsWithData(NotificationModel request)
        {
            var r = _notificationService.GetAllNotificationsWithData(request);
            return new Tuple<int, List<NotificationModel>>(r.Item1, r.Item2);
        }

        public int AddEditNotification(NotificationModel request)
        {
            return _notificationService.InsertNotification(request);
        }

        public int DeleteNotificationById(int id)
        {
            return _notificationService.DeleteNotificationById(id);
        }

    }
}
