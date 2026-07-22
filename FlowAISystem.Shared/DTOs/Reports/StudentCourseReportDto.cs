namespace FlowAISystem.Shared.DTOs.Reports;

public class StudentCourseReportDto
{

    public string CourseName { get; set; }
        = string.Empty;


    public string TeacherName { get; set; }
        = string.Empty;


    public decimal Score { get; set; }


    public decimal AttendanceRate { get; set; }


    public string Grade { get; set; }
        = string.Empty;

}