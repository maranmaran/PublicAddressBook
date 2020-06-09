using Backend.Business.Notifications.NotificationRequests.GetPushNotificationHistory;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Backend.API.Controllers
{
    public class NotificationsController : BaseController
    {
        [HttpGet("{userId}/{page}/{pageSize}")]
        public async Task<IActionResult> GetNotificationHistory(Guid userId, int page, int pageSize, CancellationToken cancellationToken = default)
        {
            return Ok(await Mediator.Send(new GetNotificationHistoryRequest(userId, page, pageSize), cancellationToken));
        }
    }
}
