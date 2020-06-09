using System;
using System.Collections.Generic;
using Backend.Domain.Entities.Notification;
using MediatR;

namespace Backend.Business.Notifications.NotificationRequests.GetPushNotificationHistory
{
    public class GetNotificationHistoryRequest : IRequest<IEnumerable<Notification>>
    {
        public Guid UserId { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }

        public GetNotificationHistoryRequest(Guid userId, int page, int pageSize)
        {
            UserId = userId;
            Page = page;
            PageSize = pageSize;
        }
    }
}