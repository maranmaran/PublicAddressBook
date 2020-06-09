using Backend.Business.Notifications.Interfaces;
using Backend.Business.Notifications.NotificationRequests.CreatePushNotification;
using Backend.Business.Notifications.NotificationRequests.NotifyUser;
using Backend.Business.Notifications.NotificationRequests.ReadNotification;
using Backend.Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Backend.Business.Notifications.NotificationRequests
{
    //[Authorize]
    public class NotificationHub : Hub<INotificationHub>
    {
        private readonly IMediator _mediator;

        public NotificationHub(IMediator mediator)
        {
            _mediator = mediator;
        }

        // TODO:
        // CancellationToken support closed on march 31 https://github.com/aspnet/AspNetCore/issues/8813
        // Check when new version releases
        public async Task SendNotification(NotificationType type, string payload, Guid senderId, Guid receiverId)
        {
            // save to db

            var notification = await _mediator.Send(new CreateNotificationRequest(type, payload, senderId, receiverId), CancellationToken.None);

            await _mediator.Publish(new NotifyUserNotification(notification, receiverId));
        }

        public async Task ReadNotification(Guid id)
        {
            await _mediator.Send(new ReadNotificationRequest() { Id = id });
        }
    }
}