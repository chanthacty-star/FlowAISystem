using FlowAISystem.Application.Interfaces.Export;
using FlowAISystem.Infrastructure.Export.PDF.Documents;
using FlowAISystem.Shared.DTOs.Dashboard;
using FlowAISystem.Shared.DTOs.Reports;
using FlowAISystem.Shared.DTOs.Students;

using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace FlowAISystem.Infrastructure.Export.PDF;

public class PdfReportGenerator : IPdfReportGenerator
{
    public PdfReportGenerator()
    {
        QuestPDF.Settings.License = LicenseType.Community;
    }


    public byte[] GenerateStudentReport(
        List<StudentListItemDto> students)
    {
        return new StudentReportDocument(students)
            .GeneratePdf();
    }


    public byte[] GenerateAIKnowledgeReport(
        List<AIKnowledgeReportDto> knowledge)
    {
        return new AIKnowledgeReportDocument(knowledge)
            .GeneratePdf();
    }


    public byte[] GenerateDashboardSummaryReport(
        DashboardSummaryDto summary)
    {
        return [];
    }
}