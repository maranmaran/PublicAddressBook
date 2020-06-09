using Backend.Domain.Entities.Auditing;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Interfaces
{
    public interface IActivityService
    {
        Task<string> GetEntityAsJson(AuditRecord audit);
    }
}