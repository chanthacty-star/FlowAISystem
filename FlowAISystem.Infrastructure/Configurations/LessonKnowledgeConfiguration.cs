using FlowAISystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FlowAISystem.Shared.Enums;

namespace FlowAISystem.Infrastructure.Persistence.Configurations;

public class LessonKnowledgeConfiguration
    : IEntityTypeConfiguration<LessonKnowledge>
{
    public void Configure(
        EntityTypeBuilder<LessonKnowledge> builder)
    {
        // ==================================================
        // Table
        // ==================================================

        builder.ToTable("LessonKnowledges");


        // ==================================================
        // Primary Key
        // ==================================================

        builder.HasKey(x => x.Id);


        // ==================================================
        // Base Entity
        // ==================================================

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .IsRequired(false);


        // ==================================================
        // Lesson Information
        // ==================================================

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(x => x.Content)
            .IsRequired();


        // ==================================================
        // AI Knowledge
        // ==================================================

        builder.Property(x => x.Keywords)
            .HasMaxLength(1000);

        builder.Property(x => x.Category)
            .HasMaxLength(200);

        builder.Property(x => x.Difficulty)
            .HasConversion<int>()
            .HasDefaultValue(LessonDifficulty.Beginner)
            .IsRequired();

        builder.Property(x => x.ActivityType)
            .HasMaxLength(50)
            .IsRequired(false);
        //Lesson Order
        builder.Property(x => x.Order)
            .IsRequired()
            .HasDefaultValue(0);

        // ==================================================
        // Teacher Relationship
        // ==================================================

        builder.HasOne(x => x.Teacher)
            .WithMany()
            .HasForeignKey(x => x.TeacherId)
            .OnDelete(DeleteBehavior.Restrict);


        // ==================================================
        // Course Relation
        // ==================================================

        builder.HasOne(x => x.CourseOffering)
            .WithMany()
            .HasForeignKey(x => x.CourseOfferingId)
            .OnDelete(DeleteBehavior.SetNull);


        // ==================================================
        // Learning Material
        // ==================================================

        builder.Property(x => x.ReferenceUrl)
            .HasMaxLength(1000);

        builder.Property(x => x.AttachmentPath)
            .HasMaxLength(1000);


        // ==================================================
        // Status
        // ==================================================

        builder.Property(x => x.IsActive)
            .IsRequired()
            .HasDefaultValue(true);
    }
}