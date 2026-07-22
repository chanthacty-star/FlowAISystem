using FlowAISystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlowAISystem.Infrastructure.Configurations;

public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
{
    public void Configure(EntityTypeBuilder<Teacher> builder)
    {
        // Table Name
        builder.ToTable("Teachers");


        // Primary Key
        builder.HasKey(t => t.Id);



        // Teacher Name
        builder.Property(t => t.TeacherName)
               .IsRequired()
               .HasMaxLength(100);



        // Value Object: PersonName
        builder.OwnsOne(t => t.Name, name =>
        {
            name.Property(n => n.FirstName)
                .HasColumnName("FirstName")
                .IsRequired()
                .HasMaxLength(50);


            name.Property(n => n.LastName)
                .HasColumnName("LastName")
                .IsRequired()
                .HasMaxLength(50);
        });



        // Email
        builder.Property(t => t.Email)
               .IsRequired()
               .HasMaxLength(150);


        builder.HasIndex(t => t.Email)
               .IsUnique();



        // Phone Number
        builder.Property(t => t.PhoneNumber)
               .IsRequired()
               .HasMaxLength(20);



        // Gender Enum
        builder.Property(t => t.Gender)
               .IsRequired()
               .HasConversion<string>()
               .HasMaxLength(20);



        // Date Of Birth
        builder.Property(t => t.DateOfBirth)
               .IsRequired()
               .HasColumnType("date");



        // Hire Date
        builder.Property(t => t.HireDate)
               .IsRequired()
               .HasColumnType("date");



        // Audit Fields
        builder.Property(t => t.CreatedAt)
               .IsRequired();

        builder.Property(t => t.UpdatedAt);



        // Relationships


        // User 1 ---> 0..1 Teacher

        builder
            .HasOne(t => t.User)
            .WithOne()
            .HasForeignKey<Teacher>(t => t.UserId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);




        // Department 1 ---> Many Teachers

        builder.HasOne(t => t.Department)
               .WithMany(d => d.Teachers)
               .HasForeignKey(t => t.DepartmentId)
               .OnDelete(DeleteBehavior.Restrict);




        // Teacher 1 ---> Many CourseOfferings

        builder.HasMany(t => t.CourseOfferings)
               .WithOne(c => c.Teacher)
               .HasForeignKey(c => c.TeacherId)
               .OnDelete(DeleteBehavior.Restrict);

    }
}