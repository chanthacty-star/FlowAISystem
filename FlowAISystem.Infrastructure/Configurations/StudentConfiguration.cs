using FlowAISystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlowAISystem.Infrastructure.Configurations;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        // Table Name
        builder.ToTable("Students");


        // Primary Key
        builder.HasKey(s => s.Id);



        // Student Number
        builder.Property(s => s.StudentNumber)
               .IsRequired()
               .HasMaxLength(20);


        builder.HasIndex(s => s.StudentNumber)
               .IsUnique();



        // Value Object: PersonName
        builder.OwnsOne(s => s.Name, name =>
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
        builder.Property(s => s.Email)
               .IsRequired()
               .HasMaxLength(150);


        builder.HasIndex(s => s.Email)
               .IsUnique();



        // Phone Number
        builder.Property(s => s.PhoneNumber)
               .IsRequired()
               .HasMaxLength(20);



        // Gender Enum
        builder.Property(s => s.Gender)
               .IsRequired()
               .HasConversion<string>()
               .HasMaxLength(20);



        // Date Of Birth
        builder.Property(s => s.DateOfBirth)
               .IsRequired()
               .HasColumnType("date");



        // Enrollment Date
        builder.Property(s => s.EnrollmentDate)
               .IsRequired()
               .HasColumnType("date");



        // Audit Fields
        builder.Property(s => s.CreatedAt)
               .IsRequired();

        builder.Property(s => s.UpdatedAt);



        // Relationships
        builder
            .HasOne(s => s.User)
            .WithOne()
            .HasForeignKey<Student>(s => s.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Department 1 ---> Many Students
        builder.HasOne(s => s.Department)
               .WithMany(d => d.Students)
               .HasForeignKey(s => s.DepartmentId)
               .OnDelete(DeleteBehavior.Restrict);



        // Student 1 ---> Many Enrollments
        builder.HasMany(s => s.Enrollments)
               .WithOne(e => e.Student)
               .HasForeignKey(e => e.StudentId)
               .OnDelete(DeleteBehavior.Restrict);


        // Student 1 ---> Many Predictions
        builder.HasMany(s => s.Predictions)
               .WithOne(p => p.Student)
               .HasForeignKey(p => p.StudentId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}