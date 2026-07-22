namespace FlowAISystem.Shared.DTOs.Enrollments;

public class EnrollmentSearchDto
{
    public string SearchTerm { get; set; }
        = string.Empty;


    public int? StudentId { get; set; }


    public int? CourseOfferingId { get; set; }


    public int? SemesterId { get; set; }
}