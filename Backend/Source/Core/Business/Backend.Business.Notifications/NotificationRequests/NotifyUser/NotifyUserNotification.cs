using System;
using Backend.Domain.Entities.Notification;
using MediatR;

namespace Backend.Business.Notifications.NotificationRequests.NotifyUser
{
    public class NotifyUserNotification : INotification
    {
        public Guid UserId { get; set; }
        public Notification Notification { get; set; }

        public NotifyUserNotification(Notification notification, Guid userId)
        {
            Notification = notification;
            UserId = userId;
        }
    }
}
