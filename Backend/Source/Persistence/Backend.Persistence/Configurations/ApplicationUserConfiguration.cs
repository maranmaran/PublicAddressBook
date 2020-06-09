using Backend.Domain.Entities.User;
using Backend.Domain.Enum;
using Backend.Persistence.ValueGenerators;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Backend.Persistence.Configurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(x => x.CreatedOn).HasDefaultValueSql("NOW()");
            builder.Property(x => x.LastModified).HasDefaultValueSql("NOW()").ValueGeneratedOnAddOrUpdate();
            builder.Property(x => x.Active).HasDefaultValue(true);
            builder.Property(x => x.Avatar).HasValueGenerator<AvatarGenerator>();
            builder.Ignore(x => x.FullName);

            builder
                .HasDiscriminator(x => x.AccountType)
                .HasValue<ApplicationUser>(AccountType.User);

            builder.Property(x => x.AccountType)
                .HasConversion(
                    v => v.ToString(),
                    v => (AccountType)Enum.Parse(typeof(AccountType), v));

            builder.Property(x => x.Gender)
                .HasDefaultValue(Gender.Male)
                .HasConversion(
                    v => v.ToString(),
                    v => (Gender)Enum.Parse(typeof(Gender), v));

            builder
                .HasMany(x => x.MediaFiles)
                .WithOne(x => x.ApplicationUser)
                .HasForeignKey(x => x.ApplicationUserId);


            builder
                .HasMany(x => x.ReceivedNotifications)
                .WithOne(x => x.Receiver)
                .HasForeignKey(x => x.ReceiverId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(x => x.ReceivedNotifications)
                .WithOne(x => x.Receiver)
                .HasForeignKey(x => x.ReceiverId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(x => x.SentNotifications)
                .WithOne(x => x.Sender)
                .HasForeignKey(x => x.SenderId)
                .OnDelete(DeleteBehavior.Cascade);


            builder
                .HasOne(x => x.UserSetting)
                .WithOne(x => x.ApplicationUser)
                .HasForeignKey<UserSetting>(x => x.ApplicationUserId);
        }
    }
}