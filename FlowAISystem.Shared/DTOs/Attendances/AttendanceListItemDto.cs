using FlowAISystem.Shared.Enums;

namespace FlowAISystem.Shared.DTOs.Attendances;

public class AttendanceListItemDto
{
    public int Id { get; set; }

    public string StudentName { get; set; }
        = string.Empty;

    public string CourseName { get; set; }
        = string.Empty;

    public string ClassName { get; set; }
        = string.Empty;

    public DateOnly AttendanceDate { get; set; }

    public AttendanceStatus Status { get; set; }
}