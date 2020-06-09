using Backend.Business.Media.MediaRequests.UploadUserAvatar;
using Backend.Domain.Entities.User;
using Backend.Infrastructure.Exceptions;
using Backend.Library.AmazonS3.Interfaces;
using Backend.Library.MediaCompression.Interfaces;
using Moq;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Backend.BusinessTests.Media
{
    public class UploadUserAvatarTests
    {
        [Fact]
        public async Task UploadUserAvatar_Valid_Uploads()
        {
            var context = TestHelper.GetContext();
            var user = new ApplicationUser()
            {
                Id = Guid.Parse("d5f80ab7-dc2b-47cf-9de4-bf8ddb831def"),
                Avatar = string.Empty
            };
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync(CancellationToken.None);

            var s3Mock = new Mock<IS3Service>();
            s3Mock.Setup(x => x.GetPresignedUrlAsync(It.IsAny<string>())).ReturnsAsync(() => "presigned");
            s3Mock.Setup(x => x.GetS3Key(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<string>())).Returns(() => "presignedKey");

            var compressMock = new Mock<IMediaCompressionService>();

            var handler = new UploadUserAvatarRequestHandler(s3Mock.Object, context, compressMock.Object);

            var request = new UploadUserAvatarRequest()
            {
                UserId = Guid.Parse("d5f80ab7-dc2b-47cf-9de4-bf8ddb831def"),
                Base64 = Convert.ToBase64String(new byte[] { 0, 1, 0, 1 })
            };

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal("presigned", result);
            Assert.Equal("presignedKey", context.Users.First().Avatar);
            Assert.Equal(1, context.MediaFiles.Count());
        }

        [Fact]
        public async Task UploadUserAvatar_Invalid_Throws()
        {
            var context = TestHelper.GetContext();
            var user = new ApplicationUser()
            {
                Id = Guid.Parse("d5f80ab7-dc2b-47cf-9de4-bf8ddb831def"),
                Avatar = string.Empty
            };
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync(CancellationToken.None);

            var s3Mock = new Mock<IS3Service>();
            s3Mock.Setup(x => x.GetPresignedUrlAsync(It.IsAny<string>())).ReturnsAsync(() => "presigned");
            s3Mock.Setup(x => x.GetS3Key(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<string>())).Throws(new Exception());

            var compressMock = new Mock<IMediaCompressionService>();

            var handler = new UploadUserAvatarRequestHandler(s3Mock.Object, context, compressMock.Object);

            var request = new UploadUserAvatarRequest()
            {
                UserId = Guid.Parse("d5f80ab7-dc2b-47cf-9de4-bf8ddb831def"),
                Base64 = Convert.ToBase64String(new byte[] { 0, 1, 0, 1 })
            };

            await Assert.ThrowsAsync<CreateFailureException>(() => handler.Handle(request, CancellationToken.None));
        }

    }
}
