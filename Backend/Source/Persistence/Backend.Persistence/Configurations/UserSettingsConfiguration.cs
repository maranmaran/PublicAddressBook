using Backend.Domain.Entities.User;
using Backend.Domain.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Backend.Persistence.Configurations
{
    public class UserSettingsConfiguration : IEntityTypeConfiguration<UserSetting>
    {
        public void Configure(EntityTypeBuilder<UserSetting> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Language).HasMaxLength(2);
            builder.Property(x => x.Language).HasDefaultValue("en");

            builder.Property(x => x.Theme)
                .HasDefaultValue(Themes.Light)
                .HasConversion(
                    v => ((Themes)v).ToString(),
                    v => (Themes)Enum.Parse(typeof(Themes), v));

            builder
                .HasMany(x => x.NotificationSettings)
                .WithOne(x => x.UserSetting)
                .HasForeignKey(x => x.UserSettingId);

            builder.HasOne(x => x.ApplicationUser).WithOne(x => x.UserSetting)
                .HasForeignKey<ApplicationUser>(x => x.UserSettingId);

            builder.HasIndex(x => x.ApplicationUserId);

        }
    }
}