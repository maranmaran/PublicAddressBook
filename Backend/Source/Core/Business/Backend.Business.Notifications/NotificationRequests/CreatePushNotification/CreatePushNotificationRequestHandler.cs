using AutoMapper;
using Backend.Domain;
using Backend.Domain.Entities.Notification;
using Backend.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Backend.Business.Notifications.NotificationRequests.CreatePushNotification
{
    public class CreateNotificationRequestHandler : IRequestHandler<CreateNotificationRequest, Notification>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateNotificationRequestHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Notification> Handle(CreateNotificationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var newNotification = _mapper.Map<CreateNotificationRequest, Notification>(request);

                // get sender avatar if we have one                
                var avatar = string.Empty;
                if (request.SenderId.HasValue)
                    avatar = (await _context.Users.FirstOrDefaultAsync(x => x.Id == request.SenderId.Value, cancellationToken)).Avatar;
                newNotification.SenderAvatar = avatar;

                await _context.Notifications.AddAsync(newNotification);
                await _context.SaveChangesAsync(cancellationToken);

                if (request.SenderId.HasValue)
                    await _context.Entry(newNotification).Reference(x => x.Sender).LoadAsync(cancellationToken);
                if (request.ReceiverId.HasValue)
                    await _context.Entry(newNotification).Reference(x => x.Receiver).LoadAsync(cancellationToken);

                return newNotification;
            }
            catch (Exception e)
            {
                throw new CreateFailureException(nameof(Notification), e);
            }
        }
    }
}