using Backend.Domain.Entities.Contacts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Persistence.Configurations
{
    public class PhoneNumberConfiguration : IEntityTypeConfiguration<PhoneNumber>
    {
        public void Configure(EntityTypeBuilder<PhoneNumber> builder)
        {
            builder.Property(x => x.Number).HasMaxLength(50);

            builder
                .HasOne(x => x.Contact)
                .WithMany(x => x.PhoneNumbers)
                .HasForeignKey(x => x.ContactId);
        }
    }
}