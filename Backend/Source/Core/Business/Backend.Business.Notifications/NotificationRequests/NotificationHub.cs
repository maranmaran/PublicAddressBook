using Backend.Business.Notifications.Interfaces;
using Backend.Business.Notifications.NotificationRequests.ReadNotification;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System;
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

        public async Task ReadNotification(Guid id)
        {
            await _mediator.Send(new ReadNotificationRequest() { Id = id });
        }
    }
}