using Backend.Business.Email.EmailRequests.NotificationEmail;
using Backend.Business.Notifications.Interfaces;
using Backend.Domain;
using Backend.Domain.Entities.Notification;
using Backend.Domain.Entities.User;
using Backend.Library.Logging.Interfaces;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("Backend.BusinessTests")]
namespace Backend.Business.Notifications.NotificationRequests.NotifyUser
{
    public class NotifyUserNotificationHandler : INotificationHandler<NotifyUserNotification>
    {
        private readonly IHubContext<NotificationHub, INotificationHub> _hubContext;
        private readonly ILoggingService _loggingService;
        private readonly IApplicationDbContext _context;
        private readonly IMediator _mediator;

        public NotifyUserNotificationHandler(IHubContext<NotificationHub, INotificationHub> hubContext, IMediator mediatorService, ILoggingService loggingService, IApplicationDbContext context)
        {
            _hubContext = hubContext;
            _loggingService = loggingService;
            _context = context;
            _mediator = mediatorService;
        }

        public async Task Handle(NotifyUserNotification request, CancellationToken cancellationToken)
        {
            var notificationSetting = (await _context.UserSettings.Include(x => x.NotificationSettings).FirstOrDefaultAsync(x => x.ApplicationUserId == request.UserId, cancellationToken)).NotificationSettings.First(x => x.NotificationType == request.Notification.Type);

            await SendMail(request.Notification, cancellationToken, notificationSetting);

            await SendNotification(request.Notification, notificationSetting);
        }


        internal async Task SendNotification(Notification notification, NotificationSetting notificationSetting)
        {
            try
            {
                // send notification
                if (notificationSetting.ReceiveNotification && notification.ReceiverId.HasValue)
                {
                    await _hubContext.Clients.User(notification.ReceiverId.Value.ToString()).SendNotification(notification);
                }
            }
            catch (Exception e)
            {
                await _loggingService.LogError(e, $"Could not send notification to receiver: {notification.ReceiverId}");
            }
        }

        internal async Task SendMail(Notification notification, CancellationToken cancellationToken,
            NotificationSetting notificationSetting)
        {
            try
            {
                // send mail
                if (notificationSetting.ReceiveMail)
                {
                    await _mediator.Send(new NotificationEmailRequest(notification), cancellationToken);
                }
            }
            catch (Exception e)
            {
                await _loggingService.LogError(e, $"Could not send mail to receiver: {notification.ReceiverId}");
            }
        }

    }
}