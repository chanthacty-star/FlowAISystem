using FlowAISystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlowAISystem.Infrastructure.Configurations;

public class SubjectConfiguration : IEntityTypeConfiguration<Subject>
{
    public void Configure(EntityTypeBuilder<Subject> builder)
    {
        // Table Name
        builder.ToTable("Subjects");


        // Primary Key
        builder.HasKey(s => s.Id);



        // Subject Code
        builder.Property(s => s.Code)
               .IsRequired()
               .HasMaxLength(20);


        builder.HasIndex(s => s.Code)
               .IsUnique();



        // Subject Name
        builder.Property(s => s.Name)
               .IsRequired()
               .HasMaxLength(100);

        builder.HasIndex(s => s.Name);// seach 

        // Credits
        builder.Property(s => s.Credits)
               .IsRequired();



        // Audit Fields
        builder.Property(s => s.CreatedAt)
               .IsRequired();

        builder.Property(s => s.UpdatedAt);



        // Relationships


        // Department 1 ---> Many Subjects

        builder.HasOne(s => s.Department)
               .WithMany(d => d.Subjects)
               .HasForeignKey(s => s.DepartmentId)
               .OnDelete(DeleteBehavior.Restrict);



        // Subject 1 ---> Many CourseOfferings

        builder.HasMany(s => s.CourseOfferings)
               .WithOne(c => c.Subject)
               .HasForeignKey(c => c.SubjectId)
               .OnDelete(DeleteBehavior.Restrict);





    }
}