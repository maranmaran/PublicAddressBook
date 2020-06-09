using Backend.Domain.Entities.Contacts;
using Backend.Domain.Enum;
using System;

namespace Backend.Business.Notifications
{
    internal static class NotificationHelper
    {
        internal static NotificationType GetNotificationType(string entityType)
        {
            switch (entityType)
            {
                case nameof(Contact):
                    return NotificationType.ContactChanged;
                default:
                    throw new ArgumentException(
                        $"Entity type not recognized. Couldn't get notification type. {entityType}");
            }
        }

    }
}
