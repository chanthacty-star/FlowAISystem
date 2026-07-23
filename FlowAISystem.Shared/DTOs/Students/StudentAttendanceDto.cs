namespace FlowAISystem.Shared.DTOs.Students;

public class StudentAttendanceDto
{
    public int AttendanceId { get; set; }


    public int SubjectId { get; set; }


    public string SubjectCode { get; set; }
        = string.Empty;


    public string SubjectName { get; set; }
        = string.Empty;


    public DateOnly AttendanceDate { get; set; }


    public string Status { get; set; }
        = string.Empty;


    public string? Remark { get; set; }
}