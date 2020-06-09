using Backend.Domain.Entities.Media;
using Backend.Domain.Enum;
using System;
using System.Collections.Generic;

namespace Backend.Domain.Entities.User
{
    public class ApplicationUser
    {

        public Guid Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";

        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public string Avatar { get; set; }
        public Gender Gender { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime LastModified { get; set; }

        public bool Active { get; set; }

        public AccountType AccountType { get; set; }

        public Guid UserSettingId { get; set; }
        public virtual UserSetting UserSetting { get; set; }

        public virtual ICollection<Notification.Notification> SentNotifications { get; set; } = new HashSet<Notification.Notification>();
        public virtual ICollection<Notification.Notification> ReceivedNotifications { get; set; } = new HashSet<Notification.Notification>();
        public virtual ICollection<MediaFile> MediaFiles { get; set; } = new HashSet<MediaFile>();

    }
}