using AutoMapper;
using Backend.Business.Notifications.NotificationRequests.CreatePushNotification;
using Backend.Business.Notifications.NotificationRequests.NotifyUser;
using Backend.Business.Notifications.NotificationRequests.SendPushNotification;
using Backend.Domain.Entities.User;
using Backend.Domain.Enum;
using Backend.Infrastructure.Exceptions;
using MediatR;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Backend.BusinessTests.Notification
{
    public class SendPushNotificationTests
    {
        private readonly IMapper _mapper;

        public SendPushNotificationTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SendNotificationRequest, CreateNotificationRequest>().ReverseMap();
            });
            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task SendPushNotificationHandler_ValidRequest_Sends()
        {
            var request = new SendNotificationRequest()
            {
                ReceiverId = Guid.Parse("4474c02a-f461-44d2-9c74-f95a28d90749"),
                SenderId = Guid.Parse("4474c02a-f461-44d2-9c74-f95a28d90749"),
                Payload = "payload",
                Type = NotificationType.MediaAdded,
            };

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(x => x.Send(It.IsAny<CreateNotificationRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Domain.Entities.Notification.Notification()
                {
                    Receiver = new ApplicationUser()
                    {
                        Id = Guid.Parse("4474c02a-f461-44d2-9c74-f95a28d90749")
                    }
                });

            var handler = new SendNotificationRequestHandler(_mapper, mediatorMock.Object);
            await handler.Handle(request, CancellationToken.None);

            mediatorMock.Verify(x => x.Send(It.IsAny<CreateNotificationRequest>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(x => x.Publish(It.IsAny<NotifyUserNotification>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task SendPushNotificationHandler_CreateNotificationFails_Throws()
        {
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(x => x.Send(It.IsAny<CreateNotificationRequest>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());

            var request = new SendNotificationRequest()
            {
                ReceiverId = Guid.Parse("4474c02a-f461-44d2-9c74-f95a28d90749"),
                SenderId = Guid.Parse("4474c02a-f461-44d2-9c74-f95a28d90749"),
                Payload = "payload",
                Type = NotificationType.MediaAdded
            };
            var handler = new SendNotificationRequestHandler(_mapper, mediatorMock.Object);
            await Assert.ThrowsAsync<CreateFailureException>(() => handler.Handle(request, CancellationToken.None));

            mediatorMock.Verify(x => x.Send(It.IsAny<CreateNotificationRequest>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(x => x.Publish(It.IsAny<NotifyUserNotification>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
