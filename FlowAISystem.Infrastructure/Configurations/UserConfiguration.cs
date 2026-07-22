using FlowAISystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlowAISystem.Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Table Name
        builder.ToTable("Users");

        // Primary Key
        builder.HasKey(u => u.Id);

        // Username
        builder.Property(u => u.Username)
               .IsRequired()
               .HasMaxLength(50);

        builder.HasIndex(u => u.Username)
               .IsUnique();

        // Email
        builder.Property(u => u.Email)
               .IsRequired()
               .HasMaxLength(100);

        builder.HasIndex(u => u.Email)
               .IsUnique();

        // Password Hash
        builder.Property(u => u.PasswordHash)
               .IsRequired()
               .HasMaxLength(255);

        // IsActive
        builder.Property(u => u.IsActive)
               .HasDefaultValue(true);

        // CreatedAt
        builder.Property(u => u.CreatedAt)
               .IsRequired();

        // UpdatedAt
        builder.Property(u => u.UpdatedAt);

        // Relationship
        builder.HasOne(u => u.Role)
               .WithMany(r => r.Users)
               .HasForeignKey(u => u.RoleId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}