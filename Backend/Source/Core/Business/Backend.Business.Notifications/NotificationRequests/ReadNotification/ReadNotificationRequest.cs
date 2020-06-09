using System;
using MediatR;

namespace Backend.Business.Notifications.NotificationRequests.ReadNotification
{
    public class ReadNotificationRequest : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
}