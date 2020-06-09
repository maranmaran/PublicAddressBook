using Backend.Business.Authorization;
using Backend.Business.Authorization.AuthorizationRequests.CurrentUser;
using Backend.Business.Authorization.AuthorizationRequests.SignIn;
using Backend.Business.Authorization.Interfaces;
using Backend.Business.Authorization.Utils;
using Backend.Domain.Entities.User;
using FluentAssertions;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Backend.BusinessTests.Authorization
{
    public class SignInTests
    {
        [Fact]
        public async Task SignInHandler_WrongUsernameOrPassword_UnauthorizedException()
        {
            // arrange
            var user = new ApplicationUser()
            {
                Id = Guid.NewGuid(),
                Username = "user",
                PasswordHash = PasswordHasher.GetPasswordHash("pass")
            };

            var context = TestHelper.GetContext();
            context.Users.Add(user);
            await context.SaveChangesAsync(CancellationToken.None);

            var request = new SignInRequest()
            {
                Username = "user1",
                Password = "pass"
            };
            var request2 = new SignInRequest()
            {
                Username = "user",
                Password = "pass1"
            };

            var jwtMock = new Mock<IJwtTokenGenerator>();
            var mediatorMock = new Mock<IMediator>();

            var handler = new SignInRequestHandler(context, jwtMock.Object, mediatorMock.Object);
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => handler.Handle(request, CancellationToken.None));
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => handler.Handle(request2, CancellationToken.None));
        }

        [Fact]
        public async Task SignInHandler_UserFound_ReturnsUserAndToken()
        {
            // arrange
            var user = new ApplicationUser()
            {
                Id = Guid.NewGuid(),
                Username = "user",
                PasswordHash = PasswordHasher.GetPasswordHash("pass")
            };

            var context = TestHelper.GetContext();
            context.Users.Add(user);
            await context.SaveChangesAsync(CancellationToken.None);

            var request = new SignInRequest()
            {
                Username = "user",
                Password = "pass"
            };


            var jwtMock = new Mock<IJwtTokenGenerator>();
            jwtMock.Setup(x => x.GenerateToken(It.IsAny<Guid>())).Returns("Token");

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(x => x.Send(It.IsAny<CurrentUserRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CurrentUserRequestResponse() { Id = user.Id });

            var handler = new SignInRequestHandler(context, jwtMock.Object, mediatorMock.Object);
            var result = await handler.Handle(request, CancellationToken.None);

            mediatorMock.Verify(x => x.Send(It.IsAny<CurrentUserRequest>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.IsType<CurrentUserRequestResponse>(result.response);
            Assert.NotNull(result.response);
            Assert.Equal(user.Id, result.response.Id);
            Assert.IsType<string>(result.token);
            Assert.NotNull(result.token);
            Assert.NotEmpty(result.token);
            Assert.NotEmpty(result.token);
            Assert.Equal("Token", result.token);

        }

        [Fact]
        public async Task SignInHandler_JwtGenerator_Generates()
        {
            var jwtGen = new JwtTokenGenerator(new JwtSettings() { JwtSecret = "ExtraSUperLongSecretForIntegrationFunctionalUnitTestsJustBecause" });
            var tokenHandler = new JwtSecurityTokenHandler();

            var validationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("ExtraSUperLongSecretForIntegrationFunctionalUnitTestsJustBecause")),
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateLifetime = false,
                ValidateActor = false,
            };

            tokenHandler.ValidateToken(jwtGen.GenerateToken(Guid.NewGuid()), validationParameters, out _);
        }

        [Theory]
        [InlineData("Active")]
        [InlineData("Trialing")]
        public async Task SignInValidator_ValidRequest_Valid(string subStatus)
        {
            // arrange
            var user = new ApplicationUser()
            {
                Id = Guid.NewGuid(),
                Username = "user",
                PasswordHash = PasswordHasher.GetPasswordHash("passw"),
                Active = true
            };

            var context = TestHelper.GetContext();
            context.Users.Add(user);
            await context.SaveChangesAsync(CancellationToken.None);


            var validator = new SignInRequestValidator(context);

            var request = new SignInRequest()
            {
                Username = "user",
                Password = "passw",
            };

            var result = validator.Validate(request);
            result.IsValid.Should().Be(true);
        }

        [Fact]
        public async Task SignInValidator_InvalidPassword_Fails()
        {
            // arrange
            var user = new ApplicationUser()
            {
                Id = Guid.NewGuid(),
                Username = "user",
                PasswordHash = PasswordHasher.GetPasswordHash("password"),
                Active = true
            };

            var context = TestHelper.GetContext();
            context.Users.Add(user);
            await context.SaveChangesAsync(CancellationToken.None);

            var validator = new SignInRequestValidator(context);

            var request = new SignInRequest()
            {
                Username = "user",
                Password = "passw",
            };

            var result = validator.Validate(request);
            result.IsValid.Should().Be(false);
        }

        [Fact]
        public async Task SignInValidator_InvalidUsername_Fails()
        {
            // arrange
            var user = new ApplicationUser()
            {
                Id = Guid.NewGuid(),
                Username = string.Empty,
                PasswordHash = PasswordHasher.GetPasswordHash("passw"),
                Active = true
            };

            var context = TestHelper.GetContext();
            context.Users.Add(user);
            await context.SaveChangesAsync(CancellationToken.None);

            var validator = new SignInRequestValidator(context);

            var request = new SignInRequest()
            {
                Username = "user",
                Password = "passw",
            };

            var result = validator.Validate(request);
            result.IsValid.Should().Be(false);
        }

        [Fact]
        public async Task SignInValidator_InactiveUser_Fails()
        {
            // arrange
            var user = new ApplicationUser()
            {
                Id = Guid.NewGuid(),
                Username = "user",
                PasswordHash = PasswordHasher.GetPasswordHash("passw"),
                Active = false
            };

            var context = TestHelper.GetContext();
            context.Users.Add(user);
            await context.SaveChangesAsync(CancellationToken.None);

            var validator = new SignInRequestValidator(context);

            var request = new SignInRequest()
            {
                Username = "user",
                Password = "passw",
            };

            var result = validator.Validate(request);
            result.IsValid.Should().Be(false);
        }

    }
}
