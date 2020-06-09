using Backend.Business.Authorization.AuthorizationRequests.ResetPassword;
using Backend.Business.Email.EmailRequests.ResetPassword;
using Backend.Domain.Entities.User;
using MediatR;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Backend.BusinessTests.Authorization
{
    public class ResetPasswordTests
    {
        [Fact]
        public async Task ResetPasswordHandler_UserNotFound_Throws()
        {
            // arrange
            var context = TestHelper.GetContext();
            var mediatorMock = new Mock<IMediator>();

            var handler = new ResetPasswordRequestHandler(context, mediatorMock.Object);

            await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(new ResetPasswordRequest("unknownEmail@gmail.com"), CancellationToken.None));
        }

        [Fact]
        public async Task ResetPasswordHandler_UserFound_SendsRecoveryMail()
        {
            // arrange
            var context = TestHelper.GetContext();
            var user = new ApplicationUser()
            {
                Id = Guid.NewGuid(),
                Email = "test@email.com"
            };
            context.Users.Add(user);
            await context.SaveChangesAsync(CancellationToken.None);

            var mediatorMock = new Mock<IMediator>();

            var handler = new ResetPasswordRequestHandler(context, mediatorMock.Object);

            await handler.Handle(new ResetPasswordRequest("test@email.com"), CancellationToken.None);
            mediatorMock.Verify(x => x.Send(It.IsAny<ResetPasswordEmailRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
