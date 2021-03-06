﻿using System;
using Backend.Domain.Entities.User;
using Backend.Domain.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Persistence.Configurations
{
    public class NotificationSettingConfiguration : IEntityTypeConfiguration<NotificationSetting>
    {
        public void Configure(EntityTypeBuilder<NotificationSetting> builder)
        {
            builder.Property(x => x.ReceiveNotification).HasDefaultValue(true);

            builder.Property(x => x.NotificationType)
                .HasConversion(
                    v => v.ToString(),
                    v => (NotificationType)Enum.Parse(typeof(NotificationType), v));

            builder
                .HasOne(x => x.UserSetting)
                .WithMany(x => x.NotificationSettings)
                .HasForeignKey(x => x.UserSettingId);

            builder.HasIndex(x => x.UserSettingId);

        }
    }
}