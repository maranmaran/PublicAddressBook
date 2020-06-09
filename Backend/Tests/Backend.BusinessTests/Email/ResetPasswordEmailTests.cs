using Backend.Business.Email.EmailRequests.ResetPassword;
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
    public class ResetPasswordEmailTests
    {
        [Fact]
        public async Task ResetPasswordEmailHandler_HandleValid_Sends()
        {
            var emailMock = new Mock<IEmailService>();
            var emailSettings = new Library.Email.EmailSettings()
            {
                Username = "username"
            };
            var handler = new ResetPasswordEmailRequestHandler(emailMock.Object, emailSettings);

            var request = new ResetPasswordEmailRequest(new ApplicationUser()
            {
                Email = "test@mail.com",
            });

            await handler.Handle(request, CancellationToken.None);

            emailMock.Verify(x => x.SendEmailAsync(It.Is<EmailMessage>(x => x.To == request.User.Email), CancellationToken.None));
            emailMock.Verify(x => x.SendEmailAsync(It.IsAny<EmailMessage>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ResetPasswordEmailHandler_Fails_Throws()
        {
            var emailMock = new Mock<IEmailService>();
            var emailSettings = new Library.Email.EmailSettings();
            var handler = new ResetPasswordEmailRequestHandler(emailMock.Object, emailSettings);

            var request = new ResetPasswordEmailRequest(null);

            await Assert.ThrowsAsync<Exception>(() => handler.Handle(request, CancellationToken.None));
        }
    }
}
