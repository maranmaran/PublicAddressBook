using Backend.Domain.Entities.Notification;
using MediatR;

namespace Backend.Business.Notifications.NotificationRequests.NotifyAllClients
{
    public class NotifyAllClientsNotification : INotification
    {
        public Notification Notification { get; set; }

        public NotifyAllClientsNotification(Notification notification)
        {
            Notification = notification;
        }
    }
}
