using Backend.Business.Notifications.NotificationRequests.ReadNotification;
using Backend.Domain;
using Backend.Infrastructure.Exceptions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Backend.BusinessTests.Notification
{
    public class ReadNotificationTests
    {
        private readonly IApplicationDbContext _context;

        public ReadNotificationTests()
        {
            _context = TestHelper.GetContext();
            _context.Notifications.Add(new Domain.Entities.Notification.Notification()
            {
                Id = Guid.Parse("de31b934-24f8-41fc-b494-29a2eb796af5"),
                Read = false,
            });
            _context.SaveChangesAsync(CancellationToken.None).Wait();
        }

        [Fact]
        public async Task ReadNotificationHandler_ValidRequest_Reads()
        {
            var handler = new ReadNotificationRequestHandler(_context);
            await handler.Handle(new ReadNotificationRequest() { Id = Guid.Parse("de31b934-24f8-41fc-b494-29a2eb796af5") },
                CancellationToken.None);

            Assert.True(_context.Notifications.First().Read);
        }

        [Fact]
        public async Task ReadNotificationHandler_NotFound_Throws()
        {
            var handler = new ReadNotificationRequestHandler(_context);
            await Assert.ThrowsAsync<UpdateFailureException>(() => handler.Handle(
                new ReadNotificationRequest() { Id = Guid.Parse("ce31b934-24f8-41fc-b494-29a2eb796af5") },
                CancellationToken.None));
        }
    }
}
