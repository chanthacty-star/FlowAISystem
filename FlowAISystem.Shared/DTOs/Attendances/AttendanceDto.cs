using FlowAISystem.Shared.Enums;

namespace FlowAISystem.Shared.DTOs.Attendances;

public class AttendanceDto
{
    public int Id { get; set; }

    public DateOnly AttendanceDate { get; set; }

    public AttendanceStatus Status { get; set; }

    public string? Remark { get; set; }

    public int EnrollmentId { get; set; }

    public string StudentName { get; set; }
        = string.Empty;

    public string CourseName { get; set; }
        = string.Empty;

    public string ClassName { get; set; }
        = string.Empty;
}