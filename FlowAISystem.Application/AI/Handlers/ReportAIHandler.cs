using FlowAISystem.Application.AI.Interfaces;
using FlowAISystem.Application.Interfaces.Services.Reports;

namespace FlowAISystem.Application.AI.Handlers;

public class ReportAIHandler : IReportAIHandler
{
    private readonly IReportService _reportService;


    public ReportAIHandler(
        IReportService reportService)
    {
        _reportService = reportService;
    }



    public async Task<string> HandleAsync(
        string question)
    {
        question =
            question.ToLower();



        if (question.Contains("student")
            &&
           question.Contains("pdf"))
        {
            var pdf =
                await _reportService
                    .ExportStudentsPdfAsync();


            return
                $"Student PDF report generated successfully. Size: {pdf.Length} bytes.";
        }



        if (question.Contains("student")
            &&
           question.Contains("excel"))
        {
            var excel =
                await _reportService
                    .ExportStudentsExcelAsync();


            return
                $"Student Excel report generated successfully. Size: {excel.Length} bytes.";
        }



        return
            "I can generate student PDF or Excel reports.";
    }
}