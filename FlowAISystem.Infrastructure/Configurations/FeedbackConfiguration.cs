using FlowAISystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlowAISystem.Infrastructure.Data.Configurations;

public class FeedbackConfiguration
    : IEntityTypeConfiguration<Feedback>
{

    public void Configure(
        EntityTypeBuilder<Feedback> builder)
    {

        builder.HasKey(f => f.Id);



        builder.Property(f => f.Rating)
            .IsRequired();



        builder.Property(f => f.Comment)
            .HasMaxLength(1000)
            .IsRequired();



        builder.Property(f => f.FeedbackDate)
            .IsRequired();




        // Enrollment relationship

        builder.HasOne(f => f.Enrollment)
            .WithMany()
            .HasForeignKey(f => f.EnrollmentId)
            .OnDelete(DeleteBehavior.Cascade);

    }

}