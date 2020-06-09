using Backend.Business.Notifications.NotificationRequests.CreatePushNotification;
using Backend.Business.Notifications.NotificationRequests.NotifyAllClients;
using Backend.Business.Notifications.NotificationRequests.NotifyUser;
using Backend.Domain.Entities.Auditing;
using Backend.Domain.Entities.Notification;
using Backend.Domain.Entities.User;
using Backend.Infrastructure.Exceptions;
using Backend.Infrastructure.Interfaces;
using Backend.Library.Logging.Interfaces;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Backend.Business.Notifications
{
    public class NotificationsAuditPusher : IAuditPusher
    {
        private readonly IMediator _mediator;
        private readonly ILoggingService _logger;
        private readonly IActivityService _activityService;

        public NotificationsAuditPusher(IServiceProvider provider)
        {
            _mediator = provider.GetService<IMediator>();
            _logger = provider.GetService<ILoggingService>();
            _activityService = provider.GetService<IActivityService>();
        }

        public async Task PushToUser(AuditRecord audit, ApplicationUser user)
        {
            try
            {
                var receiverId = user.Id;
                var notification = await GetNotification(audit, user);

                await _mediator.Publish(new NotifyUserNotification(notification, receiverId));
            }
            catch (Exception e)
            {
                await _logger.LogWarning(e,
                    $"Could not notify user about audit record. UserId: {audit.UserId} Entity: {audit.EntityType} AuditId: {audit.Id}");
            }
        }

        public async Task PushToAllClients(AuditRecord audit)
        {
            var notification = await GetNotification(audit);

            await _mediator.Publish(new NotifyAllClientsNotification(notification));
        }

        internal async Task<Notification> GetNotification(AuditRecord audit, ApplicationUser user)
        {
            try
            {
                var request = new CreateNotificationRequest()
                {
                    Type = NotificationHelper.GetNotificationType(audit.EntityType),
                    JsonEntity = await _activityService.GetEntityAsJson(audit),
                };

                return await _mediator.Send(request);
            }
            catch (Exception e)
            {
                throw new CreateFailureException(nameof(Notification), e);
            }
        }

        internal async Task<Notification> GetNotification(AuditRecord audit)
        {
            try
            {
                var request = new CreateNotificationRequest()
                {
                    Type = NotificationHelper.GetNotificationType(audit.EntityType),
                    JsonEntity = await _activityService.GetEntityAsJson(audit),
                };

                return await _mediator.Send(request);
            }
            catch (Exception e)
            {
                throw new CreateFailureException(nameof(Notification), e);
            }
        }
    }
}
