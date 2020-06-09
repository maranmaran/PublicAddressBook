using Backend.Domain.Entities.Auditing;
using Backend.Domain.Entities.User;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Interfaces
{
    public interface IAuditPusher
    {
        Task PushToUser(AuditRecord audit, ApplicationUser user);
        Task PushToAllClients(AuditRecord audit);
    }
}
