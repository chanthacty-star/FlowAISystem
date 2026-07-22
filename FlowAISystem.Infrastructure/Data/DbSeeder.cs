using FlowAISystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlowAISystem.Infrastructure.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        await context.Database.MigrateAsync();

        // AI Categories
        if (!await context.AICategories.AnyAsync())
        {
            context.AICategories.AddRange(

                new AICategory
                {
                    Name = "Programming",
                    Description = "C#, .NET, Java, Python programming knowledge"
                },

                new AICategory
                {
                    Name = "Database",
                    Description = "SQL, PostgreSQL, Entity Framework knowledge"
                },

                new AICategory
                {
                    Name = "Security",
                    Description = "Authentication, JWT, authorization knowledge"
                },

                new AICategory
                {
                    Name = "AI",
                    Description = "Artificial Intelligence and machine learning knowledge"
                }

            );


            await context.SaveChangesAsync();
        }

        // Roles
        if (!await context.Roles.AnyAsync())
        {
            context.Roles.AddRange(
                new Role
                {
                    Name = "Admin",
                    Description = "System Administrator"
                },
                new Role
                {
                    Name = "Teacher",
                    Description = "Teacher User"
                },
                new Role
                {
                    Name = "Student",
                    Description = "Student User"
                });

            await context.SaveChangesAsync();
        }


        // Departments
        if (!await context.Departments.AnyAsync())
        {
            context.Departments.AddRange(
                new Department
                {
                    Name = "Software Development",
                    Code = "SD",
                    Description = "Software Development Department"
                },
                new Department
                {
                    Name = "Information Technology",
                    Code = "IT",
                    Description = "IT Department"
                });

            await context.SaveChangesAsync();
        }


        // Users
        if (!await context.Users.AnyAsync())
        {
            var adminRole = await context.Roles
                .FirstAsync(r => r.Name == "Admin");


            context.Users.Add(
                new User
                {
                    Username = "admin",
                    Email = "admin@flowai.com",
                    PasswordHash =
                        BCrypt.Net.BCrypt.HashPassword("admin123"),
                    RoleId = adminRole.Id,
                    IsActive = true
                });


            await context.SaveChangesAsync();
        }
    }
}