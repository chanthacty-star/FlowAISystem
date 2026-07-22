using FlowAISystem.Application.Interfaces.Export;
using FlowAISystem.Application.Interfaces.Repositories;
using FlowAISystem.Application.Interfaces.Services.Reports;
using FlowAISystem.Shared.DTOs.Reports;


namespace FlowAISystem.Application.Services.Reports;


public class ReportService : IReportService
{

    private readonly IStudentRepository _studentRepository;

    private readonly IAIKnowledgeRepository _aiKnowledgeRepository;

    private readonly IAdminDashboardRepository _dashboardRepository;

    private readonly IReportRepository _reportRepository;


    private readonly IPdfReportGenerator _pdfGenerator;

    private readonly IExcelReportGenerator _excelGenerator;



    public ReportService(
        IStudentRepository studentRepository,
        IAIKnowledgeRepository aiKnowledgeRepository,
        IAdminDashboardRepository dashboardRepository,
        IReportRepository reportRepository,
        IPdfReportGenerator pdfGenerator,
        IExcelReportGenerator excelGenerator)
    {

        _studentRepository = studentRepository;

        _aiKnowledgeRepository = aiKnowledgeRepository;

        _dashboardRepository = dashboardRepository;

        _reportRepository = reportRepository;


        _pdfGenerator = pdfGenerator;

        _excelGenerator = excelGenerator;

    }





    // =====================================================
    // Academic Reports
    // =====================================================


    public async Task<StudentReportDto?> GetStudentReportAsync(
        int studentId)
    {

        return await _reportRepository
            .GetStudentReportAsync(studentId);

    }





    public async Task<List<AttendanceReportDto>> GetAttendanceReportAsync(
        int? courseOfferingId)
    {

        return await _reportRepository
            .GetAttendanceReportAsync(courseOfferingId);

    }





    public async Task<List<ScoreReportDto>> GetScoreReportAsync(
        int? courseOfferingId)
    {

        return await _reportRepository
            .GetScoreReportAsync(courseOfferingId);

    }





    public async Task<List<EnrollmentReportDto>> GetEnrollmentReportAsync(
        int? semesterId)
    {

        return await _reportRepository
            .GetEnrollmentReportAsync(semesterId);

    }





    public async Task<DashboardReportDto> GetDashboardReportAsync()
    {

        return await _reportRepository
            .GetDashboardReportAsync();

    }





    // =====================================================
    // Student Export
    // =====================================================


    public async Task<byte[]> ExportStudentsPdfAsync()
    {

        var students =
            await _studentRepository
                .GetStudentReportAsync();


        return _pdfGenerator
            .GenerateStudentReport(students);

    }





    public async Task<byte[]> ExportStudentsExcelAsync()
    {

        var students =
            await _studentRepository
                .GetStudentReportAsync();


        return _excelGenerator
            .GenerateStudentReport(students);

    }







    // =====================================================
    // AI Knowledge Export
    // =====================================================


    public async Task<byte[]> ExportAIKnowledgePdfAsync()
    {

        var knowledge =
            await _aiKnowledgeRepository
                .GetAIKnowledgeReportAsync();


        return _pdfGenerator
            .GenerateAIKnowledgeReport(knowledge);

    }






    public async Task<byte[]> ExportAIKnowledgeExcelAsync()
    {

        var knowledge =
            await _aiKnowledgeRepository
                .GetAIKnowledgeReportAsync();


        return _excelGenerator
            .GenerateAIKnowledgeReport(knowledge);

    }








    // =====================================================
    // Dashboard Export
    // =====================================================


    public async Task<byte[]> ExportDashboardPdfAsync()
    {

        var summary =
            await _dashboardRepository
                .GetDashboardSummaryAsync();


        return _pdfGenerator
            .GenerateDashboardSummaryReport(summary);

    }







    public async Task<byte[]> ExportDashboardExcelAsync()
    {

        var summary =
            await _dashboardRepository
                .GetDashboardSummaryAsync();


        return _excelGenerator
            .GenerateDashboardSummaryReport(summary);

    }

}