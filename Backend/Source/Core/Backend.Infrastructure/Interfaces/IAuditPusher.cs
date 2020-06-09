using Backend.Domain.Entities.Auditing;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Interfaces
{
    public interface IAuditPusher
    {
        Task PushToAllClients(AuditRecord audit);
    }
}
