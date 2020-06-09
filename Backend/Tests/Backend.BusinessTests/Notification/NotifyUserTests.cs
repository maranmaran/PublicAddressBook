using Backend.Business.Email.EmailRequests.NotificationEmail;
using Backend.Business.Notifications.Interfaces;
using Backend.Business.Notifications.NotificationRequests;
using Backend.Business.Notifications.NotificationRequests.NotifyUser;
using Backend.Domain.Entities.User;
using Backend.Domain.Enum;
using Backend.Library.Logging.Interfaces;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Backend.BusinessTests.Notification
{
    public class NotifyUserTests
    {
        [Fact]
        public async Task NotifyUserHandler_ValidRequest_Notifies()
        {
            var context = TestHelper.GetContext();
            await context.UserSettings.AddAsync(new UserSetting()
            {
                ApplicationUserId = Guid.Parse("1ee596f4-1585-4361-867d-acc6347ba241"),
                NotificationSettings = new List<NotificationSetting>()
                {
                    new NotificationSetting()
                    {
                        ReceiveMail = true,
                        ReceiveNotification = true,
                        NotificationType = NotificationType.MediaAdded
                    },
                }
            });

            await context.SaveChangesAsync(CancellationToken.None);

            var hubMock = new Mock<INotificationHub>();
            var hubContextMock = new Mock<IHubContext<NotificationHub, INotificationHub>>();
            var clientsMock = new Mock<IHubClients<INotificationHub>>();
            clientsMock.Setup(x => x.User(It.IsAny<string>())).Returns(() => hubMock.Object);
            hubContextMock.Setup(x => x.Clients).Returns(() => clientsMock.Object);

            var logMock = new Mock<ILoggingService>();
            var mediatorMock = new Mock<IMediator>();

            var handler = new NotifyUserNotificationHandler(hubContextMock.Object, mediatorMock.Object, logMock.Object, context);

            var userId = Guid.Parse("1ee596f4-1585-4361-867d-acc6347ba241");
            var notification = new Domain.Entities.Notification.Notification()
            {
                ReceiverId = Guid.Parse("1ee596f4-1585-4361-867d-acc6347ba242"),
                Type = NotificationType.MediaAdded,
                SenderId = userId
            };
            var request = new NotifyUserNotification(notification, userId);

            await handler.Handle(request, CancellationToken.None);

            logMock.Verify(x => x.LogError(It.IsAny<string>()), Times.Never);
            logMock.Verify(x => x.LogError(It.IsAny<Exception>(), It.IsAny<string>()), Times.Never);
            mediatorMock.Verify(x => x.Send(It.IsAny<NotificationEmailRequest>(), It.IsAny<CancellationToken>()), Times.Once);
            hubContextMock.Verify(x => x.Clients.User(notification.ReceiverId.Value.ToString()).SendNotification(notification), Times.Once);
        }

        [Fact]
        public async Task NotifyUserHandler_ValidRequest_NotifiesEmailOnly()
        {
            var context = TestHelper.GetContext();
            await context.UserSettings.AddAsync(new UserSetting()
            {
                ApplicationUserId = Guid.Parse("1ee596f4-1585-4361-867d-acc6347ba241"),
                NotificationSettings = new List<NotificationSetting>()
                {
                    new NotificationSetting()
                    {
                        ReceiveMail = true,
                        ReceiveNotification = false,
                        NotificationType = NotificationType.MediaAdded
                    },
                }
            });

            await context.SaveChangesAsync(CancellationToken.None);

            var hubMock = new Mock<INotificationHub>();
            var hubContextMock = new Mock<IHubContext<NotificationHub, INotificationHub>>();
            var clientsMock = new Mock<IHubClients<INotificationHub>>();
            clientsMock.Setup(x => x.User(It.IsAny<string>())).Returns(() => hubMock.Object);
            hubContextMock.Setup(x => x.Clients).Returns(() => clientsMock.Object);

            var logMock = new Mock<ILoggingService>();
            var mediatorMock = new Mock<IMediator>();

            var handler = new NotifyUserNotificationHandler(hubContextMock.Object, mediatorMock.Object, logMock.Object, context);

            var userId = Guid.Parse("1ee596f4-1585-4361-867d-acc6347ba241");
            var notification = new Domain.Entities.Notification.Notification()
            {
                ReceiverId = Guid.Parse("1ee596f4-1585-4361-867d-acc6347ba242"),
                Type = NotificationType.MediaAdded,
                SenderId = userId
            };
            var request = new NotifyUserNotification(notification, userId);

            await handler.Handle(request, CancellationToken.None);

            logMock.Verify(x => x.LogError(It.IsAny<Exception>(), It.IsAny<string>()), Times.Never);
            mediatorMock.Verify(x => x.Send(It.IsAny<NotificationEmailRequest>(), It.IsAny<CancellationToken>()), Times.Once);
            hubContextMock.Verify(x => x.Clients.User(notification.ReceiverId.Value.ToString()).SendNotification(notification), Times.Never);
        }

        [Fact]
        public async Task NotifyUserHandler_ValidRequest_NotifiesNotificationOnly()
        {
            var context = TestHelper.GetContext();
            await context.UserSettings.AddAsync(new UserSetting()
            {
                ApplicationUserId = Guid.Parse("1ee596f4-1585-4361-867d-acc6347ba241"),
                NotificationSettings = new List<NotificationSetting>()
                {
                    new NotificationSetting()
                    {
                        ReceiveMail = false,
                        ReceiveNotification = true,
                        NotificationType = NotificationType.MediaAdded
                    },
                }
            });

            await context.SaveChangesAsync(CancellationToken.None);

            var hubMock = new Mock<INotificationHub>();
            var hubContextMock = new Mock<IHubContext<NotificationHub, INotificationHub>>();
            var clientsMock = new Mock<IHubClients<INotificationHub>>();
            clientsMock.Setup(x => x.User(It.IsAny<string>())).Returns(() => hubMock.Object);
            hubContextMock.Setup(x => x.Clients).Returns(() => clientsMock.Object);

            var logMock = new Mock<ILoggingService>();
            var mediatorMock = new Mock<IMediator>();

            var handler = new NotifyUserNotificationHandler(hubContextMock.Object, mediatorMock.Object, logMock.Object, context);

            var userId = Guid.Parse("1ee596f4-1585-4361-867d-acc6347ba241");
            var notification = new Domain.Entities.Notification.Notification()
            {
                ReceiverId = Guid.Parse("1ee596f4-1585-4361-867d-acc6347ba242"),
                Type = NotificationType.MediaAdded,
                SenderId = userId
            };
            var request = new NotifyUserNotification(notification, userId);

            await handler.Handle(request, CancellationToken.None);

            logMock.Verify(x => x.LogError(It.IsAny<Exception>(), It.IsAny<string>()), Times.Never);
            mediatorMock.Verify(x => x.Send(It.IsAny<NotificationEmailRequest>(), It.IsAny<CancellationToken>()), Times.Never);
            hubContextMock.Verify(x => x.Clients.User(notification.ReceiverId.Value.ToString()).SendNotification(notification), Times.Once);
        }

        [Fact]
        public async Task NotifyUserHandler_ReceiverNotFound_DoesntNotifyViaNotification_Throws()
        {
            var context = TestHelper.GetContext();
            await context.UserSettings.AddAsync(new UserSetting()
            {
                ApplicationUserId = Guid.Parse("1ee596f4-1585-4361-867d-acc6347ba241"),
                NotificationSettings = new List<NotificationSetting>()
                {
                    new NotificationSetting()
                    {
                        ReceiveMail = false,
                        ReceiveNotification = true,
                        NotificationType = NotificationType.MediaAdded
                    },
                }
            });

            await context.SaveChangesAsync(CancellationToken.None);

            var hubMock = new Mock<INotificationHub>();
            var hubContextMock = new Mock<IHubContext<NotificationHub, INotificationHub>>();
            var clientsMock = new Mock<IHubClients<INotificationHub>>();
            clientsMock.Setup(x => x.User(It.IsAny<string>())).Throws(It.IsAny<Exception>());
            hubContextMock.Setup(x => x.Clients).Returns(() => clientsMock.Object);

            var logMock = new Mock<ILoggingService>();
            var mediatorMock = new Mock<IMediator>();

            var handler = new NotifyUserNotificationHandler(hubContextMock.Object, mediatorMock.Object, logMock.Object, context);

            var userId = Guid.Parse("1ee596f4-1585-4361-867d-acc6347ba241");
            var notification = new Domain.Entities.Notification.Notification()
            {
                ReceiverId = Guid.Parse("1ee596f4-1585-4361-867d-acc6347ba242"),
                Type = NotificationType.MediaAdded,
                SenderId = userId
            };
            var request = new NotifyUserNotification(notification, userId);

            await handler.Handle(request, CancellationToken.None);

            logMock.Verify(x => x.LogError(It.IsAny<Exception>(), It.IsAny<string>()), Times.Once);
            mediatorMock.Verify(x => x.Send(It.IsAny<NotificationEmailRequest>(), It.IsAny<CancellationToken>()), Times.Never);
            hubContextMock.Verify(x => x.Clients.User(notification.ReceiverId.Value.ToString()).SendNotification(notification), Times.Never);
        }

        [Fact]
        public async Task NotifyUserHandler_MailingError_DoesntNotifyViaMail_Throws()
        {
            var context = TestHelper.GetContext();
            await context.UserSettings.AddAsync(new UserSetting()
            {
                ApplicationUserId = Guid.Parse("1ee596f4-1585-4361-867d-acc6347ba241"),
                NotificationSettings = new List<NotificationSetting>()
                {
                    new NotificationSetting()
                    {
                        ReceiveMail = true,
                        ReceiveNotification = false,
                        NotificationType = NotificationType.MediaAdded
                    },
                }
            });

            await context.SaveChangesAsync(CancellationToken.None);

            var hubMock = new Mock<INotificationHub>();
            var hubContextMock = new Mock<IHubContext<NotificationHub, INotificationHub>>();
            var clientsMock = new Mock<IHubClients<INotificationHub>>();
            clientsMock.Setup(x => x.User(It.IsAny<string>())).Returns(() => hubMock.Object);
            hubContextMock.Setup(x => x.Clients).Returns(() => clientsMock.Object);

            var logMock = new Mock<ILoggingService>();
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(x => x.Send(It.IsAny<NotificationEmailRequest>(), It.IsAny<CancellationToken>()))
                .Throws(It.IsAny<Exception>());

            var handler = new NotifyUserNotificationHandler(hubContextMock.Object, mediatorMock.Object, logMock.Object, context);

            var userId = Guid.Parse("1ee596f4-1585-4361-867d-acc6347ba241");
            var notification = new Domain.Entities.Notification.Notification()
            {
                ReceiverId = Guid.Parse("1ee596f4-1585-4361-867d-acc6347ba242"),
                Type = NotificationType.MediaAdded,
                SenderId = userId
            };
            var request = new NotifyUserNotification(notification, userId);

            await handler.Handle(request, CancellationToken.None);

            logMock.Verify(x => x.LogError(It.IsAny<Exception>(), It.IsAny<string>()), Times.Once);
            mediatorMock.Verify(x => x.Send(It.IsAny<NotificationEmailRequest>(), It.IsAny<CancellationToken>()), Times.Once);
            hubContextMock.Verify(x => x.Clients.User(notification.ReceiverId.Value.ToString()).SendNotification(notification), Times.Never);

        }
    }
}
