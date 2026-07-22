using FlowAISystem.Application.AI.Interfaces;
using FlowAISystem.Application.Interfaces.Services;

namespace FlowAISystem.Application.AI.Handlers;

public class DashboardAIHandler : IDashboardAIHandler
{
    private readonly IAdminDashboardService _dashboardService;


    public DashboardAIHandler(
        IAdminDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }



    public async Task<string> HandleAsync(
        string question)
    {
        question =
            question.ToLower();



        // ======================================
        // Student Count
        // ======================================

        if (question.Contains("student"))
        {
            var count =
                await _dashboardService
                    .GetStudentCountAsync();


            return
                $"There are currently {count} students in FlowAISystem.";
        }



        // ======================================
        // Teacher Count
        // ======================================

        if (question.Contains("teacher"))
        {
            var count =
                await _dashboardService
                    .GetTeacherCountAsync();


            return
                $"There are currently {count} teachers in FlowAISystem.";
        }



        // ======================================
        // Department Count
        // ======================================

        if (question.Contains("department"))
        {
            var count =
                await _dashboardService
                    .GetDepartmentCountAsync();


            return
                $"There are currently {count} departments.";
        }



        // ======================================
        // AI Knowledge Count
        // ======================================

        if (question.Contains("knowledge"))
        {
            var count =
                await _dashboardService
                    .GetAIKnowledgeCountAsync();


            return
                $"There are currently {count} AI knowledge records.";
        }



        // ======================================
        // Full Dashboard Summary
        // ======================================

        if (question.Contains("dashboard")
            ||
            question.Contains("summary"))
        {
            var summary =
                await _dashboardService
                    .GetDashboardSummaryAsync();


            return
$"""
FlowAISystem Dashboard Summary

Students       : {summary.TotalStudents}
Teachers       : {summary.TotalTeachers}
Departments    : {summary.TotalDepartments}
Subjects       : {summary.TotalSubjects}
AI Knowledge   : {summary.TotalAIKnowledge}
Users          : {summary.TotalUsers}
""";
        }



        return
            "I can provide dashboard information about students, teachers, departments, users, and AI knowledge.";
    }
}