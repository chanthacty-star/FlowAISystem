using FlowAISystem.Application.Interfaces.Repositories;
using FlowAISystem.Infrastructure.Data;
using FlowAISystem.Shared.DTOs.Charts;
using FlowAISystem.Shared.DTOs.Dashboard;
using Microsoft.EntityFrameworkCore;

namespace FlowAISystem.Infrastructure.Repositories;

public class AdminDashboardRepository : IAdminDashboardRepository
{
    private readonly IDbContextFactory<AppDbContext> _factory;

    public AdminDashboardRepository(
        IDbContextFactory<AppDbContext> factory)
    {
        _factory = factory;
    }

    public async Task<int> GetStudentCountAsync()
    {
        await using var context =
            await _factory.CreateDbContextAsync();

        return await context.Students.CountAsync();
    }

    public async Task<int> GetTeacherCountAsync()
    {
        await using var context =
            await _factory.CreateDbContextAsync();

        return await context.Teachers.CountAsync();
    }

    public async Task<int> GetDepartmentCountAsync()
    {
        await using var context =
            await _factory.CreateDbContextAsync();

        return await context.Departments.CountAsync();
    }

    public async Task<int> GetAIKnowledgeCountAsync()
    {
        await using var context =
            await _factory.CreateDbContextAsync();

        return await context.AIKnowledge.CountAsync();
    }

    public async Task<List<RecentStudentDto>> GetRecentStudentsAsync()
    {
        await using var context =
            await _factory.CreateDbContextAsync();

        return await context.Students
            .AsNoTracking()
            .Include(s => s.Department)
            .OrderByDescending(s => s.CreatedAt)
            .Take(5)
            .Select(s => new RecentStudentDto
            {
                FullName = s.Name.FullName,
                Department = s.Department != null
                    ? s.Department.Name
                    : "N/A",
                CreatedAt = s.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<List<RecentAIKnowledgeDto>> GetRecentAIKnowledgeAsync()
    {
        await using var context =
            await _factory.CreateDbContextAsync();

        return await context.AIKnowledge
            .AsNoTracking()
            .OrderByDescending(a => a.CreatedAt)
            .Take(5)
            .Select(a => new RecentAIKnowledgeDto
            {
                Question = a.Question,
                CreatedAt = a.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<List<RecentActivityDto>> GetRecentActivitiesAsync()
    {
        await using var context =
            await _factory.CreateDbContextAsync();

        return await context.Students
            .AsNoTracking()
            .OrderByDescending(s => s.CreatedAt)
            .Take(5)
            .Select(s => new RecentActivityDto
            {
                Activity = $"Student {s.Name.FullName} registered",
                CreatedAt = s.CreatedAt
            })
            .ToListAsync();
    }
    // emplement 

    public async Task<DashboardSummaryDto> GetDashboardSummaryAsync()
    {
        await using var context =
            await _factory.CreateDbContextAsync();

        return new DashboardSummaryDto
        {
            TotalStudents = await context.Students.CountAsync(),
            TotalTeachers = await context.Teachers.CountAsync(),
            TotalDepartments = await context.Departments.CountAsync(),
            TotalSubjects = await context.Subjects.CountAsync(),
            TotalAIKnowledge = await context.AIKnowledge.CountAsync(),
            TotalUsers = await context.Users.CountAsync()
        };
    }
    // =======================================
    // Charts
    // =======================================

    public async Task<List<DepartmentStudentChartDto>> GetDepartmentStudentChartAsync()
    {
        await using var context =
            await _factory.CreateDbContextAsync();

        return await context.Departments
            .AsNoTracking()
            .Select(d => new DepartmentStudentChartDto
            {
                DepartmentName = d.Name,
                StudentCount = d.Students.Count
            })
            .OrderByDescending(d => d.StudentCount)
            .ToListAsync();
    }

    public async Task<List<AIKnowledgeCategoryChartDto>> GetAIKnowledgeCategoryChartAsync()
    {
        await using var context =
            await _factory.CreateDbContextAsync();

        return await context.AIKnowledge
            .AsNoTracking()
            .GroupBy(a => a.Category.Name)
            .Select(g => new AIKnowledgeCategoryChartDto
            {
                CategoryName = g.Key,
                KnowledgeCount = g.Count()
            })
            .OrderByDescending(g => g.KnowledgeCount)
            .ToListAsync();
    }
}
