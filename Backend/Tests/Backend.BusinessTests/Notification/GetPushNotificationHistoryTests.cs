using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Backend.Business.Notifications.NotificationRequests.GetPushNotificationHistory;
using Backend.Domain;
using Backend.Domain.Entities.User;
using Backend.Library.AmazonS3.Interfaces;
using Moq;
using Xunit;

namespace Backend.BusinessTests.Notification
{
    public class GetPushNotificationHistoryTests
    {
        private readonly IApplicationDbContext _context;

        public GetPushNotificationHistoryTests()
        {
            _context = TestHelper.GetContext();
            _context.Users.Add(new ApplicationUser()
            {
                Id = Guid.Parse("4474c02a-f461-44d2-9c74-f95a28d90749"),
                Avatar = "test2"
            });
            _context.Notifications.Add(new Domain.Entities.Notification.Notification()
            {
                Id = Guid.Parse("67d13c84-83d5-4685-a134-e236a2be595b"),
                ReceiverId = Guid.Parse("4474c02a-f461-44d2-9c74-f95a28d90749"),
                SentAt = DateTime.Today
            });
            _context.Notifications.Add(new Domain.Entities.Notification.Notification()
            {
                Id = Guid.Parse("23222299-096d-4fd4-8e19-ebc67ff03b44"),
                ReceiverId = Guid.Parse("4474c02a-f461-44d2-9c74-f95a28d90749"),
                SentAt = DateTime.Today.AddDays(-1)
            });
            _context.Notifications.Add(new Domain.Entities.Notification.Notification()
            {
                Id = Guid.Parse("de31b934-24f8-41fc-b494-29a2eb796af5"),
                ReceiverId = Guid.Parse("4474c02a-f461-44d2-9c74-f95a28d90749"),
                SentAt = DateTime.Today.AddDays(-2)
            });
            _context.Notifications.Add(new Domain.Entities.Notification.Notification()
            {
                Id = Guid.Parse("1ee596f4-1585-4361-867d-acc6347ba241"),
                ReceiverId = Guid.Parse("4474c02a-f461-44d2-9c74-f95a28d90749"),
                SentAt = DateTime.Today.AddDays(-3)
            });
            _context.Notifications.Add(new Domain.Entities.Notification.Notification()
            {
                Id = Guid.Parse("386d1689-fe80-4c61-9c84-5a7f033b1d6c"),
                ReceiverId = Guid.Parse("4474c02a-f461-44d2-9c74-f95a28d90749"),
                SentAt = DateTime.Today.AddDays(-4)
            });
            _context.SaveChangesAsync(CancellationToken.None).Wait();
        }

        [Fact]
        public async Task GetPushNotificationHistory_ValidRequest_Gets()
        {
            var s3Mock = new Mock<IS3Service>();
            var handler = new GetNotificationHistoryRequestHandler(_context, s3Mock.Object);

            var request = new GetNotificationHistoryRequest(Guid.Parse("4474c02a-f461-44d2-9c74-f95a28d90749"), 0, 3);
            var result = await handler.Handle(request, CancellationToken.None);
            Assert.Equal(3, result.Count());
            Assert.Equal(Guid.Parse("67d13c84-83d5-4685-a134-e236a2be595b"), result.ElementAt(0).Id);
            Assert.Equal(Guid.Parse("23222299-096d-4fd4-8e19-ebc67ff03b44"), result.ElementAt(1).Id);
            Assert.Equal(Guid.Parse("de31b934-24f8-41fc-b494-29a2eb796af5"), result.ElementAt(2).Id);
        }

        [Fact]
        public async Task GetPushNotificationHistory_UserNotFound_Empty()
        {
            var s3Mock = new Mock<IS3Service>();
            var handler = new GetNotificationHistoryRequestHandler(_context, s3Mock.Object);

            var request = new GetNotificationHistoryRequest(Guid.Parse("5474c02a-f461-44d2-9c74-f95a28d90749"), 0, 3);
            var result = await handler.Handle(request, CancellationToken.None);
            Assert.Empty(result);
        }
    }
}