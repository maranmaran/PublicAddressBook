using Backend.Business.Notifications.NotificationRequests;
using Backend.Business.Notifications.NotificationRequests.CreatePushNotification;
using Backend.Business.Notifications.NotificationRequests.NotifyUser;
using Backend.Business.Notifications.NotificationRequests.ReadNotification;
using Backend.Domain.Enum;
using MediatR;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Backend.BusinessTests.Notification
{
    public class NotificationHubTests
    {
        [Fact]
        public async Task SendNotification_CreateAndSends()
        {
            var mediator = new Mock<IMediator>();
            mediator.Setup(x => x.Send(It.IsAny<CreateNotificationRequest>(), CancellationToken.None));

            var notificationHub = new NotificationHub(mediator.Object);
            await notificationHub.SendNotification(It.IsAny<NotificationType>(), "test", Guid.NewGuid(), Guid.NewGuid());

            mediator.Verify(x => x.Send(It.IsAny<CreateNotificationRequest>(), CancellationToken.None), Times.Once);
            mediator.Verify(x => x.Publish(It.IsAny<NotifyUserNotification>(), CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task ReadNotification_SendReadRequest()
        {
            var mediator = new Mock<IMediator>();
            mediator.Setup(x => x.Send(It.IsAny<ReadNotificationRequest>(), CancellationToken.None));

            var notificationHub = new NotificationHub(mediator.Object);
            await notificationHub.ReadNotification(Guid.NewGuid());

            mediator.Verify(x => x.Send(It.IsAny<ReadNotificationRequest>(), CancellationToken.None), Times.Once);
        }
    }
}
