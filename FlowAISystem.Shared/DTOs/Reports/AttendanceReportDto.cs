namespace FlowAISystem.Shared.DTOs.Reports;

public class AttendanceReportDto
{

    public int StudentId { get; set; }


    public string StudentName { get; set; }
        = string.Empty;


    public string CourseName { get; set; }
        = string.Empty;



    public int TotalSessions { get; set; }


    public int PresentCount { get; set; }


    public int AbsentCount { get; set; }


    public decimal AttendanceRate { get; set; }

}