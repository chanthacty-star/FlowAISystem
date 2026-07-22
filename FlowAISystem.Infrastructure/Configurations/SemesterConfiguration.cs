using FlowAISystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlowAISystem.Infrastructure.Configurations;

public class SemesterConfiguration : IEntityTypeConfiguration<Semester>
{
    public void Configure(EntityTypeBuilder<Semester> builder)
    {
        //====================================================
        // Table
        //====================================================

        builder.ToTable("Semesters");

        builder.HasKey(s => s.Id);

        //====================================================
        // Properties
        //====================================================
        //Name 
        builder.Property(s => s.Name)
               .IsRequired()
               .HasMaxLength(100);

        builder.HasIndex(s => s.Name);

        builder.Property(s => s.SemesterType)
               .IsRequired()
               .HasConversion<string>()
               .HasMaxLength(20);

        builder.Property(s => s.StartDate)
               .IsRequired()
               .HasColumnType("date");

        builder.Property(s => s.EndDate)
               .IsRequired()
               .HasColumnType("date");

        builder.Property(s => s.IsCurrent)
               .IsRequired()
               .HasDefaultValue(false);



        //====================================================
        // Audit Fields
        //====================================================

        builder.Property(s => s.CreatedAt)
               .IsRequired();

        builder.Property(s => s.UpdatedAt);

        //====================================================
        // Relationships
        //====================================================

        builder.HasOne(s => s.AcademicYear)
               .WithMany(a => a.Semesters)
               .HasForeignKey(s => s.AcademicYearId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(s => s.CourseOfferings)
               .WithOne(c => c.Semester)
               .HasForeignKey(c => c.SemesterId)
               .OnDelete(DeleteBehavior.Restrict);


    }
}