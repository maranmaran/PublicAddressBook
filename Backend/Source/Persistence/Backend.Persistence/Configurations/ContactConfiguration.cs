using Backend.Domain.Entities.Contacts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Persistence.Configurations
{
    public class ContactConfiguration : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            // props
            builder.Property(x => x.Name).HasMaxLength(200);
            builder.Property(x => x.Address).HasMaxLength(100);

            // indexes
            builder.HasIndex(x => new { x.Name, x.Address }).IsUnique();

            // relationships
            builder
                .HasMany(x => x.PhoneNumbers)
                .WithOne(x => x.Contact)
                .HasForeignKey(x => x.ContactId);

        }
    }
}
