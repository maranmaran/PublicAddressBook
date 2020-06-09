using AutoMapper;
using Backend.Business.Authorization.AuthorizationRequests.SendRegistrationEmail;
using Backend.Business.Billing.BillingRequests.AddCustomer;
using Backend.Business.Users.UsersRequests.CreateUser;
using Backend.Business.Users.UsersRequests.DeleteUser;
using Backend.Business.Users.UsersRequests.GetAllUsers;
using Backend.Business.Users.UsersRequests.GetUser;
using Backend.Business.Users.UsersRequests.SaveUserSettings;
using Backend.Business.Users.UsersRequests.UpdateUser;
using Backend.Domain.Entities.User;
using Backend.Domain.Enum;
using Backend.Infrastructure.Exceptions;
using Backend.Library.AmazonS3.Interfaces;
using Backend.Persistence;
using MediatR;
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
        private readonly IMapper _mapper;

        public UserTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreateUserRequest, ApplicationUser>();
                cfg.CreateMap<UpdateUserRequest, ApplicationUser>();
            });
            _mapper = config.CreateMapper();


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
        public async Task GetAllUsersHandler_AccountType_Gets()
        {
            var s3Mock = new Mock<IS3Service>();
            var handler = new GetAllUsersRequestHandler(_context, s3Mock.Object);
            var result = await handler.Handle(new GetAllUsersRequest(AccountType.User), CancellationToken.None);
            Assert.Equal(_context.Users.Count(), result.Count());
        }

        [Fact]
        public async Task CreateUserHandler_ValidData_Creates()
        {
            var mediatorMock = new Mock<IMediator>();

            var handler = new CreateUserRequestHandler(mediatorMock.Object, _mapper, _context);

            var request = new CreateUserRequest()
            {
                Username = "create",
                AccountType = AccountType.User,
                Email = "create@mail.com",
                FirstName = "create",
                LastName = "create",
                Gender = Gender.Female
            };

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.NotNull(result);
            Assert.NotEqual(Guid.Empty, result.Id);
            Assert.Equal(request.Username, result.Username);

            var user = _context.Users.FirstOrDefault(x => x.Id == result.Id);
            Assert.NotNull(user);
            mediatorMock.Verify(x => x.Send(It.IsAny<AddCustomerRequest>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(x => x.Send(It.IsAny<SendRegistrationEmailRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteUserHandler_ValidData_Deletes()
        {
            _context.Users.Add(new ApplicationUser() { Id = Guid.Parse("f6c3e6d7-1504-4002-b747-bfc88cab4568") });
            await _context.SaveChangesAsync(CancellationToken.None);

            var handler = new DeleteUserRequestHandler(_context);

            var request = new DeleteUserRequest(Guid.Parse("f6c3e6d7-1504-4002-b747-bfc88cab4568"), AccountType.User);

            await handler.Handle(request, CancellationToken.None);

            Assert.Null(_context.Users.FirstOrDefault(x => x.Id == Guid.Parse("f6c3e6d7-1504-4002-b747-bfc88cab4568")));
        }

        [Fact]
        public async Task UpdateUserHandler_ValidData_Updates()
        {
            var handler = new UpdateUserRequestHandler(_context, _mapper);

            var request = new UpdateUserRequest()
            {
                Id = Guid.Parse("d5535764-d39d-42bf-9b84-429b72a6e902"),
                Username = "update",
                Email = "create@mail.com",
                FirstName = "create",
                LastName = "create",
                Gender = Gender.Female,
                AccountType = AccountType.User,
            };

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(result.Username, request.Username);

            var user = _context.Users.FirstOrDefault(x => x.Id == result.Id);
            Assert.NotNull(user);
            Assert.Equal(user.Username, request.Username);
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
        public async Task GetAllUsersHandler_InvalidAccountType_Throws()
        {
            var s3Mock = new Mock<IS3Service>();
            var handler = new GetAllUsersRequestHandler(_context, s3Mock.Object);
            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(new GetAllUsersRequest(It.IsAny<AccountType>()), CancellationToken.None));
        }

        [Fact]
        public async Task CreateUserHandler_InvalidData_DoesNotCreate()
        {
            var mediatorMock = new Mock<IMediator>();

            var handler = new CreateUserRequestHandler(mediatorMock.Object, _mapper, _context);

            var request = new CreateUserRequest()
            {
                Username = "create",
                AccountType = It.IsAny<AccountType>(),
                Email = "create@mail.com",
                FirstName = "create",
                LastName = "create",
                Gender = Gender.Female
            };

            await Assert.ThrowsAsync<CreateFailureException>(() => handler.Handle(request, CancellationToken.None));

        }

        [Fact]
        public async Task DeleteUserHandler_InvalidData_DoesNotDelete()
        {
            var handler = new DeleteUserRequestHandler(_context);

            var request = new DeleteUserRequest(Guid.Empty, AccountType.User);

            await Assert.ThrowsAsync<DeleteFailureException>(() => handler.Handle(request, CancellationToken.None));
        }

        [Fact]
        public async Task UpdateUserHandler_InvalidData_DoesNotUpdate()
        {
            var handler = new UpdateUserRequestHandler(_context, _mapper);

            var request = new UpdateUserRequest()
            {
                Id = Guid.Parse("d5535764-d39d-42bf-9b84-429b72a6e902"),
                Username = "update",
                Email = "create@mail.com",
                FirstName = "create",
                LastName = "create",
                Gender = Gender.Female,
                AccountType = It.IsAny<AccountType>(),
            };

            await Assert.ThrowsAsync<UpdateFailureException>(() => handler.Handle(request, CancellationToken.None));

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
