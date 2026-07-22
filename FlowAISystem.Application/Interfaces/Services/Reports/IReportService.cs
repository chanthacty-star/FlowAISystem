using FlowAISystem.Shared.DTOs.Reports;

namespace FlowAISystem.Application.Interfaces.Services.Reports;

public interface IReportService
{

    // =====================================
    // Academic Reports
    // =====================================


    Task<StudentReportDto?> GetStudentReportAsync(
        int studentId);



    Task<List<AttendanceReportDto>> GetAttendanceReportAsync(
        int? courseOfferingId);



    Task<List<ScoreReportDto>> GetScoreReportAsync(
        int? courseOfferingId);



    Task<List<EnrollmentReportDto>> GetEnrollmentReportAsync(
        int? semesterId);



    Task<DashboardReportDto> GetDashboardReportAsync();





    // =====================================
    // Export Reports
    // =====================================


    Task<byte[]> ExportStudentsPdfAsync();



    Task<byte[]> ExportStudentsExcelAsync();



    Task<byte[]> ExportAIKnowledgePdfAsync();



    Task<byte[]> ExportAIKnowledgeExcelAsync();



    Task<byte[]> ExportDashboardPdfAsync();



    Task<byte[]> ExportDashboardExcelAsync();

}