using Backend.Business.Notifications;
using Backend.Domain.Entities.Media;
using Backend.Domain.Enum;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Backend.BusinessTests.Notification
{
    public class NotificationHelperTests
    {
        [Fact]
        public async Task NotificationHelper_GetType_Gets()
        {
            Assert.Equal(NotificationType.MediaAdded, NotificationHelper.GetNotificationType(nameof(MediaFile)));
        }

        [Fact]
        public async Task NotificationHelper_Unknown_Throws()
        {
            Assert.Throws<ArgumentException>(() => NotificationHelper.GetNotificationType("test"));
        }
    }
}
