using Backend.Business.Email.EmailRequests.RegistrationEmail;
using Backend.Domain.Entities.User;
using Backend.Infrastructure;
using Backend.Library.Email.Interfaces;
using Backend.Library.Email.Models;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Backend.BusinessTests.Email
{
    public class RegistrationEmailTests
    {
        [Fact]
        public async Task RegistrationEmailHandler_HandleValid_Sends()
        {
            var emailMock = new Mock<IEmailService>();
            var emailSettings = new Library.Email.EmailSettings()
            {
                Username = "username"
            };
            var appSettings = new AppSettings()
            {
                FrontendDomain = "domain"
            };
            var handler = new RegistrationEmailRequestHandler(emailMock.Object, emailSettings, appSettings);

            var request = new RegistrationEmailRequest(new ApplicationUser()
            {
                Email = "test@mail.com",
            });

            await handler.Handle(request, CancellationToken.None);

            emailMock.Verify(x => x.SendEmailAsync(It.Is<EmailMessage>(x => x.To == request.User.Email), CancellationToken.None));
            emailMock.Verify(x => x.SendEmailAsync(It.IsAny<EmailMessage>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task RegistrationEmailHandler_Fails_Throws()
        {
            var emailMock = new Mock<IEmailService>();
            var emailSettings = new Library.Email.EmailSettings();
            var appSettings = new AppSettings();
            var handler = new RegistrationEmailRequestHandler(emailMock.Object, emailSettings, appSettings);

            var request = new RegistrationEmailRequest(null);

            await Assert.ThrowsAsync<Exception>(() => handler.Handle(request, CancellationToken.None));
        }
    }



}
