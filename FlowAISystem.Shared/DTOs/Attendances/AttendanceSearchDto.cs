namespace FlowAISystem.Shared.DTOs.Attendances;

public class AttendanceSearchDto
{
    public string SearchTerm { get; set; } = string.Empty;

    public int? EnrollmentId { get; set; }

    public DateOnly? AttendanceDate { get; set; }

    public int? CourseOfferingId { get; set; }
}