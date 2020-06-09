using Backend.Domain.Entities.Media;
using Backend.Domain.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Backend.Persistence.Configurations
{
    public class MediaFileConfiguration
    {
        public void Configure(EntityTypeBuilder<MediaFile> builder)
        {
            builder.Property(x => x.DateUploaded).HasDefaultValueSql("NOW()");
            builder.Property(x => x.DateModified).HasDefaultValueSql("NOW()").ValueGeneratedOnAddOrUpdate();

            builder.Property(x => x.Type)
                .HasDefaultValue(MediaType.File)
                .HasConversion(
                    v => v.ToString(),
                    v => (MediaType)Enum.Parse(typeof(MediaType), v));

            builder
                .HasOne(x => x.ApplicationUser)
                .WithMany(x => x.MediaFiles)
                .HasForeignKey(x => x.ApplicationUserId);

            builder.HasIndex(x => x.ApplicationUserId);

        }
    }
}