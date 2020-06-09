using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;
using Backend.Business.Notifications.NotificationRequests.GetPushNotificationHistory;
using Backend.Business.Notifications.NotificationRequests.SendPushNotification;

namespace Backend.API.Controllers
{
    public class NotificationsController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> SendNotification([FromBody] SendNotificationRequest request, CancellationToken cancellationToken = default)
        {
            return Ok(await Mediator.Send(request, cancellationToken));
        }

        [HttpGet("{userId}/{page}/{pageSize}")]
        public async Task<IActionResult> GetNotificationHistory(Guid userId, int page, int pageSize, CancellationToken cancellationToken = default)
        {
            return Ok(await Mediator.Send(new GetNotificationHistoryRequest(userId, page, pageSize), cancellationToken));
        }
    }
}
