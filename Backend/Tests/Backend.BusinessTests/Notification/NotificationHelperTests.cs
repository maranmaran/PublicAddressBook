using Backend.Business.Notifications;
using Backend.Domain.Entities.Contacts;
using Backend.Domain.Enum;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Backend.BusinessTests.Notification
{
    public class NotificationHelperTests
    {
        // TODO use Theory and inline data if notification type expands
        [Fact]
        public async Task NotificationHelper_GetType_Gets()
        {
            Assert.Equal(NotificationType.ContactChanged, NotificationHelper.GetNotificationType(nameof(Contact)));
        }

        [Fact]
        public async Task NotificationHelper_Unknown_Throws()
        {
            Assert.Throws<ArgumentException>(() => NotificationHelper.GetNotificationType("test"));
        }
    }
}
