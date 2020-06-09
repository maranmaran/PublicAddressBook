using Backend.Business.Media.MediaRequests.GetUserMedia;
using Backend.Domain;
using Backend.Domain.Entities.Media;
using Backend.Domain.Enum;
using Backend.Infrastructure.Exceptions;
using Backend.Library.AmazonS3.Interfaces;
using Castle.Core.Internal;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Backend.BusinessTests.Media
{
    public class GetUserMediaTests
    {
        private readonly IApplicationDbContext _context;

        public GetUserMediaTests()
        {
            _context = TestHelper.GetContext();

            var mediaState = new List<MediaFile>()
            {
                new MediaFile()
                {
                    Id = Guid.Parse("6afefa42-1950-4b17-ba90-364166f4bdc6"),
                    Type = MediaType.File,
                    ApplicationUserId = Guid.Parse("6afefa42-1950-4b17-ba90-364166f4bdc7")
                },
                new MediaFile()
                {
                    Id = Guid.Parse("3c1d2d40-9117-4303-a1ff-1862bd60dbb1"),
                    Type = MediaType.Image,
                    ApplicationUserId = Guid.Parse("6afefa42-1950-4b17-ba90-364166f4bdc7")
                },
                new MediaFile()
                {
                    Id = Guid.Parse("d5f80ab7-dc2b-47cf-9de4-bf8ddb831def"),
                    Type = MediaType.Video,
                    ApplicationUserId = Guid.Parse("6afefa42-1950-4b17-ba90-364166f4bdc7")
                },
                new MediaFile()
                {
                    Id = Guid.Parse("7afefa42-1950-4b17-ba90-364166f4bdc6"),
                    Type = MediaType.File,
                    ApplicationUserId = Guid.Parse("6afefa42-1950-4b17-ba90-364166f4bdc7")
                },
                new MediaFile()
                {
                    Id = Guid.Parse("8c1d2d40-9117-4303-a1ff-1862bd60dbb1"),
                    Type = MediaType.Image,
                    ApplicationUserId = Guid.Parse("6afefa42-1950-4b17-ba90-364166f4bdc7")
                },
                new MediaFile()
                {
                    Id = Guid.Parse("95f80ab7-dc2b-47cf-9de4-bf8ddb831def"),
                    Type = MediaType.Video,
                    ApplicationUserId = Guid.Parse("6afefa42-1950-4b17-ba90-364166f4bdc7")
                },

            };
            _context.MediaFiles.AddRange(mediaState);
            _context.SaveChangesAsync(CancellationToken.None).Wait();
        }

        [Fact]
        public async Task GetUserMedia_GetAll_Gets()
        {
            var s3Mock = new Mock<IS3Service>();

            var handler = new GetUserMediaRequestHandler(_context, s3Mock.Object);

            var request = new GetUserMediaRequest(Guid.Parse("6afefa42-1950-4b17-ba90-364166f4bdc7"));

            var result = await handler.Handle(request, CancellationToken.None);
            Assert.Equal(6, result.Count());
        }

        [Fact]
        public async Task GetUserMedia_GetSpecific_Gets()
        {
            var s3Mock = new Mock<IS3Service>();

            var handler = new GetUserMediaRequestHandler(_context, s3Mock.Object);

            var request = new GetUserMediaRequest(Guid.Parse("6afefa42-1950-4b17-ba90-364166f4bdc7"), MediaType.Image);

            var result = await handler.Handle(request, CancellationToken.None);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetUserMedia_IsOnS3_Presignes()
        {
            var s3Mock = new Mock<IS3Service>();
            s3Mock.Setup(x => x.CheckIfPresignedUrlIsExpired(It.IsAny<string>())).Returns(true);

            var handler = new GetUserMediaRequestHandler(_context, s3Mock.Object);

            var request = new GetUserMediaRequest(Guid.Parse("6afefa42-1950-4b17-ba90-364166f4bdc7"), MediaType.Image);

            await handler.Handle(request, CancellationToken.None);
            s3Mock.Verify(x => x.GetPresignedUrlAsync(It.IsAny<string>()), Times.AtLeastOnce);
        }

        [Fact]
        public async Task GetUserMedia_IsNotOnS3_DoesNotPresign()
        {
            var s3Mock = new Mock<IS3Service>();
            s3Mock.Setup(x => x.CheckIfPresignedUrlIsExpired(It.IsAny<string>())).Returns(false);

            var handler = new GetUserMediaRequestHandler(_context, s3Mock.Object);

            var request = new GetUserMediaRequest(Guid.Parse("6afefa42-1950-4b17-ba90-364166f4bdc7"), MediaType.Image);

            await handler.Handle(request, CancellationToken.None);
            s3Mock.Verify(x => x.GetPresignedUrlAsync(It.IsAny<string>()), Times.Never);
        }


        [Fact]
        public async Task GetUserMedia_NoneFound_Empty()
        {
            var s3Mock = new Mock<IS3Service>();
            s3Mock.Setup(x => x.CheckIfPresignedUrlIsExpired(It.IsAny<string>())).Returns(false);

            var handler = new GetUserMediaRequestHandler(_context, s3Mock.Object);

            var request = new GetUserMediaRequest(Guid.Parse("6adefa42-1950-4b17-ba90-364166f4bdc7"), MediaType.Image);

            var result = await handler.Handle(request, CancellationToken.None);
            Assert.True(result.IsNullOrEmpty());
        }

        [Fact]
        public async Task GetUserMedia_Exception_ThrowsFetchFailure()
        {
            var s3Mock = new Mock<IS3Service>();
            s3Mock.Setup(x => x.CheckIfPresignedUrlIsExpired(It.IsAny<string>())).Throws(new Exception());

            var handler = new GetUserMediaRequestHandler(_context, s3Mock.Object);

            var request = new GetUserMediaRequest(Guid.Parse("6afefa42-1950-4b17-ba90-364166f4bdc7"), MediaType.Image);

            await Assert.ThrowsAsync<FetchFailureException>(() => handler.Handle(request, CancellationToken.None));
        }
    }
}
