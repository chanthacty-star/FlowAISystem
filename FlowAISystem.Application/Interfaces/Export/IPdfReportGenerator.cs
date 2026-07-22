using FlowAISystem.Shared.DTOs.Dashboard;
using FlowAISystem.Shared.DTOs.Reports;
using FlowAISystem.Shared.DTOs.Students;

namespace FlowAISystem.Application.Interfaces.Export;

public interface IPdfReportGenerator
{
    byte[] GenerateStudentReport(
        List<StudentListItemDto> students);


    byte[] GenerateAIKnowledgeReport(
        List<AIKnowledgeReportDto> knowledge);


    byte[] GenerateDashboardSummaryReport(
        DashboardSummaryDto summary);
}