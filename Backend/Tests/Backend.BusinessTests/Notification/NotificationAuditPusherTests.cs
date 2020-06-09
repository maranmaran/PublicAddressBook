using Backend.Business.Notifications;
using Backend.Business.Notifications.NotificationRequests.CreatePushNotification;
using Backend.Business.Notifications.NotificationRequests.NotifyUser;
using Backend.Domain.Entities.Auditing;
using Backend.Domain.Entities.Media;
using Backend.Domain.Entities.User;
using Backend.Infrastructure.Exceptions;
using Backend.Infrastructure.Interfaces;
using Backend.Library.Logging.Interfaces;
using MediatR;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Backend.BusinessTests.Notification
{
    public class NotificationAuditPusherTests
    {
        [Fact]
        public async Task PushToUser_Valid_Pushes()
        {
            var mediatorMock = new Mock<IMediator>();
            var logMock = new Mock<ILoggingService>();
            var activityMock = new Mock<IActivityService>();
            activityMock.Setup(x => x.GetEntityAsJson(It.IsAny<AuditRecord>())).ReturnsAsync(() => "entity");

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(typeof(IMediator))).Returns(mediatorMock.Object);
            serviceProviderMock.Setup(x => x.GetService(typeof(ILoggingService))).Returns(logMock.Object);
            serviceProviderMock.Setup(x => x.GetService(typeof(IActivityService))).Returns(activityMock.Object);

            var service = new NotificationsAuditPusher(serviceProviderMock.Object);

            var audit = new AuditRecord()
            {
                UserId = Guid.Parse("23222299-096d-4fd4-8e19-ebc67ff03b44"),
                EntityType = nameof(MediaFile)
            };

            var user = new ApplicationUser()
            {
                Id = Guid.Parse("23222299-096d-4fd4-8e19-ebc67ff03b44"),
            };

            await service.PushToUser(audit, user);

            var expectedCreateNotificationRequest = new CreateNotificationRequest()
            {
                SenderId = Guid.Parse("23222299-096d-4fd4-8e19-ebc67ff03b44"),
                ReceiverId = Guid.Parse("23222299-096d-4fd4-8e19-ebc67ff03b44"),
                JsonEntity = "entity",
                Type = NotificationHelper.GetNotificationType(audit.EntityType)
            };

            mediatorMock.Verify(x => x.Send(It.Is<CreateNotificationRequest>(val => RequestMatches(val, expectedCreateNotificationRequest)), CancellationToken.None), Times.Once);
            mediatorMock.Verify(x => x.Publish(It.IsAny<NotifyUserNotification>(), CancellationToken.None), Times.Once);
            activityMock.Verify(x => x.GetEntityAsJson(audit), Times.Once);
        }

        [Fact]
        public async Task PushToUser_Fails_LogWarning()
        {
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(x => x.Publish(It.IsAny<NotifyUserNotification>(), CancellationToken.None))
                .ThrowsAsync(new Exception());

            var logMock = new Mock<ILoggingService>();
            var activityMock = new Mock<IActivityService>();
            activityMock.Setup(x => x.GetEntityAsJson(It.IsAny<AuditRecord>())).ReturnsAsync(() => "entity");

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(typeof(IMediator))).Returns(mediatorMock.Object);
            serviceProviderMock.Setup(x => x.GetService(typeof(ILoggingService))).Returns(logMock.Object);
            serviceProviderMock.Setup(x => x.GetService(typeof(IActivityService))).Returns(activityMock.Object);

            var service = new NotificationsAuditPusher(serviceProviderMock.Object);

            var audit = new AuditRecord()
            {
                UserId = Guid.Parse("23222299-096d-4fd4-8e19-ebc67ff03b44"),
                EntityType = nameof(MediaFile)
            };

            var user = new ApplicationUser()
            {
                Id = Guid.Parse("23222299-096d-4fd4-8e19-ebc67ff03b44"),
            };

            await service.PushToUser(audit, user);

            logMock.Verify(x => x.LogWarning(It.IsAny<Exception>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task GetNotification_Fails_ThrowsCreateFailure()
        {
            var mediatorMock = new Mock<IMediator>();
            var logMock = new Mock<ILoggingService>();
            var activityMock = new Mock<IActivityService>();

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(typeof(IMediator))).Returns(mediatorMock.Object);
            serviceProviderMock.Setup(x => x.GetService(typeof(ILoggingService))).Returns(logMock.Object);
            serviceProviderMock.Setup(x => x.GetService(typeof(IActivityService))).Returns(activityMock.Object);

            var service = new NotificationsAuditPusher(serviceProviderMock.Object);
            await Assert.ThrowsAsync<CreateFailureException>(() => service.GetNotification(It.IsAny<AuditRecord>(), It.IsAny<ApplicationUser>()));
        }

        private bool RequestMatches(CreateNotificationRequest expected, CreateNotificationRequest actual)
        {
            var result = true;

            if (expected.SystemNotification != actual.SystemNotification)
                return false;
            if (expected.ReceiverId != actual.ReceiverId)
                return false;
            if (expected.SenderId != actual.SenderId)
                return false;
            if (expected.JsonEntity != actual.JsonEntity)
                return false;
            if (expected.Type != actual.Type)
                return false;

            return result;
        }
    }
}
