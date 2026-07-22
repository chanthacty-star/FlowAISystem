using FlowAISystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlowAISystem.Infrastructure.Configurations;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        // Table Name
        builder.ToTable("Departments");

        // Primary Key
        builder.HasKey(d => d.Id);

        // Name
        builder.Property(d => d.Name)
               .IsRequired()
               .HasMaxLength(100);

        // Department Code
        builder.Property(d => d.Code)
               .IsRequired()
               .HasMaxLength(20);

        builder.HasIndex(d => d.Code)
               .IsUnique();

        // Description
        builder.Property(d => d.Description)
               .HasMaxLength(255);

        // Audit Fields
        builder.Property(d => d.CreatedAt)
               .IsRequired();

        builder.Property(d => d.UpdatedAt);

        // Relationships

        builder.HasMany(d => d.Teachers)
               .WithOne(t => t.Department)
               .HasForeignKey(t => t.DepartmentId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(d => d.Students)
               .WithOne(s => s.Department)
               .HasForeignKey(s => s.DepartmentId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(d => d.Subjects)
               .WithOne(s => s.Department)
               .HasForeignKey(s => s.DepartmentId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}