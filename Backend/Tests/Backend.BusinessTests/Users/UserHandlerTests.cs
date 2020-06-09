using Backend.Business.Users.UsersRequests.GetUser;
using Backend.Business.Users.UsersRequests.SaveUserSettings;
using Backend.Domain.Entities.User;
using Backend.Infrastructure.Exceptions;
using Backend.Library.AmazonS3.Interfaces;
using Backend.Persistence;
using Moq;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using AccountType = Backend.Domain.Enum.AccountType;

namespace Backend.BusinessTests.Users
{
    public class UserTests
    {
        private readonly ApplicationDbContext _context;

        public UserTests()
        {
            _context = TestHelper.GetContext();

            var users = new[]
            {
                new ApplicationUser()
                {
                    Id = Guid.Parse("f6c3e6d7-1504-4002-b747-bfc88cab4566"),
                    Username = "Username",
                    AccountType = AccountType.User,
                    Active = true,
                    Avatar = string.Empty,
                    FirstName = "User",
                    LastName = "User"
                },
                new ApplicationUser()
                {
                    Id = Guid.Parse("1f0fc7e4-ca63-4963-9283-b6806861878c"),
                    Username = "Username1",
                    AccountType = AccountType.User,
                    Active = true,
                    Avatar = string.Empty,
                    FirstName = "User",
                    LastName = "User"
                },
                new ApplicationUser()
                {
                    Id = Guid.Parse("d5535764-d39d-42bf-9b84-429b72a6e902"),
                    Username = "Username2",
                    AccountType = AccountType.User,
                    Active = true,
                    Avatar = string.Empty,
                    FirstName = "User",
                    LastName = "User"
                },
                new ApplicationUser()
                {
                    Id = Guid.Parse("92d55817-9618-4fc3-ab93-7759307df904"),
                    Username = "Username3",
                    AccountType = AccountType.User,
                    Active = false,
                    Avatar = string.Empty,
                    FirstName = "User",
                    LastName = "User"
                },
            };
            _context.Users.AddRange(users);
            _context.SaveChangesAsync(CancellationToken.None).Wait();
        }

        #region Valid

        [Fact]
        public async Task GetUserHandler_ValidId_Gets()
        {
            var s3Mock = new Mock<IS3Service>();

            var handler = new GetUserRequestHandler(_context, s3Mock.Object);
            var result = await handler.Handle(new GetUserRequest(Guid.Parse("d5535764-d39d-42bf-9b84-429b72a6e902"), AccountType.User), CancellationToken.None);
            Assert.Equal("Username2", result.Username);
        }


        [Fact]
        public async Task UpdateUserSettings_ValidData_Updates()
        {
            var userSetting = new UserSetting()
            {
                Id = Guid.Parse("f5535764-d39d-42bf-9b84-429b72a6e902"),
                ApplicationUserId = Guid.Parse("d5535764-d39d-42bf-9b84-429b72a6e902"),
                Language = "en",
            };
            _context.UserSettings.Add(userSetting);
            await _context.SaveChangesAsync(CancellationToken.None);

            userSetting.Language = "hr";
            var request = new SaveUserSettingsRequest()
            {
                UserSetting = userSetting
            };

            var handler = new SaveUserSettingsRequestHandler(_context);
            await handler.Handle(request, CancellationToken.None);

            Assert.Equal("hr", _context.UserSettings.FirstOrDefault(x => x.Id == Guid.Parse("f5535764-d39d-42bf-9b84-429b72a6e902"))?.Language);
        }

        #endregion

        #region Invalid

        [Fact]
        public async Task GetUserHandler_InvalidId_Throws()
        {
            var s3Mock = new Mock<IS3Service>();

            var handler = new GetUserRequestHandler(_context, s3Mock.Object);
            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(new GetUserRequest(Guid.Parse("d5535764-d39d-42bf-9b84-429b72a6e283"), AccountType.User), CancellationToken.None));
        }

        [Fact]
        public async Task UpdateUserSettings_InvalidData_DoesNotUpdate()
        {
            var userSetting = new UserSetting()
            {
                Id = Guid.Parse("f5535764-d39d-42bf-9b84-429b72a6e902"),
                ApplicationUserId = Guid.Parse("d5535764-d39d-42bf-9b84-429b72a6e902"),
                Language = "en",
            };
            var request = new SaveUserSettingsRequest()
            {
                UserSetting = userSetting
            };

            var handler = new SaveUserSettingsRequestHandler(_context);
            await Assert.ThrowsAsync<UpdateFailureException>(() => handler.Handle(request, CancellationToken.None));
        }

        #endregion

    }
}
