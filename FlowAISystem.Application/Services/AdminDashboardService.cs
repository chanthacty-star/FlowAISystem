using FlowAISystem.Application.Interfaces.Repositories;
using FlowAISystem.Application.Interfaces.Services;
using FlowAISystem.Shared.DTOs.Charts;
using FlowAISystem.Shared.DTOs.Dashboard;

namespace FlowAISystem.Application.Services;

public class AdminDashboardService : IAdminDashboardService
{
    private readonly IAdminDashboardRepository _repository;


    public AdminDashboardService(
        IAdminDashboardRepository repository)
    {
        _repository = repository;
    }


    // Summary
    public async Task<DashboardSummaryDto> GetDashboardSummaryAsync()
    {
        return await _repository.GetDashboardSummaryAsync();
    }


    // Statistics
    public async Task<int> GetStudentCountAsync()
    {
        return await _repository.GetStudentCountAsync();
    }


    public async Task<int> GetTeacherCountAsync()
    {
        return await _repository.GetTeacherCountAsync();
    }


    public async Task<int> GetDepartmentCountAsync()
    {
        return await _repository.GetDepartmentCountAsync();
    }


    public async Task<int> GetAIKnowledgeCountAsync()
    {
        return await _repository.GetAIKnowledgeCountAsync();
    }


    // Recent Data

    public async Task<List<RecentStudentDto>> GetRecentStudentsAsync()
    {
        return await _repository.GetRecentStudentsAsync();
    }


    public async Task<List<RecentAIKnowledgeDto>> GetRecentAIKnowledgeAsync()
    {
        return await _repository.GetRecentAIKnowledgeAsync();
    }


    public async Task<List<RecentActivityDto>> GetRecentActivitiesAsync()
    {
        return await _repository.GetRecentActivitiesAsync();
    }


    // Charts

    public async Task<List<DepartmentStudentChartDto>> GetDepartmentStudentChartAsync()
    {
        return await _repository.GetDepartmentStudentChartAsync();
    }


    public async Task<List<AIKnowledgeCategoryChartDto>> GetAIKnowledgeCategoryChartAsync()
    {
        return await _repository.GetAIKnowledgeCategoryChartAsync();
    }
}

