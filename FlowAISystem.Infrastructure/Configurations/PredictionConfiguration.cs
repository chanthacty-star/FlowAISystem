using FlowAISystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlowAISystem.Infrastructure.Configurations;

public class PredictionConfiguration : IEntityTypeConfiguration<Prediction>
{
    public void Configure(EntityTypeBuilder<Prediction> builder)
    {
        // Table Name
        builder.ToTable("Predictions");


        // Primary Key
        builder.HasKey(p => p.Id);



        // Prediction Type
        builder.Property(p => p.PredictionType)
               .IsRequired()
               .HasMaxLength(100);



        // Result
        builder.Property(p => p.Result)
               .IsRequired()
               .HasMaxLength(255);



        // Confidence Score
        builder.Property(p => p.ConfidenceScore)
               .IsRequired()
               .HasPrecision(5, 2);



        // Audit Fields
        builder.Property(p => p.CreatedAt)
               .IsRequired();

        builder.Property(p => p.UpdatedAt);



        // Relationships

        // Student 1 ---> Many Predictions
        builder.HasOne(p => p.Student)
               .WithMany(s => s.Predictions)
               .HasForeignKey(p => p.StudentId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}