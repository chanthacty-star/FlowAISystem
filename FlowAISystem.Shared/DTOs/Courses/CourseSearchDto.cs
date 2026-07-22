namespace FlowAISystem.Shared.DTOs.Courses;

public class CourseSearchDto
{
    public string SearchTerm { get; set; } = string.Empty;

    public int? DepartmentId { get; set; }

    public int? TeacherId { get; set; }

    public int? SemesterId { get; set; }
}