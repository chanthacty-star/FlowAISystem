using FlowAISystem.Shared.DTOs.Reports;

namespace FlowAISystem.Application.Interfaces.Repositories;

public interface IReportRepository
{

    Task<StudentReportDto?> GetStudentReportAsync(
        int studentId);



    Task<List<AttendanceReportDto>> GetAttendanceReportAsync(
        int? courseOfferingId);



    Task<List<ScoreReportDto>> GetScoreReportAsync(
        int? courseOfferingId);



    Task<List<EnrollmentReportDto>> GetEnrollmentReportAsync(
        int? semesterId);



    Task<DashboardReportDto> GetDashboardReportAsync();

}