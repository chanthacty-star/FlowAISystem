using FlowAISystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlowAISystem.Infrastructure.Configurations;

public class AcademicYearConfiguration : IEntityTypeConfiguration<AcademicYear>
{
    public void Configure(EntityTypeBuilder<AcademicYear> builder)
    {
        // Table Name
        builder.ToTable("AcademicYears");


        // Primary Key
        builder.HasKey(a => a.Id);


        // Name
        builder.Property(a => a.Name)
               .IsRequired()
               .HasMaxLength(50);


        // Start Date
        builder.Property(a => a.StartDate)
               .IsRequired()
               .HasColumnType("date");


        // End Date
        builder.Property(a => a.EndDate)
               .IsRequired()
               .HasColumnType("date");


        // Current Academic Year
        builder.Property(a => a.IsCurrent)
               .IsRequired();


        // Audit Fields from BaseEntity
        builder.Property(a => a.CreatedAt)
               .IsRequired();

        builder.Property(a => a.UpdatedAt);



        // Relationships

        builder.HasMany(a => a.Semesters)
               .WithOne(s => s.AcademicYear)
               .HasForeignKey(s => s.AcademicYearId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}