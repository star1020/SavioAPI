using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NLog;
using Savio.Core;
using Savio.Core.Data;
using Notification.API.App_Service;
using Notification.API.Models;

namespace Notification.API.Controllers
{
    [ApiController]
    [Route("notification")]
    public class NotificationController : ControllerBase
    {
        readonly INotificationAppService _notificationAppService;
        public readonly string _controllerName;
        public NotificationController()
        {
            _notificationAppService = new NotificationAppService();
            _controllerName = GetType().Name;
        }

        [HttpGet]
        [HttpPost]
        [Route("GetAllNotificationsWithData")]
        public IActionResult GetAllNotificationsWithData(NotificationModel request)
        {
            var w = Stopwatch.StartNew();
            string method = ControllerContext.ActionDescriptor.ActionName;

            LogHelper.Logging(_controllerName, method, request, true);

            var r = _notificationAppService.GetAllNotificationsWithData(request);

            var response = new NotificationResponse
            {
                Code = r.Item1,
                Message = r.Item1.ToErrorMsg(),
                Notification = r.Item2
            };

            LogHelper.Logging(_controllerName, method, response, false);
            return Ok(response);
        }

        [HttpPost]
        [Route("AddEditNotification")]
        public IActionResult AddEditNotification(NotificationModel request)
        {
            var w = Stopwatch.StartNew();
            string method = ControllerContext.ActionDescriptor.ActionName;

            LogHelper.Logging(_controllerName, method, request, true);

            var r = _notificationAppService.AddEditNotification(request);

            var response = new NotificationResponse
            {
                Code = r,
                Message = r.ToErrorMsg(),
            };

            LogHelper.Logging(_controllerName, method, response, false);
            return Ok(response);
        }

        [HttpPost]
        [Route("DeleteNotificationById/{id:int}")]
        public IActionResult DeleteNotificationById(int id)
        {
            var w = Stopwatch.StartNew();
            string method = ControllerContext.ActionDescriptor.ActionName;

            LogHelper.Logging(_controllerName, method, id, true);

            var r = _notificationAppService.DeleteNotificationById(id);

            var response = new NotificationResponse
            {
                Code = r,
                Message = r.ToErrorMsg(),
            };

            LogHelper.Logging(_controllerName, method, response, false);
            return Ok(response);
        }
    }
}
