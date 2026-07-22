using FlowAISystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlowAISystem.Infrastructure.Configurations;

public class CourseOfferingConfiguration
    : IEntityTypeConfiguration<CourseOffering>
{
    public void Configure(
        EntityTypeBuilder<CourseOffering> builder)
    {

        builder.ToTable("CourseOfferings");


        builder.HasKey(c => c.Id);



        builder.Property(c => c.ClassName)
               .IsRequired()
               .HasMaxLength(50);



        builder.Property(c => c.CreatedAt)
               .IsRequired();



        builder.Property(c => c.UpdatedAt);



        // Subject 1 ---> Many CourseOfferings

        builder.HasOne(c => c.Subject)
               .WithMany(s => s.CourseOfferings)
               .HasForeignKey(c => c.SubjectId)
               .OnDelete(DeleteBehavior.Restrict);



        // Teacher 1 ---> Many CourseOfferings

        builder.HasOne(c => c.Teacher)
               .WithMany(t => t.CourseOfferings)
               .HasForeignKey(c => c.TeacherId)
               .OnDelete(DeleteBehavior.Restrict);



        // Semester 1 ---> Many CourseOfferings

        builder.HasOne(c => c.Semester)
               .WithMany(s => s.CourseOfferings)
               .HasForeignKey(c => c.SemesterId)
               .OnDelete(DeleteBehavior.Restrict);



        // CourseOffering 1 ---> Many Enrollments

        builder.HasMany(c => c.Enrollments)
               .WithOne(e => e.CourseOffering)
               .HasForeignKey(e => e.CourseOfferingId)
               .OnDelete(DeleteBehavior.Restrict);



        // Prevent duplicate class in same semester

        builder.HasIndex(c => new
        {
            c.ClassName,
            c.SubjectId,
            c.SemesterId
        })
        .IsUnique();

    }
}