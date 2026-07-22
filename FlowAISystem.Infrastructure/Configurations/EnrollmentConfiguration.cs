using FlowAISystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlowAISystem.Infrastructure.Configurations;

public class EnrollmentConfiguration
    : IEntityTypeConfiguration<Enrollment>
{
    public void Configure(
        EntityTypeBuilder<Enrollment> builder)
    {

        // Table Name
        builder.ToTable("Enrollments");



        // Primary Key
        builder.HasKey(e => e.Id);



        // Enrollment Date

        builder.Property(e => e.EnrollmentDate)
               .IsRequired()
               .HasColumnType("date");
        //Enrrol stause
        builder.Property(e => e.Status)
       .HasConversion<int>()
       .IsRequired();


        // Audit Fields

        builder.Property(e => e.CreatedAt)
               .IsRequired();


        builder.Property(e => e.UpdatedAt);



        // Student 1 ---> Many Enrollments

        builder.HasOne(e => e.Student)

               .WithMany(s => s.Enrollments)

               .HasForeignKey(e => e.StudentId)

               .OnDelete(DeleteBehavior.Restrict);




        // CourseOffering 1 ---> Many Enrollments

        builder.HasOne(e => e.CourseOffering)

               .WithMany(c => c.Enrollments)

               .HasForeignKey(e => e.CourseOfferingId)

               .OnDelete(DeleteBehavior.Restrict);




        // Enrollment 1 ---> Many Scores

        builder.HasMany(e => e.Scores)

               .WithOne(s => s.Enrollment)

               .HasForeignKey(s => s.EnrollmentId)

               .OnDelete(DeleteBehavior.Cascade);




        // Prevent duplicate enrollment
        // Same student cannot enroll twice
        // in the same class offering

        builder.HasIndex(
                e => new
                {
                    e.StudentId,
                    e.CourseOfferingId
                })

               .IsUnique();

    }
}