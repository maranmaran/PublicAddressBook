using AutoMapper;
using Backend.Business.Authorization.AuthorizationRequests.CurrentUser;
using Backend.Common;
using Backend.Domain.Entities.User;
using Backend.Infrastructure.Exceptions;
using Backend.Library.AmazonS3.Interfaces;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Backend.BusinessTests.Authorization
{
    public class CurrentUserTests
    {
        [Fact]
        public async Task CurrentUser_UserNotFound_Throws()
        {
            // arrange
            var context = TestHelper.GetContext();

            var request = new CurrentUserRequest(Guid.NewGuid());

            var handler = new CurrentUserRequestHandler(new Mock<IMapper>(MockBehavior.Loose).Object, context, new Mock<IS3Service>().Object);
            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(request, CancellationToken.None));
        }

        [Fact]
        public async Task CurrentUser_UserFound_Returns()
        {
            // arrange

            // context
            var notificationSettings = new List<NotificationSetting>()
            {
                new NotificationSetting()
                {
                    Id = Guid.NewGuid()
                }
            };
            var userSettings = new UserSetting()
            {
                Id = Guid.NewGuid(),
                NotificationSettings = notificationSettings
            };
            var user = new ApplicationUser()
            {
                Id = Guid.NewGuid(),
                UserSetting = userSettings,
            };

            var context = TestHelper.GetContext();
            context.Users.Add(user);
            await context.SaveChangesAsync(CancellationToken.None);

            // automapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ApplicationUser, CurrentUserRequestResponse>();
            });
            var mapper = config.CreateMapper();


            // act
            var request = new CurrentUserRequest(user.Id);
            var handler = new CurrentUserRequestHandler(mapper, context, new Mock<IS3Service>().Object);
            var result = await handler.Handle(request, CancellationToken.None);

            // assert
            result.Should().NotBe(null);
            result.Id.Should().Be(user.Id);
            result.UserSetting.Id.Should().Be(user.UserSetting.Id);
            result.UserSetting.NotificationSettings.Count.Should().Be(notificationSettings.Count);
            result.UserSetting.NotificationSettings.First().Id.Should().Be(notificationSettings.First().Id);
        }

        [Fact]
        public async Task CurrentUser_UserFound_S3Avatar_Refreshes()
        {
            // context
            var notificationSettings = new List<NotificationSetting>()
            {
                new NotificationSetting()
                {
                    Id = Guid.NewGuid()
                }
            };
            var userSettings = new UserSetting()
            {
                Id = Guid.NewGuid(),
                NotificationSettings = notificationSettings
            };
            var user = new ApplicationUser()
            {
                Id = Guid.NewGuid(),
                UserSetting = userSettings,
                Avatar = "s3",
            };

            var context = TestHelper.GetContext();
            context.Users.Add(user);
            await context.SaveChangesAsync(CancellationToken.None);

            // automapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ApplicationUser, CurrentUserRequestResponse>();
            });
            var mapper = config.CreateMapper();

            var s3Mock = new Mock<IS3Service>();

            // act
            var request = new CurrentUserRequest(user.Id);
            var handler = new CurrentUserRequestHandler(mapper, context, s3Mock.Object);
            await handler.Handle(request, CancellationToken.None);

            s3Mock.Verify(x => x.GetPresignedUrlAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task CurrentUser_UserFound_GenericAvatar_SkipsRefresh()
        {
            // context
            var notificationSettings = new List<NotificationSetting>()
            {
                new NotificationSetting()
                {
                    Id = Guid.NewGuid()
                }
            };
            var userSettings = new UserSetting()
            {
                Id = Guid.NewGuid(),
                NotificationSettings = notificationSettings
            };
            var user = new ApplicationUser()
            {
                Id = Guid.NewGuid(),
                UserSetting = userSettings,
                Avatar = new GenericAvatarConstructor().Value(),
            };


            var context = TestHelper.GetContext();
            context.Users.Add(user);
            await context.SaveChangesAsync(CancellationToken.None);

            // automapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ApplicationUser, CurrentUserRequestResponse>();
            });
            var mapper = config.CreateMapper();

            var s3Mock = new Mock<IS3Service>();

            // act
            var request = new CurrentUserRequest(user.Id);
            var handler = new CurrentUserRequestHandler(mapper, context, s3Mock.Object);
            await handler.Handle(request, CancellationToken.None);

            s3Mock.Verify(x => x.GetPresignedUrlAsync(It.IsAny<string>()), Times.Never);
        }
    }
}
