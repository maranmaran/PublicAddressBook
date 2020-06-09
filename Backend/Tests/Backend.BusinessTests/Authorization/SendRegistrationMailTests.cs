using Backend.Business.Authorization.AuthorizationRequests.SendRegistrationEmail;
using Backend.Business.Email.EmailRequests.RegistrationEmail;
using Backend.Domain.Entities.User;
using Backend.Domain.Enum;
using Backend.Library.Logging.Interfaces;
using MediatR;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Backend.BusinessTests.Authorization
{
    public class SendRegistrationMailTests
    {
        [Fact]
        public async Task SendRegistrationEmailHandler_InvalidAccountType_Throws()
        {
            var loggingMock = new Mock<ILoggingService>();
            var user = new ApplicationUser()
            {
                Username = "user",
                AccountType = AccountType.Admin
            };

            var mediatorMock = new Mock<IMediator>();

            var handler = new SendRegistrationEmailRequestHandler(loggingMock.Object, mediatorMock.Object);
            await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(new SendRegistrationEmailRequest(user), CancellationToken.None));
        }

        [Fact]
        public async Task SendRegistrationEmailHandler_ValidAccountType_GetsMessage()
        {
            var loggingMock = new Mock<ILoggingService>();
            var user = new ApplicationUser()
            {
                Username = "user",
                AccountType = AccountType.User
            };

            var mediatorMock = new Mock<IMediator>();

            var handler = new SendRegistrationEmailRequestHandler(loggingMock.Object, mediatorMock.Object);
            await handler.Handle(new SendRegistrationEmailRequest(user), CancellationToken.None);
        }

        [Fact]
        public async Task SendRegistrationEmailHandler_MessageValid_Sends()
        {
            var loggingMock = new Mock<ILoggingService>();
            var user = new ApplicationUser()
            {
                Username = "user",
                AccountType = AccountType.User
            };

            var mediatorMock = new Mock<IMediator>();

            var handler = new SendRegistrationEmailRequestHandler(loggingMock.Object, mediatorMock.Object);
            await handler.Handle(new SendRegistrationEmailRequest(user), CancellationToken.None);

            mediatorMock.Verify(x => x.Send(It.IsAny<RegistrationEmailRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
