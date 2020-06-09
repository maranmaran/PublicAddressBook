using Backend.Business.Email.EmailRequests.NotificationEmail;
using Backend.Domain.Entities.User;
using Backend.Library.Email.Interfaces;
using Backend.Library.Email.Models;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Backend.BusinessTests.Email
{
    public class NotificationEmailTests
    {
        [Fact]
        public async Task NotificationEmailHandler_HandleValid_Sends()
        {
            var emailMock = new Mock<IEmailService>();
            var handler = new NotificationEmailRequestHandler(emailMock.Object);

            var request = new NotificationEmailRequest(new Domain.Entities.Notification.Notification()
            {
                Receiver = new ApplicationUser()
                {
                    Email = "receiver@mail.com"
                },
                Sender = new ApplicationUser()
                {
                    Email = "sender@mail.com"
                }
            });

            await handler.Handle(request, CancellationToken.None);

            emailMock.Verify(x => x.SendEmailAsync(It.Is<EmailMessage>(x => x.To == request.Notification.Receiver.Email), CancellationToken.None));
            emailMock.Verify(x => x.SendEmailAsync(It.IsAny<EmailMessage>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task NotificationEmailHandler_Fails_Throws()
        {
            var emailMock = new Mock<IEmailService>();
            var handler = new NotificationEmailRequestHandler(emailMock.Object);

            var request = new NotificationEmailRequest(new Domain.Entities.Notification.Notification());

            await Assert.ThrowsAsync<Exception>(() => handler.Handle(request, CancellationToken.None));
        }
    }
}
