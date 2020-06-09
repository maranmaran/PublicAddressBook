using System.Collections.Generic;
using System.Text;
using Backend.Domain.Entities.Notification;
using MediatR;

namespace Backend.Business.Email.EmailRequests.NotificationEmail
{
    public class NotificationEmailRequest: IRequest<Unit>
    {
        public NotificationEmailRequest(Notification notification)
        {
            Notification = notification;
        }

        public Notification Notification { get; set; }
    }
}
