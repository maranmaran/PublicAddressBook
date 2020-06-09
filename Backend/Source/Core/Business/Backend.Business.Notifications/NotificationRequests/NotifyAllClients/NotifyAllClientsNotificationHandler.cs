using Backend.Business.Notifications.Interfaces;
using Backend.Domain.Entities.Notification;
using Backend.Library.Logging.Interfaces;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("Backend.BusinessTests")]
namespace Backend.Business.Notifications.NotificationRequests.NotifyAllClients
{
    public class NotifyAllClientsNotificationHandler : INotificationHandler<NotifyAllClientsNotification>
    {
        private readonly IHubContext<NotificationHub, INotificationHub> _hubContext;
        private readonly ILoggingService _loggingService;

        public NotifyAllClientsNotificationHandler(IHubContext<NotificationHub, INotificationHub> hubContext, ILoggingService loggingService)
        {
            _hubContext = hubContext;
            _loggingService = loggingService;
        }

        public async Task Handle(NotifyAllClientsNotification request, CancellationToken cancellationToken)
        {
            await SendNotification(request.Notification);
        }


        internal async Task SendNotification(Notification notification)
        {
            try
            {
                // send notification
                await _hubContext.Clients.All.SendNotification(notification);
            }
            catch (Exception e)
            {
                await _loggingService.LogError(e, $"Could not send notification to receiver: {notification.ReceiverId}");
            }


        }
    }
}