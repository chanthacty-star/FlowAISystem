using FlowAISystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlowAISystem.Infrastructure.Configurations;

public class TrainingDataConfiguration : IEntityTypeConfiguration<TrainingData>
{
    public void Configure(EntityTypeBuilder<TrainingData> builder)
    {
        // Table Name
        builder.ToTable("TrainingData");


        // Primary Key
        builder.HasKey(t => t.Id);



        // Input Data
        builder.Property(t => t.InputData)
               .IsRequired()
               .HasMaxLength(2000);



        // Expected Output
        builder.Property(t => t.ExpectedOutput)
               .IsRequired()
               .HasMaxLength(1000);



        // Label
        builder.Property(t => t.Label)
               .IsRequired()
               .HasMaxLength(100);



        // Training Status
        builder.Property(t => t.IsUsedForTraining)
               .IsRequired();



        // Audit Fields
        builder.Property(t => t.CreatedAt)
               .IsRequired();

        builder.Property(t => t.UpdatedAt);
    }
}