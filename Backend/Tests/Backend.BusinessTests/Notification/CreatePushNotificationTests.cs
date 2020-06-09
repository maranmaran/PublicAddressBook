using AutoMapper;
using Backend.Business.Notifications.NotificationRequests.CreatePushNotification;
using Backend.Domain;
using Backend.Domain.Entities.User;
using Backend.Domain.Enum;
using Backend.Infrastructure.Exceptions;
using Backend.Infrastructure.Extensions;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Backend.BusinessTests.Notification
{
    public class CreatePushNotificationTests
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreatePushNotificationTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreateNotificationRequest, Domain.Entities.Notification.Notification>();
                cfg.CreateMap<Domain.Entities.Notification.Notification, Domain.Entities.Notification.Notification>().IgnoreAllVirtual();
            });
            _mapper = config.CreateMapper();

            _context = TestHelper.GetContext();
            _context.Users.Add(new ApplicationUser()
            {
                Id = Guid.Parse("dee237d1-cee3-4c77-a5aa-4db95445a5fe"),
                Avatar = "test"
            });
            _context.Users.Add(new ApplicationUser()
            {
                Id = Guid.Parse("4474c02a-f461-44d2-9c74-f95a28d90749"),
                Avatar = "test2"
            });
            _context.SaveChangesAsync(CancellationToken.None).Wait();
        }

        [Fact]
        public async Task CreatePushNotificationHandler_ValidModel_Creates()
        {
            var request = new CreateNotificationRequest(It.IsAny<NotificationType>(), "payload", Guid.Parse("dee237d1-cee3-4c77-a5aa-4db95445a5fe"), Guid.Parse("4474c02a-f461-44d2-9c74-f95a28d90749"));
            var handler = new CreateNotificationRequestHandler(_context, _mapper);

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(Guid.Parse("dee237d1-cee3-4c77-a5aa-4db95445a5fe"), result.SenderId);
            Assert.Equal(Guid.Parse("4474c02a-f461-44d2-9c74-f95a28d90749"), result.ReceiverId);
        }

        [Fact]
        public async Task CreatePushNotificationHandler_SenderNotFound_Throws()
        {
            var request = new CreateNotificationRequest(It.IsAny<NotificationType>(), "payload", Guid.Parse("dfe237d1-cee3-4c77-a5aa-4db95445a5fe"), Guid.Parse("4474c02a-f461-44d2-9c74-f95a28d90749"));
            var handler = new CreateNotificationRequestHandler(_context, _mapper);

            await Assert.ThrowsAsync<CreateFailureException>(() => handler.Handle(request, CancellationToken.None));
        }
    }
}
