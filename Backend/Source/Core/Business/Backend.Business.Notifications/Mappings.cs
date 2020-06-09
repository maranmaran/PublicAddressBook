using AutoMapper;
using Backend.Business.Notifications.NotificationRequests.CreatePushNotification;
using Backend.Domain.Entities.Notification;
using Backend.Infrastructure.Extensions;

namespace Backend.Business.Notifications
{
    public class Mappings : Profile
    {
        public Mappings()
        {
            CreateMap<CreateNotificationRequest, Notification>();
            CreateMap<Notification, Notification>().IgnoreAllVirtual();
        }
    }
}
