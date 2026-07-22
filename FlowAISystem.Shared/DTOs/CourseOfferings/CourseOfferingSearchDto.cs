namespace FlowAISystem.Shared.DTOs.CourseOfferings;

public class CourseOfferingSearchDto
{
    public string SearchTerm { get; set; } = string.Empty;


    public int? SubjectId { get; set; }


    public int? TeacherId { get; set; }


    public int? SemesterId { get; set; }
}