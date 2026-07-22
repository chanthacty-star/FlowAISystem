using FlowAISystem.Shared.DTOs.Charts;
using FlowAISystem.Shared.DTOs.Dashboard;

namespace FlowAISystem.Application.Interfaces.Repositories;

public interface IAdminDashboardRepository
{
    // Summary
    Task<DashboardSummaryDto> GetDashboardSummaryAsync();

    // Statistics
    Task<int> GetStudentCountAsync();
    Task<int> GetTeacherCountAsync();
    Task<int> GetDepartmentCountAsync();
    Task<int> GetAIKnowledgeCountAsync();

    // Recent Data
    Task<List<RecentStudentDto>> GetRecentStudentsAsync();
    Task<List<RecentAIKnowledgeDto>> GetRecentAIKnowledgeAsync();
    Task<List<RecentActivityDto>> GetRecentActivitiesAsync();

    // Analytics (Charts)
    Task<List<DepartmentStudentChartDto>> GetDepartmentStudentChartAsync();

    Task<List<AIKnowledgeCategoryChartDto>> GetAIKnowledgeCategoryChartAsync();
}