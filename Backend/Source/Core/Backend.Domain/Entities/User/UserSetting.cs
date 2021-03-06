﻿using Backend.Domain.Enum;
using System;
using System.Collections.Generic;

namespace Backend.Domain.Entities.User
{
    public class UserSetting
    {
        public Guid Id { get; set; }
        public Themes Theme { get; set; }
        public string Language { get; set; }

        public Guid ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        public ICollection<NotificationSetting> NotificationSettings { get; set; } = new HashSet<NotificationSetting>();
    }
}