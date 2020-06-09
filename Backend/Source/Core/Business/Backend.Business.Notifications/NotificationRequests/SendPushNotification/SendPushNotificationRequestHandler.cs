using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Backend.Business.Notifications.NotificationRequests.CreatePushNotification;
using Backend.Business.Notifications.NotificationRequests.NotifyUser;
using Backend.Infrastructure.Exceptions;
using MediatR;

namespace Backend.Business.Notifications.NotificationRequests.SendPushNotification
{
    public class SendNotificationRequestHandler : IRequestHandler<SendNotificationRequest, Unit>
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public SendNotificationRequestHandler(IMapper mapper, IMediator mediator)
        {
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(SendNotificationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var newNotificationRequest = _mapper.Map<SendNotificationRequest, CreateNotificationRequest>(request);

                var notification = await _mediator.Send(newNotificationRequest, cancellationToken);

                await _mediator.Publish(new NotifyUserNotification(notification, notification.Receiver.Id), cancellationToken);

                return Unit.Value;
            }
            catch (Exception ex)
            {
                throw new CreateFailureException($"Could not save notification", ex);
            }
        }
    }
}