using Audit.Core;
using Audit.EntityFramework;
using Backend.Business.Notifications;
using Backend.Domain;
using Backend.Domain.Entities.Auditing;
using Backend.Domain.Entities.Contacts;
using Backend.Domain.Entities.Media;
using Backend.Library.Logging.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Configuration = Audit.Core.Configuration;

namespace Backend.Persistence.Extensions
{
    public static partial class ServiceCollectionExtensions
    {

        //TODO Move all this to Dashboard project..because thats BoundedContext for FEED
        public static void ConfigureAuditEfCore(this IServiceProvider provider)
        {
            Audit.EntityFramework.Configuration.Setup()
                .ForContext<ApplicationDbContext>(config => config
                    .IncludeEntityObjects()
                    .AuditEventType("{context}:{database}"))
                .UseOptIn()
                .Include<MediaFile>()
                .Include<Contact>();

            // Audit.EntityFrameworkCore offers more granulated approach
            // Special mappings and one table per entity.. Audit_TrainingLog
            // Or putting all eggs in one basket
            // For now we'll put all eggs in one basket.. (for feed)
            Configuration.AddCustomAction(ActionType.OnScopeCreated, scope =>
            {
                using (var serviceScope = provider.CreateScope())
                {
                    //var serviceProvider = services.BuildServiceProvider();
                    var ctxAccessor = serviceScope.ServiceProvider.GetService<IHttpContextAccessor>();
                    var userId = ctxAccessor?.HttpContext?.User?.Identity?.Name;

                    scope.SetCustomField("UserId", userId);
                }
            });

            Configuration.Setup()
                .UseEntityFramework(_ => _
                    .AuditTypeMapper(t => typeof(AuditRecord))
                    .AuditEntityAction<AuditRecord>(async (ev, entry, audit) =>
                    {
                        using (var scope = provider.CreateScope())
                        {
                            MapToAudit(ev, entry, audit);
                            var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
                            var logger = scope.ServiceProvider.GetRequiredService<ILoggingService>();

                            var user = await context.Users.FirstOrDefaultAsync(x => x.Id == audit.UserId);
                            if (user == null)
                            {
                                await logger.LogWarning($"User not found to send audit information to. UserId: {audit.UserId}");
                                return;
                            }

                            var notificationAuditCoordinator = new NotificationsAuditPusher(scope.ServiceProvider);

                            try
                            {
                                await notificationAuditCoordinator.PushToAllClients(audit);
                            }
                            catch (Exception e)
                            {
                                await logger.LogError(e);
                            }
                        }
                    })
                    .IgnoreMatchedProperties(true));
        }

        /// <summary>
        /// Maps audit event and entry to AuditRecord entity for DB
        /// </summary>
        private static void MapToAudit(AuditEvent ev, EventEntry entry, AuditRecord audit)
        {
            Guid.TryParse(ev.CustomFields["UserId"].ToString(), out var userId);
            audit.UserId = userId;
            audit.Data = entry.ToJson();
            audit.EntityType = entry.EntityType.Name;
            audit.Date = DateTime.UtcNow;
            audit.PrimaryKey = entry.PrimaryKey.First().Value.ToString();
            audit.Table = entry.Table;
            audit.Action = entry.Action;
            audit.Date = DateTime.UtcNow;
        }
    }
}
