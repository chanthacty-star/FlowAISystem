using FlowAISystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlowAISystem.Infrastructure.Configurations;

public class AnnouncementConfiguration
    : IEntityTypeConfiguration<Announcement>
{

    public void Configure(
        EntityTypeBuilder<Announcement> builder)
    {

        // Table Name

        builder.ToTable("Announcements");



        // Primary Key

        builder.HasKey(a => a.Id);





        // Title

        builder.Property(a => a.Title)
            .IsRequired()
            .HasMaxLength(200);





        // Content

        builder.Property(a => a.Content)
            .IsRequired();





        // Publish Date

        builder.Property(a => a.PublishDate)
            .IsRequired();





        // Expire Date

        builder.Property(a => a.ExpireDate);





        // Audience Enum

        builder.Property(a => a.Audience)
            .IsRequired()
            .HasConversion<int>();






        // Relationship:
        //
        // User (1)
        //    |
        //    |
        // Announcement (Many)
        //
        // CreatedByUserId FK


        builder.HasOne(a => a.CreatedByUser)
            .WithMany(u => u.Announcements)
            .HasForeignKey(a => a.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);






        // Audit Fields from BaseEntity

        builder.Property(a => a.CreatedAt)
            .IsRequired();


        builder.Property(a => a.UpdatedAt);

    }
}