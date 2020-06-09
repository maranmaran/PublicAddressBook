using Backend.Business.Authorization.AuthorizationRequests.CurrentUser;
using Backend.Business.Authorization.AuthorizationRequests.SetPassword;
using Backend.Business.Authorization.AuthorizationRequests.SignIn;
using Backend.Business.Authorization.Utils;
using Backend.Domain.Entities.User;
using FluentAssertions;
using MediatR;
using Moq;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Backend.BusinessTests.Authorization
{
    public class SetPasswordTests
    {

        [Fact]
        public async Task SetPasswordHandler_UserNotFound_Throws()
        {
            // arrange
            var context = TestHelper.GetContext();

            var request = new SetPasswordRequest()
            {
                UserId = Guid.Parse("49e37a41-a1c3-4d79-8948-ef7833dfd2aa"),
                Password = "password",
                RepeatPassword = "password",
            };

            var handler = new SetPasswordRequestHandler(context, new Mock<IMediator>().Object);
            await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(request, CancellationToken.None));
        }

        [Fact]
        public async Task ChangePasswordHandler_UserFound_ReturnsUserInformationAndToken()
        {
            // arrange
            var context = TestHelper.GetContext();
            var user = new ApplicationUser()
            {
                Id = Guid.Parse("49e37a41-a1c3-4d79-8948-ef7833dfd2ab"),
            };
            context.Users.Add(user);
            await context.SaveChangesAsync(CancellationToken.None);

            var request = new SetPasswordRequest()
            {
                UserId = Guid.Parse("49e37a41-a1c3-4d79-8948-ef7833dfd2ab"),
                Password = "password",
                RepeatPassword = "password",
            };

            var userInformationResponse = new CurrentUserRequestResponse();
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(x => x.Send(It.IsAny<SignInRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((userInformationResponse, "Token"));

            var handler = new SetPasswordRequestHandler(context, mediatorMock.Object);
            var result = await handler.Handle(request, CancellationToken.None);

            Assert.IsType<(CurrentUserRequestResponse, string)>(result);
            Assert.NotNull(result.response);
            Assert.NotNull(result.token);
            Assert.NotEmpty(result.token);
            Assert.Equal("Token", result.token);
            Assert.Equal(result.response.GetHashCode(), userInformationResponse.GetHashCode());
        }

        [Fact]
        public async Task ChangePasswordHandler_UserFound_SetsPassword()
        {
            // arrange
            var context = TestHelper.GetContext();
            var user = new ApplicationUser()
            {
                Id = Guid.Parse("49e37a41-a1c3-4d79-8948-ef7833dfd2ab"),
            };
            context.Users.Add(user);
            await context.SaveChangesAsync(CancellationToken.None);

            var request = new SetPasswordRequest()
            {
                UserId = Guid.Parse("49e37a41-a1c3-4d79-8948-ef7833dfd2ab"),
                Password = "password",
                RepeatPassword = "password",
            };

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(x => x.Send(It.IsAny<CurrentUserRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CurrentUserRequestResponse());

            var handler = new SetPasswordRequestHandler(context, mediatorMock.Object);
            await handler.Handle(request, CancellationToken.None);

            var userResult = context.Users.First();
            Assert.NotEmpty(userResult.PasswordHash);
            Assert.NotNull(userResult.PasswordHash);
            Assert.Equal(PasswordHasher.GetPasswordHash("password"), userResult.PasswordHash);
        }

        [Fact]
        public async Task SetPasswordValidator_EmptyUserId_Fails()
        {
            var validator = new SetPasswordRequestValidator();
            var result = validator.Validate(new SetPasswordRequest()
            { UserId = Guid.Empty, Password = "password", RepeatPassword = "password" });

            result.IsValid.Should().Be(false);
        }

        [Fact]
        public async Task SetPasswordValidator_InvalidPassword_Fails()
        {
            var validator = new SetPasswordRequestValidator();

            var result = validator.Validate(new SetPasswordRequest { UserId = Guid.NewGuid(), Password = "", RepeatPassword = "password" });
            result.IsValid.Should().Be(false);

            var result2 = validator.Validate(new SetPasswordRequest { UserId = Guid.NewGuid(), Password = null, RepeatPassword = "password" });
            result2.IsValid.Should().Be(false);

            var result3 = validator.Validate(new SetPasswordRequest { UserId = Guid.NewGuid(), Password = "password", RepeatPassword = "" });
            result3.IsValid.Should().Be(false);

            var result4 = validator.Validate(new SetPasswordRequest { UserId = Guid.NewGuid(), Password = "password", RepeatPassword = null });
            result4.IsValid.Should().Be(false);

            var result5 = validator.Validate(new SetPasswordRequest { UserId = Guid.NewGuid(), Password = "pas", RepeatPassword = "pas" });
            result5.IsValid.Should().Be(false);

            var result6 = validator.Validate(new SetPasswordRequest { UserId = Guid.NewGuid(), Password = "password", RepeatPassword = "password2" });
            result6.IsValid.Should().Be(false);

        }

        [Fact]
        public async Task SetPasswordValidator_Valid_Success()
        {
            var validator = new SetPasswordRequestValidator();
            var result = validator.Validate(new SetPasswordRequest { UserId = Guid.NewGuid(), Password = "password", RepeatPassword = "password" });
            ;

            result.IsValid.Should().Be(true);
        }

    }
}
