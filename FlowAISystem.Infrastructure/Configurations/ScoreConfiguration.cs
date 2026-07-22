using FlowAISystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlowAISystem.Infrastructure.Configurations;

public class ScoreConfiguration : IEntityTypeConfiguration<Score>
{
    public void Configure(EntityTypeBuilder<Score> builder)
    {
        // Table Name
        builder.ToTable("Scores");


        // Primary Key
        builder.HasKey(s => s.Id);



        // Assessment Name
        builder.Property(s => s.AssessmentName)
               .IsRequired()
               .HasMaxLength(100);



        // Marks
        builder.Property(s => s.Marks)
               .IsRequired()
               .HasPrecision(5, 2);



        // Maximum Marks
        builder.Property(s => s.MaxMarks)
               .IsRequired()
               .HasPrecision(5, 2);



        // Assessment Date
        builder.Property(s => s.AssessmentDate)
               .IsRequired()
               .HasColumnType("date");



        // Audit Fields
        builder.Property(s => s.CreatedAt)
               .IsRequired();

        builder.Property(s => s.UpdatedAt);



        // Relationships

        // Enrollment 1 ---> Many Scores
        builder.HasOne(s => s.Enrollment)
               .WithMany(e => e.Scores)
               .HasForeignKey(s => s.EnrollmentId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}