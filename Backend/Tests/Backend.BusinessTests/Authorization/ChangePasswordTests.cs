using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Backend.Business.Authorization.AuthorizationRequests.ChangePassword;
using Backend.Business.Authorization.Utils;
using Backend.Domain.Entities.User;
using FluentAssertions;
using FluentValidation.Validators;
using Xunit;

namespace Backend.BusinessTests.Authorization
{
    public class ChangePasswordTests
    {
        [Fact]
        public async Task ChangePasswordHandler_UserNotFound_Throws()
        {
            // arrange
            var context = TestHelper.GetContext();

            var user = new ApplicationUser()
            {
                Id = Guid.Parse("49e37a41-a1c3-4d79-8948-ef7833dfd2ab")
            };

            var request = new ChangePasswordRequest()
            {
                Id = user.Id,
                Password = "pass",
                ConfirmPassword = "pass"
            };

            var handler = new ChangePasswordRequestHandler(context);
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(request, CancellationToken.None));
        }

        [Fact]
        public async Task ChangePasswordHandler_UserFound_PasswordHashed()
        {
            // arrange
            var user = new ApplicationUser()
            {
                Id = Guid.Parse("49e37a41-a1c3-4d79-8948-ef7833dfd2ab")
            };

            var context = TestHelper.GetContext();
            context.Users.Add(user);
            await context.SaveChangesAsync(CancellationToken.None);

            var request = new ChangePasswordRequest()
            {
                Id = user.Id,
                Password = "pass",
                ConfirmPassword = "pass"
            };

            // act
            var handler = new ChangePasswordRequestHandler(context);
            await handler.Handle(request, CancellationToken.None);

            // assert
            Assert.Equal(context.Users.FirstOrDefault().PasswordHash, PasswordHasher.GetPasswordHash(request.Password));
        }

        [Fact]
        public async Task ChangePasswordValidator_ValidRequest_Valid()
        {
            var validator = new ChangePasswordRequestValidator();

            var request = new ChangePasswordRequest()
            {
                Id = Guid.Parse("49e37a41-a1c3-4d79-8948-ef7833dfd2ab"),
                Password = "SuperDuperPassword",
                ConfirmPassword = "SuperDuperPassword"
            };

            var result = validator.Validate(request);
            result.IsValid.Should().Be(true);
        }

        [Fact]
        public async Task ChangePasswordValidator_EmptyId_Invalid()
        {
            var validator = new ChangePasswordRequestValidator();

            var request = new ChangePasswordRequest()
            {
                Id = Guid.Empty,
                Password = "SuperDuperPassword",
                ConfirmPassword = "SuperDuperPassword"
            };

            var result = validator.Validate(request);
            result.IsValid.Should().Be(false);
            result.Errors.Any(x => x.PropertyName == "Id").Should().Be(true);
        }

        [Fact]
        public async Task ChangePasswordValidator_EmptyPassword_Invalid()
        {
            var validator = new ChangePasswordRequestValidator();

            var request = new ChangePasswordRequest()
            {
                Id = Guid.NewGuid(),
                Password = string.Empty,
                ConfirmPassword = string.Empty
            };

            var result = validator.Validate(request);
            result.IsValid.Should().Be(false);
            result.Errors.Count.Should().Be(2);
            result.Errors.Should().Contain(x => x.PropertyName == "Password");
        }

        [Fact]
        public async Task ChangePasswordValidator_WhitespacePassword_Invalid()
        {
            var validator = new ChangePasswordRequestValidator();

            var request = new ChangePasswordRequest()
            {
                Id = Guid.NewGuid(),
                Password = " ",
                ConfirmPassword = " "
            };

            var result = validator.Validate(request);
            result.IsValid.Should().Be(false);
            result.Errors.Count.Should().Be(2);
            result.Errors.Should().Contain(x => x.PropertyName == "Password");
        }

        [Fact]
        public async Task ChangePasswordValidator_MismatchPassword_Invalid()
        {
            var validator = new ChangePasswordRequestValidator();

            var request = new ChangePasswordRequest()
            {
                Id = Guid.NewGuid(),
                Password = "dadad",
                ConfirmPassword = " wdwadwada"
            };

            var result = validator.Validate(request);
            result.IsValid.Should().Be(false);
            result.Errors.Single().PropertyName.Should().Be("Password");
        }

        [Fact]
        public async Task ChangePasswordValidator_ShortPassword_Invalid()
        {
            var validator = new ChangePasswordRequestValidator();

            var request = new ChangePasswordRequest()
            {
                Id = Guid.NewGuid(),
                Password = "12",
                ConfirmPassword = "12"
            };

            var result = validator.Validate(request);
            result.IsValid.Should().Be(false);
            result.Errors.Count.Should().Be(2);
            result.Errors.Should().Contain(x => x.PropertyName == "Password");
            result.Errors.Should().Contain(x => x.ErrorCode == nameof(MinimumLengthValidator));
        }

    }
}
