namespace FlowAISystem.Shared.DTOs.CourseOfferings;

public class CourseOfferingListItemDto
{
    public int Id { get; set; }


    public string ClassName { get; set; } = string.Empty;


    public string SubjectName { get; set; } = string.Empty;


    public string TeacherName { get; set; } = string.Empty;


    public string SemesterName { get; set; } = string.Empty;


    public int Capacity { get; set; }


    public string? Room { get; set; }
}