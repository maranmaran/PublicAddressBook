using Audit.EntityFramework;
using Backend.Domain;
using Backend.Domain.Entities.Auditing;
using Backend.Domain.Entities.Contacts;
using Backend.Domain.Entities.Media;
using Backend.Domain.Entities.Notification;
using Backend.Domain.Entities.System;
using Backend.Domain.Entities.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Backend.Persistence
{
    public class ApplicationDbContext : AuditDbContext, IApplicationDbContext
    {
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<UserSetting> UserSettings { get; set; }
        public DbSet<NotificationSetting> NotificationSetting { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<MediaFile> MediaFiles { get; set; }
        public DbSet<SystemLog> SystemLog { get; set; }
        public DbSet<AuditRecord> Audits { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<PhoneNumber> PhoneNumbers { get; set; }

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // comment this if you don't want to see seed values in migrations
            //modelBuilder.Seed();

            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }

    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContextFactory()
        {
        }

        //TODO: Revisit this hardcoded conn string
        // Used only for EF.NET Core CLI tools(update database / migrations etc.)
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseNpgsql("Host=localhost;Username=postgres;Password=admin;Database=public_address_book;Pooling=true;");

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}