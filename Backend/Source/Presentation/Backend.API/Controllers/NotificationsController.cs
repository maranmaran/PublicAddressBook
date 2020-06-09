using Backend.Business.Notifications.NotificationRequests.GetPushNotificationHistory;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Backend.API.Controllers
{
    /// <summary>
    /// Notifications API
    /// </summary>
    public class NotificationsController : BaseController
    {
        /// <summary>
        /// Gets paged notification history
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{userId}/{page}/{pageSize}")]
        public async Task<IActionResult> GetNotificationHistory(Guid userId, int page, int pageSize, CancellationToken cancellationToken = default)
        {
            return Ok(await Mediator.Send(new GetNotificationHistoryRequest(userId, page, pageSize), cancellationToken));
        }
    }
}
