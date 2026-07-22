using FlowAISystem.Shared.DTOs.Charts;
using FlowAISystem.Shared.DTOs.Dashboard;

namespace FlowAISystem.Application.Interfaces.Services;

public interface IAdminDashboardService
{
    // ===============================
    // Summary
    // ===============================

    Task<DashboardSummaryDto> GetDashboardSummaryAsync();


    // ===============================
    // Statistics
    // ===============================

    Task<int> GetStudentCountAsync();

    Task<int> GetTeacherCountAsync();

    Task<int> GetDepartmentCountAsync();

    Task<int> GetAIKnowledgeCountAsync();


    // ===============================
    // Recent Data
    // ===============================

    Task<List<RecentStudentDto>> GetRecentStudentsAsync();

    Task<List<RecentAIKnowledgeDto>> GetRecentAIKnowledgeAsync();

    Task<List<RecentActivityDto>> GetRecentActivitiesAsync();


    // ===============================
    // Charts
    // ===============================

    Task<List<DepartmentStudentChartDto>> GetDepartmentStudentChartAsync();

    Task<List<AIKnowledgeCategoryChartDto>> GetAIKnowledgeCategoryChartAsync();
}
