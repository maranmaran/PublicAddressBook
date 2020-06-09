using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SystemLog = Backend.Domain.Entities.System.SystemLog;

namespace Backend.Persistence.Configurations
{
    public class SystemLogConfiguration : IEntityTypeConfiguration<SystemLog>
    {
        public void Configure(EntityTypeBuilder<SystemLog> builder)
        {
            builder.Property(x => x.Date).HasDefaultValueSql("NOW()");


        }
    }
}