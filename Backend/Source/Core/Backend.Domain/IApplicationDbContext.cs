using Backend.Domain.Entities.Auditing;
using Backend.Domain.Entities.Contacts;
using Backend.Domain.Entities.Media;
using Backend.Domain.Entities.Notification;
using Backend.Domain.Entities.System;
using Backend.Domain.Entities.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Threading;
using System.Threading.Tasks;

namespace Backend.Domain
{
    public interface IApplicationDbContext
    {
        DbSet<ApplicationUser> Users { get; set; }

        DbSet<UserSetting> UserSettings { get; set; }

        DbSet<NotificationSetting> NotificationSetting { get; set; }

        DbSet<Notification> Notifications { get; set; }

        DbSet<MediaFile> MediaFiles { get; set; }

        DbSet<SystemLog> SystemLog { get; set; }

        DbSet<AuditRecord> Audits { get; set; }
        DbSet<Contact> Contacts { get; set; }
        DbSet<PhoneNumber> PhoneNumbers { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
    }
}
